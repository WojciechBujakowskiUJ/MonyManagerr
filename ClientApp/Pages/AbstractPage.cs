using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientApp.Pages
{
    public enum PageType
    {
        Summary,
        Transactions,
        TransactionTypes,
        Customers
    }

    public abstract class AbstractPage : IDisposable, INotifyPropertyChanged, INotifyDataErrorInfo
    {

        #region Properties

        public MainWindowViewModel ParentViewModel { get; set; }

        protected bool StartValidating { get; set; }

        #endregion

        #region Binding Properties

        private bool _allowInput = false;
        public bool AllowInput
        {
            get
            {
                return _allowInput;
            }
            set
            {
                if (_allowInput != value)
                {
                    _allowInput = value;
                    RaisePropertyChanged("AllowInput");
                }
            }
        }

        #endregion

        #region Commands

        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new RelayCommand<object>(
                        param => OnLoad(param),
                        param => true
                    );
                }
                return _loadCommand;
            }
        }

        protected virtual void OnLoad(object param)
        {
            AllowInput = true;
            StartValidating = true;
        }

        #endregion

        #region IDisposable Implementation

        private bool isDisposed = false;
        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public void Dispose()
        {
            Dispose(true);
            // uncomment if performance becomes an issue
            //GC.SuppressFinalize(this);
        }

        protected void Dispose(bool isDisposing)
        {
            if (!isDisposed)
            {
                PreCleanup();
                if (isDisposing)
                {
                    handle.Dispose();
                    CleanupManaged();
                }
                CleanupUnmanaged();

                isDisposed = true;
            }
        }

        protected virtual void PreCleanup() { }
        protected virtual void CleanupUnmanaged() { }
        protected virtual void CleanupManaged() { }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        #endregion

        #region INotifyDataErrorInfo Implementation

        protected virtual void FormValidationRules() { }

        protected bool IsFormValid()
        {
            _validationErrors.Clear();

            FormValidationRules();

            return !HasErrors;
        }

        protected void AppendError(string propertyName, string errorString)
        {
            if (_validationErrors.ContainsKey(propertyName))
            {
                _validationErrors[propertyName].Add(errorString);
            }
            else
            {
                ICollection<string> tmp = new Collection<string>
                {
                    errorString
                };
                _validationErrors.Add(propertyName, tmp);
            }
        }

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        protected void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName))
            {
                return null;
            }

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }

        #endregion

        #region Static SubControl Instance Provider

        public static AbstractPage GetPage(MainWindowViewModel parent, PageType type)
        {
            switch (type)
            {
            case PageType.Summary:
                return new SummaryPageViewModel() { ParentViewModel = parent };
            case PageType.Transactions:
                return new TransactionsPageViewModel() { ParentViewModel = parent };
            case PageType.TransactionTypes:
                return new TransactionTypesPageViewModel() { ParentViewModel = parent };
            case PageType.Customers:
                return new CustomersPageViewModel() { ParentViewModel = parent };
            default:
                throw new ArgumentException( "Requested page type (" + type.ToString() + ") is not supported." );
            }
        }

        #endregion

    }
}
