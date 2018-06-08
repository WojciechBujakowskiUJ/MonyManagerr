using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ICustomerService
    {
        int Save(ICustomer customer);
        ICustomer GetCustomerById(int id);
        IList<ICustomer> GetCustomers(ICustomerFilter filter);
        IList<ICustomer> GetCustomers();
        void Delete(int id);

        Task<int> SaveAsync(ICustomer customer);
        Task<ICustomer> GetCustomerByIdAsync(int id);
        Task<IList<ICustomer>> GetCustomersAsync(ICustomerFilter filter);
        Task<IList<ICustomer>> GetCustomersAsync();
        Task DeleteAsync(int id);
    }
}
