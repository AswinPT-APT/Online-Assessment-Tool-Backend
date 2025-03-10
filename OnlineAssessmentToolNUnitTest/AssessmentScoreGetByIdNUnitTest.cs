﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Application.Dtos.Assessment;
using OnlineAssessmentTool.Controllers;
using OnlineAssessmentTool.Dtos;
using System.Net;

namespace OnlineAssessmentTool.Tests
{
    [TestFixture]
    public class AssessmentScoreGetByIdNUnitTest
    {
        private Mock<IAssessmentScoreRepository> _mockAssessmentScoreRepository;
        private AssessmentScoreController _controller;

        [SetUp]
        public void Setup()
        {
            _mockAssessmentScoreRepository = new Mock<IAssessmentScoreRepository>();
            _controller = new AssessmentScoreController(_mockAssessmentScoreRepository.Object, null, null);
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose of _controller if it implements IDisposable
            if (_controller is IDisposable disposableController)
            {
                disposableController.Dispose();
                _controller = null;
            }
        }

        [Test]
        public async Task GetAssessmentScoresByTraineeId_ReturnsOkResult_WithScores()
        {
            // Arrange
            var traineeId = 1;
            var scores = new List<TraineeAssessmentScoreDTO>
            {
                new TraineeAssessmentScoreDTO
                {
                    AssessmentScoreId = 1,
                    ScheduledAssessmentId = 1,
                    AssessmentId = 1,
                    AssessmentName = "Assessment 1",
                    ScheduledDate = DateTime.Now,
                    Score = 85,
                    CalculatedOn = DateTime.Now
                }
            };
            _mockAssessmentScoreRepository
                .Setup(repo => repo.GetAssessmentScoresByTraineeIdAsync(traineeId))
                .ReturnsAsync(scores);

            // Act
            var result = await _controller.GetAssessmentScoresByTraineeId(traineeId) as ObjectResult;
            var response = result?.Value as ApiResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result?.StatusCode);
            Assert.IsTrue(response?.IsSuccess);
            Assert.AreEqual(scores, response?.Result);
        }

        [Test]
        public async Task GetAssessmentScoresByTraineeId_ReturnsNotFound_WhenNoScores()
        {
            // Arrange
            var traineeId = 1;
            _mockAssessmentScoreRepository
                .Setup(repo => repo.GetAssessmentScoresByTraineeIdAsync(traineeId))
                .ReturnsAsync(new List<TraineeAssessmentScoreDTO>());

            // Act
            var result = await _controller.GetAssessmentScoresByTraineeId(traineeId) as ObjectResult;
            var response = result?.Value as ApiResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result?.StatusCode);
            Assert.IsFalse(response?.IsSuccess);
            Assert.AreEqual("No assessment scores found for the trainee.", response?.Message[0]);
        }

        [Test]
        public async Task GetAssessmentScoresByTraineeId_ReturnsInternalServerError_WhenException()
        {
            // Arrange
            var traineeId = 1;
            _mockAssessmentScoreRepository
                .Setup(repo => repo.GetAssessmentScoresByTraineeIdAsync(traineeId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetAssessmentScoresByTraineeId(traineeId) as ObjectResult;
            var response = result?.Value as ApiResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result?.StatusCode);
            Assert.IsFalse(response?.IsSuccess);
            Assert.AreEqual("Error retrieving assessment scores.", response?.Message[0]);
        }
    }
}
