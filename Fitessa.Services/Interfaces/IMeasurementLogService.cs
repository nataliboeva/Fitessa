using System;
using System.Collections.Generic;
using Fitessa.Data.Entities;

namespace Fitessa.Services.Interfaces
{
    public interface IMeasurementLogService
    {
        IEnumerable<MeasurementLog> GetByUser(string userId);
        MeasurementLog GetById(int id);
        void Create(MeasurementLog log);
        void Update(MeasurementLog log);
        void Delete(int id);
    }
} 