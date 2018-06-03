using Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DatabaseConnect
{
    class TransactionTypeService : ITransactionTypeService
    {

        public ITransactionType GetTransactionTypeById(int id)
        {
            return GetTransactionTypes(new TransactionTypeFilter() { Id = id }).FirstOrDefault();
        }

        public List<ITransactionType> GetTransactionTypes(TransactionTypeFilter filter)
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



            return (from item in myEnumerable
                    select new TransactionType
                    {
                        Income = item.Field<bool>("Income"),
                        Color = item.Field<string>("Color"),
                        Name = item.Field<string>("Name"),
                        Description = item.Field<string>("Description"),
                        Id = item.Field<int>("Id")
                    }).ToList<ITransactionType>();
        }

        public void Delete(int id)
        {
            String query = @" DELETE FROM [dbo].[TransactionType]  WHERE Id = @Id";
            List<SqlParameter> sqlParameterCollection = new List<SqlParameter>();
            sqlParameterCollection.Add(new SqlParameter("@Id", id));
            SqlService.ExecuteNonQuery(query, sqlParameterCollection.ToArray());

        }


        public int Save(ITransactionType transactionType)
        {
            String query = @"INSERT INTO [dbo].[TransactionType]
         
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

            List<SqlParameter> sqlParameterCollection = new List<SqlParameter>();

            sqlParameterCollection.Add(new SqlParameter("@Income", transactionType.Income));

            if (string.IsNullOrWhiteSpace(transactionType.Description))
                sqlParameterCollection.Add(new SqlParameter("@Description", DBNull.Value));
            else
                sqlParameterCollection.Add(new SqlParameter("@Description", transactionType.Description));



            if (string.IsNullOrWhiteSpace(transactionType.Name))
                sqlParameterCollection.Add(new SqlParameter("@Name", DBNull.Value));
            else
                sqlParameterCollection.Add(new SqlParameter("@Name", transactionType.Name));

            if (string.IsNullOrWhiteSpace(transactionType.Color))
                sqlParameterCollection.Add(new SqlParameter("@Color", DBNull.Value));
            else
                sqlParameterCollection.Add(new SqlParameter("@Color", transactionType.Color));



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

    }
}
