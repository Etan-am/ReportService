using System;

namespace ReportService.Domain
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}