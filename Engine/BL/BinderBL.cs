using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Services.DataCollector;
using Engine.BL;
using Engine.DAL;
using Engine.Services;
using DataService.MySQL;
using DataService.Services;
using DataService.Interfaces;
using DataService;

namespace Engine.BL
{
    public static class BinderBL
    {
        public static void Start()
        {
            BaseDAL<MySqlDataBase>.OnDALError = ExceptionManager.CallbackException;
        }

        public static void SetDalError(DataServiceDelegates.DataException onConnectionError)
        {
            IDatabase.OnException = onConnectionError;            
        }
    }
}
