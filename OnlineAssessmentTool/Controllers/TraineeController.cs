using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;

namespace OnlineAssessmentTool.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TraineeController : ControllerBase
    {
        private readonly ITraineeRepository _traineeRepository;
        private readonly IMapper _mapper;

        public TraineeController(ITraineeRepository traineeRepository, IMapper mapper)
        {
            _traineeRepository = traineeRepository;
            _mapper = mapper;
        }
    }
}
