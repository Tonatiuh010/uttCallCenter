using DataService.MySQL;
using Engine.Constants;
using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Services;
using Engine.BO.CallCenter;
using System.Data;
using System.Linq.Expressions;
using DataService.SQLServer;

namespace Engine.DAL
{
    public class CallCenterDAL : BaseDAL<SQLServerDataBase>
    {
        public delegate void DALCallback(CallCenterDAL dal);
        private static ConnectionString? _ConnectionString => ConnectionString.InstanceByName(C.CALL_CENTER_DB);
        public static CallCenterDAL Instance => new();
        private CallCenterDAL() : base(_ConnectionString) { }

        public List<Agent> GetAgents(int ? id)
        {
            List<Agent> model = new ();

            DB.TransactionBlock(() =>
            {
                using var cmd = DB.CreateCommand($"SELECT * FROM AGENTS{ (id != null ? $" WHERE ID = {id}" : "") }", CommandType.Text);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["id"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["name"]),
                        Photo = Validate.getDefaultStringIfDBNull(reader["photo"]),
                        Pin = Validate.getDefaultIntIfDBNull(reader["pin"])
                    });
                }
                reader.Close();
            }, (ex, msg) => SetExceptionResult("CallCenterDAL.GetAgents", msg, ex));


            return model;
        }

        public List<Call> GetCalls(int? id)
        {
            List<Call> model = new ();

            DB.TransactionBlock(() =>
            {
                using var cmd = DB.CreateCommand($"SELECT * FROM CALLS{(id != null ? $" WHERE ID = {id}" : "")}", CommandType.Text);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["id"]),
                        Received = Validate.getDefaultDateIfDBNull(reader["datetimeReceived"]),
                        Answered = Validate.getDefaultDateIfDBNull(reader["datetimeAnswered"]),
                        Ended = Validate.getDefaultDateIfDBNull(reader["datetimeEnded"]),
                        PhoneNumber = Validate.getDefaultStringIfDBNull(reader["phoneNumber"]),
                        Status = (StatusCall)Validate.getDefaultIntIfDBNull(reader["idStatus"]),
                        StatusEnd = (StatusCallEnd)Validate.getDefaultIntIfDBNull(reader["idStatusEnd"]),
                        //Session = new Session()
                        //{
                        //    Id = Validate.getDefaultIntIfDBNull(reader["idSession"])
                        //}
                    });
                }
                reader.Close();
            }, (ex, msg) => SetExceptionResult("CallCenterDAL.GetCalls", msg, ex));

            return model;
        }

        public List<Session> GetSessions(int? id)
        {
            List<Session> model = new ();

            DB.TransactionBlock(() =>
            {
                using var cmd = DB.CreateCommand($"SELECT * FROM SESSIONS{(id != null ? $" WHERE ID = {id}" : "")}", CommandType.Text);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["id"]),
                        LoginDate = Validate.getDefaultDateIfDBNull(reader["dateTimeLogin"]),
                        LogoutDate = Validate.getDefaultDateIfDBNull(reader["dateTimeLogout"]),
                        Agent = new ()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["idAgent"]),
                        },
                        CurrentCall = new ()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["idCurrentCall"])
                        },
                        IsActive = Validate.getDefaultBoolIfDBNull(reader["active"]),                       
                    });
                }
                reader.Close();
            }, (ex, msg) => SetExceptionResult("CallCenterDAL.GetSessions", msg, ex));


            return model;
        }

        public List<SessionLog> GetSessionLogs(int? id)
        {
            List<SessionLog> model = new ();

            DB.TransactionBlock(() =>
            {
                using var cmd = DB.CreateCommand($"SELECT * FROM SESSIONLOG{(id != null ? $" WHERE ID = {id}" : "")}", CommandType.Text);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["id"]),
                        Session = new ()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["idSession"])
                        },
                        Start = Validate.getDefaultDateIfDBNull(reader["dateTimeStart"]),
                        End = Validate.getDefaultDateIfDBNull(reader["dateTimeEnd"]),
                    });
                }
                reader.Close();
            }, (ex, msg) => SetExceptionResult("CallCenterDAL.GetSessionLogs", msg, ex));


            return model;
        }

        public List<Station> GetStations(int? id)
        {
            List<Station> model = new ();

            DB.TransactionBlock(() =>
            {
                using var cmd = DB.CreateCommand($"SELECT * FROM STATIONS{(id != null ? $" WHERE ID = {id}" : "")}", CommandType.Text);                
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["id"]),
                        Row = Validate.getDefaultIntIfDBNull(reader["rowNumber"]),
                        Desk = Validate.getDefaultIntIfDBNull(reader["deskNumber"]),
                        IPAddress = Validate.getDefaultStringIfDBNull(reader["ipAddress"]),
                        IsActive = Validate.getDefaultBoolIfDBNull(reader["active"])                        
                    });
                }
                reader.Close();
            }, (ex, msg) => SetExceptionResult("CallCenterDAL.GetStations", msg, ex));

            return model;
        }

        public int SetCall(string phone)
        {
            int status = 0;

            DB.TransactionBlock(() => {
                using var cmd = DB.CreateCommand(SQL.spReceiveCall, CommandType.StoredProcedure);

                IDataParameter result = DB.CreateParameterOut("status", DbType.Int32);

                cmd.Parameters.Add(DB.CreateParameter("phoneNumber", phone, DbType.String));
                cmd.Parameters.Add(result);

                cmd.ExecuteNonQuery();                

                if (result.Value != null)
                {
                    status = (int)result.Value;
                }

            }, (ex, msg) => SetExceptionResult("CallCenterDAL.SetCall", msg, ex));


            return status;
        }
    }
}
