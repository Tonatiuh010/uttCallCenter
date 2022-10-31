using DataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D = DataService.Services.DataServiceDelegates;
using static DataService.Interfaces.IDatabase;

namespace DataService
{
    public abstract class Database : IDatabase
    {
        public abstract IDbConnection Connection { get; set; }

        public abstract string? ConnectionString { get; set; }       

        public abstract void CloseConnection();

        public abstract IDbCommand CreateCommand(string cmdText, CommandType type);

        public abstract void CreateConnection();

        public abstract IDataParameter CreateParameter(string name, object? value, DbType type, bool isNullable = true);

        public abstract IDataParameter CreateParameterOut(string name, DbType type);

        public abstract void OpenConnection();

        public void TransactionBlock(TransactionCallback action, D.DataException onException, Action? onProcess = null)
            => IDatabase.TransactionBlock(this, action, onException, onProcess);
    }
}
