using BOBA.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BOBA.Server.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class TaskFlowController : ControllerBase
    {
        private readonly ITaskFlowService _taskFlowService;

        public TaskFlowController(ITaskFlowService taskFlowService)
        {
            _taskFlowService = taskFlowService;
        }


    }
}
