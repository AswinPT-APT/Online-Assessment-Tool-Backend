﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Application.Common.Interfaces.IService;
using OnlineAssessmentTool.Application.Dtos.Assessment;
using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Dtos;
using System.Net;

namespace OnlineAssessmentTool.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AssessmentScoreController : Controller
    {
        private readonly IAssessmentScoreRepository _assessmentScoreRepository;
        private readonly IAssessmentScoreService _assessmentScoreService;
        private readonly ILogger<AssessmentScoreController> _logger; // Add logger

        public AssessmentScoreController(
            IAssessmentScoreRepository assessmentScoreRepository,
            IAssessmentScoreService assessmentScoreService,
            ILogger<AssessmentScoreController> logger) // Inject logger
        {
            _assessmentScoreRepository = assessmentScoreRepository;
            _assessmentScoreService = assessmentScoreService;
            _logger = logger; // Initialize logger
        }

        [HttpGet("{traineeId}")]
        public async Task<IActionResult> GetAssessmentScoresByTraineeId(int traineeId)
        {
            var response = new ApiResponse();

            try
            {
                var assessmentScores = await _assessmentScoreRepository.GetAssessmentScoresByTraineeIdAsync(traineeId);

                if (assessmentScores == null || assessmentScores.Count == 0)
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = new List<string> { "No assessment scores found for the trainee." };
                    return NotFound(response);
                }

                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Result = assessmentScores;
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = new List<string> { "Error retrieving assessment scores." };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssessmentScore>>> GetAssessmentScores()
        {
            try
            {
                _logger.LogInformation("Retrieving all assessment scores.");
                var assessmentScores = await _assessmentScoreRepository.GetAllAsync();

                if (assessmentScores == null || !assessmentScores.Any())
                {
                    _logger.LogWarning("No assessment scores found.");
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "No scores found." }
                    });
                }

                return Ok(assessmentScores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving assessment scores.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error retrieving scores from database." }
                });
            }
        }

        [HttpGet("score-distribution/{assessmentId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetScoreDistribution(int assessmentId)
        {
            var scoreDistribution = await _assessmentScoreRepository.GetScoreDistributionAsync(assessmentId);
            return Ok(scoreDistribution);
        }

        [HttpGet("{assessmentId}")]
        public async Task<IActionResult> GetAssessment(int assessmentId)
        {
            try
            {
                _logger.LogInformation("Retrieving assessment with ID {Id}.", assessmentId);
                var response = new ApiResponse();

                var assessment = await _assessmentScoreRepository.GetAssessmentByIdAsync(assessmentId);
                if (assessment == null)
                {
                    _logger.LogWarning("Assessment with ID {Id} not found.", assessmentId);
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(response);
                }

                response.IsSuccess = true;
                response.Result = assessment;
                response.StatusCode = HttpStatusCode.OK;

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving assessment with ID {Id}.", assessmentId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error retrieving assessment." } });
            }
        }

        [HttpGet("batch/{batchName}")]
        public async Task<ActionResult> GetTraineesWithAverageScore(string batchName)
        {
            try
            {
                _logger.LogInformation("Retrieving trainees with batchname {batchname}.", batchName);
                var response = new ApiResponse();

                var result = await _assessmentScoreRepository.GetTraineesWithAverageScore(batchName);
                if (result == null)
                {
                    _logger.LogWarning("Batch with name {batchname} not found.", batchName);
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(response);
                }

                response.IsSuccess = true;
                response.Result = result.Value;
                response.StatusCode = HttpStatusCode.OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving trainees with Batch Name {batchname}.", batchName);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error retrieving trainee scores." }, Result = ex });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PostAssessmentScore([FromBody] AssessmentScoreDTO assessmentScoreDTO)
        {
            try
            {
                _logger.LogInformation("Creating new assessment score.");
                var assessmentScore = new AssessmentScore
                {
                    ScheduledAssessmentId = assessmentScoreDTO.ScheduledAssessmentId,
                    TraineeId = assessmentScoreDTO.TraineeId,
                    AvergeScore = assessmentScoreDTO.AvergeScore,
                    CalculatedOn = assessmentScoreDTO.CalculatedOn
                };

                await _assessmentScoreRepository.AddAsync(assessmentScore);

                _logger.LogInformation("Assessment score created with ID {Id}.", assessmentScore.AssessmentScoreId);
                return CreatedAtAction(nameof(GetAssessmentScores), new { id = assessmentScore.AssessmentScoreId }, new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.Created, Result = assessmentScore });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating assessment score.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error creating assessment score." } });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> PutAssessmentScore(int id, AssessmentScoreDTO assessmentScoreDTO)
        {
            try
            {
                _logger.LogInformation("Updating assessment score with ID {Id}.", id);
                var assessmentScore = await _assessmentScoreRepository.GetByIdAsync(id);
                if (assessmentScore == null)
                {
                    _logger.LogWarning("Assessment score with ID {Id} not found.", id);
                    return NotFound(new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, Message = new List<string> { "Score not found." } });
                }

                assessmentScore.ScheduledAssessmentId = assessmentScoreDTO.ScheduledAssessmentId;
                assessmentScore.TraineeId = assessmentScoreDTO.TraineeId;
                assessmentScore.AvergeScore = assessmentScoreDTO.AvergeScore;
                assessmentScore.CalculatedOn = assessmentScoreDTO.CalculatedOn;

                await _assessmentScoreRepository.UpdateAsync(assessmentScore);

                _logger.LogInformation("Assessment score with ID {Id} updated successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating assessment score with ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error updating score." } });
            }
        }



        [HttpPut("update-assessment-scores")]
        public async Task<ActionResult<ApiResponse>> UpdateAssessmentScores([FromBody] UpdateAssessmentScoreDTO updateAssessmentScoreDTO)
        {
            try
            {
                _logger.LogInformation("Updating assessment scores.");
                await _assessmentScoreService.UpdateAssessmentScoresAsync(updateAssessmentScoreDTO.AssessmentScores);

                _logger.LogInformation("Assessment scores updated successfully.");
                return Ok(new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = new List<string> { "Assessment scores updated successfully." } });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating assessment scores.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error updating assessment scores." } });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteAssessmentScore(int id)
        {
            try
            {
                _logger.LogInformation("Deleting assessment score with ID {Id}.", id);
                var assessmentScore = await _assessmentScoreRepository.GetByIdAsync(id);
                if (assessmentScore == null)
                {
                    _logger.LogWarning("Assessment score with ID {Id} not found.", id);
                    return NotFound(new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, Message = new List<string> { "Score not found." } });
                }

                await _assessmentScoreRepository.DeleteAsync(assessmentScore);

                _logger.LogInformation("Assessment score with ID {Id} deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting assessment score with ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error deleting score." } });
            }
        }
    }
}
