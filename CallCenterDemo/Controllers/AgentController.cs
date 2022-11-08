using Classes;
using Engine.BL;
using Engine.BO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Security;
using System.Text.Json.Nodes;

namespace CallCenterDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : CustomController
    {
        private readonly CallCenterBL bl = new();

        [HttpGet]
        public Result GetAgents() => RequestResponse(() => bl.GetAgents());

        [HttpGet("{id:int}")]
        public Result GetAgent(int id) => RequestResponse(() => bl.GetAgent(id));

        [HttpGet("stations")]
        public Result GetStations() => RequestResponse(() => bl.GetStations());

        [HttpPost]
        public object SetAgent(dynamic obj) => RequestResponse(() =>
        {
            JsonObject jObj = JsonObject.Parse(obj.ToString());

            var agentId = ParseProperty<int>.GetValue("agentId", jObj, OnMissingProperty);

            var agentPin = ParseProperty<int>.GetValue("agentPin", jObj, OnMissingProperty);

            var stationId = ParseProperty<int>.GetValue("stationId", jObj, OnMissingProperty);

            return bl.SetAgent(agentId, agentPin, stationId);
        });
    }
}
