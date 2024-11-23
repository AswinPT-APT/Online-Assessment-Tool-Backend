using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Application.Common.Interfaces.IService;
using OnlineAssessmentTool.Application.Dtos.Assessment;

namespace OnlineAssessmentTool.Services
{
    public class AssessmentScoreService : IAssessmentScoreService
    {
        private readonly IAssessmentScoreRepository _assessmentScoreRepository;

        public AssessmentScoreService(IAssessmentScoreRepository assessmentScoreRepository)
        {
            _assessmentScoreRepository = assessmentScoreRepository;
        }

        public async Task UpdateAssessmentScoresAsync(List<AssessmentScoreDTO> assessmentScoreDTOs)
        {
            await _assessmentScoreRepository.UpdateAssessmentScoresAsync(assessmentScoreDTOs);
        }
    }
}
