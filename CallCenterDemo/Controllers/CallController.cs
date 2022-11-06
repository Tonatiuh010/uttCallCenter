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
            JsonObject jObj = JsonObject.Parse(obj.ToString());

            var phone = ParseProperty<string>.GetValue("phone", jObj, OnMissingProperty);

            if (string.IsNullOrEmpty(phone))
                throw new Exception("Propiedad Phone no se encuentra en el request...");

            return bl.SetCall(phone);
        });

        [HttpPost("/end")]
        public Result EndCall() => RequestResponse(() =>
        {
            return C.OK;
        });

    }
}