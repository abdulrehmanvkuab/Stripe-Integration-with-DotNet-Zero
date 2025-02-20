
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


using Abp.Data;
using Abp.EntityFramework;
using Abp.Zero.EntityFrameworkCore;
using Arch.Authorization.Roles;
using Arch.Authorization.Users;
using Arch.MultiTenancy;
using Arch.Sql;
using System.Threading.Tasks;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using Abp.Application.Services.Dto;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Arch.EntityFrameworkCore;
//using Arch.Authorization.Users.Dtos;

using System.Configuration;
using AutoMapper;
using FastMember;

using Arch.MultiTenancy.Accounting;
using Abp.Runtime.Session;
using Org.BouncyCastle.Utilities.Collections;
//using Arch.OrganizationalUnits;
using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace Arch.Sql
{

    //None stimulsoft reports
    public class SqlFinancialReports :  EntityFrameworkCore.Repositories.ArchRepositoryBase<IEntity, int> , ISqlFinancialReports
    {

        /** This needs to be done for each Stored Procedure to be Put into the system. Define it as an Async function
         * 
         * */
        ArchDbContext Context;



        public async Task<object> LoadData(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
     {
            dynamic ret_value = new JsonObject ();
           
            dynamic row;

            int tblcount = 1;
            var table = new List<object>();

            

            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, sqlParameters)) {

              
                using (var reader = await cmd.ExecuteReaderAsync()) {
                    while (reader.Read()) {
                        row = new JsonObject();

                        for (int i = 0; i < reader.FieldCount; i++)
                            row.Add(reader.GetName(i), JsonValue.Create (reader[i]));

                        ret_value.Add(row);

                                            }
                   
               
               
                                while (reader.NextResult()) {
                                        if (reader.HasRows) {


                                            while (reader.Read()) {
                                row = new JsonObject ();

                                for (int i = 0; i < reader.FieldCount; i++)
                                    row.Add(reader.GetName(i), JsonValue.Create (reader[i]));

                                ret_value.Add(row);
                                                                }
                                                            }
                                                            }
                                }


                 }
            // table.Add(ret_value);
            return   ret_value;
        }


        public async Task<object> GetBalanceSheet(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
     {
            dynamic ret_value = new JsonObject ();
            ret_value.Assets = new JsonArray();
            ret_value.AssetTotal = 0.0;
            ret_value.AssetTotalComparative = 0.0;
            ret_value.Liabilities = new JsonArray();
            ret_value.LiabilityTotal = 0.0;
            ret_value.LiabilityTotalComparative = 0.0;
            ret_value.Equities = new JsonArray();
            ret_value.EquityTotal = 0.0;
            ret_value.EquityTotalComparative = 0.0;
            ret_value.NetIncome = 0.0;
            ret_value.EndPeriod = "";

            dynamic row;

            int tblcount = 1;
            var table = new List<object>();

            List<object> spObjects = sqlParameters.ToList();


            SqlParameter sp_ret_value = new SqlParameter("@PeriodEndDate", SqlDbType.DateTime) { Direction = ParameterDirection.Output };
     
            spObjects.Add(sp_ret_value);


            Object[] finalparams = spObjects.ToArray();


            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, finalparams)) {

                if (cmd.Parameters["@PeriodEndDate"].Value != DBNull.Value) {
                    var dtcheck = (string) cmd.Parameters["@PeriodEndDate"].Value  ;
                    if (!string.IsNullOrWhiteSpace(dtcheck)) { 
                    DateTime dt = DateTime.Parse(dtcheck);
                    ret_value.EndPeriod = dt.ToString("MMMM d, yy");
                    }
                    else 
                        ret_value.EndPeriod = ""; 
                }

                using (var reader = await cmd.ExecuteReaderAsync()) {
                    while (reader.Read()) {
                        row = new JsonObject ();

                        for (int i = 0; i < reader.FieldCount; i++)
                            row.Add(reader.GetName(i), JsonValue.Create(reader[i]));

                        if (row.GLBalanceType == "Asset") {
                            ret_value.Assets.Add(row);
                            if (reader["GLAccountBalance"] != DBNull.Value) {
                                ret_value.AssetTotal += reader["GLAccountBalance"];
                            }
                            if ((sqlSelect.Trim().ToLower().EndsWith("comparative")) && (reader["GLAccountBalanceComparative"] != DBNull.Value)) {
                                ret_value.AssetTotalComparative += reader["GLAccountBalanceComparative"];
                            }
                        }
                        if (row.GLBalanceType == "Liability") {
                            ret_value.Liabilities.Add(row);
                            if (reader["GLAccountBalance"] != DBNull.Value) {
                                ret_value.LiabilityTotal += reader["GLAccountBalance"];
                            }
                            if ((sqlSelect.Trim().ToLower().EndsWith("comparative")) && (reader["GLAccountBalanceComparative"] != DBNull.Value)) {
                                ret_value.LiabilityTotalComparative += reader["GLAccountBalanceComparative"];
                            }
                        }
                        if (row.GLBalanceType == "Equity") {
                            ret_value.Equities.Add(row);
                            if (reader["GLAccountBalance"] != DBNull.Value) {
                                ret_value.EquityTotal += reader["GLAccountBalance"];
                            }
                            if ((sqlSelect.Trim().ToLower().EndsWith("comparative")) && (reader["GLAccountBalanceComparative"] != DBNull.Value)) {
                                ret_value.EquityTotalComparative += reader["GLAccountBalanceComparative"];
                            }
                        }

                    }
                }
                ret_value.NetIncome = ret_value.AssetTotal - (ret_value.LiabilityTotal + ret_value.EquityTotal);
                ret_value.NetIncomeComparative = ret_value.AssetTotalComparative - (ret_value.LiabilityTotalComparative + ret_value.EquityTotalComparative);


/*
                while (reader.NextResult()) {
                        if (reader.HasRows) {

                            table.Add("@@NEXT@@TABLE@@" + (tblcount++).ToString());


                            while (reader.Read()) {
                                table.Add(reader);
                            }
                        }
                    }
                }
                */

            }
               // table.Add(ret_value);
            return ret_value;
        }


        public async Task<object> GetIncomeStatement(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
     {
            dynamic ret_value = new JsonObject ();
            ret_value.Incomes = new JsonArray();
            ret_value.Cogs = new JsonArray();
            ret_value.Expenses = new JsonArray();
            ret_value.EndPeriod = "";

            dynamic row;

            int tblcount = 1;
            var table = new List<object>();
            List<object> spObjects = sqlParameters.ToList();

            



            if ( sqlSelect.Trim().ToLower().EndsWith("RptGLIncomeStatementCompanyComparative".ToLower())  ) {  //Not projects

                SqlParameter sp_ret_value = new SqlParameter("@PeriodEndDate", SqlDbType.DateTime) { Direction = ParameterDirection.Output };
                spObjects.Add(sp_ret_value);

                SqlParameter sp_ret_IncomeTotal = new SqlParameter("@IncomeTotal", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(sp_ret_IncomeTotal);

                SqlParameter sp_ret_CogsTotal = new SqlParameter("@CogsTotal", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(sp_ret_CogsTotal);

                SqlParameter sp_ret_ExpenseTotal = new SqlParameter("@ExpenseTotal", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(sp_ret_ExpenseTotal);

                SqlParameter IncomeTotalComparative = new SqlParameter("@IncomeTotalComparative", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(IncomeTotalComparative);

                SqlParameter CogsTotalComparative = new SqlParameter("@CogsTotalComparative", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(CogsTotalComparative);

                SqlParameter ExpenseTotalComparative = new SqlParameter("@ExpenseTotalComparative", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(ExpenseTotalComparative);


                
            }

            else if (sqlSelect.Trim().ToLower().EndsWith("RptGLIncomeStatementDivisionComparative".ToLower())) {
                SqlParameter sp_ret_value = new SqlParameter("@PeriodEndDate", SqlDbType.DateTime) { Direction = ParameterDirection.Output };
                spObjects.Add(sp_ret_value);

                SqlParameter TradingIncomeTotal = new SqlParameter("@TradingIncomeTotal", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(TradingIncomeTotal);

                SqlParameter TradingExpenseTotal = new SqlParameter("@TradingExpenseTotal", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(TradingExpenseTotal);

                SqlParameter IncomeTotal = new SqlParameter("@IncomeTotal", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(IncomeTotal);

                SqlParameter ExpenseTotal = new SqlParameter("@ExpenseTotal", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(ExpenseTotal);

               


                SqlParameter TradingIncomeTotalComparative = new SqlParameter("@TradingIncomeTotalComparative", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(TradingIncomeTotalComparative);

                SqlParameter TradingExpenseTotalComparative = new SqlParameter("@TradingExpenseTotalComparative", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(TradingExpenseTotalComparative);
                SqlParameter IncomeTotalComparative = new SqlParameter("@IncomeTotalComparative", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(IncomeTotalComparative);

                SqlParameter ExpenseTotalComparative = new SqlParameter("@ExpenseTotalComparative", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(ExpenseTotalComparative);


               
            }


            else if (sqlSelect.Trim().ToLower().EndsWith("RptGLIncomeStatementPeriodCompanyYTD".ToLower()) || sqlSelect.Trim().ToLower().EndsWith("RptGLIncomeStatementPeriodDivisionYTD".ToLower())) {
                SqlParameter sp_ret_value = new SqlParameter("@PeriodEndDate", SqlDbType.DateTime) { Direction = ParameterDirection.Output };
                spObjects.Add(sp_ret_value);


                SqlParameter sp_ret_IncomeTotal = new SqlParameter("@IncomeTotal", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(sp_ret_IncomeTotal);

                SqlParameter sp_ret_CogsTotal = new SqlParameter("@CogsTotal", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(sp_ret_CogsTotal);

                SqlParameter sp_ret_ExpenseTotal = new SqlParameter("@ExpenseTotal", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(sp_ret_ExpenseTotal);

                SqlParameter IncomeTotalYTD = new SqlParameter("@IncomeTotalYTD", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(IncomeTotalYTD);

                SqlParameter CogsTotalYTD = new SqlParameter("@CogsTotalYTD", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(CogsTotalYTD);

                SqlParameter ExpenseTotalYTD = new SqlParameter("@ExpenseTotalYTD", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(ExpenseTotalYTD);
               
            }

            else if (!sqlSelect.Trim().ToLower().Contains("RptGLIncomeStatementRange".ToLower()) ) {
                SqlParameter sp_ret_value = new SqlParameter("@PeriodEndDate", SqlDbType.DateTime) { Direction = ParameterDirection.Output };
                spObjects.Add(sp_ret_value);

                SqlParameter TradingIncomeTotal = new SqlParameter("@TradingIncomeTotal", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(TradingIncomeTotal);

                SqlParameter TradingExpenseTotal = new SqlParameter("@TradingExpenseTotal", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(TradingExpenseTotal);

                SqlParameter IncomeTotal = new SqlParameter("@IncomeTotal", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(IncomeTotal);

                SqlParameter ExpenseTotal = new SqlParameter("@ExpenseTotal", SqlDbType.Money) { Direction = ParameterDirection.Output };
                spObjects.Add(ExpenseTotal);


            }




            Object[] finalparams = spObjects.ToArray();


            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, finalparams)) {

                if (!sqlSelect.Trim().ToLower().Contains("RptGLIncomeStatementRange".ToLower())) {
                    if (cmd.Parameters["@PeriodEndDate"].Value != DBNull.Value) {
                        var dtcheck = (string)cmd.Parameters["@PeriodEndDate"].Value;
                        if (!string.IsNullOrWhiteSpace(dtcheck)) {
                            DateTime dt = DateTime.Parse(dtcheck);
                            ret_value.EndPeriod = dt.ToString("MMMM d, yy");
                        }
                        else

                            ret_value.EndPeriod = "";
                    }
                }
                else {
                    if (cmd.Parameters["@PeriodStop"].Value != DBNull.Value) {
                        var dtcheck = (string)cmd.Parameters["@PeriodStop"].Value;
                        if (!string.IsNullOrWhiteSpace(dtcheck)) {
                            DateTime dt = DateTime.Parse(dtcheck);
                            ret_value.EndPeriod = dt.ToString("MMMM d, yy");
                        }
                        else

                            ret_value.EndPeriod = "";
                    }
                }






                if (sqlSelect.Trim().ToLower().EndsWith("RptGLIncomeStatementCompanyComparative".ToLower())) {  //Not projects

                    ret_value.IncomeTotal = Convert.ToDecimal(cmd.Parameters["@IncomeTotal"].Value);
                    ret_value.CogsTotal = Convert.ToDecimal(cmd.Parameters["@CogsTotal"].Value);
                    ret_value.ExpenseTotal = Convert.ToDecimal(cmd.Parameters["@ExpenseTotal"].Value);

                    ret_value.IncomeTotalComparative = Convert.ToDecimal(cmd.Parameters["@IncomeTotalComparative"].Value);
                    ret_value.CogsTotalComparative = Convert.ToDecimal(cmd.Parameters["@CogsTotalComparative"].Value);
                    ret_value.ExpenseTotalComparative = Convert.ToDecimal(cmd.Parameters["@ExpenseTotalComparative"].Value);

                }

                else if (sqlSelect.Trim().ToLower().EndsWith("RptGLIncomeStatementDivisionComparative".ToLower())) {
                    ret_value.TradingIncomeTotal = Convert.ToDecimal(cmd.Parameters["@TradingIncomeTotal"].Value);
                    ret_value.IncomeTotal = Convert.ToDecimal(cmd.Parameters["@IncomeTotal"].Value);
                    ret_value.TradingExpenseTotal = Convert.ToDecimal(cmd.Parameters["@TradingExpenseTotal"].Value);
                    ret_value.ExpenseTotal = Convert.ToDecimal(cmd.Parameters["@ExpenseTotal"].Value);

                    ret_value.TradingIncomeTotalComparative = Convert.ToDecimal(cmd.Parameters["@TradingIncomeTotalComparative"].Value);
                    ret_value.TradingExpenseTotalComparative = Convert.ToDecimal(cmd.Parameters["@TradingExpenseTotalComparative"].Value);
                    ret_value.IncomeTotalComparative = Convert.ToDecimal(cmd.Parameters["@IncomeTotalComparative"].Value);
                    ret_value.ExpenseTotalComparative = Convert.ToDecimal(cmd.Parameters["@ExpenseTotalComparative"].Value);

                    
                }

                

                else if (sqlSelect.Trim().ToLower().EndsWith("RptGLIncomeStatementPeriodCompanyYTD".ToLower()) || sqlSelect.Trim().ToLower().EndsWith("RptGLIncomeStatementPeriodDivisionYTD".ToLower()) ) {
                    ret_value.IncomeTotal = Convert.ToDecimal(cmd.Parameters["@IncomeTotal"].Value);
                    ret_value.CogsTotal = Convert.ToDecimal(cmd.Parameters["@CogsTotal"].Value);
                    ret_value.ExpenseTotal = Convert.ToDecimal(cmd.Parameters["@ExpenseTotal"].Value);

                    ret_value.IncomeTotalYTD = Convert.ToDecimal(cmd.Parameters["@IncomeTotalYTD"].Value);
                    ret_value.CogsTotalYTD = Convert.ToDecimal(cmd.Parameters["@CogsTotalYTD"].Value);
                    ret_value.ExpenseTotalYTD = Convert.ToDecimal(cmd.Parameters["@ExpenseTotalYTD"].Value);
                }

                else if (!sqlSelect.Trim().ToLower().Contains("RptGLIncomeStatementRange".ToLower())) {
                    ret_value.TradingIncomeTotal = Convert.ToDecimal(cmd.Parameters["@TradingIncomeTotal"].Value);
                    ret_value.IncomeTotal = Convert.ToDecimal(cmd.Parameters["@IncomeTotal"].Value);
                    ret_value.TradingExpenseTotal = Convert.ToDecimal(cmd.Parameters["@TradingExpenseTotal"].Value);
                    ret_value.ExpenseTotal = Convert.ToDecimal(cmd.Parameters["@ExpenseTotal"].Value);

                }





                using (var reader = await cmd.ExecuteReaderAsync()) {
                    while (reader.Read()) {
                        row = new JsonObject ();

                        for (int i = 0; i < reader.FieldCount; i++)
                            row.Add(reader.GetName(i), JsonValue.Create (reader[i]));

                        if (row.GLBalanceType == "Income") {
                            ret_value.Incomes.Add(row);
                        }

                        if ((row.GLBalanceType == "Expense") && (row.GLAccountType.ToString().ToLower().Contains("trading"))) {
                            ret_value.Cogs.Add(row);
                        }
                        if ((row.GLBalanceType == "Expense") && (!row.GLAccountType.ToString().ToLower().Contains("trading"))) {
                            ret_value.Expenses.Add(row);
                        }

                    }
                  }

            }
           // table.Add(ret_value);
            return ret_value;
        }


        public async Task<object> GetCashFlow(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
     {

            dynamic ret_value = new JsonObject ();
            ret_value.OperatingActivities = new JsonArray();
            ret_value.OperatingActivitiesCreditTotal = 0.0;
            ret_value.OperatingActivitiesDebitTotal = 0.0;
            ret_value.OperatingActivitiesTotal = 0.0;
            ret_value.OperatingActivitiesCreditTotalComparative = 0.0;
            ret_value.OperatingActivitiesDebitTotalComparative = 0.0;
            ret_value.OperatingActivitiesTotalComparative = 0.0;
            ret_value.OperatingActivitiesCreditTotalYTD = 0.0;
            ret_value.OperatingActivitiesDebitTotalYTD = 0.0;
            ret_value.OperatingActivitiesTotalYTD = 0.0;


            ret_value.InvestingActivities = new JsonArray();
            ret_value.InvestingActivitiesCreditTotal = 0.0;
            ret_value.InvestingActivitiesDebitTotal = 0.0;
            ret_value.InvestingActivitiesTotal = 0.0;
            ret_value.InvestingActivitiesCreditTotalComparative = 0.0;
            ret_value.InvestingActivitiesDebitTotalComparative = 0.0;
            ret_value.InvestingActivitiesTotalComparative = 0.0;
            ret_value.InvestingActivitiesCreditTotalYTD = 0.0;
            ret_value.InvestingActivitiesDebitTotalYTD = 0.0;
            ret_value.InvestingActivitiesTotalYTD = 0.0;

            ret_value.FinancingActivities = new JsonArray();
            ret_value.FinancingActivitiesCreditTotal = 0.0;
            ret_value.FinancingActivitiesDebitTotal = 0.0;
            ret_value.FinancingActivitiesTotal = 0.0;
            ret_value.FinancingActivitiesCreditTotalComparative = 0.0;
            ret_value.FinancingActivitiesDebitTotalComparative = 0.0;
            ret_value.FinancingActivitiesTotalComparative = 0.0;
            ret_value.FinancingActivitiesCreditTotalYTD = 0.0;
            ret_value.FinancingActivitiesDebitTotalYTD = 0.0;
            ret_value.FinancingActivitiesTotalYTD = 0.0;

            ret_value.BeginingYearBalance = 0.0;
            ret_value.TotalYearBalance = 0.0;
            ret_value.EndYearBalance = 0.0;
            ret_value.BeginingYearBalanceComparative = 0.0;
            ret_value.TotalYearBalanceComparative = 0.0;
            ret_value.EndYearBalanceComparative = 0.0;
            ret_value.BeginingYearBalanceYTD = 0.0;
            ret_value.TotalYearBalanceYTD = 0.0;
            ret_value.EndYearBalanceYTD = 0.0;

            dynamic row;
        

            int tblcount = 1;
            var table = new List<object>();
            List<object> spObjects = sqlParameters.ToList();

            SqlParameter sp_ret_value = new SqlParameter("@PeriodEndDate", SqlDbType.DateTime) { Direction = ParameterDirection.Output };
            spObjects.Add(sp_ret_value);

            Object[] finalparams = spObjects.ToArray();


            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, finalparams)) {

                if (cmd.Parameters["@PeriodEndDate"].Value != DBNull.Value) {
                    var dtcheck = (string)cmd.Parameters["@PeriodEndDate"].Value;
                    if (!string.IsNullOrWhiteSpace(dtcheck)) {
                        DateTime dt = DateTime.Parse(dtcheck);
                        ret_value.EndPeriod = dt.ToString("MMMM d, yy");
                    }
                    else
                        ret_value.EndPeriod = "";
                }



                using (var reader = await cmd.ExecuteReaderAsync()) {
                    while (reader.Read()) {
                        row = new JsonObject ();

                        for (int i = 0; i < reader.FieldCount; i++)
                            row.Add(reader.GetName(i), reader[i] == DBNull.Value ? JsonValue.Create ("") : JsonValue.Create (reader[i]));

                        if (row.CashFlowType == "Operating Activities") {
                            ret_value.OperatingActivities.Add(row);

                            if (reader["Credit"] != DBNull.Value) {
                                ret_value.OperatingActivitiesCreditTotal += reader["Credit"];
                            }
                            if (reader["Debit"] != DBNull.Value) {
                                ret_value.OperatingActivitiesDebitTotal += reader["Debit"];
                            }
                            if ((sqlSelect.Trim().ToLower().EndsWith("comparative")) && (reader["CreditComparative"] != DBNull.Value)) {
                                ret_value.OperatingActivitiesCreditTotalComparative += reader["CreditComparative"];
                            }
                            if ((sqlSelect.Trim().ToLower().EndsWith("comparative")) && (reader["DebitComparative"] != DBNull.Value)) {
                                ret_value.OperatingActivitiesDebitTotalComparative += reader["DebitComparative"];
                            }
                            if ((sqlSelect.Trim().ToLower().EndsWith("ytd")) && (reader["CreditYTD"] != DBNull.Value)) {
                                ret_value.OperatingActivitiesCreditTotalYTD += reader["CreditYTD"];
                            }
                            if ((sqlSelect.Trim().ToLower().EndsWith("ytd")) && (reader["DebitYTD"] != DBNull.Value)) {
                                ret_value.OperatingActivitiesDebitTotalYTD += reader["DebitYTD"];
                            }
                        }

                        if (row.CashFlowType == "Investing Activities") {
                            ret_value.InvestingActivities.Add(row);

                            if (reader["Credit"] != DBNull.Value) {
                                ret_value.InvestingActivitiesCreditTotal += reader["Credit"];
                            }
                            if (reader["Debit"] != DBNull.Value) {
                                ret_value.InvestingActivitiesDebitTotal += reader["Debit"];
                            }
                            if ((sqlSelect.Trim().ToLower().EndsWith("comparative")) && (reader["CreditComparative"] != DBNull.Value)) {
                                ret_value.InvestingActivitiesCreditTotalComparative += reader["CreditComparative"];
                            }
                            if ((sqlSelect.Trim().ToLower().EndsWith("comparative")) && (reader["DebitComparative"] != DBNull.Value)) {
                                ret_value.InvestingActivitiesDebitTotalComparative += reader["DebitComparative"];
                            }
                            if ((sqlSelect.Trim().ToLower().EndsWith("ytd")) && (reader["CreditYTD"] != DBNull.Value)) {
                                ret_value.InvestingActivitiesCreditTotaYTD += reader["CreditYTD"];
                            }
                            if ((sqlSelect.Trim().ToLower().EndsWith("ytd")) && (reader["DebitYTD"] != DBNull.Value)) {
                                ret_value.InvestingActivitiesDebitTotalYTD += reader["DebitYTD"];
                            }
                        }

                        if (row.CashFlowType == "Financing Activities") {
                            ret_value.FinancingActivities.Add(row);

                            if (reader["Credit"] != DBNull.Value) {
                                ret_value.FinancingActivitiesCreditTotal += reader["Credit"];
                            }
                            if (reader["Debit"] != DBNull.Value) {
                                ret_value.FinancingActivitiesDebitTotal += reader["Debit"];
                            }
                            if ((sqlSelect.Trim().ToLower().EndsWith("comparative")) && (reader["CreditComparative"] != DBNull.Value)) {
                                ret_value.FinancingActivitiesCreditTotalComparative += reader["CreditComparative"];
                            }
                            if ((sqlSelect.Trim().ToLower().EndsWith("comparative")) && (reader["DebitComparative"] != DBNull.Value)) {
                                ret_value.FinancingActivitiesDebitTotalComparative += reader["DebitComparative"];
                            }
                            if ((sqlSelect.Trim().ToLower().EndsWith("ytd")) && (reader["CreditYTD"] != DBNull.Value)) {
                                ret_value.FinancingActivitiesCreditTotalYTD += reader["CreditYTD"];
                            }
                            if ((sqlSelect.Trim().ToLower().EndsWith("ytd")) && (reader["DebitComparative"] != DBNull.Value)) {
                                ret_value.FinancingActivitiesDebitTotalYTD += reader["DebitYTD"];
                            }
                        }

                        if (row.CashFlowType == "BeginningYearBalance") {
                            ret_value.BeginingYearBalance += reader["Total"];
                            if ((sqlSelect.Trim().ToLower().EndsWith("comparative")) && (reader["TotalComparative"] != DBNull.Value)) {
                                ret_value.BeginingYearBalanceComparative += reader["TotalComparative"];
                            }
                            if ((sqlSelect.Trim().ToLower().EndsWith("ytd")) && (reader["TotalYTD"] != DBNull.Value)) {
                                ret_value.BeginingYearBalanceYTD += reader["TotalYTD"];
                            }
                        }
                    }
                }


                ret_value.OperatingActivitiesTotal = ret_value.OperatingActivitiesDebitTotal + ret_value.OperatingActivitiesCreditTotal;
                ret_value.InvestingActivitiesTotal = ret_value.InvestingActivitiesDebitTotal + ret_value.InvestingActivitiesCreditTotal;
                ret_value.FinancingActivitiesTotal = ret_value.FinancingActivitiesDebitTotal + ret_value.FinancingActivitiesCreditTotal;

                ret_value.TotalYearBalance = ret_value.OperatingActivitiesTotal + ret_value.InvestingActivitiesTotal + ret_value.FinancingActivitiesTotal;
                ret_value.EndYearBalance = ret_value.BeginingYearBalance + ret_value.TotalYearBalance;


                if (sqlSelect.Trim().ToLower().EndsWith("comparative")) {
                    ret_value.OperatingActivitiesTotalComparative = ret_value.OperatingActivitiesDebitTotalComparative + ret_value.OperatingActivitiesCreditTotalComparative;
                    ret_value.InvestingActivitiesTotalComparative = ret_value.InvestingActivitiesDebitTotalComparative + ret_value.InvestingActivitiesCreditTotalComparative;
                    ret_value.FinancingActivitiesTotalComparative = ret_value.FinancingActivitiesDebitTotalComparative + ret_value.FinancingActivitiesCreditTotalComparative;

                    ret_value.TotalYearBalanceComparative = ret_value.OperatingActivitiesTotalComparative + ret_value.InvestingActivitiesTotalComparative + ret_value.FinancingActivitiesTotalComparative;
                    ret_value.EndYearBalanceComparative = ret_value.BeginingYearBalanceComparative + ret_value.TotalYearBalanceComparative;
                }

                if (sqlSelect.Trim().ToLower().EndsWith("ytd")) {
                    ret_value.OperatingActivitiesTotalYTD = ret_value.OperatingActivitiesDebitTotalYTD + ret_value.OperatingActivitiesCreditTotalYTD;
                    ret_value.InvestingActivitiesTotalYTD = ret_value.InvestingActivitiesDebitTotalYTD + ret_value.InvestingActivitiesCreditTotalYTD;
                    ret_value.FinancingActivitiesTotalYTD = ret_value.FinancingActivitiesDebitTotalYTD + ret_value.FinancingActivitiesCreditTotalYTD;

                    ret_value.TotalYearBalanceYTD = ret_value.OperatingActivitiesTotalYTD + ret_value.InvestingActivitiesTotalYTD + ret_value.FinancingActivitiesTotalYTD;
                    ret_value.EndYearBalanceYTD = ret_value.BeginingYearBalanceYTD + ret_value.TotalYearBalanceYTD;
                }

            }
           // table.Add(ret_value);
            return ret_value;
        }



        public async Task<object> GetTrialBalanceRender(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
     {
            dynamic ret_value = new JsonObject ();
            ret_value.Debits = new JsonArray();
            ret_value.Credits = new JsonArray();
            ret_value.EndPeriod = "";
            ret_value.DebitTotal = 0;
            ret_value.CreditTotal = 0;
            ret_value.DebitTotalComparative = 0;
            ret_value.CreditTotalComparative = 0;
            ret_value.DebitTotalYTD = 0;
            ret_value.CreditTotalYTD = 0;

            dynamic row;

            int tblcount = 1;
            var table = new List<object>();
            List<object> spObjects = sqlParameters.ToList();

            SqlParameter sp_ret_value = new SqlParameter("@PeriodEndDate", SqlDbType.DateTime) { Direction = ParameterDirection.Output };
            spObjects.Add(sp_ret_value);

            Object[] finalparams = spObjects.ToArray();


            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, finalparams)) {

                if (cmd.Parameters["@PeriodEndDate"].Value != DBNull.Value) {
                    var dtcheck = (string)cmd.Parameters["@PeriodEndDate"].Value;
                    if (!string.IsNullOrWhiteSpace(dtcheck)) {
                        DateTime dt = DateTime.Parse(dtcheck);
                        ret_value.EndPeriod = dt.ToString("MMMM d, yy");
                    }
                    else
                        ret_value.EndPeriod = "";
                }


                using (var reader = await cmd.ExecuteReaderAsync()) {
                    while (reader.Read()) {
                        row = new JsonObject ();

                        for (int i = 0; i < reader.FieldCount; i++)
                            row.Add(reader.GetName(i), JsonValue.Create (reader[i]));

                        if (row.GLBalanceType == "Debit") {
                            ret_value.Debits.Add(row);

                            ret_value.DebitTotal += reader["GLAccountBalance"];

                            if ((sqlSelect.Trim().ToLower().EndsWith("comparative")) && (reader["GLAccountBalanceComparative"] != DBNull.Value)) {
                                ret_value.DebitTotalComparative += reader["GLAccountBalanceComparative"];
                            }

                            if ((sqlSelect.Trim().ToLower().EndsWith("ytd")) && (reader["GLAccountBalanceYTD"] != DBNull.Value)) {
                                ret_value.DebitTotalYTD += reader["GLAccountBalanceYTD"];
                            }
                        }

                        if (row.GLBalanceType == "Credit") {
                            ret_value.Credits.Add(row);

                            ret_value.CreditTotal += reader["GLAccountBalance"];

                            if ((sqlSelect.Trim().ToLower().EndsWith("comparative")) && (reader["GLAccountBalanceComparative"] != DBNull.Value)) {
                                ret_value.CreditTotalComparative += reader["GLAccountBalanceComparative"];
                            }

                            if ((sqlSelect.Trim().ToLower().EndsWith("ytd")) && (reader["GLAccountBalanceYTD"] != DBNull.Value)) {
                                ret_value.CreditTotalYTD += reader["GLAccountBalanceYTD"];
                            }
                        }
                    }

                }

            }
          //  table.Add(ret_value);
            return ret_value;
        }



/*
 * AGED REPORTS SECTION
 */
        public async Task<object> GetPayables(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
     {
            dynamic result = new JsonObject ();

            int tblcount = 1;
            var table = new List<object>();
           

            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, sqlParameters)) {


                using (var reader = await cmd.ExecuteReaderAsync()) {

                    #region Summary Part

                    var sumArray = new JsonArray();

                    while (reader.Read()) {
                        dynamic summary = new JsonObject ();
                        summary.VendorId = reader["VendorID"] != DBNull.Value ? reader["VendorID"] : (string)null;
                        summary.VendorName = reader["VendorName"] != DBNull.Value ? reader["VendorName"] : (string)null;
                        summary.Under30 = reader["Under30"];
                        summary.Over30 = reader["Over30"];
                        summary.Over60 = reader["Over60"];
                        summary.Over90 = reader["Over90"];
                        summary.Over120 = reader["Over120"];
                        summary.Over150 = reader["Over150"];
                        summary.Over180 = reader["Over180"];

                        sumArray.Add(summary);
                    }

                    #endregion

                    #region Totals Part

                    reader.NextResult();
                    reader.Read();

                    dynamic totals = new JsonArray();
                    dynamic totalRow = new JsonObject ();
                    totalRow.Under30 = reader["Under30"];
                    totalRow.Over30 = reader["Over30"];
                    totalRow.Over60 = reader["Over60"];
                    totalRow.Over90 = reader["Over90"];
                    totalRow.Over120 = reader["Over120"];
                    totalRow.Over150 = reader["Over150"];
                    totalRow.Over180 = reader["Over180"];
                    totalRow.Totals = reader["Totals"];

                    totals.Add(totalRow);

                    #endregion

                    #region Details Part

                    var detArray = new JsonArray();

                    reader.NextResult();
                    while (reader.Read()) {
                        dynamic details = new JsonObject ();
                        details.VendorId = reader["VendorID"] != DBNull.Value ? reader["VendorID"] : (string)null;
                        details.VendorName = reader["VendorName"] != DBNull.Value ? reader["VendorName"] : (string)null;
                        details.Reference = reader["Reference"];
                        details.Under30 = reader["Under30"];
                        details.Over30 = reader["Over30"];
                        details.Over60 = reader["Over60"];
                        details.Over90 = reader["Over90"];
                        details.Over120 = reader["Over120"];
                        details.Over150 = reader["Over150"];
                        details.Over180 = reader["Over180"];

                        detArray.Add(details);
                    }

                    #endregion

                    result.Summary = sumArray;
                    result.Totals = totals;
                    result.Details = detArray;
                }

            }
           // table.Add(result);
            return result;
        }




        public async Task<object> GetPayablesComparative(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
     {
            dynamic result = new JsonObject ();

            int tblcount = 1;
            var table = new List<object>();


            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, sqlParameters)) {


                using (var reader = await cmd.ExecuteReaderAsync()) {

                    #region Summary Part

                    var sumArray = new JsonArray();

                    while (reader.Read()) {
                        dynamic summary = new JsonObject ();
                        summary.VendorId = reader["VendorID"] != DBNull.Value ? reader["VendorID"] : (string)null;
                        summary.VendorName = reader["VendorName"] != DBNull.Value ? reader["VendorName"] : (string)null;
                        summary.Under30 = reader["Under30"];
                        summary.Over30 = reader["Over30"];
                        summary.Over60 = reader["Over60"];
                        summary.Over90 = reader["Over90"];
                        summary.Over120 = reader["Over120"];
                        summary.Over150 = reader["Over150"];
                        summary.Over180 = reader["Over180"];

                        sumArray.Add(summary);
                    }

                    #endregion

                    #region Totals Part

                    reader.NextResult();
                    reader.Read();

                    dynamic totals = new JsonArray();
                    dynamic totalRow = new JsonObject ();
                    totalRow.Under30 = reader["Under30"];
                    totalRow.Over30 = reader["Over30"];
                    totalRow.Over60 = reader["Over60"];
                    totalRow.Over90 = reader["Over90"];
                    totalRow.Over120 = reader["Over120"];
                    totalRow.Over150 = reader["Over150"];
                    totalRow.Over180 = reader["Over180"];
                    totalRow.Totals = reader["Totals"];

                    totals.Add(totalRow);

                    #endregion

                    #region Details Part

                    var detArray = new JsonArray();

                    reader.NextResult();
                    while (reader.Read()) {
                        dynamic details = new JsonObject ();
                        details.VendorId = reader["VendorID"] != DBNull.Value ? reader["VendorID"] : (string)null;
                        details.VendorName = reader["VendorName"] != DBNull.Value ? reader["VendorName"] : (string)null;
                        details.Reference = reader["Reference"];
                        details.Under30 = reader["Under30"];
                        details.Over30 = reader["Over30"];
                        details.Over60 = reader["Over60"];
                        details.Over90 = reader["Over90"];
                        details.Over120 = reader["Over120"];
                        details.Over150 = reader["Over150"];
                        details.Over180 = reader["Over180"];

                        detArray.Add(details);
                    }

                    #endregion

                    result.Summary = sumArray;
                    result.Totals = totals;
                    result.Details = detArray;
                }

            }
         //   table.Add(result);
            return result;
        }


        public async Task<object> GetPayablesYTD(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
     {
            dynamic result = new JsonObject ();

            int tblcount = 1;
            var table = new List<object>();


            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, sqlParameters)) {


                using (var reader = await cmd.ExecuteReaderAsync()) {

                    #region Summary Part

                    var sumArray = new JsonArray();

                    while (reader.Read()) {
                        dynamic summary = new JsonObject ();
                        summary.VendorId = reader["VendorID"] != DBNull.Value ? reader["VendorID"] : (string)null;
                        summary.VendorName = reader["VendorName"] != DBNull.Value ? reader["VendorName"] : (string)null;
                        summary.Under30 = reader["Under30"];
                        summary.Over30 = reader["Over30"];
                        summary.Over60 = reader["Over60"];
                        summary.Over90 = reader["Over90"];
                        summary.Over120 = reader["Over120"];
                        summary.Over150 = reader["Over150"];
                        summary.Over180 = reader["Over180"];
                        summary.YTD = reader["YTD"];

                        sumArray.Add(summary);
                    }

                    #endregion

                    #region Totals Part

                    reader.NextResult();
                    reader.Read();

                    dynamic totals = new JsonArray();
                    dynamic totalRow = new JsonObject ();
                    totalRow.Under30 = reader["Under30"];
                    totalRow.Over30 = reader["Over30"];
                    totalRow.Over60 = reader["Over60"];
                    totalRow.Over90 = reader["Over90"];
                    totalRow.Over120 = reader["Over120"];
                    totalRow.Over150 = reader["Over150"];
                    totalRow.Over180 = reader["Over180"];
                    totalRow.Totals = reader["Totals"];

                    totals.Add(totalRow);

                    #endregion

                    #region Details Part

                    var detArray = new JsonArray();

                    reader.NextResult();
                    while (reader.Read()) {
                        dynamic details = new JsonObject ();
                        details.VendorId = reader["VendorID"] != DBNull.Value ? reader["VendorID"] : (string)null;
                        details.VendorName = reader["VendorName"] != DBNull.Value ? reader["VendorName"] : (string)null;
                        details.Reference = reader["Reference"];
                        details.Under30 = reader["Under30"];
                        details.Over30 = reader["Over30"];
                        details.Over60 = reader["Over60"];
                        details.Over90 = reader["Over90"];
                        details.Over120 = reader["Over120"];
                        details.Over150 = reader["Over150"];
                        details.Over180 = reader["Over180"];

                        detArray.Add(details);
                    }

                    #endregion

                    result.Summary = sumArray;
                    result.Totals = totals;
                    result.Details = detArray;
                }

            }
         //   table.Add(result);
            return result;
        }


        public async Task<object> GetReceivables(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
     {
            dynamic result = new JsonObject ();

            int tblcount = 1;
            var table = new List<object>();


            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, sqlParameters)) {


                using (var reader = await cmd.ExecuteReaderAsync()) {

                    #region Summary Part

                    var sumArray = new JsonArray();

                    while (reader.Read()) {
                        dynamic summary = new JsonObject ();
                        summary.CustomerId = reader["CustomerID"] != DBNull.Value ? reader["CustomerID"] : (string)null;
                        summary.CustomerName = reader["CustomerName"] != DBNull.Value ? reader["CustomerName"] : (string)null;
                        summary.Under30 = reader["Under30"];
                        summary.Over30 = reader["Over30"];
                        summary.Over60 = reader["Over60"];
                        summary.Over90 = reader["Over90"];
                        summary.Over120 = reader["Over120"];
                        summary.Over150 = reader["Over150"];
                        summary.Over180 = reader["Over180"];

                        sumArray.Add(summary);
                    }

                    #endregion

                    #region Totals Part

                    reader.NextResult();
                    reader.Read();

                    dynamic totals = new JsonArray();
                    dynamic totalRow = new JsonObject ();
                    totalRow.Under30 = reader["Under30"];
                    totalRow.Over30 = reader["Over30"];
                    totalRow.Over60 = reader["Over60"];
                    totalRow.Over90 = reader["Over90"];
                    totalRow.Over120 = reader["Over120"];
                    totalRow.Over150 = reader["Over150"];
                    totalRow.Over180 = reader["Over180"];
                    totalRow.Totals = reader["Totals"];

                    totals.Add(totalRow);

                    #endregion

                    #region Details Part

                    var detArray = new JsonArray();

                    reader.NextResult();
                    while (reader.Read()) {
                        dynamic details = new JsonObject ();
                        details.CustomerId = reader["CustomerID"] != DBNull.Value ? reader["CustomerID"] : (string)null;
                        details.CustomerName = reader["CustomerName"] != DBNull.Value ? reader["CustomerName"] : (string)null;
                        details.Reference = reader["Reference"];
                        details.Under30 = reader["Under30"];
                        details.Over30 = reader["Over30"];
                        details.Over60 = reader["Over60"];
                        details.Over90 = reader["Over90"];
                        details.Over120 = reader["Over120"];
                        details.Over150 = reader["Over150"];
                        details.Over180 = reader["Over180"];

                        detArray.Add(details);
                    }

                    #endregion

                    result.Summary = sumArray;
                    result.Totals = totals;
                    result.Details = detArray;

                }

            }
            //table.Add(result);
            return result;
        }


        public async Task<object> GetReceivablesComparative(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
     {
            dynamic result = new JsonObject ();

            int tblcount = 1;
            var table = new List<object>();


            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, sqlParameters)) {


                using (var reader = await cmd.ExecuteReaderAsync()) {

                    #region Summary Part

                    var sumArray = new JsonArray();

                    while (reader.Read()) {
                        dynamic summary = new JsonObject ();
                        summary.CustomerId = reader["CustomerID"] != DBNull.Value ? reader["CustomerID"] : (string)null;
                        summary.CustomerName = reader["CustomerName"] != DBNull.Value ? reader["CustomerName"] : (string)null;
                        summary.Under30 = reader["Under30"];
                        summary.Over30 = reader["Over30"];
                        summary.Over60 = reader["Over60"];
                        summary.Over90 = reader["Over90"];
                        summary.Over120 = reader["Over120"];
                        summary.Over150 = reader["Over150"];
                        summary.Over180 = reader["Over180"];

                        sumArray.Add(summary);
                    }

                    #endregion

                    #region Totals Part

                    reader.NextResult();
                    reader.Read();

                    dynamic totals = new JsonArray();
                    dynamic totalRow = new JsonObject ();
                    totalRow.Under30 = reader["Under30"];
                    totalRow.Over30 = reader["Over30"];
                    totalRow.Over60 = reader["Over60"];
                    totalRow.Over90 = reader["Over90"];
                    totalRow.Over120 = reader["Over120"];
                    totalRow.Over150 = reader["Over150"];
                    totalRow.Over180 = reader["Over180"];
                    totalRow.Totals = reader["Totals"];

                    totals.Add(totalRow);

                    #endregion

                    #region Details Part

                    var detArray = new JsonArray();

                    reader.NextResult();
                    while (reader.Read()) {
                        dynamic details = new JsonObject ();
                        details.CustomerId = reader["CustomerID"] != DBNull.Value ? reader["CustomerID"] : (string)null;
                        details.CustomerName = reader["CustomerName"] != DBNull.Value ? reader["CustomerName"] : (string)null;
                        details.Reference = reader["Reference"];
                        details.Under30 = reader["Under30"];
                        details.Over30 = reader["Over30"];
                        details.Over60 = reader["Over60"];
                        details.Over90 = reader["Over90"];
                        details.Over120 = reader["Over120"];
                        details.Over150 = reader["Over150"];
                        details.Over180 = reader["Over180"];

                        detArray.Add(details);
                    }

                    #endregion

                    result.Summary = sumArray;
                    result.Totals = totals;
                    result.Details = detArray;

                }

            }
        //    table.Add(result);
            return result;
        }



        public async Task<object> GetReceivablesYTD(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
     {
            dynamic result = new JsonObject ();

            int tblcount = 1;
            var table = new List<object>();


            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, sqlParameters)) {


                using (var reader = await cmd.ExecuteReaderAsync()) {


                    #region Summary Part

                    var sumArray = new JsonArray();

                    while (reader.Read()) {
                        dynamic summary = new JsonObject ();
                        summary.CustomerId = reader["CustomerID"] != DBNull.Value ? reader["CustomerID"] : (string)null;
                        summary.CustomerName = reader["CustomerName"] != DBNull.Value ? reader["CustomerName"] : (string)null;
                        summary.Under30 = reader["Under30"];
                        summary.Over30 = reader["Over30"];
                        summary.Over60 = reader["Over60"];
                        summary.Over90 = reader["Over90"];
                        summary.Over120 = reader["Over120"];
                        summary.Over150 = reader["Over150"];
                        summary.Over180 = reader["Over180"];

                        sumArray.Add(summary);
                    }

                    #endregion

                    #region Totals Part

                    reader.NextResult();
                    reader.Read();

                    dynamic totals = new JsonArray();
                    dynamic totalRow = new JsonObject ();
                    totalRow.Under30 = reader["Under30"];
                    totalRow.Over30 = reader["Over30"];
                    totalRow.Over60 = reader["Over60"];
                    totalRow.Over90 = reader["Over90"];
                    totalRow.Over120 = reader["Over120"];
                    totalRow.Over150 = reader["Over150"];
                    totalRow.Over180 = reader["Over180"];
                    totalRow.Totals = reader["Totals"];

                    totals.Add(totalRow);

                    #endregion

                    #region Details Part

                    var detArray = new JsonArray();

                    reader.NextResult();
                    while (reader.Read()) {
                        dynamic details = new JsonObject ();
                        details.CustomerId = reader["CustomerID"] != DBNull.Value ? reader["CustomerID"] : (string)null;
                        details.CustomerName = reader["CustomerName"] != DBNull.Value ? reader["CustomerName"] : (string)null;
                        details.Reference = reader["Reference"];
                        details.Under30 = reader["Under30"];
                        details.Over30 = reader["Over30"];
                        details.Over60 = reader["Over60"];
                        details.Over90 = reader["Over90"];
                        details.Over120 = reader["Over120"];
                        details.Over150 = reader["Over150"];
                        details.Over180 = reader["Over180"];

                        detArray.Add(details);
                    }

                    #endregion

                    result.Summary = sumArray;
                    result.Totals = totals;
                    result.Details = detArray;
                }

            }
           // table.Add(result);
            return result;
        }


        public async Task<object> Get10ColumnData(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
     {
            dynamic result = new JsonArray();

            sqlSelect = "SELECT GLAccountNumber,GLAccountName,GLAccountType,GLBalanceType,GLAccountBalance FROM LedgerChartOfAccounts WHERE OrganizationalUnitID=@OrganizationalUnitID AND TenantId=@TenantId AND IsDeleted=@IsDeleted ORDER BY GLAccountNumber ASC";

            commandtype = "Text";
                int tblcount = 1;
            var table = new List<object>();


            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, sqlParameters)) {


                using (var reader = await cmd.ExecuteReaderAsync()) {

                    while (reader.Read()) {
                        dynamic row = new JsonObject ();


                        row.GLAccountNumber = reader["GLAccountNumber"];
                        row.GLAccountName = reader["GLAccountName"];
                        row.GLAccountType = reader["GLAccountType"];
                        row.GLBalanceType = reader["GLBalanceType"];
                        row.GLAccountBalance = reader["GLAccountBalance"];

                        result.Add(row);
                    }
                }

            }
         //   table.Add(result);
            return result;
        }

        /**
         * This is generic. Do not Touch it.
         * 
         * 
         * */
        private readonly IActiveTransactionProvider _transactionProvider;

        public SqlFinancialReports(IDbContextProvider<ArchDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider)
        {
            _transactionProvider = transactionProvider;
        }

        public long? LongTryToParse(string value)
        {
            long number;
            bool result = long.TryParse(value, out number);
            if (result)
            {
                return number;
            }
            else
            {
                return null;
            }
        }

        public DbCommand CreateCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            //var command = Context.Database.Connection.CreateCommand();
            var connection = Context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
         

            command.CommandTimeout = 300000;
            /*      DbTransaction activetransaction = GetActiveTransaction();
             *      if (activetransaction != null) 
                 command.Transaction = GetActiveTransaction();
            */
            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;
        }

        public DbCommand CreateCommand(string commandText, string commandtype, params SqlParameter[] parameters)
        {

            var connection = Context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = commandText;

            if (commandtype.ToLower().Contains("text"))
                command.CommandType = CommandType.Text;
            else
                command.CommandType = CommandType.StoredProcedure;
            
            command.CommandTimeout = 300000;
          /*  DbTransaction activetransaction = GetActiveTransaction();

            if (activetransaction != null)
                command.Transaction = GetActiveTransaction();
          */
            // command.Transaction = GetActiveTransaction();
            command.Parameters.Clear();
            //foreach (var parameter in parameters)
            //{
            command.Parameters.AddRange(parameters);
            //}

            return command;
        }


        public DbCommand CreateCommand(string commandText, string commandtype, params object[] parameters)
    {
            var connection = Context.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = commandText;
       
        if (commandtype.ToLower().Contains("text"))
            command.CommandType = CommandType.Text;
        else
            command.CommandType = CommandType.StoredProcedure;

            command.CommandTimeout = 300000;
         /*   DbTransaction activetransaction = GetActiveTransaction();

           if (activetransaction != null)
               command.Transaction = GetActiveTransaction();  //command.Transaction = GetActiveTransaction();
         */
        foreach (var parameter in parameters)
        {
            command.Parameters.Add(parameter);
        }

        return command;
    }

    private void EnsureConnectionOpen()
        {
            //var connection = new SqlConnection();
            // connection =  Context.Database.Connection.();
            ArchDbContextFactory NewDBContext = new ArchDbContextFactory();
            string[] arg = new string[1]; ;
            
            Context = NewDBContext.CreateDbContext(arg);

            var connection = Context.Database.GetDbConnection();
           
            if (connection.State != ConnectionState.Open)
            {
                //Context.Database.OpenConnection();
                connection.Open();
                // connection.Open();
                


            }
        }

        private DbTransaction GetActiveTransaction()
        {
            try
            {
                return (DbTransaction) _transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs
    {
        {"ContextType", typeof(ArchDbContext) },
        {"MultiTenancySide", MultiTenancySide }
    });
            }
            catch(Exception ex)
            {
                string err = ex.Message;
                return null;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SqlRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }




        #endregion
    }



}
