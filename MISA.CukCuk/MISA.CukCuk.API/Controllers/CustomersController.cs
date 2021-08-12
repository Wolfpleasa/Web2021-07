using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MISA.CukCuk.API.Model;
using System.Data;
using MySqlConnector;
using Dapper;
using System.Text.RegularExpressions;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //GET,POST,PUT,DELETE
        #region method GET
        /// <summary>
        /// Hàm lấy tất cả khách hàng
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        [HttpGet]
        public IActionResult GetCustomer()
        {
            try
            {
                //Truy cập vào database:
                // 1.Khai báo đối tượng
                var connectionString = "Host = 47.241.69.179;" +
                    "Database = MISA.CukCuk_Demo_NVMANH;" +
                    "User Id = dev;" +
                    "Password = 12345678";
                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);
                // 3.Lấy dữ liệu
                var sqlCommand = "SELECT * FROM Customer";
                var customers = dbConnection.Query<object>(sqlCommand);
                // Trả về cho client
                if (customers.Count() > 0)
                {
                    var response = StatusCode(200, customers);
                    return response;
                }
                else
                {
                    return StatusCode(204, customers);
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
        /// Hàm lấy khách hàng theo id
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>Khách hàng</returns>
        [HttpGet("{customerId}")]
        public IActionResult GetCustomerById(Guid customerId)
        {
            try
            {
                //Truy cập vào database:
                // 1.Khai báo đối tượng
                var connectionString = "Host = 47.241.69.179;" +
                    "Database = MISA.CukCuk_Demo_NVMANH;" +
                    "User Id = dev;" +
                    "Password = 12345678";
                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);
                // 3.Lấy dữ liệu
                var sqlCommand = $"SELECT * FROM Customer WHERE CustomerId = @CustomerIdParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CustomerIdParam", customerId);
                var customer = dbConnection.QueryFirstOrDefault<object>(sqlCommand, param: parameters);
                // Trả về cho client

                var response = StatusCode(200, customer);
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
        /// Sinh mã khách hàng mới
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns>Mã khách hàng mới</returns>
        [HttpGet("NewCustomerCode")]
        public IActionResult getNewCustomerCode()
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
                string sqlCommand = "SELECT CustomerCode FROM Customer ORDER BY CustomerCode DESC LIMIT 1";
                var customerCode = dbConnection.QueryFirstOrDefault<string>(sqlCommand);

                int currentMax = 0;

                try
                {
                    int codeValue = int.Parse(customerCode.ToString().Split("-")[1]);
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

                string newCustomerCode = "NV-" + (currentMax + 1);
                var response = StatusCode(200, newCustomerCode);
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
        /// Hàm thêm khách hàng mới
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertCustomer(Customer customer)
        {
            try
            {   
                // Kiểm tra dữ liệu
                // 1. Mã khách hàng bắt buộc nhập
                if (customer.CustomerCode == "" || customer.CustomerCode == null)
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
                var isMatch = Regex.IsMatch(customer.Email, emailFormat, RegexOptions.IgnoreCase);
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
                // 3. Check mã trùng

                customer.CustomerId = Guid.NewGuid();
                //Truy cập vào database:
                // 1.Khai báo đối tượng
                var connectionString = "Host = 47.241.69.179;" +
                    "Database = MISA.CukCuk_Demo_NVMANH;" +
                    "User Id = dev;" +
                    "Password = 12345678";
                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);
                //khai báo dynamicParam:
                var dynamicParam = new DynamicParameters();
                // .2.1 Check mã trùng
                var validateCommand = "SELECT * FROM Customer WHERE CustomerCode = @CustomerCodeParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CustomerIdParam", customer.CustomerCode);
                var customerCheck = dbConnection.QueryFirstOrDefault<Customer>(validateCommand);
                // Trả về cho client
                if (customerCheck != null)
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
                var properties = customer.GetType().GetProperties();


                //Duyệt từng property:
                foreach (var prop in properties)
                {
                    //lấy tên của prop:
                    var propName = prop.Name;

                    //Lấy value của prop:
                    var propValue = prop.GetValue(customer);

                    //Lấy kiểu dữ liệu của prop:
                    var propType = prop.PropertyType;

                    //thêm param tương ứng với mỗi property của đối tượng
                    dynamicParam.Add($"@{propName}", propValue);

                    columnsName += $"@{propName},";
                    columnsParam += $"@{propName},";
                }
                columnsName = columnsName.Remove(columnsName.Length - 1, 1);
                columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);
                var sqlCommand = $"INSERT INTO Customer({columnsName}) VALUES({columnsParam}) ";
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
        /// Hàm cập nhật thông tin khách hàng
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPut("{customerId}")]
        public IActionResult UpdateCustomer(Guid customerId, Customer customer)
        {
            try
            {
                // Kiểm tra dữ liệu
                // 1. Mã khách hàng bắt buộc nhập
                if (customer.CustomerCode == "" || customer.CustomerCode == null)
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
                var isMatch = Regex.IsMatch(customer.Email, emailFormat, RegexOptions.IgnoreCase);
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
                     "Password = 123456";
                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);
                //khai báo dynamicParam:
                var dynamicParam = new DynamicParameters();

                // .2.1 Check mã trùng
                var validateCommand = "SELECT * FROM Customer WHERE CustomerCode = @CustomerCodeParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CustomerIdParam", customer.CustomerCode);
                Customer customerCheck = dbConnection.QueryFirstOrDefault<Customer>(validateCommand);
                // Trả về cho client
                if (customerCheck != null && customerCheck.CustomerId != customer.CustomerId)
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
                var properties = customer.GetType().GetProperties();

                //Duyệt từng property:
                foreach (var prop in properties)
                {
                    //lấy tên của prop:
                    var propName = prop.Name;

                    //Lấy value của prop:
                    var propValue = prop.GetValue(customer);

                    //Lấy kiểu dữ liệu của prop:
                    var propType = prop.PropertyType;

                    //thêm param tương ứng với mỗi property của đối tượng
                    dynamicParam.Add($"@{propName}", propValue);

                    columnsName += $"{propName} = @{propName},";

                }
                columnsName = columnsName.Remove(columnsName.Length - 1, 1);

                var sqlCommand = $"UPDATE Customer SET {columnsName} WHERE CustomerId = @CustomerIdParam ";

                dynamicParam.Add("@CustomerIdParam", customerId);
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
        /// Hàm xóa khách hàng
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpDelete("{customerId}")]
        public IActionResult DeleteCustomer(Guid customerId)
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
                var sqlCommand = $"DELETE FROM Customer WHERE CustomerId = @CustomerIdParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CustomerIdParam", customerId);
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
