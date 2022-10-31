using Engine.BL;
using Engine.Constants;
using Engine.Services;
using BaseAPI;
using BaseAPI.Classes;

Builder.Build(new WebProperties("CallCenterAPI", WebApplication.CreateBuilder(args))
    {
        ConnectionString = C.CALL_CENTER_DB,
    },
    builder => builder.Services.AddSignalR()  
);