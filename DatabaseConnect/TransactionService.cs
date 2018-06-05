using Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DatabaseConnect
{
    class TransactionService : ITransactionService
    {
        public ITransaction GetTransactionById(int id)
        {
            return GetTransactions(new TransactionFilter() { Id = id }).FirstOrDefault();
        }

        public IList<ITransaction> GetTransactions(ITransactionFilter filter)
        {
            SqlQueryBuilder sqlQueryBuilder = new SqlQueryBuilder();
            sqlQueryBuilder.Select = " SELECT * ";
            sqlQueryBuilder.From = " FROM [dbo].[Transaction] ";
            if (filter.Id.HasValue)
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "Id = @Id", Param = new SqlParameter("@Id", filter.Id.Value) });
            }
            if (filter.CustomerId.HasValue)
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "CustomerId = @CustomerId", Param = new SqlParameter("@CustomerId", filter.CustomerId.Value) });
            }
            if (filter.TransactionTypeId.HasValue)
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "TransactionTypeId = @TransactionTypeId", Param = new SqlParameter("@TransactionTypeId", filter.TransactionTypeId.Value) });
            }
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "Name = @Name", Param = new SqlParameter("@Name", filter.Name) });
            }
            if (filter.Value.HasValue)
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "Value = @Value", Param = new SqlParameter("@Value", filter.Value.Value) });
            }
            if (filter.ValueMin.HasValue)
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "Value >= @ValueMin", Param = new SqlParameter("@ValueMin", filter.ValueMin.Value) });
            }
            if (filter.ValueMax.HasValue)
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "Value <= @ValueMax", Param = new SqlParameter("@ValueMax", filter.ValueMax.Value) });
            }
            if (filter.Date.HasValue)
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "Date = @Date", Param = new SqlParameter("@Date", filter.Date.Value) });
            }
            if (filter.DateMin.HasValue)
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "Date >= @DateMin", Param = new SqlParameter("@DateMin", filter.DateMin.Value) });
            }
            if (filter.DateMax.HasValue)
            {
                sqlQueryBuilder.Where.Add(new SqlWhere() { Where = "Date <= @DateMax", Param = new SqlParameter("@DateMax", filter.DateMax.Value) });
            }

            var table = SqlService.GetDataTable(sqlQueryBuilder);
            var myEnumerable = table.AsEnumerable();

            return (from item in myEnumerable select new Transaction {
                        Id = item.Field<int>("Id"),
                        Name = item.Field<string>("Name"),
                        Description = item.Field<string>("Description"),
                        Value = item.Field<decimal>("Value"),
                        TransactionTypeId = item.Field<int>("TransactionTypeId"),
                        CustomerId = item.Field<int?>("CustomerId"),
                        Date = item.Field<DateTime>("Date")
            }).ToList<ITransaction>();
        }

        public void Delete(int id)
        {
            string query = @" DELETE FROM [dbo].[Transaction] WHERE Id = @Id";
            IList<SqlParameter> sqlParameterCollection = new List<SqlParameter>();
            sqlParameterCollection.Add(new SqlParameter("@Id", id));
            SqlService.ExecuteNonQuery(query, sqlParameterCollection.ToArray());
        }

        public int Save(ITransaction transaction)
        {
            string query = @"INSERT INTO [dbo].[Transaction]
         
           ([Name]
           ,[Description]
           ,[Value]
           ,[TransactionTypeId]
           ,[CustomerId]
           ,[Date])
            OUTPUT INSERTED.Id
            VALUES
           ( @Name
           , @Description
           , @Value
           , @TransactionTypeId
           , @CustomerId
           , @Date )";

            if (transaction.Id != 0)
            {
                query = @"UPDATE [dbo].[TransactionType]
                   SET [Name] = @Name
                      ,[Description] = @Description
                      ,[Value] = @Value
                      ,[TransactionTypeId] = @TransactionTypeId
                      ,[CustomerId] = @CustomerId
                      ,[Date] = @Date
                 WHERE Id = @Id";
            }

            IList<SqlParameter> sqlParameterCollection = new List<SqlParameter>();

            if (string.IsNullOrWhiteSpace(transaction.Name))
            {
                sqlParameterCollection.Add(new SqlParameter("@Name", DBNull.Value));
            }
            else
            {
                sqlParameterCollection.Add(new SqlParameter("@Name", transaction.Name));
            }

            if (string.IsNullOrWhiteSpace(transaction.Description))
            {
                sqlParameterCollection.Add(new SqlParameter("@Description", DBNull.Value));
            }
            else
            {
                sqlParameterCollection.Add(new SqlParameter("@Description", transaction.Description));
            }

            sqlParameterCollection.Add(new SqlParameter("@Value", transaction.Value));
            sqlParameterCollection.Add(new SqlParameter("@TransactionTypeId", transaction.TransactionTypeId));
            sqlParameterCollection.Add(new SqlParameter("@Date", transaction.Date));

            if (transaction.CustomerId.HasValue)
            {
                sqlParameterCollection.Add(new SqlParameter("@CustomerId", transaction.CustomerId));
            }
            else
            {
                sqlParameterCollection.Add(new SqlParameter("@CustomerId", DBNull.Value));
            }

            if (transaction.Id != 0)
            {
                sqlParameterCollection.Add(new SqlParameter("@Id", transaction.Id));
                SqlService.ExecuteNonQuery(query, sqlParameterCollection.ToArray());
                return transaction.Id;
            }
            else
            {
                return SqlService.ExecuteScalar(query, sqlParameterCollection.ToArray());
            }

        }
    }
}
