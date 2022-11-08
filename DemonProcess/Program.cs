using System;
using System.Timers;
using T = System.Timers.Timer;
using T_Event = System.Timers.ElapsedEventHandler;
using System.Threading;
using Engine.BO.CallCenter;
using Engine.Constants;
using Engine.BO;
using System.Net;
using Engine.DAL;
using System.Diagnostics.CodeAnalysis;

namespace DemonProcess
{
    public static class Program
    {

        // Peak hours: 9 a.m.to 12 p.m.
        // Peak hours: 3 p.m.to 8 p.m.

        // Calls received: calls each 2 - 5 mins; Peak hour 1 - 3 mins
        // Call duration: 3 to 20 mins, avg 9        

        private static int AgentIndex = 0;        
        private static int StationIndex = 0;

        private static int CallMinRange = 2;
        private static int CallMaxRange = 5;

        // 10 seconds to log stuff
        private static int OnLoopDelay = 1000 * 10;

        private static T? AgentTimer;
        private static T? CallTimer;

        private static List<Agent> Agents = new();
        private static List<Call> Calls = new ();
        private static List<Station> Stations = new ();
        private static List<string> Phones = new ();

        private static Random Random = new Random();

        public async static Task Main()
        {
            await Start();
        }        
        
        public static async Task Start()
        {
            LogStuff(C.STARTUP, "Starting Call Center Deamon!...");

            await LoadData();

            AgentTimer = CreateTimer(
                MinutesToMillis(3),
                dt => CreateThread( async () => await AgentThread(dt) )
            );

            CallTimer = CreateTimer(
              GetCallIntervalMillis(),
              dt => CreateThread( async () => await CallThread(dt))
            );

            var timer2timers = CreateTimer(
                MinutesToMillis(1),
                async (dt) =>
                {
                    await LoadData();                    

                    if (IsPeakHour(dt))
                    {
                        CallMinRange = 1;
                        CallMaxRange = 3;

                        lock (CallTimer)
                        {
                            CallTimer.Stop();
                            CallTimer.Interval = GetCallIntervalMillis();
                            CallTimer.Start();
                        }

                        lock (AgentTimer)
                        {
                            AgentTimer.Stop();
                            AgentTimer.Interval = MinutesToMillis(2);
                            AgentTimer.Start();

                        }
                    }                    
                },
                false
            );

            Console.ReadLine();
        }

        private static async Task AgentThread(DateTime startTime)
        {            
            DateTime endTime = startTime.AddMinutes(20);
            Agent agent = NextAgent();
            Station station = NextStation();

            await CallAPI.SetAgentLogin(agent, (int)station.Id);

            LogStuff(C.AGENT_THREAD, $"Simulating Agent Login for {agent.Name}, ID ({agent.Id}). Started at {startTime:G}, will be ended at {endTime:G}");            

            while (endTime > DateTime.Now)
            {
                Thread.Sleep(OnLoopDelay);
                LogStuff(C.AGENT_THREAD, $"Agent Login {agent.Name}, ID ({agent.Id}). Keeps login!");
            }

            // End Call

            LogStuff(C.AGENT_THREAD, $"Agent Login {agent.Name}, ID ({agent.Id}). Has logout!");
        }

        private async static Task CallThread(DateTime startTime)
        {
            long durationMillis = GetCallDurationMillis();
            DateTime endTime = startTime.AddMilliseconds(durationMillis);
            string phone = Phones[Random.Next(Phones.Count - 1)];
            
            LogStuff(C.CALL_THREAD, $"Call Thread with Number: {phone}. Started at {startTime:G}, will be ended at {endTime:G}");

            var res = await CallAPI.SetCall(phone);
            int? callId = 0;

            while ( endTime > DateTime.Now )
            {
                callId = Calls.Find(x => x.PhoneNumber == phone)?.Id;
                Thread.Sleep(OnLoopDelay);                
                LogStuff(C.CALL_THREAD, $"Call Thread with Number: {phone}. Keeps on line!");
            }

            await CallAPI.SetEndCall((int)callId, StatusCallEnd.Customer_Ended);
            LogStuff(C.CALL_THREAD, $"Call Thread with Number: {phone}. Has been ended!");
        }

        private static Thread CreateThread(Action threadAction)
        {         
            Thread t = new (() => threadAction());
            t.Start();
            return t;
        }

        private static T CreateTimer(double time, Action<DateTime> onInterval, bool firstCall = true)
        {
            if(firstCall)
                onInterval(DateTime.Now);

            var t = new T(time);            
            t.Elapsed += (obj, e) => onInterval(e.SignalTime);
            t.AutoReset = true;
            t.Enabled = true;          
            return new T();
        }

        private static long GetCallDurationMillis()
        {
            var mins = GenerateLong(3, 20);
            return MinutesToMillis(mins);
        }

        private static long GetCallIntervalMillis() 
        {
            var mins = GenerateLong(CallMinRange, CallMaxRange);
            return MinutesToMillis(mins);
        }

        private static long GenerateLong(long minRange, long maxRange) => Random.NextInt64(minRange, maxRange);

        private static long MinutesToMillis(long mins) => mins * 60 * 1000;

        private static Agent NextAgent()
        {
            int index = AgentIndex;
            AgentIndex++;
            return Agents[index];
        }

        private static Station NextStation()
        {
            int index = StationIndex;
            StationIndex++;
            return Stations[index];
        }

        private static bool IsPeakHour(DateTime dt)
        {
            var today = DateTime.Today;

            var atNine = today.AddHours(9);
            var atTwelve = today.AddHours(12);

            var atFith = today.AddHours(15);
            var atEigth = today.AddHours(20);

            bool conditionPeakHour1 = dt >= atNine && dt <= atTwelve;
            bool conditionPeakHour2 = dt >= atFith && dt <= atEigth;

            return conditionPeakHour1 || conditionPeakHour2;
        }

        private static void LogStuff(string type, string msg)
        {
            DateTime now = DateTime.Now;
            Console.WriteLine($"[{now:G}]: ({type}) -> {msg}");
        }

        private async static Task LoadData()
        {
            LogStuff(C.LOAD, "Loading phone numbers...");
            Phones = GetPhones();
            LogStuff(C.LOAD, "Loading Agents...");
            Agents = await CallAPI.GetAgents();
            LogStuff(C.LOAD, "Loading Calls...");
            Calls = await CallAPI.GetCalls();
            LogStuff(C.LOAD, "Loading Stations...");
            Stations = await CallAPI.GetStations();
        }

        private static List<string> GetPhones() => new() {
             "6640000001",
             "6640000002",
             "6640000003",
             "6640000004",
             "6640000005",
             "6640000006",
             "6640000007",
             "6640000008",
             "6640000009",
             "6640000010",
             "6640000011",
             "6640000012",
             "6640000013",
             "6640000014",
             "6640000015"
        };

        // This solution is pure trash
        // meaningless code, just triying to improve as the dumbass "student" i am.
        // fuck u all!
    }
}