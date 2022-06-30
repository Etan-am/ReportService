namespace ReportService.Domain
{
    public class Employee : BaseEntity
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public string Inn { get; set; }
    }
}