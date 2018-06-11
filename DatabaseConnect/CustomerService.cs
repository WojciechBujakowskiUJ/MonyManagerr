using Interfaces;
using Interfaces.Implementation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseConnect
{
    public class CustomerService : ICustomerService
    {

        public ICustomer GetCustomerById(int id)
        {
           return GetCustomers(new CustomerFilter() { Id = id }).FirstOrDefault();
        }

        public IList<ICustomer> GetCustomers()
        {
            return GetCustomers(new CustomerFilter());
        }

        public IList<ICustomer> GetCustomers(ICustomerFilter filter)
        {

            SqlQueryBuilder sqlQueryBuilder = new SqlQueryBuilder();
            sqlQueryBuilder.Select = " SELECT * ";
            sqlQueryBuilder.From = " FROM [dbo].[Customer] ";
            if(filter.Id.HasValue)
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "Id = @Id", Param = new SqlParameter("@Id", filter.Id.Value ) });
            }
            if (filter.Active.HasValue)
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "Active = @Active", Param = new SqlParameter("@Active", filter.Active.Value) });
            }

            if (filter.DefaultTransactionTypeId.HasValue)
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "DefaultTransactionTypeId = @DefaultTransactionTypeId", Param = new SqlParameter("@DefaultTransactionTypeId", filter.DefaultTransactionTypeId.Value) });
            }
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "Name = @Name", Param = new SqlParameter("@Name", filter.Name) });
            }
            var table = SqlService.GetDataTable(sqlQueryBuilder);
            var myEnumerable = table.AsEnumerable();

            var result = ( from item in myEnumerable select new Customer
                 {
                     Active = item.Field<bool>("Active"),
                     DefaultTransactionTypeId = item.Field<int?>("DefaultTransactionTypeId"),
                     Name = item.Field<string>("Name"),
                     Description = item.Field<string>("Description"),
                     Id = item.Field<int>("Id")
                 }).ToList<ICustomer>();

            var ttService = new TransactionTypeService();
            var tts = ttService.GetTransactionTypes();

            foreach (var customer in result)
            {
                if (customer.DefaultTransactionTypeId.HasValue)
                {
                    customer.DefaultTransactionType = tts.Where(tt => tt.Id == customer.DefaultTransactionTypeId.Value).FirstOrDefault();
                }
            }

            return result;
        }
        public void ClearTables()
        {
            string query = @" DELETE FROM [dbo].[Customer] ";
            IList<SqlParameter> sqlParameterCollection = new List<SqlParameter>();
            SqlService.ExecuteNonQuery(query, sqlParameterCollection.ToArray());
        }
        public void Delete(int id)
        {
            String query = @" DELETE FROM [dbo].[Customer] WHERE Id = @Id";
            IList<SqlParameter> sqlParameterCollection = new List<SqlParameter>();
            sqlParameterCollection.Add(new SqlParameter("@Id", id));
            SqlService.ExecuteNonQuery(query, sqlParameterCollection.ToArray());
        }

        public int Save(ICustomer customer)
        {
            String query = @"INSERT INTO [dbo].[Customer]
         
           ([Name]
           ,[Description]
           ,[Active]
           ,[DefaultTransactionTypeId])
            OUTPUT INSERTED.Id
            VALUES
           ( @Name
           , @Description
           , @Active
           , @DefaultTransactionTypeId)";

            if (customer.Id != 0)
            {
                query = @"UPDATE [dbo].[Customer]
                   SET [Name] = @Name
                      ,[Description] = @Description
                      ,[Active] = @Active
                      ,[DefaultTransactionTypeId] = @DefaultTransactionTypeId
                 WHERE Id = @Id";
            }

            IList<SqlParameter> sqlParameterCollection = new List<SqlParameter>();

            sqlParameterCollection.Add(new SqlParameter("@Name", customer.Name));
            sqlParameterCollection.Add(new SqlParameter("@Description", customer.Description));
            sqlParameterCollection.Add(new SqlParameter("@Active", customer.Active));

            if (customer.DefaultTransactionTypeId.HasValue)
            {
                sqlParameterCollection.Add(new SqlParameter("@DefaultTransactionTypeId", customer.DefaultTransactionTypeId.Value));
            }
            else
            {
                sqlParameterCollection.Add(new SqlParameter("@DefaultTransactionTypeId", DBNull.Value));
            }

            if (customer.Id != 0)
            {
                sqlParameterCollection.Add(new SqlParameter("@Id", customer.Id));
                SqlService.ExecuteNonQuery(query, sqlParameterCollection.ToArray());
                return customer.Id;
            }
            else
            {
                return SqlService.ExecuteScalar(query, sqlParameterCollection.ToArray());
            }
        }

        #region Async

        public Task<int> SaveAsync(ICustomer customer)
        {
            return Task.Factory.StartNew<int>(() => 
            {
                return Save(customer);
            });
        }

        public Task<ICustomer> GetCustomerByIdAsync(int id)
        {
            return Task.Factory.StartNew<ICustomer>(() =>
            {
                return GetCustomerById(id);
            });
        }

        public Task<IList<ICustomer>> GetCustomersAsync(ICustomerFilter filter)
        {
            return Task.Factory.StartNew<IList<ICustomer>>(() =>
            {
                return GetCustomers(filter);
            });
        }

        public Task<IList<ICustomer>> GetCustomersAsync()
        {
            return Task.Factory.StartNew<IList<ICustomer>>(() =>
            {
                return GetCustomers();
            });
        }

        public Task DeleteAsync(int id)
        {
            return Task.Factory.StartNew(() =>
            {
                Delete(id);
            });
        }

        #endregion
    }
}
