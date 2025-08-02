using AutoMapper;
using Fitessa.Data.Entities;
using Fitessa.Models;

namespace Fitessa.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, ProfileEditViewModel>()
                .ReverseMap();

            CreateMap<WorkoutProgram, WorkoutProgramViewModel>()
                .ForMember(dest => dest.ExerciseCount, opt => opt.MapFrom(src => src.WorkoutProgramExercises.Count))
                .ReverseMap();

            CreateMap<Exercise, ExerciseViewModel>()
                .ReverseMap();

            CreateMap<MealPlan, MealPlanViewModel>()
                .ReverseMap();

            CreateMap<MeasurementLog, MeasurementLogViewModel>()
                .ReverseMap();

            CreateMap<SubscriptionPlan, SubscriptionPlanViewModel>()
                .ReverseMap();
        }
    }
} 