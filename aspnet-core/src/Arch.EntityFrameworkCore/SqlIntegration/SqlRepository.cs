
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


using System.Configuration;
using AutoMapper;
using FastMember;

using Arch.MultiTenancy.Accounting;
using Abp.Runtime.Session;
using Org.BouncyCastle.Utilities.Collections;

using Abp.Extensions;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using static Abp.Domain.Uow.AbpDataFilters;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Arch.Dto;

namespace Arch.Sql
{
    public class SqlRepository : EntityFrameworkCore.Repositories.ArchRepositoryBase<IEntity, int>, ISqlRepository
    {

        /** This needs to be done for each Stored Procedure to be Put into the system. Define it as an Async function
         * 
         * */
        ArchDbContext Context;



        public async Task<List<Dictionary<string, object>>> LoadData(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
        {
            int tblcount = 1;
            var table = new List<Dictionary<string, object>>();
            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, sqlParameters))
            {

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                            row[reader.GetName(i)] = reader[i];
                        table.Add(row);
                    }

                    while (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {

                            var row_table = new Dictionary<string, object>();
                            row_table["@@NEXT@@TABLE@@" + (tblcount).ToString()] = tblcount++;

                            while (reader.Read())
                            {
                                var row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                    row[reader.GetName(i)] = reader[i];
                                table.Add(row);
                            }
                        }
                    }
                }

            }

            return table;
        }



        public async Task<List<object>> LoadDataMultiple(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
        {
            int tblcount = 1;

            var datalist = new List<object>();
            var finallist = new List<object>();
            var table = new List<Dictionary<string, object>>();
            var columnlist = new List<string>();




            // check if its a stored procedure or a tablename
            //Ensure all storedprocedures have fullnames like dbo.* or enterprise.*
            if (!sqlSelect.Contains(".") && sqlSelect.ToLower().Trim().Left(3) != "abp")
            {
                //before making it a text; Clean up for security reasons
                sqlSelect = sqlSelect.Replace(" ", "").Replace(";", "").Replace("/", "").Replace("\\", "");
                sqlSelect = sqlSelect.ToLower().Replace("abp", "").Replace("app", "");

                commandtype = "text";

                sqlSelect = "select * from " + sqlSelect + " WHERE  IsDeleted = 0 ";

                //turn parameters into where clauses
                foreach (var parameter in sqlParameters)
                {
                    var param2 = new SqlParameter();
                    param2 = (SqlParameter)parameter;
                    var col = param2.ParameterName;
                    sqlSelect = sqlSelect + " AND " + col.Replace("@", "") + " = " + col;
                }


            }

            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, sqlParameters))
            {

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader[i];
                            columnlist.Add(reader.GetName(i));
                        }
                        table.Add(row);




                    }
                    datalist.Add(columnlist);
                    datalist.Add(table);
                    finallist.Add(datalist);

                    while (reader.NextResult())
                    {


                        if (reader.HasRows)
                        {
                            var table2 = new List<Dictionary<string, object>>();
                            var columnlist2 = new List<string>();
                            var datalist2 = new List<object>();
                            // var row_table = new Dictionary<string, object>();
                            // row_table["@@NEXT@@TABLE@@" + (tblcount).ToString()] = tblcount++;

                            while (reader.Read())
                            {
                                var row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[reader.GetName(i)] = reader[i];
                                    columnlist2.Add(reader.GetName(i));
                                }
                                table2.Add(row);
                            }

                            datalist2.Add(columnlist2);
                            datalist2.Add(table2);

                            finallist.Add(datalist2);

                        }
                    }
                }

            }

            return finallist;
        }


        public async Task<List<object>> LoadDataObject(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
        {
            int tblcount = 1;
            var table = new List<object>();
            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, sqlParameters))
            {

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {

                        table.Add(reader);
                    }

                    while (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {

                            table.Add("@@NEXT@@TABLE@@" + (tblcount++).ToString());


                            while (reader.Read())
                            {
                                table.Add(reader);
                            }
                        }
                    }
                }

            }

            return table;
        }




        public async Task<List<T>> FromSQL3<T>(string sqlSelect, params object[] sqlParameters) where T : class, new()
        {
            var newListObject = new List<T>();

            EnsureConnectionOpen();

            using (var sqlCommand = CreateCommand(sqlSelect, "text", sqlParameters))
            {

                using (var dataReader = await sqlCommand.ExecuteReaderAsync(CommandBehavior.Default))
                {
                    if (dataReader.HasRows)
                    {
                        while (await dataReader.ReadAsync())
                        {
                            var newObject = new T();
                            dataReader.MapDataToObject(newObject);
                            newListObject.Add(newObject);
                        }
                    }
                }
            }



            return await Task.FromResult(newListObject);
        }


        public async Task<IQueryable<T>> FromSQL<T>(string sqlSelect, params object[] sqlParameters) where T : class, new()
        {
            var newListObject = new List<T>();

            EnsureConnectionOpen();

            using (var sqlCommand = CreateCommand(sqlSelect, "text", sqlParameters))
            {

                using (var dataReader = await sqlCommand.ExecuteReaderAsync(CommandBehavior.Default))
                {
                    if (dataReader.HasRows)
                    {
                        while (await dataReader.ReadAsync())
                        {
                            var newObject = new T();
                            dataReader.MapDataToObject(newObject);
                            newListObject.Add(newObject);
                        }
                    }
                }
            }






            return newListObject.AsQueryable();
        }


        public async Task<string> GetNextID(int TenantId, string OrganizationalUnitId, string NextIDRequired)
        {

            var args = new List<SqlParameter>();

            try
            {
                var organizationUnitIdParamVal = new SqlParameter("@organizationalUnitId", SqlDbType.NVarChar)
                {
                    Direction = ParameterDirection.Input,
                    Value = OrganizationalUnitId ?? "0"
                };
                var tenantIdParamVal = new SqlParameter("@TenantId", SqlDbType.BigInt)
                {
                    Direction = ParameterDirection.Input,
                    Value = TenantId
                };
                var isDeletedParamVal = new SqlParameter("@IsDeleted", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Input,
                    Value = 0
                };

                var entity = new SqlParameter("@Entity", SqlDbType.NVarChar)
                {
                    Direction = ParameterDirection.Input,
                    Value = NextIDRequired
                };

                var entityid = new SqlParameter("@EntityID", SqlDbType.NVarChar)
                {
                    Direction = ParameterDirection.Output,
                    Value = ""
                };

                args.Add(tenantIdParamVal);
                args.Add(organizationUnitIdParamVal);
                args.Add(isDeletedParamVal);
                args.Add(entity);
                args.Add(entityid);
                List<string> output = new List<string>();
                output.Add("@EntityID");



                var result = await ProcessData("[Enterprise].[GetNextEntityID]", "StoredProcedure", output.ToArray(), args.ToArray());

                var msg = (result["@EntityID"] != System.DBNull.Value && result["@EntityID"] != null) ? result["@EntityID"].ToString() : "(new)";
                return msg;
            }

            catch (Exception ex)
            {

                var tst = ex.Message + "\n" + ex.StackTrace;
                return "(new)";
            }


        }
        public async Task<Dictionary<string, object>> ProcessData(string sqlSelect, string commandtype, string[] outputparameters, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
        {

            int cmdresult = -2;
            //  var table = new List<Dictionary<string, object>>();
            var row = new Dictionary<string, object>();
            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, sqlParameters))
            {

                for (int i = 0; i < outputparameters.Length; i++)
                {
                    cmd.Parameters[outputparameters[i]].Direction = ParameterDirection.InputOutput;

                    if (cmd.Parameters[outputparameters[i]].Value == null)
                        cmd.Parameters[outputparameters[i]].Value = DBNull.Value;

                }

                // is a returnvalue specified? If not, add it
                //    if (cmd.Parameters["@ReturnValue"] == null)
                //  {
                //       cmd.Parameters.Add(
                //        new SqlParameter("@ReturnValue", SqlDbType.Variant)
                //        {
                //            Direction = ParameterDirection.ReturnValue
                //        });
                //    }
                //  try {

                cmdresult = await cmd.ExecuteNonQueryAsync();
                /*    }
                    catch(Exception ex) {
                        var tst = ex.Message;
                    }
                */
                //Get all OUTPUT variables
                for (int i = 0; i < outputparameters.Length; i++)
                {
                    row[outputparameters[i]] = ConvertFromDBVal(cmd.Parameters[outputparameters[i]].Value);
                }
                //  Add for return value
                //  row["@ReturnValue"] = cmd.Parameters["@ReturnValue"].Value;

            }


            return row;
        }

        private static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }

        private static object ConvertFromDBVal(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return null; // returns the default value for the type
            }
            else
            {
                return obj;
            }
        }
        public List<Dictionary<string, object>> LoadDataSimple(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
        {
            int tblcount = 1;
            var table = new List<Dictionary<string, object>>();
            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, sqlParameters))
            {

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                            row[reader.GetName(i)] = reader[i];
                        table.Add(row);
                    }
                    while (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {

                            var row_table = new Dictionary<string, object>();
                            row_table["@@NEXT@@TABLE@@" + (tblcount).ToString()] = tblcount++;

                            while (reader.Read())
                            {
                                var row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                    row[reader.GetName(i)] = reader[i];
                                table.Add(row);
                            }
                        }
                    }

                }
            }

            return table;
        }



        public async Task<int> AdjustData(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
        {

            int result = -2;
            EnsureConnectionOpen();
            try
            {
                using (var cmd = CreateCommand(sqlSelect, commandtype, sqlParameters))
                {

                    result = await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                result = -3;
            }

            return result;

        }


        public async Task<int> ExecNonQuery(string SqlQuery, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
        {
            // "EXEC UpdateEmailById @email, @id"
            int result = -2;
            EnsureConnectionOpen();
            result = await Context.Database.ExecuteSqlRawAsync(
                SqlQuery,
                 default(CancellationToken), sqlParameters

             );

            //result =  await Context.Database.ExecuteSqlCommandAsync(TransactionalBehavior.DoNotEnsureTransaction, 
            //    "EXEC TestHeader1"
            //);

            return result;

        }
        public async Task<string> GetCodeName(string Table, long Id)
        {

            var result = "";

            var args = new List<SqlParameter>();
            args.Add(new SqlParameter("@Id", (object)Id));
            args.Add(new SqlParameter("@Table", (object)Table));

            SqlParameter[] sqlp = (SqlParameter[])args.ToArray();

            try
            {

                result = await LoadSingleData("dbo.GetCodeName", "sp", sqlp);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return result;
            }

            return result;
        }


        public async Task<string> LoadSingleData(string sqlSelect, string commandtype, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
        {

            var result = "".ToLower().Trim();
            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, sqlParameters))
            {
                result = (await cmd.ExecuteScalarAsync()).ToString();
                return result;
            }

            //  return result;
        }

        public async Task<List<string>> LoadData(string sqlSelect, string commandtype, string fieldname, params object[] sqlParameters)  //commandtype = "StoredProcedure" or "Text"
        {

            var result = new List<string>();
            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sqlSelect, commandtype, sqlParameters))
            {

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result.Add(reader[fieldname].ToString());
                    }

                }
            }

            return result;
        }


        public async Task<Dictionary<string, string>> LoadParent(string table, long? Id)  //commandtype = "StoredProcedure" or "Text"
        {

            var args = new List<SqlParameter>();

            table = table.Replace(" ", "").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");


            args.Add(new SqlParameter("@ID", (object)(Id.HasValue ? Id : -1)));

            SqlParameter[] sqlp = (SqlParameter[])args.ToArray();

            var result = new Dictionary<string, string>();
            EnsureConnectionOpen();
            using (var cmd = CreateCommand("Select * from " + table + " where isdeleted = 0 and Id = ISNULL(@ID,-1) ", "Text", sqlp))
            {

                using (var reader = await cmd.ExecuteReaderAsync())
                {


                    while (reader.Read())
                    {
                        var row = new Dictionary<string, string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                            row[reader.GetName(i)] = (reader[i] ?? string.Empty).ToString();
                        result = row;
                    }


                }
            }


            return result;
        }

        //version for Parent referenced by non Id column
        public async Task<Dictionary<string, string>> LoadParent(string table, string value, string column, string orgId)  //commandtype = "StoredProcedure" or "Text"
        {

            var args = new List<SqlParameter>();

            table = table.Replace(" ", "").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            column = ("" + column.Trim() + "").Replace(" ", "%").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("[[", "[").Replace("]]", "]").Replace("@@@", " + ");
            //@@@ to concatenante multiple fields together

            args.Add(new SqlParameter("@ID", (object)orgId));

            args.Add(new SqlParameter("@Value", (object)value));

            string sql = @"BEGIN TRY   " +
             " SELECT  *  from " + table + " where isdeleted = 0 and (organizationalunitID = @ID or @ID = '0') and " + column + " = @Value " +
             " END TRY   BEGIN CATCH " +
             " SELECT *  from " + table + " where isdeleted = 0 and (organizationalunitID = '' ) " +
             " END CATCH  ";

            // args.Add(new SqlParameter("@ID", (object)(Id.HasValue ? Id : -1)));

            SqlParameter[] sqlp = (SqlParameter[])args.ToArray();

            var result = new Dictionary<string, string>();
            EnsureConnectionOpen();
            EnsureConnectionOpen();

            try
            {
                using (var cmd = CreateCommand(sql, "Text", sqlp))
                {

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {


                        while (reader.Read())
                        {
                            var row = new Dictionary<string, string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                                row[reader.GetName(i)] = (reader[i] ?? string.Empty).ToString();
                            result = row;
                        }


                    }
                }

            }
            catch (Exception Ex)
            {

                return null; // on error return null

            }


            return result;
        }


        public async Task<long?> LoadSingleId(string orgId, string table, string column, string value)  //commandtype = "StoredProcedure" or "Text"
        {

            var args = new List<SqlParameter>();

            table = table.Replace(" ", "").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            column = ("[" + column.Trim() + "]").Replace(" ", "%").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("[[", "[").Replace("]]", "]");


            args.Add(new SqlParameter("@ID", (object)orgId));

            args.Add(new SqlParameter("@Value", (object)value));

            string sql = @"BEGIN TRY   " +
             " SELECT  Id  from " + table + " where (organizationalunitID = @ID or @ID = '0') and " + column + " = @Value " +
             " END TRY   BEGIN CATCH " +
             " SELECT null as Id " +
             " END CATCH  ";

            SqlParameter[] sqlp = (SqlParameter[])args.ToArray();

            long? result = null;
            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sql, "Text", sqlp))
            {

                using (var reader = await cmd.ExecuteReaderAsync())
                {


                    while (reader.Read())
                    {


                        result = null;
                        long res = 0;
                        long.TryParse(reader["Id"].ToString(), out res);
                        result = res;

                    }


                }
            }


            return result;
        }

        public async Task<List<Dictionary<string, string>>> ComboData(string table, string selectvalue, string selecttext, string organizationalunitID)  //commandtype = "StoredProcedure" or "Text"
        {

            var args = new List<SqlParameter>();
            args.Add(new SqlParameter("@ORGID", (object)organizationalunitID));

            SqlParameter[] sqlp = (SqlParameter[])args.ToArray();

            var result = new List<Dictionary<string, string>>();
            EnsureConnectionOpen();
            using (var cmd = CreateCommand("Select * from " + table + " where isdeleted = 0 and organizationalunitID = @ORGID", "Text", sqlp))
            {

                using (var reader = await cmd.ExecuteReaderAsync())
                {


                    while (reader.Read())
                    {
                        var row = new Dictionary<string, string>();
                        // for (int i = 0; i < reader.FieldCount; i++)
                        row[reader[selectvalue].ToString()] = reader[selecttext].ToString();
                        result.Add(row);
                    }


                }
            }

            return result;
        }


        public async Task<List<Dictionary<string, string>>> ComboData(string selectquery, string selectvalue, string selecttext, params object[] sqlparameters)  //commandtype = "StoredProcedure" or "Text"
        {



            var result = new List<Dictionary<string, string>>();
            EnsureConnectionOpen();
            using (var cmd = CreateCommand(selectquery, "Text", sqlparameters))
            {

                using (var reader = await cmd.ExecuteReaderAsync())
                {


                    while (reader.Read())
                    {
                        var row = new Dictionary<string, string>();
                        // for (int i = 0; i < reader.FieldCount; i++)
                        row[reader[selectvalue].ToString()] = reader[selecttext].ToString();
                        result.Add(row);
                    }


                }
            }

            return result;
        }


        public async Task<List<NameValueDto>> SelectData(string selectquery, string selectvalue, string selecttext, params object[] sqlparameters)  //commandtype = "StoredProcedure" or "Text"
        {




            var result = new List<NameValueDto>();
            EnsureConnectionOpen();
            using (var cmd = CreateCommand(selectquery, "Text", sqlparameters))
            {

                using (var reader = await cmd.ExecuteReaderAsync())
                {


                    while (reader.Read())
                    {
                        var row = new NameValueDto(reader[selecttext].ToString(), reader[selectvalue].ToString());


                        result.Add(row);
                    }


                }
            }

            return result;
        }
        public async Task<List<NameValueDto>> SelectData(string table, string selectvalue, string selecttext, string organizationalunitID, string filter, int skipCount, int maxResultCount, int? tenant)  //commandtype = "StoredProcedure" or "Text"
        {

            //prevent SQL injection

            table = table.Replace(" ", "").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            selectvalue = selectvalue.Replace(" ", "%").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            selecttext = selecttext.Replace(" ", "%").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");

            selectvalue = selectvalue.Replace(" ", "%").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");


            var args = new List<SqlParameter>();
            args.Add(new SqlParameter("@OrganizationalUnitID", (object)organizationalunitID));
            args.Add(new SqlParameter("@FILTER", (object)filter));
            args.Add(new SqlParameter("@Table", (object)table));
            args.Add(new SqlParameter("@selectvalue", (object)selectvalue));
            args.Add(new SqlParameter("@selecttext", (object)selecttext));
            args.Add(new SqlParameter("@PageSize", (object)maxResultCount));
            args.Add(new SqlParameter("@PageNumber", (object)(skipCount)));
            args.Add(new SqlParameter("@TenantId", tenant ?? 0));
            //   args.Add(new SqlParameter("@TABLE", (object)table));



            SqlParameter[] sqlp = (SqlParameter[])args.ToArray();

            var result = new List<NameValueDto>();
            EnsureConnectionOpen();
            /*  using (var cmd = CreateCommand("DECLARE @VALID NVARCHAR(200)" +
                  "SET @VALID = ISNULL( (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @table)  ,'')" +
                  "IF LEN(@VALID) > 3 AND LEFT(UPPER(@VALID),3) <> 'ABP' " +
                  " SELECT  ISNULL((" + selectvalue + "),'') as value, ISNULL((" + selecttext + "),'') as text  from " + table + " where isdeleted = 0 and (organizationalunitID = @OrganizationalUnitID or @OrganizationalunitID = '0') and ( ISNULL((" + selectvalue + "),'')  LIKE '%'+ RTRIM(ISNULL(@FILTER,'')) + '%'     OR ISNULL((" + selecttext + "),'')  LIKE '%'+ RTRIM(ISNULL(@FILTER,'')) + '%' )" +
                  " ELSE SELECT '0' AS value, 'INVALID ACCESS' as text " +
                  "", "Text", sqlp))
                  */


            string sqlquery = "";

            if (table.ToUpper().StartsWith("ABP"))
                sqlquery = "SELECT '0' AS value, 'INVALID ACCESS-RIGHTS' as text ";
            //var customerCount = await query.CountAsync();
            //var customers = await query
            //    .OrderBy(c => c.CustomerCode)
            //    .ThenBy(c => c.CustomerName)
            //    .PageBy(input)
            //    .ToListAsync(); 


            using (var cmd = CreateCommand("GetLookup", "StoredProcedure", sqlp))
            {

                using (var reader = await cmd.ExecuteReaderAsync())
                {


                    while (reader.Read())
                    {
                        var row = new NameValueDto(reader["text"].ToString(), reader["value"].ToString());


                        result.Add(row);
                    }


                }
            }

            return result;
        }

        //public async Task<List<NameValueDto>> SelectData(string table, string selectvalue, string selecttext, long? organizationalunitID, string filter, int skipCount, int maxResultCount)  //commandtype = "StoredProcedure" or "Text"
        //{

        //    var args = new List<SqlParameter>();
        //    args.Add(new SqlParameter("@OrganizationalUnitID", (object)organizationalunitID));
        //    args.Add(new SqlParameter("@FILTER", (object)filter));
        //    args.Add(new SqlParameter("@PageSize", (object)maxResultCount));
        //    args.Add(new SqlParameter("@PageNumber", (object)(skipCount + 1)));
        //    //   args.Add(new SqlParameter("@TABLE", (object)table));

        //    SqlParameter[] sqlp = (SqlParameter[])args.ToArray();

        //    var result = new List<NameValueDto>();
        //    EnsureConnectionOpen();
        //    /*  using (var cmd = CreateCommand("DECLARE @VALID NVARCHAR(200)" +
        //          "SET @VALID = ISNULL( (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @table)  ,'')" +
        //          "IF LEN(@VALID) > 3 AND LEFT(UPPER(@VALID),3) <> 'ABP' " +
        //          " SELECT  ISNULL((" + selectvalue + "),'') as value, ISNULL((" + selecttext + "),'') as text  from " + table + " where isdeleted = 0 and (organizationalunitID = @OrganizationalUnitID or @OrganizationalunitID = 0) and ( ISNULL((" + selectvalue + "),'')  LIKE '%'+ RTRIM(ISNULL(@FILTER,'')) + '%'     OR ISNULL((" + selecttext + "),'')  LIKE '%'+ RTRIM(ISNULL(@FILTER,'')) + '%' )" +
        //          " ELSE SELECT '0' AS value, 'INVALID ACCESS' as text " +
        //          "", "Text", sqlp))
        //          */

        //    //prevent SQL injection

        //    table = table.Replace(" ", "").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");
        //    selectvalue = selectvalue.Replace(" ", "%").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");
        //    selecttext = selecttext.Replace(" ", "%").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");

        //    string sqlquery = "BEGIN TRY   " +
        //   " SELECT  cast(ISNULL((" + selectvalue + "),'') as nvarchar(50)) as value, '(' + cast(ISNULL((" + selectvalue + "),'') as nvarchar(50)) + ') ' + ISNULL((" + selecttext + "),'') as text  from " + table + "  WITH (NOLOCK)  where isdeleted = 0 and (organizationalunitID = @OrganizationalUnitID or @OrganizationalunitID = 0) and ( cast(ISNULL((" + selectvalue + "),'') as nvarchar(50))  LIKE '%'+ RTRIM(ISNULL(@FILTER,'')) + '%'     OR ISNULL((" + selecttext + "),'')  LIKE '%'+ RTRIM(ISNULL(@FILTER,'')) + '%' ) " +
        //   " ORDER BY " + selectvalue + " " +
        //    "OFFSET  @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY OPTION " +
        //   " END TRY   BEGIN CATCH " +
        //   " SELECT '0' AS value, 'INVALID ACCESS' as text " +
        //   " END CATCH  ";
        //    selectvalue = selectvalue.Replace(" ", "%").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");


        //    if (table.ToUpper().StartsWith("ABP"))
        //        sqlquery = "SELECT '0' AS value, 'INVALID ACCESS-RIGHTS' as text ";
        //    //var customerCount = await query.CountAsync();
        //    //var customers = await query
        //    //    .OrderBy(c => c.CustomerCode)
        //    //    .ThenBy(c => c.CustomerName)
        //    //    .PageBy(input)
        //    //    .ToListAsync(); 


        //    using (var cmd = CreateCommand(sqlquery, "Text", sqlp))
        //    {

        //        using (var reader = await cmd.ExecuteReaderAsync())
        //        {


        //            while (reader.Read())
        //            {
        //                var row = new NameValueDto(reader["text"].ToString(), reader["value"].ToString());


        //                result.Add(row);
        //            }


        //        }
        //    }

        //    return result;
        //}
        public async Task<List<NameValueDto>> SelectData(string table, string selectvalue, string selecttext, string organizationalunitID, string filter, int? tenant)  //commandtype = "StoredProcedure" or "Text"
        {

            var args = new List<SqlParameter>();
            args.Add(new SqlParameter("@OrganizationalUnitID", (object)organizationalunitID));
            args.Add(new SqlParameter("@FILTER", (object)filter));
            args.Add(new SqlParameter("@TenantId", tenant ?? 0));
            //   args.Add(new SqlParameter("@TABLE", (object)table));

            SqlParameter[] sqlp = (SqlParameter[])args.ToArray();

            var result = new List<NameValueDto>();
            EnsureConnectionOpen();
            /*  using (var cmd = CreateCommand("DECLARE @VALID NVARCHAR(200)" +
                  "SET @VALID = ISNULL( (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @table)  ,'')" +
                  "IF LEN(@VALID) > 3 AND LEFT(UPPER(@VALID),3) <> 'ABP' " +
                  " SELECT  ISNULL((" + selectvalue + "),'') as value, ISNULL((" + selecttext + "),'') as text  from " + table + " where isdeleted = 0 and (organizationalunitID = @OrganizationalUnitID or @OrganizationalunitID = '0') and ( ISNULL((" + selectvalue + "),'')  LIKE '%'+ RTRIM(ISNULL(@FILTER,'')) + '%'     OR ISNULL((" + selecttext + "),'')  LIKE '%'+ RTRIM(ISNULL(@FILTER,'')) + '%' )" +
                  " ELSE SELECT '0' AS value, 'INVALID ACCESS' as text " +
                  "", "Text", sqlp))
                  */

            //prevent SQL injection

            table = table.Replace(" ", "").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            selectvalue = selectvalue.Replace(" ", "%").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            selecttext = selecttext.Replace(" ", "%").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");

            string sqlquery = "BEGIN TRY   " +
           " SELECT  cast(ISNULL((" + selectvalue + "),'') as nvarchar(50)) as value, '(' + cast(ISNULL((" + selectvalue + "),'') as nvarchar(50)) + ') ' + ISNULL((" + selecttext + "),'') as text  from " + table + "  WITH (NOLOCK)  where isdeleted = 0 and (organizationalunitID = @OrganizationalUnitID or @OrganizationalunitID = '0') and ( cast(ISNULL((" + selectvalue + "),'') as nvarchar(50))  LIKE '%'+ RTRIM(ISNULL(@FILTER,'')) + '%'     OR ISNULL((" + selecttext + "),'')  LIKE '%'+ RTRIM(ISNULL(@FILTER,'')) + '%' ) " +
           " ORDER BY " + selectvalue + " " +
           " END TRY   BEGIN CATCH " +
           " SELECT '0' AS value, 'INVALID ACCESS' as text " +
           " END CATCH  ";

            if (table.ToUpper().StartsWith("ABP"))
                sqlquery = "SELECT '0' AS value, 'INVALID ACCESS' as text ";
            //var customerCount = await query.CountAsync();
            //var customers = await query
            //    .OrderBy(c => c.CustomerCode)
            //    .ThenBy(c => c.CustomerName)
            //    .PageBy(input)
            //    .ToListAsync(); 




            using (var cmd = CreateCommand(sqlquery, "Text", sqlp))
            {

                using (var reader = await cmd.ExecuteReaderAsync())
                {


                    while (reader.Read())
                    {
                        var row = new NameValueDto(reader["text"].ToString(), reader["value"].ToString());


                        result.Add(row);
                    }


                }
            }

            return result;
        }


        public async Task<List<NameValueDto>> SelectLookupSP(string table, string selectvalue, string selecttext, string organizationalunitID, string filter, int? tenant)  //commandtype = "StoredProcedure" or "Text"
        {

            var args = new List<SqlParameter>();
            args.Add(new SqlParameter("@OrganizationalUnitID", (object)organizationalunitID));
            args.Add(new SqlParameter("@TenantId", tenant ?? 0));
            args.Add(new SqlParameter("@IsDeleted", (object)false));
            args.Add(new SqlParameter("@FILTER", (object)filter));
            //   args.Add(new SqlParameter("@TABLE", (object)table));

            SqlParameter[] sqlp = (SqlParameter[])args.ToArray();

            var result = new List<NameValueDto>();
            EnsureConnectionOpen();
            /*  using (var cmd = CreateCommand("DECLARE @VALID NVARCHAR(200)" +
                  "SET @VALID = ISNULL( (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @table)  ,'')" +
                  "IF LEN(@VALID) > 3 AND LEFT(UPPER(@VALID),3) <> 'ABP' " +
                  " SELECT  ISNULL((" + selectvalue + "),'') as value, ISNULL((" + selecttext + "),'') as text  from " + table + " where isdeleted = 0 and (organizationalunitID = @OrganizationalUnitID or @OrganizationalunitID = '0') and ( ISNULL((" + selectvalue + "),'')  LIKE '%'+ RTRIM(ISNULL(@FILTER,'')) + '%'     OR ISNULL((" + selecttext + "),'')  LIKE '%'+ RTRIM(ISNULL(@FILTER,'')) + '%' )" +
                  " ELSE SELECT '0' AS value, 'INVALID ACCESS' as text " +
                  "", "Text", sqlp))
                  */

            //prevent SQL injection

            table = table.Replace(" ", "").Replace(";", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            selectvalue = selectvalue.Replace(" ", "%").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            selecttext = selecttext.Replace(" ", "%").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            filter = filter.Replace(" ", "%").Replace(";", "").Replace(".", "").Replace("/", "").Replace("--", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "");

            string sqlquery = "" + "BEGIN TRY   " +
            "declare @def nvarchar(4000) \r\ndeclare @def2 nvarchar(4000) \r\n Declare @sql nvarchar(max) \r\nset @def = (SELECT top 1 STRING_AGG( ISNULL(name +' ' + system_type_name, ' '), ',') As result \r\n " +
            " FROM sys.dm_exec_describe_first_result_set_for_object(\r\n  " +
            "  OBJECT_ID('" + table + " '), \r\n    1\r\n    ) where is_hidden = 0) " +
            " \r\nset @def2 = (SELECT top 1 STRING_AGG(ISNULL(name, ' '), ',') As result \r\n " +
            " FROM sys.dm_exec_describe_first_result_set_for_object(\r\n  " +
            "  OBJECT_ID('" + table + " '), \r\n    1\r\n    ) where is_hidden = 0) " +
                " set @sql = 'DECLARE @result TABLE ( ' + @def + ' ) " +
            " INSERT INTO @result ( ' + @def2 + ') " +
            " EXEC " + table + " ''" + organizationalunitID + "'' , " + tenant.ToString() + ", 0  " +
           " SELECT  cast(ISNULL((" + selectvalue + "),'''') as nvarchar(50)) as value, ''('' + cast(ISNULL((" + selectvalue + "),'''') as nvarchar(50)) + '') '' + ISNULL((" + selecttext + "),'''') as text  from @result    where  ( cast(ISNULL((" + selectvalue + "),'''') as nvarchar(50))  LIKE ''%''+ RTRIM(ISNULL(''" + filter + "'','''')) + ''%''     OR ISNULL((" + selecttext + "),'''')  LIKE ''%''+ RTRIM(ISNULL(''" + filter + "'' ,'''')) + ''%'' ) " +
           " ORDER BY " + selectvalue + " ' " +
           "exec sp_executesql @sql " +
             " END TRY   BEGIN CATCH " +
           " SELECT '0' AS value, 'INVALID ACCESS' as text " +
           " END CATCH  ";

            if (table.ToUpper().StartsWith("ABP"))
                sqlquery = "SELECT '0' AS value, 'INVALID ACCESS' as text ";
            //var customerCount = await query.CountAsync();
            //var customers = await query
            //    .OrderBy(c => c.CustomerCode)
            //    .ThenBy(c => c.CustomerName)
            //    .PageBy(input)
            //    .ToListAsync(); 




            using (var cmd = CreateCommand(sqlquery, "Text", sqlp))
            {

                using (var reader = await cmd.ExecuteReaderAsync())
                {


                    while (reader.Read())
                    {
                        var row = new NameValueDto(reader["text"].ToString(), reader["value"].ToString());


                        result.Add(row);
                    }


                }
            }

            return result;
        }


        public async Task<List<string>> GetUserNames()
        {
            EnsureConnectionOpen();

            using (var command = CreateCommand("GetUsernames", CommandType.StoredProcedure))
            {
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    var result = new List<string>();

                    while (dataReader.Read())
                    {
                        result.Add(dataReader["UserName"].ToString());
                    }

                    return result;
                }
            }
        }



        public async Task DeleteUser(EntityDto input)
        {
            await Context.Database.ExecuteSqlRawAsync(
                "EXEC TestHeader @OrderID",
                default(CancellationToken),
                new SqlParameter("OrderID", input.Id)
            );
        }


        //public async Task UpdateEmail(UpdateEmailDto input)
        //{
        //    await Context.Database.ExecuteSqlCommandAsync(
        //        "EXEC UpdateEmailById @email, @id",
        //        default(CancellationToken),
        //        new SqlParameter("id", input.Id),
        //        new SqlParameter("email", input.EmailAddress)
        //    );
        //}

        public async Task<List<string>> GetAdminUsernames()
        {
            EnsureConnectionOpen();
            using (var command = CreateCommand("SELECT * FROM dbo.UserAdminView", CommandType.Text))
            {
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    var result = new List<string>();
                    while (dataReader.Read())
                    {
                        result.Add(dataReader["UserName"].ToString());
                    }
                    return result;
                }
            }
        }




        public async Task<GetUserByIdOutput> GetUserById(EntityDto input)
        {
            EnsureConnectionOpen();

            using (var command = CreateCommand("SELECT top 1 username from [AbpUsers] where id = @id", System.Data.CommandType.Text, new SqlParameter("@id", input.Id)))
            {
                var username = (await command.ExecuteScalarAsync()).ToString();
                return new GetUserByIdOutput() { Username = username };
            }
        }


        /**
         * This is generic. Do not Touch it.
         * 
         * 
         * */
        private readonly IActiveTransactionProvider _transactionProvider;

        public SqlRepository(IDbContextProvider<ArchDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
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
                // var param2 = new SqlParameter();
                //  param2 = (SqlParameter) parameter;


                if (!command.Parameters.Contains(parameter))
                {

                    command.Parameters.Add(parameter);
                }

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
                return (DbTransaction)_transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs
    {
        {"ContextType", typeof(ArchDbContext) },
        {"MultiTenancySide", MultiTenancySide }
    });
            }
            catch (Exception ex)
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
