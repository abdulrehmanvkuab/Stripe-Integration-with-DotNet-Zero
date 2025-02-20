using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Organizations;
using Arch.Authorization.Users;
using Arch.Configuration;
using Arch.Reports;
using Arch.Reports.Financials;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using  Arch.CustomReportss;
using Arch.Sql;


using Abp.AutoMapper;
using Microsoft.AspNetCore.Identity;

using Arch.EntityFrameworkCore;

using AutoMapper;

using Newtonsoft.Json;


namespace Arch.Reporting
{
    [AbpAuthorize]
    [EnableCors]
    public class ReportAppService : ArchAppServiceBase, IReportAppService
    {
        #region Declarations

        private readonly IConfigurationRoot _appConfiguration;
       // private readonly IRepository<CustomReports> _customReportRepository;
        private readonly IRepository<CustomReports, long> _customReportRepository;
        private readonly UserManager _userManager;
        bool isdeleted = false;
        #endregion

        #region Constructor

        public ReportAppService(UserManager userManager,
            IWebHostEnvironment env, IRepository<CustomReports,long> customReportRepository)
        {
            _customReportRepository = customReportRepository;
            _appConfiguration = env.GetAppConfiguration();
            _userManager = userManager;
        }
        #endregion

        #region Get Methods
        public async Task<List<PeriodsDTO>> GetPeriods()
        {
            List<PeriodsDTO> periodsDto = null;
            string OrganizationalUnitId = string.Empty; int isdeleted = 0;
            User user = await _userManager.GetUserByIdAsync((long)AbpSession.UserId);
            List<OrganizationUnit> organization = _userManager.GetOrganizationUnits(user);
            if (organization.Count > 0)
            {
                OrganizationalUnitId = organization[0].Code;
            }
            using (SqlConnection conn = new SqlConnection(_appConfiguration.GetConnectionString(
                ArchConsts.ConnectionStringName
            )))
            {
                try
                {
                    await conn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("[Enterprise].[LedgerMain_FinancialPeriod]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@TenantId", user.TenantId));
                    cmd.Parameters.Add(new SqlParameter("@OrganizationalUnitId", OrganizationalUnitId));
                    cmd.Parameters.Add(new SqlParameter("@IsDeleted", isdeleted));
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    await conn.CloseAsync();
                    string bal = DataTableToJSONWithStringBuilder(ds.Tables[0]);
                    periodsDto = JsonConvert.DeserializeObject<List<PeriodsDTO>>(bal);
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            return periodsDto;
        }
        public async Task<List<FinancialReportTranslation_CurrencyTypeDTO>> GetCurrencyType()
        {
            List<FinancialReportTranslation_CurrencyTypeDTO> periodsDto = null;
            string OrganizationalUnitId = string.Empty; int isdeleted = 0;
            User user = await _userManager.GetUserByIdAsync((long)AbpSession.UserId);
            List<OrganizationUnit> organization = _userManager.GetOrganizationUnits(user);
            if (organization.Count > 0)
            {
                OrganizationalUnitId = organization[0].Code;
            }
            using (SqlConnection conn = new SqlConnection(_appConfiguration.GetConnectionString(ArchConsts.ConnectionStringName)))
            {
                try
                {
                    await conn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("[Enterprise].[FinancialReportTranslation_CurrencyType]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@TenantId", user.TenantId));
                    cmd.Parameters.Add(new SqlParameter("@OrganizationalUnitId", OrganizationalUnitId));
                    cmd.Parameters.Add(new SqlParameter("@IsDeleted", isdeleted));
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    await conn.CloseAsync();
                    string bal = DataTableToJSONWithStringBuilder(ds.Tables[0]);
                    periodsDto = JsonConvert.DeserializeObject<List<FinancialReportTranslation_CurrencyTypeDTO>>(bal);
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            return periodsDto;
        }
        public async Task<ReportDataResponseDTO> GetFinancialPositionReport(int Period, string ReportType, string ConversionCurrencyID, double ConversionRate)
        {
            ReportDataResponseDTO responseDTO = new ReportDataResponseDTO();
            string[] incomestatementreport = {AppConstsERP.IncomeStatementCompanyYTD, AppConstsERP.IncomeStatementCompanyTranslationYTD, AppConstsERP.IncomeStatementCompanyTranslationRange, AppConstsERP.IncomeStatementCompanyTranslationComparative,
                AppConstsERP.IncomeStatementCompanyTranslation, AppConstsERP.IncomeStatementCompanyRange, AppConstsERP.IncomeStatementCompanyComparative, AppConstsERP.IncomeStatementCompany,
            AppConstsERP.IncomeStatementDepartment,AppConstsERP.IncomeStatementDepartmentComparative,AppConstsERP.IncomeStatementDepartmentRange,AppConstsERP.IncomeStatementDepartmentTranslation,AppConstsERP.IncomeStatementDepartmentTranslationComparative,
            AppConstsERP.IncomeStatementDepartmentTranslationRange,AppConstsERP.IncomeStatementDepartmentTranslationYTD,AppConstsERP.IncomeStatementDepartmentYTD,AppConstsERP.IncomeStatementDivision,AppConstsERP.IncomeStatementDivisionComparative,
            AppConstsERP.IncomeStatementDivisionRange,AppConstsERP.IncomeStatementDivisionTranslation,AppConstsERP.IncomeStatementDivisionTranslationComparative,AppConstsERP.IncomeStatementDivisionTranslationRange,AppConstsERP.IncomeStatementDivisionTranslationYTD,
            AppConstsERP.IncomeStatementDivisionYTD,AppConstsERP.IncomeStatementSummaryDivisionYTD,AppConstsERP.IncomeStatementSummaryDivisionTranslationYTD,AppConstsERP.IncomeStatementSummaryDivisionTranslationRange,AppConstsERP.IncomeStatementSummaryDivisionTranslationComparative,
            AppConstsERP.IncomeStatementSummaryDivisionTranslation,AppConstsERP.IncomeStatementSummaryDivisionRange,AppConstsERP.IncomeStatementSummaryDivisionComparative,AppConstsERP.IncomeStatementSummaryDivision,AppConstsERP.IncomeStatementSummaryDepartmentYTD,AppConstsERP.IncomeStatementSummaryDepartmentTranslationYTD,
            AppConstsERP.IncomeStatementSummaryDepartmentTranslationRange,AppConstsERP.IncomeStatementSummaryDepartmentTranslationComparative,AppConstsERP.IncomeStatementSummaryDepartmentTranslation,AppConstsERP.IncomeStatementSummaryDepartmentRange,AppConstsERP.IncomeStatementSummaryDepartmentComparative,
            AppConstsERP.IncomeStatementSummaryDepartment,AppConstsERP.IncomeStatementSummaryCompanyYTD,AppConstsERP.IncomeStatementSummaryCompanyTranslationYTD,AppConstsERP.IncomeStatementSummaryCompanyTranslationRange,AppConstsERP.IncomeStatementSummaryCompanyTranslationComparative,
            AppConstsERP.IncomeStatementSummaryCompanyTranslation,AppConstsERP.IncomeStatementSummaryCompanyRange,AppConstsERP.IncomeStatementSummaryCompanyComparative,AppConstsERP.IncomeStatementSummaryCompany,AppConstsERP.IncomeStatementAltCompany,AppConstsERP.IncomeStatementAltCompanyRange,
            AppConstsERP.IncomeStatementAltDepartment,AppConstsERP.IncomeStatementAltDepartmentRange,AppConstsERP.IncomeStatementAltDivision,AppConstsERP.IncomeStatementAltDivisionRange,AppConstsERP.IncomeStatementAltSummaryCompany,AppConstsERP.IncomeStatementAltSummaryCompanyYTD,AppConstsERP.IncomeStatementAltSummaryDepartment,AppConstsERP.IncomeStatementAltSummaryDepartmentComparative,
            AppConstsERP.IncomeStatementAltSummaryDepartmentYTD,AppConstsERP.IncomeStatementAltSummaryDivision,AppConstsERP.IncomeStatementAltSummaryDivisionYTD};
            responseDTO.schema = GetDefaultReportSchemaByName(ReportType);
            string OrganizationalUnitId = string.Empty;
            User user = await _userManager.GetUserByIdAsync((long)AbpSession.UserId);
            List<OrganizationUnit> organization = _userManager.GetOrganizationUnits(user);
            if (organization.Count > 0)
            {
                OrganizationalUnitId = organization[0].Code;
            }
            using (SqlConnection conn = new SqlConnection(_appConfiguration.GetConnectionString(
               ArchConsts.ConnectionStringName
           )))
            {
                try
                {
                    await conn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("getReports", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@TenantId", user.TenantId));
                    cmd.Parameters.Add(new SqlParameter("@OrganizationalUnitId", OrganizationalUnitId));
                    cmd.Parameters.Add(new SqlParameter("@ReportName", ReportType));
                    cmd.Parameters.Add(new SqlParameter("@Period", Period));
                    cmd.Parameters.Add(new SqlParameter("@ConversionRate", ConversionRate));
                    cmd.Parameters.Add(new SqlParameter("@ConversionCurrencyID", String.IsNullOrEmpty(ConversionCurrencyID) ? "" : ConversionCurrencyID));
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    await conn.CloseAsync();
                    if (ds.Tables.Count > 0)
                    {
                        if (incomestatementreport.Contains(ReportType))
                        {
                            string ProfitandLoss = ds.Tables[0] != null ? ds.Tables[0].Rows.Count > 0 ? DataTableToJSONWithStringBuilder(ds.Tables[0]) : "":string.Empty;
                            string comp = ds.Tables[1] != null ? ds.Tables[1].Rows.Count > 0 ? DataTableToJSONWithStringBuilder(ds.Tables[1]) : "":string.Empty;
                            responseDTO.data = new { ProfitandLoss = JsonConvert.DeserializeObject<List<ProfitandLossDTO>>(ProfitandLoss), Companies = JsonConvert.DeserializeObject<List<CompanieDTO>>(comp) };
                        }
                        else
                        {
                            string bal = ds.Tables[0] != null ? ds.Tables[0].Rows.Count > 0 ? DataTableToJSONWithStringBuilder(ds.Tables[0]) : "" : string.Empty;
                            string comp = ds.Tables[1]!=null? ds.Tables[1].Rows.Count > 0 ? DataTableToJSONWithStringBuilder(ds.Tables[1]) : "":string.Empty;
                            responseDTO.data = new { BalanceSheet = JsonConvert.DeserializeObject<List<BalanceSheetDTO>>(bal), Companies = JsonConvert.DeserializeObject<List<CompanieDTO>>(comp) };
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

            }

            return responseDTO;
        }
        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }

        public string GetDefaultReportSchemaByName(string reportName)
        {
            CustomReports customReport = _customReportRepository.GetAll().FirstOrDefault(x => x.ReportName == reportName);
            if (customReport != null)
            {
                return customReport.ReportSchema;
            }
            else
            {
                return String.Empty;
            }
        }
        #endregion

        #region Crud Methods

        public async Task CreateOrUpdateCustomReport(CreateOrEditCustomReportDto input)
        {
            CustomReports customReport = _customReportRepository.FirstOrDefault(x => x.ReportName == input.ReportName);
            if (customReport != null)
            {
                long id = customReport.Id;
                ObjectMapper.Map(input, customReport);
                customReport.Id = id;
                customReport.CreatorUserId = AbpSession.UserId;
                await _customReportRepository.UpdateAsync(customReport);
            }
            else
            {
                customReport = new CustomReports();
                ObjectMapper.Map(input, customReport);
                customReport.CreatorUserId = AbpSession.UserId;
                await _customReportRepository.InsertAsync(customReport);
            }
        }
        #endregion
    }

}
