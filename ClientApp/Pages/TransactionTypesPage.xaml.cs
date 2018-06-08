using DatabaseConnect;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

    public class TransactionTypesPageViewModel : AbstractPage
    {

        #region Local Fields

        private bool isColorBoxDirty = false;

        #endregion

        #region Binding Properties

        private int _transactionTypeId;
        public int TransactionTypeId
        {
            get
            {
                return _transactionTypeId;
            }
            set
            {
                if (_transactionTypeId != value)
                {
                    _transactionTypeId = value;
                    RaisePropertyChanged("TransactionTypeId");

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

                    RaisePropertyChanged("EditorLabel");
                    RaisePropertyChanged("EditorButtonLabel");

                    ReloadEditor();
                }
            }
        }

        private ObservableCollection<ITransactionType> _transactionTypes;
        public ObservableCollection<ITransactionType> TransactionTypes
        {
            get
            {
                return _transactionTypes;
            }
            set
            {
                if (_transactionTypes != value)
                {
                    _transactionTypes = value;
                    TransactionTypesView = CollectionViewSource.GetDefaultView(_transactionTypes);
                    TransactionTypesView.Filter = e =>
                    {
                        TransactionTypeId = 0;
                        TransactionType = null;

                        ITransactionType t = e as ITransactionType;

                        return t != null
                            && (string.IsNullOrWhiteSpace(FilterName) || (t.Name != null && t.Name.Contains(FilterName)))
                            && (string.IsNullOrWhiteSpace(FilterDescription) || (t.Description != null && t.Description.Contains(FilterDescription)))
                            && (FilterColor == null || (t.Color != null && FilterColor.ToString() == t.Color ))
                            && (!FilterIncome.HasValue || (t.Income == FilterIncome.Value));
                    };

                    RaisePropertyChanged("TransactionTypesView");
                    RaisePropertyChanged("TransactionType");
                }
            }
        }
        public ICollectionView TransactionTypesView { get; set; }

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
                    TransactionTypesView.Refresh();
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
                    TransactionTypesView.Refresh();
                }
            }
        }

        private Brush _filterColor = null;
        public Brush FilterColor
        {
            get
            {
                return _filterColor;
            }
            set
            {
                if (_filterColor != value)
                {
                    _filterColor = value;
                    RaisePropertyChanged("FilterColor");
                    TransactionTypesView.Refresh();
                }
            }
        }

        private bool? _filterIncome = null;
        public bool? FilterIncome
        {
            get
            {
                return _filterIncome;
            }
            set
            {
                if (_filterIncome != value)
                {
                    _filterIncome = value;
                    RaisePropertyChanged("FilterIncome");
                    TransactionTypesView.Refresh();
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

        private Brush _color;
        public Brush Color
        {
            get
            {
                return _color;
            }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    RaisePropertyChanged("Color");

                    isColorBoxDirty = true;
                }
            }
        }

        private bool _income;
        public bool Income
        {
            get
            {
                return _income;
            }
            set
            {
                if (_income != value)
                {
                    _income = value;
                    RaisePropertyChanged("Income");
                }
            }
        }

        public string EditorLabel
        {
            get
            {
                return TransactionType == null ? "ADD" : "EDIT";
            }
        }

        public string EditorButtonLabel
        {
            get
            {
                return TransactionType == null ? "Add New" : "Save Changes";
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
                        param => AllowInput && IsFormValid() && Name != null && Color != null
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
                        param => AllowInput && TransactionType != null
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
                        param => { TransactionType = null; },
                        param => AllowInput && TransactionType != null
                    );
                }
                return _deselectCommand;
            }
        }

        #endregion

        #region Methods

        protected async override void OnLoad()
        {
            IDatabaseService dbconn = new DatabaseService();
            dbconn.ConnectionString = ConnectionStringsProvider.Get();

            var transactionTypesRaw = await dbconn.TransactionTypeService.GetTransactionTypesAsync();
            TransactionTypes = new ObservableCollection<ITransactionType>(transactionTypesRaw);

            ReloadEditor();
            base.OnLoad();
        }

        private void ReloadEditor()
        {
            if (TransactionType == null)
            {
                Name = "New type";
                Description = string.Empty;
                Color = null;
                Income = false;
            }
            else
            {
                Name = TransactionType.Name;
                Description = TransactionType.Description;
                Color = ColorOptions.Where(kvp => (kvp.Value == null ? null : kvp.Value.ToString()) == TransactionType.Color).Select(kvp => kvp.Value).FirstOrDefault();
                Income = TransactionType.Income;
            }
            isColorBoxDirty = false;
        }

        private async void Save()
        {
            AllowInput = false;

            try
            {
                IDatabaseService dbconn = new DatabaseService();
                dbconn.ConnectionString = ConnectionStringsProvider.Get();

                int newId = await dbconn.TransactionTypeService.SaveAsync(new TransactionType()
                {
                    Id = TransactionType == null ? 0 : TransactionType.Id,
                    Name = this.Name,
                    Description = this.Description,
                    Color = this.Color.ToString(),
                    Income = this.Income
                });

                var transactionTypesRaw = await dbconn.TransactionTypeService.GetTransactionTypesAsync();
                TransactionTypes = new ObservableCollection<ITransactionType>(transactionTypesRaw);
                TransactionType = TransactionTypes.Where(tt => tt.Id == newId).FirstOrDefault();
                ReloadEditor();
            }
            catch (Exception)
            {
                MessageBox.Show("Error", "Unexpected error occurred while saving entry in database", MessageBoxButton.OK, MessageBoxImage.Error);
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
                await dbconn.TransactionTypeService.DeleteAsync(TransactionType.Id);
                var transactionTypesRaw = await dbconn.TransactionTypeService.GetTransactionTypesAsync();
                TransactionTypes = new ObservableCollection<ITransactionType>(transactionTypesRaw);
                TransactionType = null;
                ReloadEditor();
            }
            catch (Exception)
            {
                MessageBox.Show("Error", "Unexpected error occurred while deleting entry in database", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    else if (TransactionTypes.Where(tt => TransactionType == null || tt.Id != TransactionType.Id).Any(tt => tt.Name == Name))
                    {
                        AppendError("Name", "Name must be unique");
                    }
                }
                RaiseErrorsChanged("Name");
                
                if (isColorBoxDirty && Color == null)
                {
                    AppendError("Color", "Color must be selected");
                }
                RaiseErrorsChanged("Color");
            }
        }

        #endregion

    }

    public partial class TransactionTypesPage : UserControl
    {
        public TransactionTypesPage()
        {
            InitializeComponent();
        }
    }
}
