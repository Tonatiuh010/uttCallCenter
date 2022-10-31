using Microsoft.AspNetCore.Mvc;
using BaseAPI.Classes;
using Classes;
using Engine.BO;
using Engine.BL;
using System.Text.Json.Nodes;
using Engine.Constants;

namespace CallCenterDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CallController : CustomController
    {
        private readonly CallCenterBL bl = new();

        [HttpGet]
        public Result GetCalls() => RequestResponse(() => bl.GetCalls());

        [HttpGet("{id:int}")]
        public Result GetCall(int id) => RequestResponse(() => bl.GetCall(id));

        [HttpPost]
        public Result SetCall(dynamic obj) => RequestResponse(() => 
        {
            var jObj = JsonObject.Parse(obj.ToString());

            return C.OK;
        });

        [HttpPost("/end")]
        public Result EndCall() => RequestResponse(() =>
        {

            return C.OK;
        });

    }
}