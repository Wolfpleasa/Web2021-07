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
using MISA.Core.Interfaces.Services;
using MISA.Core.Interfaces.Repository;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        ICustomerService _customerSevice;
        IBaseRepository<Customer> _baseRepository;
        public CustomersController(ICustomerService customerSevice, IBaseRepository<Customer> baseRepository)
        {
            _customerSevice = customerSevice;
            _baseRepository = baseRepository;
        }
        //GET,POST,PUT,DELETE
        #region method GET
        /// <summary>
        /// Hàm lấy tất cả khách hàng
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        [HttpGet]
        public IActionResult GetAllCustomer()
        {
            try
            {
                var customers = _baseRepository.GetAll();
                return Ok(customers);           
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resource.Error_Message_UserVN,                
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
                var customer = _baseRepository.GetById(customerId);
                // Trả về cho client
                return Ok(customer);                     
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

                string newCustomerCode = "KH-" + (currentMax + 1);
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
                var serviceResult = _customerSevice.Add(customer);                          

                // Trả về cho client
                if (serviceResult.isValid == true)
                {
                    return StatusCode(201, serviceResult.Data);
                }
                else
                {
                    return BadRequest(serviceResult.Data);
                }
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = MISA.Core.Properties.ResourceVN.Error_Message_UserVN,
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
                var serviceResult = _customerSevice.Edit(customer, customerId);

                // Trả về cho client
                if (serviceResult.isValid == true)
                {
                    return StatusCode(200, serviceResult.Data);
                }
                else
                {
                    return BadRequest(serviceResult.Data);
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
                var rowEffects = _baseRepository.Delete(customerId);
                return Ok(rowEffects);

            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resource.Error_Message_UserVN,
                };
                return StatusCode(500, errorObj);
            }

        }
        #endregion

    }
}
