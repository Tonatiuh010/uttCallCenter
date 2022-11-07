using System;
using System.Timers;
using System.Threading;
using Engine.BO.CallCenter;
using Engine.BO;
using System.Net;

namespace DemonProcess
{
    public class LoginAgent
    {
        //tengo que obtener de algun lado el procedure spLoginAgent
        //despues de obtener el proceduer lo tengo que ejecutar de alguna manera
        //tengo que obtener agentes aleatorios disponibles cada cierto tiempo
        private static System.Timers.Timer aTimer;
        private static List<Agent> agents;
        private static List<Call> calls;
        private static List<String> phones;

        public static void Main()
        {
            aTimer = new System.Timers.Timer();
            aTimer.Interval = (2000);

            aTimer.Elapsed += TimerExample;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

            Console.ReadLine();

            Console.WriteLine("Initializing MainThread");
            //MainThread();
            Console.ReadLine();
        }
        public static void TimerExample(Object source, System.Timers.ElapsedEventArgs e)
        {
            Thread t = new Thread(AgentThread);
            t.Start();
        }

        public static void MainThread()
        {
            Thread t = new Thread(AgentThread);
            t.Start();

            Thread t2 = new Thread(CallsThread);
            t2.Start();
        }
        public static void AgentThread()
        {
            Console.WriteLine("Agent Loged");
            //traer el spLoginAgent
        }

        public static void CallsThread()
        {
            Console.WriteLine("Call ended");
            //traer las calls
        }
    }
}