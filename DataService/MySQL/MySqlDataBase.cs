using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using DataService.Interfaces;
using MySql.Data;
using MySql.Data.MySqlClient;
using static DataService.Interfaces.IDatabase;
using D = DataService.Services.DataServiceDelegates;

namespace DataService.MySQL
{
    public class MySqlDataBase : Database
    {

        // Properties         
        public override IDbConnection Connection { get; set; } = new MySqlConnection();
        public override string? ConnectionString { get; set; }
        
        public override void OpenConnection() => Connection.Open();

        public override void CreateConnection() {

            try
            {

                if (string.IsNullOrEmpty(ConnectionString))
                {
                    throw new Exception("MySQL Connection string is empty");
                }

                var conn = new MySqlConnection(ConnectionString);
                conn.StateChange += OnStateChange;
                Connection = conn;

            }
            catch (Exception ex)
            {
                if (OnException != null)
                {
                    if (ex.GetType() == typeof(MySqlException))
                    {
                        MySqlException e = (MySqlException)ex;
                        switch (e.Number)
                        {
                            case 0:
                                OnException(ex, "Cannot connect to server.  Contact administrator");
                                break;
                            case 1045:
                                OnException(ex, "Invalid username/password, please try again");
                                break;
                        }
                    }
                    else
                    {
                        OnException(ex, "Exception creating connection...");
                    }
                }
            }           
        }

        public override void CloseConnection()
        {          
            if (Connection != null && Connection.State == ConnectionState.Open)
            {
                Connection.Close();
                Connection.Dispose();
            }            
        }

        public override IDbCommand CreateCommand(string cmdText, CommandType type) => CreateCommand(cmdText, Connection, type);

        private void OnStateChange(object obj, StateChangeEventArgs args)
        {
            if (args.CurrentState == ConnectionState.Closed)
            {
                
            }
        }
        
        public IDbCommand CreateCommand(string cmdText, IDbConnection conn, CommandType type) => new MySqlCommand (cmdText, (MySqlConnection)conn)
        {
            CommandType = type
        };

        public override IDataParameter CreateParameter(string name, object? value, DbType type, bool isNullable = true) => new MySqlParameter(name, value)
        {
            Direction = ParameterDirection.Input,            
            IsNullable = isNullable,
            DbType = type,
        };

        public override IDataParameter CreateParameterOut(string name, DbType type) => new MySqlParameter()
        {
            ParameterName = name,
            Direction = ParameterDirection.Output,
            DbType = type,
        };

    }
}