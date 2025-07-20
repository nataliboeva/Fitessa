using System.Collections.Generic;
using System.Linq;
using Fitessa.Data;
using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;

namespace Fitessa.Services.Services
{
    public class MeasurementLogService : IMeasurementLogService
    {
        private readonly ApplicationDbContext _context;
        public MeasurementLogService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<MeasurementLog> GetByUser(string userId)
        {
            return _context.MeasurementLogs.Where(m => m.UserId == userId).OrderBy(m => m.LoggedAt).ToList();
        }
        public MeasurementLog GetById(int id)
        {
            return _context.MeasurementLogs.FirstOrDefault(m => m.Id == id);
        }
        public void Create(MeasurementLog log)
        {
            _context.MeasurementLogs.Add(log);
            _context.SaveChanges();
        }
        public void Update(MeasurementLog log)
        {
            _context.MeasurementLogs.Update(log);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var log = _context.MeasurementLogs.Find(id);
            if (log != null)
            {
                _context.MeasurementLogs.Remove(log);
                _context.SaveChanges();
            }
        }
    }
} 