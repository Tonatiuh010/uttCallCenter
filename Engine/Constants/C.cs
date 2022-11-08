using System;

namespace Engine.Constants {
    public class C {

        public const string GLOBAL_USER = "API_CTL";
        public const string PROCESS_USER = "CTL_DATASET";

        public const string OK = "OK";
        public const string ERROR = "ERROR";
        public const string COMPLETE = "COMPLETE";
        public const string PENDING = "PENDING";
        public const string ROLLABACK = "ROLLBACK";

        public const string STARTUP = "STARTUP";
        public const string LOAD = "LOAD";
        public const string AGENT_THREAD = "AGENT_THREAD";
        public const string CALL_THREAD = "CALL_THREAD";
        public const string SYSTEM = "SYSTEM";

        public const string ENABLED = "ENABLED";
        public const string DISABLED = "DISABLED";

        public const string EmployeeCsv = "employees.csv";
        public const string AccessCsv = "access.csv";        
        public const string JobsCsv = "jobs.csv";
        public const string DepartamentsCsv = "department.csv";
        public const string PositionsCsv = "positions.csv";

        /* Connection Strings */
        public const string ACCESS_DB = "DB_ACCESS";
        public const string HINT_DB = "DB_HINT";
        public const string CALL_CENTER_DB = "CALL_CENTER_DB";
    }
}