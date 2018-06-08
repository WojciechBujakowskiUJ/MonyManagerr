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

    public class TransactionsPageViewModel : AbstractPage
    {

        #region Local Fields

        private bool isTransactionTypeBoxDirty = false;

        #endregion

        #region Binding Properties

        private ITransaction _transaction;
        public ITransaction Transaction
        {
            get
            {
                return _transaction;
            }
            set
            {
                if (_transaction != value)
                {
                    _transaction = value;
                    RaisePropertyChanged("Transaction");

                    RaisePropertyChanged("EditorLabel");
                    RaisePropertyChanged("EditorButtonLabel");

                    ReloadEditor();
                }
            }
        }

        private ObservableCollection<ITransaction> _transactions;
        public ObservableCollection<ITransaction> Transactions
        {
            get
            {
                return _transactions;
            }
            set
            {
                if (_transactions != value)
                {
                    _transactions = value;
                    TransactionsView = CollectionViewSource.GetDefaultView(_transactions);
                    TransactionsView.Filter = e =>
                    {
                        Transaction = null;

                        ITransaction t = e as ITransaction;

                        return t != null
                            && (string.IsNullOrWhiteSpace(FilterName) || (t.Name != null && t.Name.Contains(FilterName)))
                            && (string.IsNullOrWhiteSpace(FilterDescription) || (t.Description != null && t.Description.Contains(FilterDescription)))
                            && (FilterTransactionType == null || FilterTransactionType.Id == 0 || (t.TransactionType != null && FilterTransactionType.Id == t.TransactionTypeId))
                            && (FilterCustomer == null || FilterCustomer.Id == 0 || (t.Customer != null && FilterCustomer.Id == t.CustomerId))
                            && (!DateMinEnabled || (t.Date >= DateMin))
                            && (!DateMaxEnabled || (t.Date <= DateMax))
                            && (!ValueMinEnabled || (t.Value >= ValueMin))
                            && (!ValueMaxEnabled || (t.Value <= ValueMax));
                    };

                    RaisePropertyChanged("TransactionsView");
                    RaisePropertyChanged("Transaction");
                }
            }
        }
        public ICollectionView TransactionsView { get; set; }

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
                    TransactionsView.Refresh();
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
                    TransactionsView.Refresh();
                }
            }
        }

        private ITransactionType _filterTransactionType;
        public ITransactionType FilterTransactionType
        {
            get
            {
                return _filterTransactionType;
            }
            set
            {
                if (_filterTransactionType != value)
                {
                    _filterTransactionType = value;
                    RaisePropertyChanged("FilterTransactionType");
                    TransactionsView.Refresh();
                }
            }
        }

        private ICustomer _filterCustomer;
        public ICustomer FilterCustomer
        {
            get
            {
                return _filterCustomer;
            }
            set
            {
                if (_filterCustomer != value)
                {
                    _filterCustomer = value;
                    RaisePropertyChanged("FilterCustomer");
                    TransactionsView.Refresh();
                }
            }
        }

        private bool _dateMinEnabled;
        public bool DateMinEnabled
        {
            get
            {
                return _dateMinEnabled;
            }
            set
            {
                if (_dateMinEnabled != value)
                {
                    _dateMinEnabled = value;
                    RaisePropertyChanged("DateMinEnabled");
                    TransactionsView.Refresh();
                }
            }
        }

        private bool _dateMaxEnabled;
        public bool DateMaxEnabled
        {
            get
            {
                return _dateMaxEnabled;
            }
            set
            {
                if (_dateMaxEnabled != value)
                {
                    _dateMaxEnabled = value;
                    RaisePropertyChanged("DateMaxEnabled");
                    TransactionsView.Refresh();
                }
            }
        }

        private DateTime _dateMin = DateTime.Now;
        public DateTime DateMin
        {
            get
            {
                return _dateMin;
            }
            set
            {
                if (_dateMin != value)
                {
                    _dateMin = value;
                    RaisePropertyChanged("DateMin");
                    TransactionsView.Refresh();
                }
            }
        }

        private DateTime _dateMax = DateTime.Now;
        public DateTime DateMax
        {
            get
            {
                return _dateMax;
            }
            set
            {
                if (_dateMax != value)
                {
                    _dateMax = value;
                    RaisePropertyChanged("DateMax");
                    TransactionsView.Refresh();
                }
            }
        }

        private bool _valueMinEnabled;
        public bool ValueMinEnabled
        {
            get
            {
                return _valueMinEnabled;
            }
            set
            {
                if (_valueMinEnabled != value)
                {
                    _valueMinEnabled = value;
                    RaisePropertyChanged("ValueMinEnabled");
                    TransactionsView.Refresh();
                }
            }
        }

        private bool _valueMaxEnabled;
        public bool ValueMaxEnabled
        {
            get
            {
                return _valueMaxEnabled;
            }
            set
            {
                if (_valueMaxEnabled != value)
                {
                    _valueMaxEnabled = value;
                    RaisePropertyChanged("ValueMaxEnabled");
                    TransactionsView.Refresh();
                }
            }
        }

        private decimal _valueMin;
        public decimal ValueMin
        {
            get
            {
                return _valueMin;
            }
            set
            {
                if (_valueMin != value)
                {
                    _valueMin = value;
                    RaisePropertyChanged("ValueMin");
                    TransactionsView.Refresh();
                }
            }
        }

        private decimal _valueMax;
        public decimal ValueMax
        {
            get
            {
                return _valueMax;
            }
            set
            {
                if (_valueMax != value)
                {
                    _valueMax = value;
                    RaisePropertyChanged("ValueMax");
                    TransactionsView.Refresh();
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
                    _transactionTypeOptions.Add(new TransactionType() { Id = 0, Name = "None" });
                }
                return _transactionTypeOptions;
            }
            set
            {
                if (_transactionTypeOptions != value)
                {
                    TransactionTypeOptions.Clear();
                    TransactionTypeOptions.Add(new TransactionType() { Id = 0, Name = "None" });
                    foreach (var tt in value.OrderBy(tt => tt.Name))
                    {
                        TransactionTypeOptions.Add(tt);
                    }
                    RaisePropertyChanged("TransactionTypeOptions");
                }
            }
        }

        private IList<ICustomer> _customerOptions;
        public IList<ICustomer> CustomerOptions
        {
            get
            {
                if (_customerOptions == null)
                {
                    _customerOptions = new List<ICustomer>();
                    _customerOptions.Add(new Customer() { Id = 0, Name = "None" });
                }
                return _customerOptions;
            }
            set
            {
                if (_customerOptions != value)
                {
                    CustomerOptions.Clear();
                    CustomerOptions.Add(new Customer() { Id = 0, Name = "None" });
                    foreach (var c in value.OrderBy(c => c.Name))
                    {
                        CustomerOptions.Add(c);
                    }
                    RaisePropertyChanged("CustomerOptions");
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

        private ITransactionType _transactionType;
        public ITransactionType TransactionType
        {
            get
            {
                return _transactionType;
            }
            set
            {
                if (_transactionType != value)
                {
                    _transactionType = value;
                    RaisePropertyChanged("TransactionType");

                    if ((value.Income && Value < 0M) || (!value.Income && Value > 0M))
                    {
                        Value = -Value;
                    }

                    isTransactionTypeBoxDirty = true;

                    RaisePropertyChanged("EditorMaxValue");
                    RaisePropertyChanged("EditorMinValue");
                }
            }
        }

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

                    if (_customer != null && _customer.DefaultTransactionTypeId.HasValue)
                    {
                        TransactionType = TransactionTypeOptions.Where(tt => tt.Id == _customer.DefaultTransactionTypeId.Value).FirstOrDefault();
                    }
                }
            }
        }

        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    RaisePropertyChanged("Date");
                }
            }
        }

        private decimal _value;
        public decimal Value
        {
            get
            {
                return _value;
            }
            set
            {
                value = value < EditorMinValue ? EditorMinValue : (value > EditorMaxValue ? EditorMaxValue : value);
                if (_value != value)
                {
                    _value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }

        public string EditorLabel
        {
            get
            {
                return Transaction == null ? "ADD" : "EDIT";
            }
        }

        public string EditorButtonLabel
        {
            get
            {
                return Transaction == null ? "Add New" : "Save Changes";
            }
        }

        public decimal EditorMaxValue
        {
            get
            {
                return (TransactionType == null || TransactionType.Income) ? 1000000000M : 0M;
            }
        }

        public decimal EditorMinValue
        {
            get
            {
                return (TransactionType == null || !TransactionType.Income) ? -1000000000M : 0M;
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
                        param => AllowInput && IsFormValid() && Name != null && TransactionType != null && TransactionType.Id != 0
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
                        param => AllowInput && Transaction != null
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
                        param => { Transaction = null; },
                        param => AllowInput && Transaction != null
                    );
                }
                return _deselectCommand;
            }
        }

        private ICommand _timespanCommand;
        public ICommand TimespanCommand
        {
            get
            {
                if (_timespanCommand == null)
                {
                    _timespanCommand = new RelayCommand<string>(
                        param => SetTimespan(param),
                        param => AllowInput
                    );
                }
                return _timespanCommand;
            }
        }

        private ICommand _incomeCommand;
        public ICommand IncomeCommand
        {
            get
            {
                if (_incomeCommand == null)
                {
                    _incomeCommand = new RelayCommand<string>(
                        param => SetValueFilter(param),
                        param => AllowInput
                    );
                }
                return _incomeCommand;
            }
        }

        #endregion

        #region Methods

        protected async override void OnLoad()
        {
            AllowInput = false;

            try
            {
                IDatabaseService dbconn = new DatabaseService();
                dbconn.ConnectionString = ConnectionStringsProvider.Get();

                TransactionTypeOptions = await dbconn.TransactionTypeService.GetTransactionTypesAsync();
                CustomerOptions = await dbconn.CustomerService.GetCustomersAsync();

                var transactionsRaw = await dbconn.TransactionService.GetTransactionsAsync();
                Transactions = new ObservableCollection<ITransaction>(transactionsRaw);

                ReloadEditor();

                RaisePropertyChanged("TransactionTypeOptions");
                RaisePropertyChanged("CustomerOptions");

                TransactionType = TransactionTypeOptions.FirstOrDefault();
                FilterTransactionType = TransactionTypeOptions.FirstOrDefault();
                Customer = CustomerOptions.FirstOrDefault();
                FilterCustomer = CustomerOptions.FirstOrDefault();

                base.OnLoad();
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

        }

        private void ReloadEditor()
        {
            if (Transaction == null)
            {
                Name = "New transaction";
                Description = string.Empty;
                TransactionType = TransactionTypeOptions.FirstOrDefault();
                Customer = CustomerOptions.FirstOrDefault();
                Date = DateTime.Now;
                Value = 0M;
            }
            else
            {
                Name = Transaction.Name;
                Description = Transaction.Description;
                TransactionType = TransactionTypeOptions.Where(tt => tt != null && tt.Id == Transaction.TransactionTypeId).FirstOrDefault();
                if (TransactionType == null)
                {
                    TransactionType = TransactionTypeOptions.FirstOrDefault();
                }
                Customer = CustomerOptions.Where(c => c != null && c.Id == Transaction.CustomerId).FirstOrDefault();
                if (Customer == null)
                {
                    Customer = CustomerOptions.FirstOrDefault();
                }
                Date = Transaction.Date;
                Value = Transaction.Value;
            }
            isTransactionTypeBoxDirty = false;
        }

        private async void Save()
        {
            AllowInput = false;

            try
            {
                IDatabaseService dbconn = new DatabaseService();
                dbconn.ConnectionString = ConnectionStringsProvider.Get();

                int newId = await dbconn.TransactionService.SaveAsync(new Transaction()
                {
                    Id = Transaction == null ? 0 : Transaction.Id,
                    Name = this.Name,
                    Description = this.Description,
                    TransactionTypeId = TransactionType.Id,
                    CustomerId = Customer.Id == 0 ? null : (int?)Customer.Id,
                    Date = this.Date,
                    Value = this.Value
                });

                var transactionsRaw = await dbconn.TransactionService.GetTransactionsAsync();
                Transactions = new ObservableCollection<ITransaction>(transactionsRaw);

                Transaction = Transactions.Where(t => t.Id == newId).FirstOrDefault();

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

                await dbconn.TransactionService.DeleteAsync(Transaction.Id);

                var transactionsRaw = await dbconn.TransactionService.GetTransactionsAsync();
                Transactions = new ObservableCollection<ITransaction>(transactionsRaw);

                Transaction = null;

                ReloadEditor();
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

        private void SetTimespan(string param)
        {
            DateMaxEnabled = true;
            DateMinEnabled = true;
            DateMax = DateTime.Now;

            switch(param)
            {
            case "DAY":
                DateMin = DateMax - new TimeSpan(1, 0, 0, 0);
                break;

            case "WEEK":
                DateMin = DateMax - new TimeSpan(7, 0, 0, 0);
                break;

            case "MONTH":
                DateMin = DateMax - new TimeSpan(30, 0, 0, 0);
                break;

            case "YEAR":
                DateMin = DateMax - new TimeSpan(365, 0, 0, 0);
                break;

            default:
                throw new NotImplementedException();
            }
        }

        private void SetValueFilter(string param)
        {
            if (param == "INCOME")
            {
                ValueMaxEnabled = false;
                ValueMinEnabled = true;
                ValueMin = 0M;
            }
            else if (param == "EXPENSE")
            {
                ValueMaxEnabled = true;
                ValueMinEnabled = false;
                ValueMax = 0M;
            }
            else
            {
                throw new NotImplementedException();
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
                    else if (Transactions.Where(c => Transaction == null || c.Id != Transaction.Id).Any(c => c.Name == Name))
                    {
                        AppendError("Name", "Name must be unique");
                    }
                }
                RaiseErrorsChanged("Name");

                if (isTransactionTypeBoxDirty && (TransactionType == null || TransactionType.Id == 0))
                {
                    AppendError("TransactionType", "Transaction Type must be selected");
                }
                RaiseErrorsChanged("TransactionType");

            }
        }

        #endregion
    }

    public partial class TransactionsPage : UserControl
    {
        public TransactionsPage()
        {
            InitializeComponent();
        }
    }
}
