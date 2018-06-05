using Interfaces;
using System;
namespace DatabaseConnect
{
    public class DatabaseService : IDatabaseService
    {
        private string _connectionString;

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
                SqlService.ConnectionString = value;
            }
        }

        private ITransactionTypeService _transactionTypeService;
        public ITransactionTypeService TransactionTypeService
        {
            get
            {
                if (_transactionTypeService == null)
                {
                    _transactionTypeService = new TransactionTypeService();
                }
                return _transactionTypeService;
            }
        }

        private ITransactionService _transactionService;
        public ITransactionService TransactionService
        {
            get
            {
                if (_transactionService == null)
                {
                    _transactionService = new TransactionService();
                }
                return _transactionService;
            }
        }

        private ICustomerService _customerService;
        public ICustomerService CustomerService
        {
            get
            {
                if (_customerService == null)
                {
                    _customerService = new CustomerService();
                }
                return _customerService;
            }
        }
    }
}
