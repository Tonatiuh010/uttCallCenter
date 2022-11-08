using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebRequest = DataService.WebClient.WebRequest;
using DataService.WebClient;
using Engine.BO.CallCenter;
using System.Text.Json.Nodes;
using System.Net.Http.Json;
using Engine.BO;
using System.Text.Json;
using System.Reflection.Metadata.Ecma335;
using DataService.Services;
using Ubiety.Dns.Core;

namespace DemonProcess
{
    public class CallAPI
    {
        private static string URL = "https://localhost:7060/api/";

        private static WebRequest Client = new(URL);

        public static async Task<List<Agent>> GetAgents()
        {
            List<Agent> agents = new ();
            var result = await GetRequest("agent");           
            
            foreach(JsonObject? obj in ParseResult(result?.Data))
            {
                if (obj != null)
                    agents.Add(MapAgent(obj));
            }
            
            return agents;
        }

        public static async Task<List<Call>> GetCalls()
        {
            List<Call> calls = new ();
            var result = await GetRequest("call");

            foreach (JsonObject? obj in ParseResult(result?.Data))
            {
                if (obj != null)
                    calls.Add(MapCall(obj));
            }

            return calls;
        }

        public static async Task<List<Station>> GetStations()
        {
            List<Station> stations = new ();
            var result = await GetRequest("agent/stations");

            foreach (JsonObject? obj in ParseResult(result?.Data))
            {
                if (obj != null)
                    stations.Add(MapStation(obj));
            }

            return stations;
        }

        public static async Task<Result?> SetEndCall(int callId, StatusCallEnd status)
        {
            var data = new JsonObject() {
                ["callId"]= callId,
                ["statusEndId"] = (int)status
            };

            var result = await PostRequest("call/end", data.ToJsonString());

            return result;
        }

        public static async Task<Result?> SetCall(string phone)
        {
            var data = new JsonObject()
            {
                ["phone"] = phone
            };

            var result = await PostRequest("call", data.ToJsonString());

            return result;
        }

        public static async Task<Result> SetAgentLogin(Agent agent, int stationId)
        {
            var data = new JsonObject()
            {
                ["agentId"] = agent.Id,
                ["agentPin"] = agent.Pin,
                ["stationId"] = stationId
            };

            var result = await PostRequest("agent", data.ToJsonString());
            
            return result;
        }

        private static async Task<Result?> PostRequest(string route, string data)
        {
            var response = await Client.PostRequest(
                new RequestProperties(
                    route,
                    HttpMethod.Post,
                    new HttpContentService(data) 
                )
            );

            return await response.Content.ReadFromJsonAsync<Result>();            
        }

        private static async Task<Result?> GetRequest(string route)
        {
            var response = await Client.GetRequest(new RequestProperties(route, HttpMethod.Get));
            return await response.Content.ReadFromJsonAsync<Result>();
        }

        private static JsonArray ParseResult(object? data)
        {
            JsonArray array = new JsonArray();

            if (data != null)
            {
                var jsonStr = data.ToString();

                if (!string.IsNullOrEmpty(jsonStr))
                {
                    var raw = JsonNode.Parse(jsonStr);

                    if (raw != null)
                    {
                        array = raw.AsArray();
                    }
                }                
            }

            return array;
        }

        private static Agent MapAgent(JsonObject obj)
        {
            Agent model = new Agent();

            try
            {
                model.Name = obj["name"].ToString();
                model.Id = int.Parse(obj["id"].ToString());
                model.Photo = obj["photo"].ToString();
                model.Pin = int.Parse(obj["pin"].ToString());
            } catch
            {
                
            }
            
            return model;
        }

        private static Call MapCall(JsonObject obj)
        {
            Call model = new Call();

            try
            {
                model.Id = int.Parse(obj["id"].ToString());
                model.PhoneNumber = obj["phoneNumber"].ToString();
                model.Status = GetStatusCall(obj["status"].ToString());
                model.StatusEnd = GetStatusCallEnd(obj["statusEnd"].ToString());
                model.Received = ParseDateTime(obj["received"].ToString());
                model.Ended = ParseDateTime(obj["ended"].ToString());
                model.Answered = ParseDateTime(obj["answered"].ToString());
            }
            catch
            {

            }

            return model;
        }

        private static Station MapStation(JsonObject obj)
        {
            Station model = new Station();

            try
            {
                model.IPAddress = obj["ipAddress"].ToString();
                model.Id = int.Parse(obj["id"].ToString());
                model.IsActive = bool.Parse(obj["isActive"].ToString());
                model.Row = int.Parse( obj["row"].ToString() );
                model.Desk = int.Parse(obj["desk"].ToString());
            }
            catch
            {

            }

            return model;
        }

        private static DateTime ParseDateTime(string dateStr)
        {
            try
            {
                return DateTime.Parse(dateStr);
            } catch
            {
                return DateTime.Now;
            }
        }

        private static StatusCallEnd GetStatusCallEnd(string? status)
        {
            switch(status)
            {
                case "Customer_Ended":
                    return StatusCallEnd.Customer_Ended;
                case "Transfered":
                    return StatusCallEnd.Transfered;
                case "Agent_Ended":
                    return StatusCallEnd.Agent_Ended;
                case "Call_Dropped":
                    return StatusCallEnd.Call_Dropped;
                default:
                    throw new Exception();
            }
        }

        private static StatusCall GetStatusCall(string? status)
        {
            switch (status)
            {
                case "Answered":
                    return StatusCall.Answered;
                case "Queue":
                    return StatusCall.Queue;
                case "Ended":
                    return StatusCall.Ended;                
                default:
                    throw new Exception();
            }
        }

        //private static StatusSession GetStatusSession(string? status)
        //{
        //    switch (status)
        //    {
        //        case "Answered":
        //            return StatusSession.;
        //        case "Queue":
        //            return StatusCall.Queue;
        //        case "Ended":
        //            return StatusCall.Ended;
        //        default:
        //            throw new Exception();
        //    }
        //}

    }
}
