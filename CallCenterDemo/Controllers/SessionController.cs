using Classes;
using Engine.BL;
using Engine.BO;
using Engine.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Text.Json.Nodes;

namespace CallCenterDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : CustomController
    {

        private readonly CallCenterBL bl = new();

        [HttpGet]
        public Result Get() => RequestResponse(() => bl.GetSessions());

        [HttpGet("{id}")]
        public Result Get(int id) => RequestResponse(() => bl.GetSession(id));

        [HttpPost]
        public Result Post(dynamic obj) => RequestResponse(() =>
        {
            var jObj = JsonObject.Parse(obj.ToString());

            return C.OK;
        });
    }
}
