using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MISA.Core.Entities;
using System.Data;
using MySqlConnector;
using Dapper;
using System.Text.RegularExpressions;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        //GET,POST,PUT,DELETE
        #region method GET

        /// <summary>
        /// Lấy tất cả nhân viên
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        //[HttpGet]
        //public IActionResult GetAllEmployee()
        //{
        //    try {
        //        //Truy cập vào database:
        //        // 1.Khai báo đối tượng
        //        var connectionString = "Host = localhost;" +
        //             "Database = MISA.CukCuk_Demo;" +
        //             "User Id = root;" +
        //             "Password = 123456";
        //        // 2.Khởi tạo đối tượng kết nối với database
        //        IDbConnection dbConnection = new MySqlConnection(connectionString);
        //        // 3.Lấy dữ liệu
        //        var sqlCommand = "SELECT * FROM Employee";
        //        var employees = dbConnection.Query<object>(sqlCommand);
        //        // Trả về cho client
        //        if (employees.Count() > 0)
        //        {
        //            var response = StatusCode(200, employees);
        //            return response;
        //        }
        //        else {
        //            return StatusCode(204, employees);
        //        }


        //    } catch (Exception ex)
        //    {
        //        var errorObj = new
        //        {
        //            devMsg = ex.Message,
        //            userMsg = Properties.Resource.Error_Message_UserVN,
        //            errorCode = "misa-001",
        //            moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
        //            traceId = ""
        //        };
        //        return StatusCode(500, errorObj);
        //    }
        //}


        /// <summary>
        /// Hàm lấy nhân viên theo Id
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>1 nhân viên</returns>
        [HttpGet("{employeeId}")]
        public IActionResult GetEmployeeById(Guid employeeId)
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
                var sqlCommand = $"SELECT * FROM Employee WHERE EmployeeId = @EmployeeIdParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeIdParam", employeeId);
                var employee = dbConnection.QueryFirstOrDefault<object>(sqlCommand, param: parameters);
                // Trả về cho client

                var response = StatusCode(200, employee);
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

        /// <summary>
        /// Hàm sinh mã nhân viên mới
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        [HttpGet("NewEmployeeCode")]
        public IActionResult getNewEmployeeCode()
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

                //Thực hiện query lấy mảng mã nhân viên  từ csdl
                string sqlCommand = "SELECT EmployeeCode FROM Employee ORDER BY EmployeeCode DESC LIMIT 1";
                var employeeCode = dbConnection.QueryFirstOrDefault<string>(sqlCommand);

                //var Heading = ((IDictionary<string, object>)employeeCodeRow).Keys.ToArray();
                //var details = ((IDictionary<string, object>)employeeCodeRow);
                //var employeeCode = details[Heading[0]];
                // Xử lí sinh mã  mới
                int currentMax = 0;

                try
                {
                    int codeValue = int.Parse(employeeCode.ToString().Split("-")[1]);
                    if (currentMax < codeValue)
                    {
                        currentMax = codeValue;
                    }
                }
                catch (Exception)
                {
                    var errorResponse = StatusCode(500, 1);
                    return errorResponse;
                }


                string newEmployeeCode = "NV-" + (currentMax + 1);
                var response = StatusCode(200, newEmployeeCode);
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

        /// <summary>
        /// Hàm phân trang
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <param name="pagenumber"></param>
        /// <param name="pagesize"></param>
        /// <returns>Tổng số trang, số bản ghi/trang</returns>
        //pagesize = limit
        //offset = (pagenumber-1) * pagesize
        [HttpGet]

        public IActionResult pagination(int pagesize, int pagenumber, string searchContent)
        {
            try
            {
                //Truy cập vào database:
                // 1.Khai báo đối tượng
                var connectionString = "Host = localhost;" +
                     "Database = MISA.CukCuk_Demo;" +
                     "User Id = root;" +
                     "Password = 123456;"+ 
                     "Allow User Variables=true";
                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);

                // 3.Bắt đầu phần trang
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@limit", pagesize);
                parameters.Add("@offset", (pagenumber - 1) * pagesize);
                parameters.Add("@searchContent",  $"%{searchContent}%");
                var sqlCommand = $"SELECT * FROM Employee Where EmployeeCode LIKE @searchContent LIMIT @limit OFFSET @offset";
                var employees = dbConnection.Query<object>(sqlCommand, param: parameters);
                // Trả về cho client
                if (employees.Count() > 0)
                {
                    var response = StatusCode(200, employees);
                    return response;
                }
                else
                {
                    return StatusCode(204, employees);
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
        #endregion

        #region method POST
        /// <summary>
        /// Hàm thêm nhân viên mới
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertEmployee([FromBody] Employee employee)
        {
            try
            {
                // Kiểm tra dữ liệu
                // 1. Mã nhân viên bắt buộc nhập
                if (employee.EmployeeCode == "" || employee.EmployeeCode == null)
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

                // 2. Email phải đúng định dạng
                var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
                var isMatch = Regex.IsMatch(employee.Email, emailFormat, RegexOptions.IgnoreCase);
                if (isMatch == false)
                {
                    var errorObj = new
                    {
                        userMsg = Properties.Resource.Error_Email,
                        errorCode = "misa-003",
                        moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-003",
                        traceId = ""
                    };
                    return BadRequest(errorObj);
                }
              
                //sinh id nhân viên mới
                employee.EmployeeId = Guid.NewGuid();
                //Truy cập vào database:
                // 1.Khai báo đối tượng
                var connectionString = "Host = localhost;" +
                    "Database = MISA.CukCuk_Demo;" +
                    "User Id = root;" +
                    "Password = 123456;" + 
                    "Allow User Variables=true;";

                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);
                //khai báo dynamicParam:
                var dynamicParam = new DynamicParameters();
                // .2.1 Check mã trùng
                var validateCommand = "SELECT * FROM Employee WHERE EmployeeCode = @EmployeeCodeParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeCodeParam", employee.EmployeeCode);
                var employeeCheck = dbConnection.QueryFirstOrDefault<object>(validateCommand, param: parameters);
                // Trả về cho client
                if (employeeCheck != null)
                {
                    var errorObj = new
                    {
                        userMsg = Properties.Resource.Duplicate_Code,
                        errorCode = "misa-003",
                        moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-003",
                        traceId = ""
                    };
                    return BadRequest(errorObj);
                }
              
                // 3.Thêm dữ liệu vào database
                var columnsName = string.Empty;
                var columnsParam = string.Empty;

                //Đọc từng property của object:
                var properties = employee.GetType().GetProperties();


                //Duyệt từng property:
                foreach (var prop in properties)
                {
                    //lấy tên của prop:
                    var propName = prop.Name;

                    //Lấy value của prop:
                    var propValue = prop.GetValue(employee);

                    //Lấy kiểu dữ liệu của prop:
                    var propType = prop.PropertyType;

                    //thêm param tương ứng với mỗi property của đối tượng
                    dynamicParam.Add($"@{propName}", propValue);

                    columnsName += $"{propName},";
                    columnsParam += $"@{propName},";
                }
                columnsName = columnsName.Remove(columnsName.Length - 1, 1);
                columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);
                var sqlCommand = $"INSERT INTO Employee({columnsName}) VALUES({columnsParam}) ";

                var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);
                // Trả về cho client
                if (rowEffects > 0)
                {
                    return StatusCode(201);        
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

        #endregion

        #region method PUT
        /// <summary>
        /// Hàm cập nhật nhân viên
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut("{employeeId}")]
        public IActionResult UpdateEmployee(Guid employeeId, Employee employee)
        {
            try
            {
                // Kiểm tra dữ liệu
                // 1. Mã nhân viên bắt buộc nhập
                if (employee.EmployeeCode == "" || employee.EmployeeCode == null)
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

                // 2. Email phải đúng định dạng
                var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
                var isMatch = Regex.IsMatch(employee.Email, emailFormat, RegexOptions.IgnoreCase);
                if (isMatch == false)
                {
                    var errorObj = new
                    {
                        userMsg = Properties.Resource.Error_Email,
                        errorCode = "misa-003",
                        moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-003",
                        traceId = ""
                    };
                    return BadRequest(errorObj);
                }

                //Truy cập vào database:
                // 1.Khai báo đối tượng
                var connectionString = "Host = localhost;" +
                     "Database = MISA.CukCuk_Demo;" +
                     "User Id = root;" +
                     "Password = 123456;" +
                     "Allow User Variables=true;";
                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);
                //khai báo dynamicParam:
                var dynamicParam = new DynamicParameters();
                // .2.1 Check mã trùng
                var validateCommand = "SELECT * FROM Employee WHERE EmployeeCode = @EmployeeCodeParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeCodeParam", employee.EmployeeCode);
                Employee employeeCheck = dbConnection.QueryFirstOrDefault<Employee>(validateCommand, param: parameters);
                // Trả về cho client
                if (employeeCheck != null && employeeCheck.EmployeeId != employee.EmployeeId)
                {
                    var errorObj = new
                    {
                        userMsg = Properties.Resource.Duplicate_Code,
                        errorCode = "misa-003",
                        moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-003",
                        traceId = ""
                    };
                    return BadRequest(errorObj);
                }

                // 3.Thêm dữ liệu vào database
                var columnsName = string.Empty;

                //Đọc từng property của object:
                var properties = employee.GetType().GetProperties();

                //Duyệt từng property:
                foreach (var prop in properties)
                {
                    //lấy tên của prop:
                    var propName = prop.Name;

                    //Lấy value của prop:
                    var propValue = prop.GetValue(employee);

                    //Lấy kiểu dữ liệu của prop:
                    var propType = prop.PropertyType;

                    //thêm param tương ứng với mỗi property của đối tượng
                    dynamicParam.Add($"@{propName}", propValue);

                    columnsName += $"{propName} = @{propName},";

                }
                columnsName = columnsName.Remove(columnsName.Length - 1, 1);

                var sqlCommand = $"UPDATE Employee SET {columnsName} WHERE EmployeeId = @EmployeeIdParam ";

                dynamicParam.Add("@EmployeeIdParam", employeeId);
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
        /// Hàm xóa nhân viên
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpDelete("{employeeId}")]
        public IActionResult DeleteEmployee(Guid employeeId)
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
                var sqlCommand = $"DELETE FROM Employee WHERE EmployeeId = @EmployeeIdParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeIdParam", employeeId);
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


