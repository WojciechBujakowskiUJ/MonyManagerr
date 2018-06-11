using Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnect
{
    class TransactionTypeService : ITransactionTypeService
    {

        public ITransactionType GetTransactionTypeById(int id)
        {
            return GetTransactionTypes(new TransactionTypeFilter() { Id = id }).FirstOrDefault();
        }

        public IList<ITransactionType> GetTransactionTypes()
        {
            return GetTransactionTypes(new TransactionTypeFilter());
        }

        public IList<ITransactionType> GetTransactionTypes(ITransactionTypeFilter filter)
        {
            SqlQueryBuilder sqlQueryBuilder = new SqlQueryBuilder();
            sqlQueryBuilder.Select = " SELECT * ";
            sqlQueryBuilder.From = " FROM [dbo].[TransactionType] ";
            if (filter.Id.HasValue)
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "Id = @Id", Param = new SqlParameter("@Id", filter.Id.Value) });
            }
            if (filter.Income.HasValue)
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "Income = @Income", Param = new SqlParameter("@Income", filter.Income.Value) });
            }
            if (!string.IsNullOrWhiteSpace(filter.Color))
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "Color = @Color", Param = new SqlParameter("@Color", filter.Color) });
            }
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "Name = @Name", Param = new SqlParameter("@Name", filter.Name) });
            }
            var table = SqlService.GetDataTable(sqlQueryBuilder);
            var myEnumerable = table.AsEnumerable();

            return ( from item in myEnumerable select new TransactionType {
                        Income = item.Field<bool>("Income"),
                        Color = item.Field<string>("Color"),
                        Name = item.Field<string>("Name"),
                        Description = item.Field<string>("Description"),
                        Id = item.Field<int>("Id")
                    }).ToList<ITransactionType>();
        }

        public void Delete(int id)
        {
            string query = @" DELETE FROM [dbo].[TransactionType] WHERE Id = @Id";
            IList<SqlParameter> sqlParameterCollection = new List<SqlParameter>();
            sqlParameterCollection.Add(new SqlParameter("@Id", id));
            SqlService.ExecuteNonQuery(query, sqlParameterCollection.ToArray());
        }


        public void ClearTables()
        {
            string query = @" DELETE FROM [dbo].[TransactionType] ";
            IList<SqlParameter> sqlParameterCollection = new List<SqlParameter>();
            SqlService.ExecuteNonQuery(query, sqlParameterCollection.ToArray());
        }
        public int Save(ITransactionType transactionType)
        {
            string query = @"INSERT INTO [dbo].[TransactionType]
         
           ([Name]
           ,[Description]
           ,[Color]
           ,[Income])
            OUTPUT INSERTED.Id
            VALUES
           ( @Name
           , @Description
           , @Color
           , @Income)";

            if (transactionType.Id != 0)
            {
                query = @"UPDATE [dbo].[TransactionType]
                   SET [Name] = @Name
                      ,[Description] = @Description
                      ,[Color] = @Color
                      ,[Income] = @Income
                 WHERE Id = @Id";

            }

            IList<SqlParameter> sqlParameterCollection = new List<SqlParameter>();

            sqlParameterCollection.Add(new SqlParameter("@Income", transactionType.Income));

            if (string.IsNullOrWhiteSpace(transactionType.Description))
            {
                sqlParameterCollection.Add(new SqlParameter("@Description", DBNull.Value));
            }
            else
            {
                sqlParameterCollection.Add(new SqlParameter("@Description", transactionType.Description));
            }



            if (string.IsNullOrWhiteSpace(transactionType.Name))
            {
                sqlParameterCollection.Add(new SqlParameter("@Name", DBNull.Value));
            }
            else
            {
                sqlParameterCollection.Add(new SqlParameter("@Name", transactionType.Name));
            }

            if (string.IsNullOrWhiteSpace(transactionType.Color))
            {
                sqlParameterCollection.Add(new SqlParameter("@Color", DBNull.Value));
            }
            else
            {
                sqlParameterCollection.Add(new SqlParameter("@Color", transactionType.Color));
            }

            if (transactionType.Id != 0)
            {
                sqlParameterCollection.Add(new SqlParameter("@Id", transactionType.Id));
                SqlService.ExecuteNonQuery(query, sqlParameterCollection.ToArray());
                return transactionType.Id;
            }
            else
            {
                return SqlService.ExecuteScalar(query, sqlParameterCollection.ToArray());
            }
        }

        #region Async

        public Task<int> SaveAsync(ITransactionType transactionType)
        {
            return Task.Factory.StartNew<int>(() => 
            {
                return Save(transactionType);
            });
        }

        public Task<ITransactionType> GetTransactionTypeByIdAsync(int id)
        {
            return Task.Factory.StartNew<ITransactionType>(() => 
            {
                return GetTransactionTypeById(id);
            });
        }

        public Task<IList<ITransactionType>> GetTransactionTypesAsync()
        {
            return Task.Factory.StartNew<IList<ITransactionType>>(() =>
            {
                return GetTransactionTypes();
            });
        }

        public Task<IList<ITransactionType>> GetTransactionTypesAsync(ITransactionTypeFilter filter)
        {
            return Task.Factory.StartNew<IList<ITransactionType>>(() =>
            {
                return GetTransactionTypes(filter);
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
