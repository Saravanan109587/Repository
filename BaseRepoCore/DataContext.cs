﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
 
 
namespace BaseRepoCore
{
    public class DataContext : IDataContext
    {
        #region Fields
        private readonly IProvider _provider =new SqlServerProvider();
        private readonly IDbConnection _connection;
       
        #endregion

        #region Ctor
        public DataContext(string connectionstring)
        {
            Check.IsEmpty(connectionstring);
            // var connectionString = ConfigurationManager.ConnectionStrings[connectionName];

            // _provider = ProviderHelper.GetProvider(connectionString.ProviderName);
            //_connection = _provider.CreateConnection(connectionString.ConnectionString);             
             _connection = _provider.CreateConnection(connectionstring);
           
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Get parameter name and value from item collections to dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        protected virtual IDictionary<string, object> GetParameters<T>(IEnumerable<T> items)
        {
            Check.IsNullOrEmpty(items);
            try
            {
                var parameters = new Dictionary<string, object>();
                var entityArray = items.ToArray();
                var entityType = entityArray[0].GetType();
                for (int i = 0; i < entityArray.Length; i++)
                {
                    var properties = entityArray[i].GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    properties = properties.Where(x => x.Name != "Id").ToArray();

                    foreach (var property in properties)
                        parameters.Add(property.Name + (i + 1), entityType.GetProperty(property.Name).GetValue(entityArray[i], null));
                }

                return parameters;
            }
            catch (Exception e)
            {
               
                throw;
            }
           
        }
        #endregion

        #region Sync Methods
        /// <summary>
        /// Insert an item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="transaction"></param>
        public virtual void Insert<T>(T item, IDbTransaction transaction = null) where T : class
        {

            try
            { 
                     Check.IsNull(item); 
                   //_connection.Insert<T>();
                 
            }
            catch (Exception e)
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
        public virtual int InsertBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : class
        {
            Check.IsNullOrEmpty(items);

            string commandText = _provider.InsertBulkQuery(typeof(T).Name, items);
            var parameters = GetParameters(items);
             
                return _connection.Execute(commandText, parameters, transaction);
             
        }

        /// <summary>
        /// Update an item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="transaction"></param>
        public virtual int Update<T>(T item, IDbTransaction transaction = null) where T : class
        {
            Check.IsNull(item);

            string commandText = _provider.UpdateQuery(typeof(T).Name, item);

            //execute
            return _connection.Execute(commandText, item, transaction);
        }

        /// <summary>
        /// Update item collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="transaction"></param>
        public virtual int UpdateBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : class
        {
            Check.IsNullOrEmpty(items);

            string commandText = _provider.UpdateBulkQuery(typeof(T).Name, items);
            var parameters = GetParameters(items);
             
            return _connection.Execute(commandText, parameters, transaction);
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
            var parameters = expression is  null ?null: ExpressionHelper.GetWhereParemeters(expression);

            //execute query
            return _connection.Query<T>(commandText, parameters);
        }

        /// <summary>
        /// Execute command with query
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        public virtual int Execute(string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            Check.IsNullOrEmpty(commandText);

            //execute
            return _connection.Execute(commandText, parameters, transaction);
        }

        /// <summary>
        /// Execute reader with query
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual IDataReader ExecuteReader(string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            Check.IsNullOrEmpty(commandText);

            //execute reader
            return _connection.ExecuteReader(commandText, parameters, transaction);
        }

        /// <summary>
        /// Execute scalar with query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual T ExecuteScalar<T>(string commandText, object parameters = null, IDbTransaction transaction = null) where T : class
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
        public virtual int ExecuteProcedure(string storedProcedureName, object parameters = null, IDbTransaction transaction = null)
        {
            Check.IsNullOrEmpty(storedProcedureName);

            //execute
            return _connection.Execute(sql: storedProcedureName,
                param: parameters,
                transaction: transaction,
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Execute reader with stored procedure by name
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual IDataReader ExecuteReaderProcedure(string storedProcedureName, object parameters = null, IDbTransaction transaction = null)
        {
            Check.IsNullOrEmpty(storedProcedureName);

            //execute reader
            return _connection.ExecuteReader(sql: storedProcedureName,
                param: parameters,
                transaction: transaction,
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Execute scalar with stored procedure by name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual T ExecuteScalarProcedure<T>(string storedProcedureName, object parameters = null, IDbTransaction transaction = null) where T : class
        {
            Check.IsNullOrEmpty(storedProcedureName);
             
            
                return _connection.ExecuteScalar<T>(sql: storedProcedureName,
              param: parameters,
              transaction: transaction,
              commandType: CommandType.StoredProcedure);
           
        }

        /// <summary>
        ///Execute Sp and returns the reader & Dont use using in this method
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual SqlMapper.GridReader ExecuteProcedureMultipleResult(string storedProcedureName, DynamicParameters parameters = null, IDbTransaction transaction = null)  
        {
            Check.IsNullOrEmpty(storedProcedureName);
                return _connection.QueryMultiple(sql: storedProcedureName,
               param: parameters,
               transaction: transaction,
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
        public virtual IEnumerable<T> ExecuteProcedureSingleResult<T>(string storedProcedureName, DynamicParameters parameters = null, IDbTransaction transaction = null)  
        {
            Check.IsNullOrEmpty(storedProcedureName);

             return _connection.Query<T>(sql: storedProcedureName,
               param: parameters,
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
        #endregion
    }
}
