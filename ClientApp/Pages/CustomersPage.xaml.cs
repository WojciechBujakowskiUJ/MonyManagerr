using DatabaseConnect;
using Interfaces;
using Interfaces.Implementation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp.Pages
{

    public class CustomersPageViewModel : AbstractPage
    {

        #region Binding Properties

        private ICustomer _customer;
        public ICustomer Customer
        {
            get
            {
                return _customer;
            }
            set
            {
                if (_customer != value)
                {
                    _customer = value;
                    RaisePropertyChanged("Customer");

                    RaisePropertyChanged("EditorLabel");
                    RaisePropertyChanged("EditorButtonLabel");

                    ReloadEditor();
                }
            }
        }

        private ObservableCollection<ICustomer> _customers;
        public ObservableCollection<ICustomer> Customers
        {
            get
            {
                return _customers;
            }
            set
            {
                if (_customers != value)
                {
                    _customers = value;
                    CustomersView = CollectionViewSource.GetDefaultView(_customers);
                    CustomersView.Filter = e =>
                    {
                        Customer = null;

                        ICustomer t = e as ICustomer;

                        return t != null
                            && (string.IsNullOrWhiteSpace(FilterName) || (t.Name != null && t.Name.Contains(FilterName)))
                            && (string.IsNullOrWhiteSpace(FilterDescription) || (t.Description != null && t.Description.Contains(FilterDescription)))
                            && (FilterDtt == null || FilterDtt.Id == 0 || (t.DefaultTransactionType != null && FilterDtt.Id == t.DefaultTransactionTypeId))
                            && (!FilterActive.HasValue || (t.Active == FilterActive.Value));
                    };

                    RaisePropertyChanged("CustomersView");
                    RaisePropertyChanged("Customer");
                }
            }
        }
        public ICollectionView CustomersView { get; set; }

        private string _filterName = string.Empty;
        public string FilterName
        {
            get
            {
                return _filterName;
            }
            set
            {
                if (_filterName != value)
                {
                    _filterName = value;
                    RaisePropertyChanged("FilterName");
                    CustomersView.Refresh();
                }
            }
        }

        private string _filterDescription = string.Empty;
        public string FilterDescription
        {
            get
            {
                return _filterDescription;
            }
            set
            {
                if (_filterDescription != value)
                {
                    _filterDescription = value;
                    RaisePropertyChanged("FilterDescription");
                    CustomersView.Refresh();
                }
            }
        }

        private ITransactionType _filterDtt;
        public ITransactionType FilterDtt
        {
            get
            {
                return _filterDtt;
            }
            set
            {
                if (_filterDtt != value)
                {
                    _filterDtt = value;
                    RaisePropertyChanged("FilterDtt");
                    CustomersView.Refresh();
                }
            }
        }

        private bool? _filterActive = null;
        public bool? FilterActive
        {
            get
            {
                return _filterActive;
            }
            set
            {
                if (_filterActive != value)
                {
                    _filterActive = value;
                    RaisePropertyChanged("FilterActive");
                    CustomersView.Refresh();
                }
            }
        }

        private IDictionary<string, Brush> _colorOptions;
        public IDictionary<string, Brush> ColorOptions
        {
            get
            {
                if (_colorOptions == null)
                {
                    _colorOptions = ColorsDictionary.GetDictionaryWithNull();
                }
                return _colorOptions;
            }
        }

        private IList<ITransactionType> _transactionTypeOptions;
        public IList<ITransactionType> TransactionTypeOptions
        {
            get
            {
                if (_transactionTypeOptions == null)
                {
                    _transactionTypeOptions = new List<ITransactionType>();
                    _transactionTypeOptions.Add(new TransactionType() { Id = 0, Name = "None" } );
                }
                return _transactionTypeOptions;
            }
            set
            {
                if (_transactionTypeOptions != value)
                {
                    _transactionTypeOptions.Clear();
                    _transactionTypeOptions.Add(new TransactionType() { Id = 0, Name = "None" });
                    foreach (var tt in value.OrderBy(tt => tt.Name))
                    {
                        _transactionTypeOptions.Add(tt);
                    }
                    RaisePropertyChanged("TransactionTypeOptions");
                }
            }
        }

        #region Editor

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    RaisePropertyChanged("Description");
                }
            }
        }

        private ITransactionType _dtt;
        public ITransactionType Dtt
        {
            get
            {
                return _dtt;
            }
            set
            {
                if (_dtt != value)
                {
                    _dtt = value;
                    RaisePropertyChanged("Dtt");
                }
            }
        }

        private bool _active;
        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                if (_active != value)
                {
                    _active = value;
                    RaisePropertyChanged("Active");
                }
            }
        }

        public string EditorLabel
        {
            get
            {
                return Customer == null ? "ADD" : "EDIT";
            }
        }

        public string EditorButtonLabel
        {
            get
            {
                return Customer == null ? "Add New" : "Save Changes";
            }
        }

        #endregion

        #endregion

        #region Command

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand<string>(
                        param => Save(),
                        param => AllowInput && IsFormValid() && Name != null
                    );
                }
                return _saveCommand;
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand<string>(
                        param => Delete(),
                        param => AllowInput && Customer != null
                    );
                }
                return _deleteCommand;
            }
        }

        private ICommand _deselectCommand;
        public ICommand DeselectCommand
        {
            get
            {
                if (_deselectCommand == null)
                {
                    _deselectCommand = new RelayCommand<string>(
                        param => { Customer = null; },
                        param => AllowInput && Customer != null
                    );
                }
                return _deselectCommand;
            }
        }

        #endregion

        #region Methods

        protected async override void OnLoad(object param)
        {
            AllowInput = false;

            try
            {
                IDatabaseService dbconn = new DatabaseService();
                dbconn.ConnectionString = ConnectionStringsProvider.Get();

                TransactionTypeOptions = await dbconn.TransactionTypeService.GetTransactionTypesAsync();

                var customersRaw = await dbconn.CustomerService.GetCustomersAsync();
                Customers = new ObservableCollection<ICustomer>(customersRaw);

                ReloadEditor();
                RaisePropertyChanged("TransactionTypeOptions");

                FilterDtt = _transactionTypeOptions.FirstOrDefault();
                Dtt = _transactionTypeOptions.FirstOrDefault();
            }
            catch (SqlException e)
            {
                MessageBox.Show("Unexpected SQL error occurred while accessing database. Details:/n" + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Unexpected error occurred while accessing database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                AllowInput = true;
            }


            base.OnLoad(param);
        }

        private void ReloadEditor()
        {
            if (Customer == null)
            {
                Name = "New customer";
                Description = string.Empty;
                Dtt = _transactionTypeOptions.FirstOrDefault();
                Active = true;
            }
            else
            {
                Name = Customer.Name;
                Description = Customer.Description;
                Dtt = TransactionTypeOptions.Where(tt => tt != null && tt.Id == Customer.DefaultTransactionTypeId).FirstOrDefault();
                if (Dtt == null)
                {
                    Dtt = _transactionTypeOptions.FirstOrDefault();
                }
                Active = Customer.Active;
            }
        }

        private async void Save()
        {
            AllowInput = false;

            try
            {
                IDatabaseService dbconn = new DatabaseService();
                dbconn.ConnectionString = ConnectionStringsProvider.Get();

                int newId = await dbconn.CustomerService.SaveAsync(new Customer()
                {
                    Id = Customer == null ? 0 : Customer.Id,
                    Name = this.Name,
                    Description = this.Description,
                    DefaultTransactionTypeId = Dtt.Id == 0 ? null : (int?)Dtt.Id,
                    Active = this.Active
                });

                var customersRaw = await dbconn.CustomerService.GetCustomersAsync();
                Customers = new ObservableCollection<ICustomer>(customersRaw);

                Customer = Customers.Where(c => c.Id == newId).FirstOrDefault();

                ReloadEditor();
            }
            catch (SqlException e)
            {
                MessageBox.Show("Unexpected SQL error occurred while saving entry in database. Details:/n" + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Unexpected error occurred while saving entry in database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                AllowInput = true;
            }
        }

        private async void Delete()
        {
            AllowInput = false;

            try
            {
                IDatabaseService dbconn = new DatabaseService();
                dbconn.ConnectionString = ConnectionStringsProvider.Get();

                await dbconn.CustomerService.DeleteAsync(Customer.Id);

                var customersRaw = await dbconn.CustomerService.GetCustomersAsync();
                Customers = new ObservableCollection<ICustomer>(customersRaw);

                Customer = null;

                ReloadEditor();
            }
            catch (SqlException e) when (e.ToString().Contains("REFERENCE") && e.ToString().Contains("FK_"))
            {
                MessageBox.Show("Selected Customer cannot be deleted.\n\nThere are Transactions in the database that refer to it.", "Database Constraint Notice", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (SqlException e)
            {
                MessageBox.Show("Unexpected SQL error occurred while deleting entry in database. Details:/n" + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Unexpected error occurred while deleting entry in database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                AllowInput = true;
            }
        }

        #endregion

        #region Validation

        protected override void FormValidationRules()
        {

            if (StartValidating)
            {
                if (Name != null)
                {
                    if (string.IsNullOrWhiteSpace(Name))
                    {
                        AppendError("Name", "Name cannot be empty");
                    }
                    else if (Customers.Where(c => Customer == null || c.Id != Customer.Id).Any(c => c.Name == Name))
                    {
                        AppendError("Name", "Name must be unique");
                    }
                }
                RaiseErrorsChanged("Name");

            }
        }

        #endregion

    }


    public partial class CustomersPage : UserControl
    {
        public CustomersPage()
        {
            InitializeComponent();
        }
    }
}
