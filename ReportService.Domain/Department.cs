namespace ReportService.Domain
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}