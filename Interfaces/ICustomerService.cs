using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface ICustomerService
    {
        int Save(ICustomer customer);
        ICustomer GetCustomerById(int id);
        IList<ICustomer> GetCustomers(ICustomerFilter filter);
        void Delete(int id);
    }
}
