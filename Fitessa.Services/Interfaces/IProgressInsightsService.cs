using System;
using System.Collections.Generic;

namespace Fitessa.Services.Interfaces
{
    public class ProgressInsight
    {
        public string Message { get; set; }
        public string Type { get; set; }
    }
    public interface IProgressInsightsService
    {
        List<ProgressInsight> GetInsights(string userId);
        List<(DateTime date, decimal value)> GetWeightTrend(string userId);
        List<(DateTime week, int count)> GetWorkoutFrequency(string userId);
    }
} 