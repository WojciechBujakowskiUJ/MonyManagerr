using ClientApp.Pages;
using System;
using System.Collections.Generic;
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

namespace ClientApp
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        #region Binding Properties

        private AbstractPage _currentPage;
        public AbstractPage CurrentPage
        {
            get
            {
                if (_currentPage == null)
                {
                    _currentPage = AbstractPage.GetPage(this, PageType.Summary);
                    _currentPageType = PageType.Summary;
                }
                return _currentPage;
            }
            set
            {
                if (_currentPage != value)
                {
                    if (_currentPage != null)
                    {
                        _currentPage.Dispose();
                    }
                    _currentPage = value;
                    RaisePropertyChanged("CurrentPage");
                }
            }
        }

        private PageType _currentPageType;
        public PageType CurrentPageType
        {
            get
            {
                return _currentPageType;
            }
            set
            {
                if (_currentPageType != value)
                {
                    _currentPageType = value;
                    RaisePropertyChanged("CurrentPageType");

                    CurrentPage = AbstractPage.GetPage(this, value);
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
                    _loadCommand = new RelayCommand<string>(
                        param => { },
                        param => true
                    );
                }
                return _loadCommand;
            }
        }

        private ICommand _summaryPageCommand = null;
        public ICommand SummaryPageCommand 
        {
            get
            {
                if (_summaryPageCommand == null)
                {
                    _summaryPageCommand = new RelayCommand<string>(
                        param => { CurrentPageType = PageType.Summary; },
                        param => true
                    );
                }
                return _summaryPageCommand;
            }
        }

        private ICommand _transactionsPageCommand = null;
        public ICommand TransactionsPageCommand
        {
            get
            {
                if (_transactionsPageCommand == null)
                {
                    _transactionsPageCommand = new RelayCommand<string>(
                        param => { CurrentPageType = PageType.Transactions; },
                        param => true
                    );
                }
                return _transactionsPageCommand;
            }
        }

        private ICommand _transactionTypesPageCommand = null;
        public ICommand TransactionTypesPageCommand
        {
            get
            {
                if (_transactionTypesPageCommand == null)
                {
                    _transactionTypesPageCommand = new RelayCommand<string>(
                        param => { CurrentPageType = PageType.TransactionTypes; },
                        param => true
                    );
                }
                return _transactionTypesPageCommand;
            }
        }

        private ICommand _customersPageCommand = null;
        public ICommand CustomersPageCommand
        {
            get
            {
                if (_customersPageCommand == null)
                {
                    _customersPageCommand = new RelayCommand<string>(
                        param => { CurrentPageType = PageType.Customers; },
                        param => true
                    );
                }
                return _customersPageCommand;
            }
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        #endregion

    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }
    }
}
