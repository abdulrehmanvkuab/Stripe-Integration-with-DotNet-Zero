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
    

        public interface ISqlRepository : IRepository, IDisposable
    {

        Task<int> AdjustData(string sqlSelect, string commandtype, params object[] sqlParameters);

        Task<List<string>> LoadData(string sqlSelect, string commandtype, string fieldname, params object[] sqlParameters);

        Task<int> ExecNonQuery(string SqlQuery, params object[] sqlParameters);
         Task<string> LoadSingleData(string sqlSelect, string commandtype, params object[] sqlParameters);
        Task<string> GetCodeName(string table, long Id);
        Task<List<Dictionary<string, object>>> LoadData(string sqlSelect, string commandtype, params object[] sqlParameters);

        Task<List<object>> LoadDataMultiple(string sqlSelect, string commandtype, params object[] sqlParameters);  //commandtype = "StoredProcedure" or "Text"

        List<Dictionary<string, object>> LoadDataSimple(string sqlSelect, string commandtype, params object[] sqlParameters);

        Task<Dictionary<string, object>> ProcessData(string sqlSelect, string commandtype, string[] outputparameters, params object[] sqlParameters);

 
        Task<List<Dictionary<string, string>>> ComboData(string selectquery, string selectvalue, string selecttext, params object[] sqlparameters);

        Task<List<NameValueDto>> SelectData(string selectquery, string selectvalue, string selecttext, params object[] sqlparameters);

        Task<List<Dictionary<string, string>>> ComboData(string table, string selectvalue, string selecttext, string organizationalunitID);
         long? LongTryToParse(string value);
        ////not needed but left for test
        //Task<List<string>> GetUserNames();

        Task<List<NameValueDto>> SelectData(string table, string selectvalue, string selecttext, string organizationalunitID, string filter,int? tenant);
        Task<List<NameValueDto>> SelectData(string table, string selectvalue, string selecttext, string organizationalunitID, string filter, int skipCount, int maxResultCount, int? tenant);
        Task<Dictionary<string, string>> LoadParent(string table, long? Id);
        Task<Dictionary<string, string>> LoadParent(string table, string value, string column, string orgId);
        Task<long?> LoadSingleId(string organizationalunitID, string table, string column, string value);  //commandtype = "StoredProcedure" or "Text"

        Task<List<object>> LoadDataObject(string sqlSelect, string commandtype, params object[] sqlParameters);
   //     IAsyncEnumerable<T> FromSQL<T>(string sqlSelect, params object[] sqlParameters) where T : class, new();
        Task<List<T>> FromSQL3<T>(string sqlSelect, params object[] sqlParameters) where T : class, new();
        Task<IQueryable<T>> FromSQL<T>(string sqlSelect, params object[] sqlParameters) where T : class, new();

        Task<List<NameValueDto>> SelectLookupSP(string table, string selectvalue, string selecttext, string organizationalunitID, string filter, int? tenant);  //commandtype = "StoredProcedure" or "Text"

        Task<string> GetNextID(int TenantId, string OrganizationalUnitId, string NextIDRequired);
            //    Task<List<string>> GetAdminUsernames();

        //    Task DeleteUser(EntityDto input);

        //    Task UpdateEmail(UpdateEmailDto input);

        //    Task<GetUserByIdOutput> GetUserById(EntityDto input);
        }

    }




