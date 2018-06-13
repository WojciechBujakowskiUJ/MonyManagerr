using ClientApp.Helpers;
using ClientApp.Helpers.Drawing;
using DatabaseConnect;
using Interfaces;
using Interfaces.Implementation;
using Statistics;
using Statistics.TransactionStatistics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class SummaryPageViewModel : AbstractPage
    {

        #region Local Fields

        private readonly BarChartDrawerFactory bcdFactory = new BarChartDrawerFactory();

        private readonly TransactionStatisticsProvider stats = new TransactionStatisticsProvider();

        private IList<ITransaction> data = new List<ITransaction>();

        #endregion

        #region Local Properties

        private IBarChartDrawer<DateTime,decimal> _bcDrawer = null;
        public IBarChartDrawer<DateTime, decimal> BcDrawer
        {
            get
            {
                return _bcDrawer;
            }
            set
            {
                if (_bcDrawer != value)
                {
                    _bcDrawer = value;
                    _bcDrawer.Redraw(null);
                }
            }
        }

        private IList<TimeStepType> _allTimeStepOptions = null;
        public IList<TimeStepType> AllTimeStepOptions
        {
            get
            {
                if (_allTimeStepOptions == null)
                {
                    _allTimeStepOptions = Enum.GetValues(typeof(TimeStepType)).Cast<TimeStepType>().ToList();
                }
                return _allTimeStepOptions;
            }
        }

        #endregion

        #region Binding Properties

        private TimeStepType _timeStep = TimeStepType.Default;
        public TimeStepType TimeStep
        {
            get
            {
                return _timeStep;
            }
            set
            {
                if (_timeStep != value)
                {
                    _timeStep = value;
                    RaisePropertyChanged("TimeStep");

                    ReloadBarChart();
                }
            }
        }

        private DateTime _dateMin = DateTime.Now - new TimeSpan(7, 0, 0, 0);
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

                    ReloadTimeStepOptions();
                    ReloadBarChart();
                    ReloadPieChart();
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

                    ReloadTimeStepOptions();
                    ReloadBarChart();
                    ReloadPieChart();
                }
            }
        }

        private bool _isIncome = false;
        public bool IsIncome
        {
            get
            {
                return _isIncome;
            }
            set
            {
                if (_isIncome != value)
                {
                    _isIncome = value;
                    RaisePropertyChanged("IsIncome");

                    ReloadPieChart();
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

                    ReloadPieChart();
                }
            }
        }

        #region Options

        private ObservableCollection<TimeStepType> _currentTimeStepOptions;
        public ObservableCollection<TimeStepType> CurrentTimeStepOptions
        {
            get
            {
                return _currentTimeStepOptions;
            }
            set
            {
                if (_currentTimeStepOptions != value)
                {
                    _currentTimeStepOptions = value;
                    RaisePropertyChanged("CurrentTimeStepOptions");
                }
            }
        }

        private IReadOnlyList<bool> _categoryOptions = null;
        public IReadOnlyList<bool> CategoryOptions
        {
            get
            {
                if (_categoryOptions == null)
                {
                    _categoryOptions = new List<bool>() { false, true };
                }
                return _categoryOptions;
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

        private ObservableCollection<PieChartEntry> _pieChartData = new ObservableCollection<PieChartEntry>();
        public ObservableCollection<PieChartEntry> PieChartData
        {
            get
            {
                return _pieChartData;
            }
            set
            {
                if (_pieChartData != value)
                {
                    _pieChartData = value;
                    RaisePropertyChanged("PieChartData");
                }
            }
        }

        public OxyPlot.PlotModel _barModel;
        public OxyPlot.PlotModel BarModel
        {
            get
            {
                return _barModel;
            }
            set
            {
                _barModel = value;
                RaisePropertyChanged("BarModel");

            }

        }


        private OxyPlot.PlotModel _pieModel = null;
        public OxyPlot.PlotModel PieModel
        {
            get
            {
                return _pieModel;
            }
            set
            {
                if (_pieModel != value)
                {
                    _pieModel = value;
                    RaisePropertyChanged("PieModel");
                }
            }
        }

        #endregion

        #endregion

        #region Commands

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

        #endregion

        #region Methods

        protected async override void OnLoad(object param)
        {
            AllowInput = false;

            try
            {
                BcDrawer = bcdFactory.CreateForTransactions(param as Grid);
                ReloadTimeStepOptions();

                IDatabaseService dbconn = new DatabaseService();
                dbconn.ConnectionString = ConnectionStringsProvider.Get();

                CustomerOptions = await dbconn.CustomerService.GetCustomersAsync();
                data = await dbconn.TransactionService.GetTransactionsAsync();

                RaisePropertyChanged("CustomerOptions");

                FilterCustomer = CustomerOptions.FirstOrDefault();
                IsIncome = false;

                ReloadBarChart();
                ReloadPieChart();

                base.OnLoad(param);
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

        private void SetTimespan(string param)
        {
            DateMax = DateTime.Now;

            switch (param)
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

        private async void ReloadPieChart()
        {
            var pcData = await PreparePieDataAsync(data.ToList());

            PieChartData.Clear();
            foreach (var item in pcData)
            {
                PieChartData.Add(item);
            }

            PieModel = PieChartModelProvider.GetModel(pcData);
        }

        private async void ReloadBarChart()
        {
            var statsRes = await stats.CalculateAsync(data.ToList(), DateMin, DateMax, TimeStep);
            // BcDrawer.Redraw(statsRes);
            BarModel = BarChartModelProvider.GetModel(statsRes, TimeStep);
        }

        private void ReloadTimeStepOptions()
        {
            CurrentTimeStepOptions = new ObservableCollection<TimeStepType>(TimeStepUtils.GetOptionsForTimeSpan(DateMax - DateMin));
        }

        #endregion

        #region Async

        private Task<IList<PieChartEntry>> PreparePieDataAsync(IList<ITransaction> rawData)
        {
            return Task.Factory.StartNew<IList<PieChartEntry>>(() =>
            {
                var query = rawData.Where(t => t.Date >= DateMin && t.Date <= DateMax);
                if (FilterCustomer != null && FilterCustomer.Id != 0)
                {
                    query = query.Where(t => t.CustomerId == FilterCustomer.Id);
                }

                query = IsIncome ? query.Where(t => t.Value >= 0M) : query.Where(t => t.Value <= 0M);

                return query.GroupBy(t => t.TransactionType)
                    .Select(grp => new PieChartEntry(grp.Key.Name, grp.Sum(t => t.Value), grp.Key.Color))
                    .ToList();
            });

        }

        #endregion

    }

    public partial class SummaryPage : UserControl
    {
        public SummaryPage()
        {
            InitializeComponent();
        }
    }

}
