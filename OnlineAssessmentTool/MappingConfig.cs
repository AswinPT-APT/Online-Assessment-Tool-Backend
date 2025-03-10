﻿using AutoMapper;
using OnlineAssessmentTool.Application.Dtos.Assessment;
using OnlineAssessmentTool.Application.Dtos.User;
using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Dtos;

namespace OnlineAssessmentTool
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Batch, CreateBatchDTO>().ReverseMap();
            CreateMap<Batch, UpdateBatchDTO>().ReverseMap();

            CreateMap<AssessmentDTO, Assessment>().ReverseMap();
            CreateMap<QuestionDTO, Question>().ReverseMap();
            CreateMap<QuestionOptionDTO, QuestionOption>().ReverseMap();

            CreateMap<CreateUserDTO, Users>().ReverseMap();
            CreateMap<CreateUserRequestDTO, CreateUserRequestDTO>().ReverseMap();
            CreateMap<TrainerDTO, Trainer>().ReverseMap();
            CreateMap<TraineeDTO, Trainee>().ReverseMap();

            CreateMap<TraineeAnswerDTO, TraineeAnswer>().ReverseMap();
            CreateMap<ScheduledAssessmentDTO, ScheduledAssessment>().ReverseMap();
            CreateMap<AssessmentScoreDTO, AssessmentScore>().ReverseMap();
        }
    }
}
