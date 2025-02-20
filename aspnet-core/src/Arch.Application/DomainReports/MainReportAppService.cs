using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Abp.Application.Navigation;
using Arch.Authorization.Users;
using Microsoft.Data.SqlClient;
using Arch.Sql;
using Arch.EntityFrameworkCore;
using System.Data;
using Arch.Domain.MainReports.Dto;
using Newtonsoft.Json.Linq;
using NUglify.Helpers;
using static Abp.Domain.Uow.AbpDataFilters;
using Arch.Domain.FinancialReports.Dto;
using Newtonsoft.Json;
using System.Globalization;
//using Arch.OrganizationalUnits;
//using Arch.Runtime.Session;
//using Arch.Domain.OrganizationalUnits;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Arch.Organizations;
namespace Arch.Domain.MainReports
{

    public class MainReportAppService : ArchAppServiceBase, IMainReportAppService
    {
        private readonly IUserNavigationManager _userNavigationManager;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISqlRepository _sqlRepository;
        private readonly ISqlFinancialReports _sqlFinancialReports;
        private readonly IUserAppService _userService;
        //  private readonly IOrganizationalUnitAppService _organizationalUnitAppservice;

        private Dictionary<string, string> _reportColumnsToProc;

        private Dictionary<string, string> _typesToProc;
        public MainReportAppService(
          ISqlRepository sqlRepository,
          ISqlFinancialReports sqlFinancialReports,
            IHttpContextAccessor httpContextAccessor,
              IUserNavigationManager userNavigationManager,
                 IUserAppService userService
       //        ,    IOrganizationalUnitAppService organizationalUnitAppservice
       )
        {
            _httpContextAccessor = httpContextAccessor;
            _sqlRepository = sqlRepository;
            _sqlFinancialReports = sqlFinancialReports;
            _userNavigationManager = userNavigationManager;
            _userService = userService;
            //     _organizationalUnitAppservice = organizationalUnitAppservice;
        }



        private string IfInt(string check)
        {
            bool ifok = false;
            int rstint = 0;
            ifok = int.TryParse(check, out rstint);
            if (ifok)
            {
                return check;
            }
            return "0";
        }

        private ArchDbContext GetContext()
        {

            ArchDbContextFactory NewDBContext = new ArchDbContextFactory();
            string[] arg = new string[1]; ;

            var Context = NewDBContext.CreateDbContext(arg);

            return Context;
        }

        public Task<object> MainReportAsync<T>(T objectId, string objectName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MainReportDto>> GetMainReportEntityAsync(string reportName)
        {
            var result = await MainReportDictionaryAsync(reportName);
            var itemsToReturn = JArray.FromObject(result).ToObject<List<MainReportDto>>();
            return itemsToReturn;
        }

        //called to get the list of columns
        public async Task<List<Dictionary<string, object>>> MainReportDictionaryAsync(string objectName)
        {
            var result = await _sqlRepository.LoadData("MainReportlist", "StoredProcedure", new SqlParameter("@ReportName", objectName));
            return result;

        }


        public async Task<List<object>> MainReportObjectAsync(string objectName)
        {
            var result = await _sqlRepository.LoadDataObject("MainReportlist", "StoredProcedure", new SqlParameter("@ReportName", objectName));
            return result;
        }

        public async Task<List<Dictionary<string, object>>> RunReportDictionaryAsync(string objectName, List<Dictionary<string, string>> paramlist)
        {
            var args = new List<SqlParameter>();

            foreach (var param in paramlist)
            {
                var paramtype = new SqlDbType();
                SqlParameter _test = null;
                if (param["paramtype"].ToLower().Contains("char") || param["paramtype"].ToLower().Contains("string"))
                {

                    _test = new SqlParameter("@" + param["paramname"], SqlDbType.NVarChar)
                    {
                        Size = 500,
                        IsNullable = true,
                        Direction = ParameterDirection.Input,
                        Value = param["paramvalue"]
                    };

                }

                else if (param["paramtype"].ToLower().Contains("int") || param["paramtype"].ToLower().Contains("long"))
                    _test = new SqlParameter("@" + param["paramname"], SqlDbType.BigInt)
                    {
                        //  Size = 500,
                        IsNullable = true,
                        Direction = ParameterDirection.Input,
                        Value = param["paramvalue"]
                    };

                else if (param["paramtype"].ToLower().Contains("bit"))
                    _test = new SqlParameter("@" + param["paramname"], SqlDbType.Bit)
                    {
                        //  Size = 500,
                        //IsNullable = true,
                        Direction = ParameterDirection.Input,
                        Value = param["paramvalue"]
                    };
                else if (param["paramtype"].ToLower().Contains("date"))
                    _test = new SqlParameter("@" + param["paramname"], SqlDbType.DateTime)
                    {
                        //  Size = 500,
                        //IsNullable = true,
                        Direction = ParameterDirection.Input,
                        Value = param["paramvalue"]
                    };

                else
                    _test = new SqlParameter("@" + param["paramname"], SqlDbType.Float)
                    {
                        //  Size = 500,
                        //IsNullable = true,
                        Direction = ParameterDirection.Input,
                        Value = param["paramvalue"]
                    };


                args.Add(_test);

            }

            SqlParameter[] sqlp = (SqlParameter[])args.ToArray();



            return await _sqlRepository.LoadData(objectName, "StoredProcedure", sqlp);
        }


        public async Task<List<object>> RunReportObjectAsync(string objectName, List<Dictionary<string, string>> paramlist)
        {
            var args = new List<SqlParameter>();
            foreach (var param in paramlist)
            {
                var paramtype = new SqlDbType();
                SqlParameter _test = null;
                if (param["paramtype"].ToLower().Contains("char") || param["paramtype"].ToLower().Contains("string"))
                {

                    _test = new SqlParameter("@" + param["paramname"], SqlDbType.NVarChar)
                    {
                        Size = 500,
                        IsNullable = true,
                        Direction = ParameterDirection.Input,
                        Value = param["paramvalue"]
                    };

                }

                else if (param["paramtype"].ToLower().Contains("int") || param["paramtype"].ToLower().Contains("long"))
                    _test = new SqlParameter("@" + param["paramname"], SqlDbType.BigInt)
                    {
                        //  Size = 500,
                        IsNullable = true,
                        Direction = ParameterDirection.Input,
                        Value = param["paramvalue"]
                    };

                else if (param["paramtype"].ToLower().Contains("bit"))
                    _test = new SqlParameter("@" + param["paramname"], SqlDbType.Bit)
                    {
                        //  Size = 500,
                        //IsNullable = true,
                        Direction = ParameterDirection.Input,
                        Value = param["paramvalue"]
                    };
                else if (param["paramtype"].ToLower().Contains("date"))
                    _test = new SqlParameter("@" + param["paramname"], SqlDbType.DateTime)
                    {
                        //  Size = 500,
                        //IsNullable = true,
                        Direction = ParameterDirection.Input,
                        Value = param["paramvalue"]
                    };

                else
                    _test = new SqlParameter("@" + param["paramname"], SqlDbType.Float)
                    {
                        //  Size = 500,
                        //IsNullable = true,
                        Direction = ParameterDirection.Input,
                        Value = param["paramvalue"]
                    };


                args.Add(_test);

            }

            SqlParameter[] sqlp = (SqlParameter[])args.ToArray();



            return await _sqlRepository.LoadDataObject(objectName, "StoredProcedure", sqlp);
        }

        //called by the generic 1 result main report
        public async Task<List<Dictionary<string, object>>> RunMainReportObjectAsync(string reportName, List<MainReportDto> paramlist)
        {
            try
            {
                var organizationUnitIdParam = "@OrganizationalUnitId";
                var tenantIdParam = "@TenantId";
                var isDeletedParam = "@IsDeleted";
                var args = new List<SqlParameter>();

                var outargs = new List<string>();

                var parameters = paramlist;
                if (paramlist != null && paramlist.Count > 0)
                {

                    parameters = paramlist.Where(x => x.RowType == "ParameterDetails" &&
               x.ParameterName != organizationUnitIdParam && x.ParameterName != tenantIdParam &&
               x.ParameterName != isDeletedParam).ToList();

                    foreach (var param in parameters)
                    {
                        SqlParameter sqlParam = null;
                        if (param.ParameterDataType.ToLower().Contains("char") || param.ParameterDataType.ToLower().Contains("string"))
                        {
                            sqlParam = new SqlParameter(param.ParameterName, SqlDbType.NVarChar)
                            {
                                Size = 500,
                                IsNullable = true,
                                Direction = ParameterDirection.Input,
                                Value = param.ParameterValue.ToString(),
                            };
                        }
                        else if (param.ParameterDataType.ToLower().Contains("int")
                            || param.ParameterDataType.ToLower().Contains("long"))

                            sqlParam = new SqlParameter(param.ParameterName, SqlDbType.BigInt)
                            {
                                IsNullable = true,
                                Direction = ParameterDirection.Input,
                                Value = Int32.Parse(param.ParameterValue)
                            };
                        else if (param.ParameterDataType.ToLower().Contains("bit"))

                            sqlParam = new SqlParameter(param.ParameterName, SqlDbType.Bit)
                            {
                                Direction = ParameterDirection.Input,
                                Value = Convert.ToByte(param.ParameterValue)
                            };

                        else if (param.ParameterDataType.ToLower().Contains("date"))
                            sqlParam = new SqlParameter(param.ParameterName, SqlDbType.DateTime)
                            {
                                Direction = ParameterDirection.Input,
                                Value = Convert.ToDateTime(param.ParameterValue)
                            };
                        else
                            sqlParam = new SqlParameter(param.ParameterName, SqlDbType.Float)
                            {
                                Direction = ParameterDirection.Input,
                                Value = Convert.ToDouble(param.ParameterValue)
                            };

                        args.Add(sqlParam);
                    }
                }
                var organizationUnitIdParamVal = new SqlParameter(organizationUnitIdParam, SqlDbType.NVarChar)
                {
                    Direction = ParameterDirection.Input,
                    Value = "00001" //  OrganizationalUnitSession.OrganizationalUnitId ?? "00001"
                };
                var tenantIdParamVal = new SqlParameter(tenantIdParam, SqlDbType.BigInt)
                {
                    Direction = ParameterDirection.Input,
                    Value = AbpSession.TenantId
                };
                var isDeletedParamVal = new SqlParameter(isDeletedParam, SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Input,
                    Value = 0
                };

                args.Add(tenantIdParamVal);
                args.Add(organizationUnitIdParamVal);
                args.Add(isDeletedParamVal);

                SqlParameter[] sqlp = (SqlParameter[])args.ToArray();

                //before running the stored procedure. Check if it has output parameters. If it does then it is 

                var outparameters = paramlist.Where(x => x.RowType == "ParameterDetails" &&
               x.ParameterName != organizationUnitIdParam && x.ParameterName != tenantIdParam &&
               x.ParameterName != isDeletedParam && x.IsOutPutParameter == true).ToList();

                List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
                if (outparameters == null || outparameters.Count == 0)
                {
                    //no out parameter so go ahead
                    result = await _sqlRepository.LoadData(reportName, "StoredProcedure", sqlp);
                }
                else
                {

                    foreach (var param in outparameters)
                    {

                        outargs.Add(param.ParameterName);
                    }
                    var outresult = await _sqlRepository.ProcessData(reportName, "StoredProcedure", outargs.ToArray(), sqlp);

                    result.Add(outresult);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /*
         * paramtype = "companyinfo" to get company details like company name e.t.c
         * 
         * paramtype = "financialreport" to get company details like company name e.t.c
         */
        public async Task<object> RunDefinedReportAsync(string reportName, object selectedparameters, string paramtype = "financialreport")
        {

            try
            {
                if (reportName == null)
                    reportName = "";

                UserCompanyInfo user = new UserCompanyInfo();

                Dictionary<string, string> _companyInfo = null;

                var orgid = "00001";// OrganizationalUnitSession.OrganizationalUnitId;
                //if (orgid == null)
                //{

                //    var tst = await _organizationalUnitAppservice.GetCurrentOrganizationalUnit();
                //    orgid = tst.Code;


                //}
                _companyInfo = await _sqlRepository.LoadParent("AppOrganizationalUnit", orgid, "OrganizationalUnitCode", orgid);


                user.Company = _companyInfo["TenancyName"];
                user.Division = _companyInfo["OrganizationalUnitName"];
                user.Department = "";


                if (paramtype.ToLower().Contains("user") || paramtype.ToLower().Contains("company"))
                {
                    return (object)JsonConvert.SerializeObject(user);
                }

                else
                {
                    initializetypes();
                    var organizationUnitIdParam = "@OrganizationalUnitId";
                    var tenantIdParam = "@TenantId";
                    var isDeletedParam = "@IsDeleted";
                    var args = new List<SqlParameter>();
                    var outargs = new List<string>();
                    //      var jconvreportinfo = JsonObject .Parse(selectedparameters);
                    ReportInfo info = JsonConvert.DeserializeObject<ReportInfo>(selectedparameters.ToString());
                    //  ReportInfo info = new ReportInfo();
                    //   info.Year = objs["Year"].ToString();

                    object result = null;





                    SqlParameter sqlParam = null;

                    if ((info.ColumnName.Contains("Period")))
                    {

                        sqlParam = new SqlParameter("@Year", SqlDbType.Int)
                        {
                            IsNullable = true,
                            Direction = ParameterDirection.Input,
                            Value = Int32.Parse(info.Year)
                        };
                        args.Add(sqlParam);

                        sqlParam = new SqlParameter("@Period", SqlDbType.Int)
                        {
                            IsNullable = true,
                            Direction = ParameterDirection.Input,
                            Value = Int32.Parse(info.Period)
                        };
                        args.Add(sqlParam);
                    }

                    if ((info.ColumnName.Contains("Project")))
                    {

                        sqlParam = new SqlParameter("@ProjectID", SqlDbType.Int)
                        {
                            IsNullable = true,
                            Direction = ParameterDirection.Input,
                            Value = Int32.Parse(info.ProjectID)
                        };
                        args.Add(sqlParam);

                    }



                    var organizationUnitIdParamVal = new SqlParameter(organizationUnitIdParam, SqlDbType.NVarChar)
                    {
                        Direction = ParameterDirection.Input,
                        Value = "00001" //OrganizationalUnitSession.OrganizationalUnitId ?? "00001"
                    };
                    args.Add(organizationUnitIdParamVal);

                    var tenantIdParamVal = new SqlParameter(tenantIdParam, SqlDbType.BigInt)
                    {
                        Direction = ParameterDirection.Input,
                        Value = AbpSession.TenantId
                    };
                    args.Add(tenantIdParamVal);

                    var isDeletedParamVal = new SqlParameter(isDeletedParam, SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Input,
                        Value = 0
                    };
                    args.Add(isDeletedParamVal);





                    SqlParameter[] sqlp = (SqlParameter[])args.ToArray();

                    if (paramtype != "Aged" && !info.ColumnName.ToLower().Contains("payables") && !info.ColumnName.ToLower().Contains("receivables") && !info.ColumnName.ToLower().Contains("10column"))
                        checkReportTypes(info);

                    if (info.RowName.ToLower().Contains("balancesheet"))
                        result = await _sqlFinancialReports.GetBalanceSheet("enterprise.RptGLBalanceSheet" + _reportColumnsToProc.GetValueOrDefault(info.ColumnName, info.ColumnName) + _typesToProc.GetValueOrDefault(info.ReportType, info.ReportType), "StoredProcedure", sqlp);

                    else if (info.RowName.ToLower().Contains("incomestatement"))
                        result = await _sqlFinancialReports.GetIncomeStatement("enterprise.RptGLIncomeStatement" + _reportColumnsToProc.GetValueOrDefault(info.ColumnName, info.ColumnName) + _typesToProc.GetValueOrDefault(info.ReportType, info.ReportType), "StoredProcedure", sqlp);

                    else if (info.RowName.ToLower().Contains("cashflow"))
                        result = await _sqlFinancialReports.GetCashFlow("enterprise.RptGLCashFlow" + _reportColumnsToProc.GetValueOrDefault(info.ColumnName, info.ColumnName) + _typesToProc.GetValueOrDefault(info.ReportType, info.ReportType), "StoredProcedure", sqlp);

                    else if (info.RowName.ToLower().Contains("trialbalance") && info.RowName.ToLower().Contains("budget"))
                        result = await _sqlFinancialReports.GetTrialBalanceRender("enterprise.RptGLBudgetingTrialBalance" + _reportColumnsToProc.GetValueOrDefault(info.ColumnName, info.ColumnName) + _typesToProc.GetValueOrDefault(info.ReportType, info.ReportType), "StoredProcedure", sqlp);

                    else if (info.RowName.ToLower().Contains("trialbalance"))
                        result = await _sqlFinancialReports.GetTrialBalanceRender("enterprise.RptGLTrialBalance" + _reportColumnsToProc.GetValueOrDefault(info.ColumnName, info.ColumnName) + _typesToProc.GetValueOrDefault(info.ReportType, info.ReportType), "StoredProcedure", sqlp);



                    else if (info.ColumnName.ToLower().Contains("payablescomparative"))
                        result = await _sqlFinancialReports.GetPayablesComparative("enterprise.RptGLAgedPayablesDetailComparative", "StoredProcedure", sqlp);

                    else if (info.ColumnName.ToLower().Contains("payablesytd"))
                        result = await _sqlFinancialReports.GetPayablesYTD("enterprise.RptGLAgedPayablesDetailYTD", "StoredProcedure", sqlp);

                    else if (info.ColumnName.ToLower().Contains("payables"))
                        result = await _sqlFinancialReports.GetPayables("enterprise.RptGLAgedPayablesDetail", "StoredProcedure", sqlp);

                    else if (info.ColumnName.ToLower().Contains("receivablescomparative"))
                        result = await _sqlFinancialReports.GetReceivablesComparative("enterprise.RptGLAgedReceivablesDetailComparative", "StoredProcedure", sqlp);

                    else if (info.ColumnName.ToLower().Contains("receivablesytd"))
                        result = await _sqlFinancialReports.GetReceivablesYTD("enterprise.RptGLAgedReceivablesDetailYTD", "StoredProcedure", sqlp);

                    else if (info.ColumnName.ToLower().Contains("receivables"))
                        result = await _sqlFinancialReports.GetReceivables("enterprise.RptGLAgedReceivablesDetail", "StoredProcedure", sqlp);

                    else if (info.ColumnName.ToLower().Contains("10column"))
                        result = await _sqlFinancialReports.Get10ColumnData("definedinline", "text", sqlp);
                    else
                        result = await _sqlFinancialReports.LoadData(info.RowName, "StoredProcedure", sqlp);


                    //  return result;
                    List<object> resultset = new List<object>();
                    resultset.Add(result);

                    resultset.Add(user);

                    return (object)JsonConvert.SerializeObject(resultset.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //used by the auto load mainreport and other generics
        public async Task<object> RunDefinedReportObjectAsync(string reportName, object selectedparameters, object paramtype = null)
        {
            //do not use this one. Here for future use
            try
            {
                var organizationUnitIdParam = "@OrganizationalUnitId";
                var tenantIdParam = "@TenantId";
                var isDeletedParam = "@IsDeleted";
                var args = new List<SqlParameter>();

                var outargs = new List<string>();
                List<MainReportDto> paramlist = JsonConvert.DeserializeObject<List<MainReportDto>>(selectedparameters.ToString());
                var parameters = paramlist.Where(x => x.RowType == "ParameterDetails" &&
                x.ParameterName != organizationUnitIdParam && x.ParameterName != tenantIdParam &&
                x.ParameterName != isDeletedParam).ToList();

                foreach (var param in parameters)
                {
                    SqlParameter sqlParam = null;
                    if (param.ParameterDataType.ToLower().Contains("char") || param.ParameterDataType.ToLower().Contains("string"))
                    {
                        sqlParam = new SqlParameter(param.ParameterName, SqlDbType.NVarChar)
                        {
                            Size = 500,
                            IsNullable = true,
                            Direction = ParameterDirection.Input,
                            Value = param.ParameterValue.ToString(),
                        };
                    }
                    else if (param.ParameterDataType.ToLower().Contains("int")
                        || param.ParameterDataType.ToLower().Contains("long"))

                        sqlParam = new SqlParameter(param.ParameterName, SqlDbType.BigInt)
                        {
                            IsNullable = true,
                            Direction = ParameterDirection.Input,
                            Value = Int64.Parse(param.ParameterValue)
                        };
                    else if (param.ParameterDataType.ToLower().Contains("bit"))

                        sqlParam = new SqlParameter(param.ParameterName, SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Input,
                            Value = Convert.ToByte(param.ParameterValue)
                        };

                    else if (param.ParameterDataType.ToLower().Contains("date"))
                        sqlParam = new SqlParameter(param.ParameterName, SqlDbType.DateTime)
                        {
                            Direction = ParameterDirection.Input,
                            Value = Convert.ToDateTime(param.ParameterValue)
                        };
                    else
                        sqlParam = new SqlParameter(param.ParameterName, SqlDbType.Float)
                        {
                            Direction = ParameterDirection.Input,
                            Value = Convert.ToDouble(param.ParameterValue)
                        };

                    args.Add(sqlParam);
                }

                var organizationUnitIdParamVal = new SqlParameter(organizationUnitIdParam, SqlDbType.NVarChar)
                {
                    Direction = ParameterDirection.Input,
                    Value = "00001" // OrganizationalUnitSession.OrganizationalUnitId ?? "00001"
                };
                var tenantIdParamVal = new SqlParameter(tenantIdParam, SqlDbType.BigInt)
                {
                    Direction = ParameterDirection.Input,
                    Value = AbpSession.TenantId
                };
                var isDeletedParamVal = new SqlParameter(isDeletedParam, SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Input,
                    Value = 0
                };

                args.Add(tenantIdParamVal);
                args.Add(organizationUnitIdParamVal);
                args.Add(isDeletedParamVal);

                SqlParameter[] sqlp = (SqlParameter[])args.ToArray();

                //before running the stored procedure. Check if it has output parameters. If it does then it is 

                var outparameters = paramlist.Where(x => x.RowType == "ParameterDetails" &&
               x.ParameterName != organizationUnitIdParam && x.ParameterName != tenantIdParam &&
               x.ParameterName != isDeletedParam && x.IsOutPutParameter == true).ToList();

                List<object> result = new List<object>();

                result = await _sqlRepository.LoadDataMultiple(reportName, "StoredProcedure", sqlp);


                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void initializetypes()
        {
            // Filling up dictionaries
            _reportColumnsToProc = new Dictionary<string, string>
            {
                {"Standard", ""},
                {"Company", "Company"},
                {"Division", "Division"},
                {"Period", "Period"},
                {"DivisionPeriod", "PeriodDivision"},
                {"CompanyPeriod", "PeriodCompany"}
            };
            _typesToProc = new Dictionary<string, string>
            {
                {"Standard", ""},
                {"YTD", "YTD"},
                {"Comparative", "Comparative"},
                {"Aged", "Aged"}
            };
        }

        private void checkReportTypes(ReportInfo info)
        {
            if (!info.ColumnName.Contains("Project") && !info.ColumnName.Contains("Budget") && !info.RowName.ToLower().Contains("budget") && !info.RowName.ToLower().Contains("project"))
            {
                if (!_reportColumnsToProc.ContainsKey(info.ColumnName))
                    throw new Exception("Unknown Report Type");

                if (!_typesToProc.ContainsKey(info.ReportType))
                    throw new Exception("Unknown Type");
            }
        }
    }
    public class UserCompanyInfo
    {
        /// <summary>
        /// Id of User's company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Id of User's division
        /// </summary>
        public string Division { get; set; }

        /// <summary>
        /// Id of User's department
        /// </summary>
        public string Department { get; set; }
    }

    public class ReportInfo
    {
        public string Year { get; set; }

        public string Period { get; set; }

        public string ReportType { get; set; }

        public string ColumnName { get; set; }

        public string RowName { get; set; }

        public string ProjectID { get; set; }

    }


}
