using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;



namespace Arch.Domain.MainReports.Dto
{
    public class MainReportDto
    {
        public bool IsOutPutParameter { get; set; }
        public string ObjectName { get; set; }
        public string ParameterDataType { get; set; }
        public int ParameterID { get; set; }
        public int ParameterMaxBytes { get; set; }
        public string ParameterName { get; set; }
        public string RowType { get; set; }
        public string Schema { get; set; }
        public string ParameterValue { get; set; }
        public bool ShowCriteria { get; set; } = true;
        public bool TotalCriteria { get; set; } = false;
        public int SortCriteria { get; set; } = 1;
        public string DirectionCateria { get; set; } = "ASC";
        public string OperatorCriteria { get; set; } = "None";
        public string SearchCriteriaString { get; set; }

    }
}


