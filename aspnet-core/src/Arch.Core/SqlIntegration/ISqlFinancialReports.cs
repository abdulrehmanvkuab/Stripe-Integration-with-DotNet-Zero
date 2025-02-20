using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Arch.Authorization.Users;
using System.Text.RegularExpressions;

//using Arch.Authorization.Users.Dtos;

using System.Data.Common;
using System.Data;

namespace Arch.Sql
{
    

        public interface ISqlFinancialReports : IRepository, IDisposable
    {

        public Task<object> LoadData(string sqlSelect, string commandtype, params object[] sqlParameters);  //commandtype = "StoredProcedure" or "Text";
        public Task<object> GetBalanceSheet(string sqlSelect, string commandtype, params object[] sqlParameters);  //commandtype = "StoredProcedure" or "Text";

        public Task<object> GetIncomeStatement(string sqlSelect, string commandtype, params object[] sqlParameters);

        public Task<object> GetCashFlow(string sqlSelect, string commandtype, params object[] sqlParameters);

        public Task<object> GetTrialBalanceRender(string sqlSelect, string commandtype, params object[] sqlParameters);

        public Task<object> GetPayables(string sqlSelect, string commandtype, params object[] sqlParameters);

        public Task<object> GetPayablesComparative(string sqlSelect, string commandtype, params object[] sqlParameters);

        public Task<object> GetPayablesYTD(string sqlSelect, string commandtype, params object[] sqlParameters);

        public Task<object> GetReceivables(string sqlSelect, string commandtype, params object[] sqlParameters);


        public Task<object> GetReceivablesComparative(string sqlSelect, string commandtype, params object[] sqlParameters);

        public Task<object> GetReceivablesYTD(string sqlSelect, string commandtype, params object[] sqlParameters);


        public Task<object> Get10ColumnData(string sqlSelect, string commandtype, params object[] sqlParameters);

        }

    }




