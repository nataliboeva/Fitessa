using AutoMapper;
using Fitessa.Data.Entities;
using Fitessa.Models;

namespace Fitessa.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, AdminUserViewModel>();
            CreateMap<ApplicationUser, AdminUserDetailsViewModel>();
            CreateMap<WorkoutProgram, WorkoutProgramViewModel>();
            CreateMap<WorkoutProgramViewModel, WorkoutProgram>();
            CreateMap<Exercise, ExerciseViewModel>();
            CreateMap<ExerciseViewModel, Exercise>();
            CreateMap<MealPlan, MealPlanViewModel>();
            CreateMap<MealPlanViewModel, MealPlan>();
            CreateMap<MeasurementLog, MeasurementLogViewModel>();
            CreateMap<MeasurementLogViewModel, MeasurementLog>();
            CreateMap<SubscriptionPlan, SubscriptionPlanViewModel>();
            CreateMap<SubscriptionPlanViewModel, SubscriptionPlan>();
            CreateMap<Notification, NotificationViewModel>();
            CreateMap<NotificationViewModel, Notification>();
        }
    }
} 