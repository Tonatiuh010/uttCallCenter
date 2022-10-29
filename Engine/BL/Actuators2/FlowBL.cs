using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.BO.FlowControl;
using Engine.DAL;

namespace Engine.BL.Actuators2
{
    public class FlowBL : BaseBL<FlowControlDAL>
    {
        public List<Flow> GetFlows(int? id = null, int? flowDetailId = null, string? name = null)
            => Dal.GetFlows(id, flowDetailId, name);

        public Flow? GetFlow(int id) => GetFlows(id).FirstOrDefault();

        public List<Step> GetSteps(int? id = null, int? parameterId = null, int? apiId = null, int? flowId = null)
            => Dal.GetSteps(id, parameterId, apiId, flowId);

        public Step? GetStep(int id) => GetSteps(id).FirstOrDefault();

        public Parameter? GetParameter(int id) 
            => GetSteps(parameterId: id)?
            .FirstOrDefault()?
            .Endpoint
            .Params
            .Find(x => x.Id == id);

        public List<API> GetAPIs() => new();

        public API? GetAPI() => new API();
    }
}
