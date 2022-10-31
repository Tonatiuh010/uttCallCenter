using Engine.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.BO.CallCenter;

namespace Engine.BL
{
    public class CallCenterBL
    {
        private static CallCenterDAL Dal => CallCenterDAL.Instance;

        public List<Call> GetCalls(int? id = null)
        {
            var calls = Dal.GetCalls(id);

            return calls;
        }
        public Call? GetCall(int id) => GetCalls(id).FirstOrDefault();

        public List<Agent> GetAgents(int? id = null) => Dal.GetAgents(id);
        public Agent? GetAgent(int id) => GetAgents(id).FirstOrDefault();

        public List<Session> GetSessions(int? id = null)
        {
            var sessions = Dal.GetSessions(id);

            foreach(Session s in sessions) 
                CompleteSession(s);            

            return sessions;
        }
        public Session? GetSession(int id) => GetSessions(id).FirstOrDefault();

        public List<SessionLog> GetSessionLogs(int? id = null)
        {
            var logs = Dal.GetSessionLogs(id);

            foreach (SessionLog l in logs)
                CompleteSessionLog(l);

            return logs;
        }
        public SessionLog? GetSessionLog(int id) => GetSessionLogs(id).FirstOrDefault();


        public List<Station> GetStations(int? id = null) => Dal.GetStations(id);
        public Station? GetStation(int id) => GetStations(id).FirstOrDefault();


        private void CompleteSession(Session session)
        {            

            if(session.Agent != null && !session.Agent.IsValid())            
                session.Agent = GetAgent((int)session.Agent.Id);            

            if(session.CurrentCall != null && !session.CurrentCall.IsValid())            
                session.CurrentCall = GetCall((int)session.CurrentCall.Id);
            
        }

        private void CompleteSessionLog(SessionLog log)
        {
            if (log.Session != null && log.Session.IsValid())
                log.Session = GetSession((int)log.Session.Id);            
        }

    }
}
