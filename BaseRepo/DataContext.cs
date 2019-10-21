using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using static Dapper.SqlMapper;

namespace BaseRepo
{
    public class DataContext : IDataContext
    {
        #region Fields
        private readonly IProvider _provider = new SqlServerProvider();
        private readonly IDbConnection _connection;
        private readonly string _connectionstring;
        #endregion

        #region Ctor
        public DataContext(string connectionstring)
        {

            Check.IsEmpty(connectionstring);
            _connectionstring = connectionstring;
            // var connectionString = ConfigurationManager.ConnectionStrings[connectionName];

            // _provider = ProviderHelper.GetProvider(connectionString.ProviderName);
            //_connection = _provider.CreateConnection(connectionString.ConnectionString);             
            _connection = _provider.CreateConnection(connectionstring);

        }
        #endregion
          
        #region Sync Methods
        /// <summary>
        /// Insert an item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="transaction"></param>
        public virtual void Insert<T>(T item, IDbTransaction transaction = null, int? timeout = 30) where T : class
        {
            try
            {
                Check.IsNull(item);
                //  _connection.ExecuteScalar<int>(commandText, item, transaction);
                _connection.Insert<T>(item, transaction, timeout);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Insert item collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="transaction"></param>
        public virtual int InsertBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null, int? timeout = null) where T : class
        {
            Check.IsNullOrEmpty(items);
            items.ToList().ForEach(z =>
            {
                _connection.Insert<T>(z, transaction, timeout);
            });
            return 1;
        }
         
        /// <summary>
        /// Find an item by lambda expressions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual T Find<T>(Expression<Func<T, bool>> expression) where T : class
        {
            string commandText = _provider.SelectFirstQuery<T>(expression, typeof(T).Name);
            var parameters = ExpressionHelper.GetWhereParemeters(expression);

            //execute first query
            return _connection.QueryFirst<T>(commandText, parameters);
        }

        /// <summary>
        /// Find all items by lambda expressions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> FindAll<T>(Expression<Func<T, bool>> expression) where T : class
        {
            IEnumerable<T> items = new List<T>();
            string commandText = _provider.SelectQuery<T>(expression, typeof(T).Name);
            var parameters = expression is null ? null : ExpressionHelper.GetWhereParemeters(expression);

            //execute query
            return _connection.Query<T>(commandText, parameters);
        }

        /// <summary>
        /// Execute command with query
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        public virtual int Execute(string commandText, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null)
        {
            Check.IsNullOrEmpty(commandText);

            //execute
            return _connection.Execute(commandText, parameters, transaction, commandTimeout: timeout);
        }

        /// <summary>
        /// Execute reader with query
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual IDataReader ExecuteReader(string commandText, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null)
        {
            Check.IsNullOrEmpty(commandText);

            //execute reader
            return _connection.ExecuteReader(commandText, parameters, transaction, commandTimeout: timeout);
        }

        /// <summary>
        /// Execute scalar with query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual T ExecuteScalar<T>(string commandText, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null) where T : class
        {
            Check.IsNullOrEmpty(commandText);

            //execute scalar
            return _connection.ExecuteScalar<T>(commandText, parameters, transaction);
        }

        /// <summary>
        /// Execute with stored procedure by name
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        public virtual int ExecuteProcedure(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null)
        {
            Check.IsNullOrEmpty(storedProcedureName);

            //execute
            return _connection.Execute(sql: storedProcedureName,
                param: parameters,
                transaction: transaction,
                 commandTimeout: timeout,
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Execute reader with stored procedure by name
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual IDataReader ExecuteReaderProcedure(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null)
        {
            Check.IsNullOrEmpty(storedProcedureName);

            //execute reader
            return _connection.ExecuteReader(sql: storedProcedureName,
                param: parameters,
                transaction: transaction,
                 commandTimeout: timeout,
                commandType: CommandType.StoredProcedure);
        }

        public virtual bool BulkInsert(DataTable table, string tableName, int? timeout = null, IDbTransaction transaction = null)
        {
            using (var sqlCon = new SqlConnection(_connectionstring))
            {
                sqlCon.Open();
                try
                {
                    using (var bulkCopy = new SqlBulkCopy(sqlCon))
                    {
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            DataColumn col = table.Columns[i];
                            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        }
                        bulkCopy.BulkCopyTimeout = 100000;
                        bulkCopy.DestinationTableName = tableName;

                        try
                        {
                            bulkCopy.WriteToServer(table);
                        }

                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                return true;
            }

        }
        /// <summary>
        /// Execute scalar with stored procedure by name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual T ExecuteScalarProcedure<T>(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null) where T : class
        {
            Check.IsNullOrEmpty(storedProcedureName);

            return _connection.ExecuteScalar<T>(sql: storedProcedureName,
          param: parameters,
          transaction: transaction,
           commandTimeout: timeout,
          commandType: CommandType.StoredProcedure);

        }

        /// <summary>
        ///Execute Sp and returns the reader & Dont use using in this method
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual SqlMapper.GridReader ExecuteProcedureMultipleResult(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null)
        {
            Check.IsNullOrEmpty(storedProcedureName);
            return _connection.QueryMultiple(sql: storedProcedureName,
           param: parameters,
           transaction: transaction,
            commandTimeout: timeout,
           commandType: CommandType.StoredProcedure);
        }
        /// <summary>
        /// /Execute Sp and returns the result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> ExecuteProcedureSingleResult<T>(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null)
        {
            Check.IsNullOrEmpty(storedProcedureName);

            return _connection.Query<T>(sql: storedProcedureName,
              param: parameters,
              commandTimeout: timeout,
              transaction: transaction,
              commandType: CommandType.StoredProcedure);


        }
        #endregion


        #region Context Management
        /// <summary>
        /// Begin transcation scope
        /// </summary>
        /// <returns></returns>
        public virtual IDbTransaction BeginTransaction()
        {
            OpenConnection();

            return _connection.BeginTransaction();
        }

        /// <summary>
        /// Open connection with whether open or close
        /// </summary>
        public virtual void OpenConnection()
        {
            if (_connection != null &&
                _connection.State != ConnectionState.Open &&
                _connection.State != ConnectionState.Connecting)
                _connection.Open();
        }

        /// <summary>
        /// Gets the current connection
        /// </summary>
        public virtual IDbConnection Connection
        {
            get
            {
                OpenConnection();

                return _connection;
            }
        }

        /// <summary>
        /// Dispose the current connection
        /// </summary>
        public virtual void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
        }

        public bool Delete<T>(T item, IDbTransaction transaction = null, int? timeout = null) where T : class
        {
            return _connection.Delete<T>(item, transaction, timeout);
        }

        public bool DeleteBulk<T>(IEnumerable<T> item, IDbTransaction transaction = null, int? timeout = null) where T : class
        {
            return _connection.DeleteAll<T>();

        }

        public bool Update<T>(T item, IDbTransaction transaction = null, int? timeout = null) where T : class
        {
            return _connection.Update<T>(item, transaction, timeout);

        }
        public DataTable ExecuteExportToDataTable(string storedProcedureName, SqlParameter[] parameters = null, int? timeout = null, IDbTransaction transaction = null) 
        {              
            DataTable dt = new DataTable();
            using (SqlConnection c = new SqlConnection(_connectionstring))
            using (SqlDataAdapter sda = new SqlDataAdapter(storedProcedureName, c))
            {
                sda.SelectCommand.CommandType = CommandType.StoredProcedure;           
                sda.SelectCommand.Parameters.AddRange(parameters);
                sda.Fill(dt);
            }
            return dt;
        }
        public IEnumerable<T> FindAll<T>() where T : class
        {
            return _connection.GetAll<T>();
        }

        public List<Dictionary<string, string>> ExecuteProcWithUnknownModal(string storedProcedureName, DynamicParameters parameters = null, int? timeout = null, IDbTransaction transaction = null)
        {
            DataTable tbl = new DataTable();
              
            IDataReader reader = _connection.ExecuteReader(sql: storedProcedureName,
            param: parameters,
            commandTimeout: timeout,
            transaction: transaction,
            commandType: CommandType.StoredProcedure);
           // tbl.Load(reader);
            Dictionary<string, dynamic> test = new Dictionary<string, dynamic>();
              
            List<List<KeyValuePair<String, string>>> tableLsit = new List<List<KeyValuePair<string, string>>>();
            while (reader.Read())
            {
               
                    List<KeyValuePair<String, string>> row = new List<KeyValuePair<string, string>>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(new KeyValuePair<string, string>(reader.GetName(i), Convert.ToString(reader[i])));
                    }
                    tableLsit.Add(row);
                 
            }

            //foreach (DataRow dr in tbl.Rows)
            //{
            //    List<KeyValuePair<String, string>> row = new List<KeyValuePair<string, string>>();
            //    for (int i = 0; i < tbl.Columns.Count; i++)
            //    {
            //        row.Add(new KeyValuePair<string, string>(tbl.Columns[i].ColumnName, Convert.ToString(dr[tbl.Columns[i].ColumnName])));
            //    }
            //    tableLsit.Add(row);
            //}

            return tableLsit.Select(c => c.ToDictionary(b => b.Key, b => b.Value)).ToList();

             


        }
        #endregion
    }
}
