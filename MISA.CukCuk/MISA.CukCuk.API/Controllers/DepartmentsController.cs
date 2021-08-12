using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.API.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        //GET, POST, PUT, DELETE
        #region method GET
        /// <summary>
        /// Hàm lấy tất cả phòng ban
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns>Danh sách phòng ban</returns>
        [HttpGet]
        public IActionResult GetAllDepartment()
        {
            try
            {
                //Truy cập vào database:
                // 1.Khai báo đối tượng
                var connectionString = "Host = localhost;" +
                     "Database = MISA.CukCuk_Demo;" +
                     "User Id = root;" +
                     "Password = 123456";

                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);

                // 3.Lấy dữ liệu
                var sqlCommand = "SELECT * FROM Department";
                var departments = dbConnection.Query<object>(sqlCommand);
                // Trả về cho client
                if (departments.Count() > 0)
                {
                    var response = StatusCode(200, departments);
                    return response;
                }
                else
                {
                    return StatusCode(204);
                }
               
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resource.Error_Message_UserVN,
                    errorCode = "misa-001",
                    moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }        
        }

        /// <summary>
        /// Hàm lấy phòng ban theo id
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns>Phòng ban</returns>
        [HttpGet("{departmentId}")]
        public IActionResult GetDepartmentById(Guid departmentId)
        {
            try
            {
                //Truy cập vào database:
                // 1.Khai báo đối tượng
                var connectionString = "Host = localhost;" +
                    "Database = MISA.CukCuk_Demo;" +
                    "User Id = root;" +
                    "Password = 123456";
                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);
                // 3.Lấy dữ liệu
                var sqlCommand = $"SELECT * FROM Department WHERE departmentId = @DepartmentIdParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DepartmentIdParam", departmentId);
                var department = dbConnection.QueryFirstOrDefault<object>(sqlCommand, param: parameters);
                // Trả về cho client

                var response = StatusCode(200, department);
                return response;
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resource.Error_Message_UserVN,
                    errorCode = "misa-001",
                    moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
          
        }
        #endregion

        #region method POST
        /// <summary>
        /// Hàm thêm phòng ban mới
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertDepartment([FromBody]Department department)
        {
            try
            {   
                // Kiểm tra dữ liệu
                // 1. Mã phòng ban bắt buộc nhập
                if (department.DepartmentCode == "" || department.DepartmentCode == null)
                {
                    var errorObj = new
                    {
                        userMsg = Properties.Resource.Error_Message_UserVN,
                        errorCode = "misa-001",
                        moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                        traceId = ""
                    };
                    return BadRequest(errorObj);
                }

                //sinh id phòng ban mới 
                department.DepartmentId = Guid.NewGuid();
                //Truy cập vào database:
                // 1.Khai báo đối tượng
                var connectionString = "Host = localhost;" +
                    "Database = MISA.CukCuk_Demo;" +
                    "User Id = root;" +
                    "Password = 123456";
                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);
                //khai báo dynamicParam:
                var dynamicParam = new DynamicParameters();

                // 3.Thêm dữ liệu vào database
                var columnsName = string.Empty;
                var columnsParam = string.Empty;

                //Đọc từng property của object:
                var properties = department.GetType().GetProperties();


                //Duyệt từng property:
                foreach (var prop in properties)
                {
                    //lấy tên của prop:
                    var propName = prop.Name;

                    //Lấy value của prop:
                    var propValue = prop.GetValue(department);

                    //Lấy kiểu dữ liệu của prop:
                    var propType = prop.PropertyType;

                    //thêm param tương ứng với mỗi property của đối tượng
                    dynamicParam.Add($"@{propName}", propValue);

                    columnsName += $"{propName},";
                    columnsParam += $"@{propName},";
                }
                columnsName = columnsName.Remove(columnsName.Length - 1, 1);
                columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);
                var sqlCommand = $"INSERT INTO Department({columnsName}) VALUES({columnsParam}) ";

                var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);
                // Trả về cho client

                var response = StatusCode(200, rowEffects);
                return response;
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resource.Error_Message_UserVN,
                    errorCode = "misa-001",
                    moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }        
        }
        #endregion

        #region method PUT
        /// <summary>
        /// Hàm cập nhật phòng ban 
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns></returns>
        [HttpPut("{departmentId}")]
        public IActionResult UpdateDepartment(Guid departmentId, Department department)
        {
            try
            {
                //Truy cập vào database:
                // 1.Khai báo đối tượng
                var connectionString = "Host = localhost;" +
                     "Database = MISA.CukCuk_Demo;" +
                     "User Id = root;" +
                     "Password = 123456";
                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);
                //khai báo dynamicParam:
                var dynamicParam = new DynamicParameters();

                // 3.Thêm dữ liệu vào database
                var columnsName = string.Empty;

                //Đọc từng property của object:
                var properties = department.GetType().GetProperties();

                //Duyệt từng property:
                foreach (var prop in properties)
                {
                    //lấy tên của prop:
                    var propName = prop.Name;

                    //Lấy value của prop:
                    var propValue = prop.GetValue(department);

                    //Lấy kiểu dữ liệu của prop:
                    var propType = prop.PropertyType;

                    //thêm param tương ứng với mỗi property của đối tượng
                    dynamicParam.Add($"@{propName}", propValue);

                    columnsName += $"{propName} = @{propName},";

                }
                columnsName = columnsName.Remove(columnsName.Length - 1, 1);

                var sqlCommand = $"UPDATE Department SET {columnsName} WHERE departmentId = @DepartmentIdParam ";

                dynamicParam.Add("@DepartmentIdParam", departmentId);
                var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);

                // Trả về cho client
                var response = StatusCode(200, rowEffects);
                return response;
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resource.Error_Message_UserVN,
                    errorCode = "misa-001",
                    moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
          
        }
        #endregion

        #region method DELETE
        /// <summary>
        /// Hàm xóa phòng ban 
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{departmentId}")]
        public IActionResult DeleteDepartment(Guid departmentId)
        {
            try
            {
                //Truy cập vào database:
                // 1.Khai báo đối tượng
                var connectionString = "Host = localhost;" +
                     "Database = MISA.CukCuk_Demo;" +
                     "User Id = root;" +
                     "Password = 123456";
                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);

                // 3.Lấy dữ liệu
                var sqlCommand = $"DELETE FROM Department WHERE DepartmentId = @DepartmentIdParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DepartmentIdParam", departmentId);
                var rowEffects = dbConnection.Execute(sqlCommand, param: parameters);

                // 4.Trả về cho client
                var response = StatusCode(200, rowEffects);
                return response;
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resource.Error_Message_UserVN,
                    errorCode = "misa-001",
                    moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
        }
        #endregion
    }
}
