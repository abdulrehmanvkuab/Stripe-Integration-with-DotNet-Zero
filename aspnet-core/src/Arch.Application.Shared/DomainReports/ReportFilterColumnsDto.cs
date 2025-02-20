namespace Arch.Domain.MainReports.Dto
{
    public class ReportFilterColumnsDto
    {
        public string SearchCriteria { get; set; }
        public bool ShowCriteria { get; set; } = true;
        public bool TotalCriteria { get; set; } = true;
        public sbyte SortCriteria { get; set; } = 1;
        public string DirectionCriteria { get; set; } = "ASC";
        public string OperatorCriteria { get; set; } = "None";
    }
}

