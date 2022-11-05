using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.SQLServer
{
    public class SQLServerDataBase : Database
    {
        private string connectionString;
        private SqlCommand command;

        public override IDbConnection Connection { get; set; } = new SqlConnection();
        public override string? ConnectionString { get; set; }

        public override void OpenConnection() => Connection.Open();

        public override void CreateConnection(SqlCommand command)
        {
            DataTable table = new DataTable();
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                command.Connection = connection;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }
            catch (SqlException ex)
            {
                AppendErrorLog(ex.Message); 
            }
            catch (Exception ex)
            {
                AppendErrorLog(ex.Message); 
            }
           
        }

        public IDbCommand CreateCommand(string cmdText, IDbConnection conn, CommandType type) => new SqlCommand(cmdText, (SqlConnection)conn)
        {
            CommandType = type
        };

        public override void CloseConnection()
        {
            if (Connection != null && Connection.State == ConnectionState.Open)
            {
                Connection.Close();
                Connection.Dispose();
            }
        }

        public override IDbCommand CreateCommand(string cmdText, CommandType type) => CreateCommand(cmdText, Connection, type);


        private static void AppendErrorLog(string message)
        {
            string folder = string.Empty; 
            if (!System.IO.Directory.Exists(folder))
            {
                System.IO.Directory.CreateDirectory(folder);
            }
            string filePath = folder + "errors.txt";
            string line = DateTime.Now.ToString() + "\t" + message + "\n";
            if (System.IO.File.Exists(filePath))
                System.IO.File.AppendAllText(filePath, line);
            else
                System.IO.File.WriteAllText(filePath, line);
        }

        public override IDataParameter CreateParameter(string name, object? value, DbType type, bool isNullable = true)
        {
            throw new NotImplementedException();
        }

        public override IDataParameter CreateParameterOut(string name, DbType type)
        {
            throw new NotImplementedException();
        }
    }
}
