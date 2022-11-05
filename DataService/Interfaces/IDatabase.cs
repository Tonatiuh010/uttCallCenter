using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D = DataService.Services.DataServiceDelegates;
using DataService.MySQL;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace DataService.Interfaces
{
    public interface IDatabase
    {
        // Delegates
        public delegate void TransactionCallback();
        public delegate void ReaderAction(IDataReader reader);

        // Properties
        public static D.DataException? OnException { get; set; }
        public IDbConnection Connection { get; set; }
        public string? ConnectionString { get; set; }

        // Methods

        public void OpenConnection();

        public void CreateConnection();

        public void CloseConnection();

        public IDbCommand CreateCommand(string cmdText, CommandType type);

        public IDataParameter CreateParameter(string name, object? value, DbType type, bool isNullable = true);

        public IDataParameter CreateParameterOut(string name, DbType type);

        public static void NonQueryBlock(IDbCommand cmd, Action action)
        {
            cmd.ExecuteNonQuery();
            action();
        }

        public static void TransactionBlock(
           IDatabase db,
           TransactionCallback action,
           D.DataException onException,
           Action? onProcess = null
        )
        {
            IDbConnection conn = db.Connection;
            bool isTxnSuccess;

            if (db.Connection.State == ConnectionState.Closed)
            {
                db.OpenConnection();
            }

            try
            {
                action();
                isTxnSuccess = true;
            }
            catch (Exception e)
            {
                onException(e);
                isTxnSuccess = false;
            }

            if (isTxnSuccess)
                onProcess?.Invoke();

            db.Connection.Dispose();
            db.Connection.Close();
        }

    }
}
