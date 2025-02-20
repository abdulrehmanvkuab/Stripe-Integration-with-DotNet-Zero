using Abp.Application.Services.Dto;
using System;

namespace Arch.Reports.Financials
{
    public class BalanceSheetCompanyDto : EntityDto
    {
        public string OrganizationalUnitId { get; set; }
        public int TenantId { get; set; }
        public string GLBalanceType { get; set; }
        public string GLBalanceTypeDescription { get; set; }
        public string NoteNumber { get; set; }
        public string LockedBy { get; set; }
        public DateTime? LockTS { get; set; }
        public bool IsDeleted { get; set; }
        public string GLAccountNumber { get; set; }
        public string GLAccountName { get; set; }
        public string GLAccountType { get; set; }
        public decimal CurrentYear { get; set; }
        public decimal PriorYear { get; set; }
        public decimal Budget { get; set; }
        public decimal CurrentMonth { get; set; }
        public decimal PriorYearMonth { get; set; }
        public decimal BudgetMonth { get; set; }

    }    

}
