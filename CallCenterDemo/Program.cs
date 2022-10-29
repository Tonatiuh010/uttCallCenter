using Engine.BL;
using Engine.Constants;
using Engine.Services;
using BaseAPI;
using BaseAPI.Classes;

Builder.Build(new WebProperties("CallCenterAPI", WebApplication.CreateBuilder(args))
{
    ConnectionString = C.HINT_DB,
    ConnectionStrings = new List<string>()
        {
            C.HINT_DB,
            C.ACCESS_DB
        }
    },
    builderCallback: builder =>
    {
        builder.Services.AddSignalR();
    }
);