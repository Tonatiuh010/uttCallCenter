using Classes;
using Engine.BL;
using Engine.BO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Security;

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

        [HttpPost]
        public object SetAgent() => null;
    }
}
