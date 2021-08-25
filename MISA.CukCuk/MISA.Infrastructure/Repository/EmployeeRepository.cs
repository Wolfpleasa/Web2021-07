using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        #region DECLARE
        IDbConnection _dbConnection;
        //public readonly string _connectionString;
        PagingResult<Employee> _pagingResult;
        #endregion

        #region Constructor
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {
            //_connectionString = configuration.GetConnectionString("LocalDatabase");
            _pagingResult = new PagingResult<Employee>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Phân trang nhân viên
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="searchContent"></param>
        /// <param name="positionId"></param>
        /// <param name="departmentId"></param>
        /// <returns>Danh sách nhân viên</returns>
        /// Created By: Ngọc 19/8/2021
        public PagingResult<Employee> Pagination(int? pageSize, int? pageNumber, string searchContent, Guid? positionId, Guid? departmentId)
        {
            _dbConnection = new MySqlConnection(_connectionString);

            var positionIdString = (positionId == null) ? string.Empty : positionId.ToString();
            var departmentIdString = (departmentId == null) ? string.Empty : departmentId.ToString();

            DynamicParameters parameters = new DynamicParameters();

            var sqlCommand = $"SELECT * FROM Employee Where 1=1 ";

            //Thay đổi query nếu có lọc theo từ khóa
            if (!string.IsNullOrEmpty(searchContent))
            {
                parameters.Add("@searchContent", $"%{searchContent}%");
                sqlCommand += "AND (EmployeeCode LIKE @searchContent OR FullName LIKE @searchContent OR PhoneNumber LIKE @searchContent) ";
            }

            /// Thay đổi query nếu có lọc theo phòng ban
            if (!string.IsNullOrEmpty(departmentIdString))
            {
                parameters.Add("@departmentId", $"%{departmentIdString}%");
                sqlCommand += $"AND departmentId LIKE @departmentId ";
            }

            /// Thay đổi query nếu có lọc theo vị trí
            if (!string.IsNullOrEmpty(positionIdString))
            {
                parameters.Add("@positionId", $"%{positionIdString}%");
                sqlCommand += $"AND positionId LIKE @positionId ";
            }

            // Lấy tổng bản ghi theo điều kiện lọc trước
            var totalRecord = _dbConnection.Query<Employee>(sqlCommand, param: parameters).ToList();
            var totalPageNumber = Math.Ceiling((decimal)((decimal)totalRecord.Count() / (decimal)pageSize));
            _pagingResult.TotalPageNumber = (int)totalPageNumber;
            _pagingResult.TotalRecord = totalRecord.Count();
            

            /// Thay đổi query nếu có phân trang
            if (pageNumber > 0 && pageSize > 0)
            {
                parameters.Add("@limit", pageSize);
                parameters.Add("@offset", (pageNumber - 1) * pageSize);
                sqlCommand += $"LIMIT @limit OFFSET @offset ";
            }

            var employees = _dbConnection.Query<Employee>(sqlCommand, param: parameters).ToList();
            _pagingResult.Entities = employees;

            return _pagingResult;
        }

        public string GetNewCode()
        {
            _dbConnection = new MySqlConnection(_connectionString);

            string sqlCommand = "SELECT EmployeeCode FROM Employee ORDER BY  LENGTH(EmployeeCode) DESC, EmployeeCode DESC";
            var employeeCode = _dbConnection.QueryFirstOrDefault<string>(sqlCommand);

            int currentMax = 0;
         
            int codeValue = int.Parse(employeeCode.ToString().Split("-")[1]);
            if (currentMax < codeValue)
            {
                currentMax = codeValue;
            }

            string newEmployeeCode = "NV-" + (currentMax + 1);        
            return newEmployeeCode;
        }
        #endregion
    }
}
