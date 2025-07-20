using System;
using System.Collections.Generic;
using System.Linq;
using Fitessa.Data;
using Fitessa.Services.Interfaces;

namespace Fitessa.Services.Services
{
    public class ProgressInsightsService : IProgressInsightsService
    {
        private readonly ApplicationDbContext _context;
        public ProgressInsightsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<ProgressInsight> GetInsights(string userId)
        {
            var insights = new List<ProgressInsight>();
            var logs = _context.MeasurementLogs.Where(l => l.UserId == userId).OrderBy(l => l.LoggedAt).ToList();
            if (logs.Count >= 2)
            {
                var first = logs.First().WeightKg;
                var last = logs.Last().WeightKg;
                var diff = last - first;
                var days = (logs.Last().LoggedAt - logs.First().LoggedAt).TotalDays;
                if (days > 0)
                {
                    var rate = diff / (decimal)days * 7; // per week
                    if (rate < -0.1m)
                        insights.Add(new ProgressInsight { Message = $"You're losing {Math.Abs(rate):0.0} kg/week. Great job!", Type = "success" });
                    else if (rate > 0.1m)
                        insights.Add(new ProgressInsight { Message = $"You're gaining {rate:0.0} kg/week.", Type = "trend" });
                    else
                        insights.Add(new ProgressInsight { Message = "Your weight is stable.", Type = "trend" });
                }
            }
            var freq = GetWorkoutFrequency(userId);
            if (freq.Any())
            {
                var avg = freq.Average(f => f.count);
                insights.Add(new ProgressInsight { Message = $"You log workouts {avg:0.0}x per week.", Type = "trend" });
                if (avg < 2)
                    insights.Add(new ProgressInsight { Message = "Try to increase your workout frequency for better results!", Type = "warning" });
            }
            return insights;
        }
        public List<(DateTime date, decimal value)> GetWeightTrend(string userId)
        {
            return _context.MeasurementLogs
                .Where(l => l.UserId == userId)
                .OrderBy(l => l.LoggedAt)
                .Select(l => new { Date = l.LoggedAt.Date, Value = l.WeightKg })
                .ToList()
                .Select(x => (x.Date, x.Value))
                .ToList();
        }
        public List<(DateTime week, int count)> GetWorkoutFrequency(string userId)
        {
            var logs = _context.MeasurementLogs.Where(l => l.UserId == userId).ToList();
            return logs
                .GroupBy(l => System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(l.LoggedAt, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .Select(g => new { Week = g.First().LoggedAt.Date.AddDays(-(int)g.First().LoggedAt.DayOfWeek), Count = g.Count() })
                .OrderBy(t => t.Week)
                .ToList()
                .Select(x => (x.Week, x.Count))
                .ToList();
        }
    }
} 