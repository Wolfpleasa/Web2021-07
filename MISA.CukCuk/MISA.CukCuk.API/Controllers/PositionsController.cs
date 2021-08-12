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
    public class PositionsController : ControllerBase
    {   
        //GET, POST, PUT, DELETE
        #region method GET
        /// <summary>
        /// Hàm lấy tất cả vị trí
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns>Danh sách vị trí</returns>
        [HttpGet]
        public IActionResult GetAllPosition()
        {
            try
            {

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
            //Truy cập vào database:
            // 1.Khai báo đối tượng
            var connectionString = "Host = localhost;" +
                 "Database = MISA.CukCuk_Demo;" +
                 "User Id = root;" +
                 "Password = 123456";

            // 2.Khởi tạo đối tượng kết nối với database
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3.Lấy dữ liệu
            var sqlCommand = "SELECT * FROM Position";
            var departments = dbConnection.Query<object>(sqlCommand);
            // Trả về cho client

            var response = StatusCode(200, departments);
            return response;
        }

        /// <summary>
        /// Hàm lấy vị trí theo id
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns>Vị trí</returns>
        [HttpGet("{positionId}")]
        public IActionResult GetPositionById(Guid positionId)
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
                var sqlCommand = $"SELECT * FROM  Position WHERE positionId = @PositionIdParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@PositionIdParam", positionId);
                var position = dbConnection.QueryFirstOrDefault<object>(sqlCommand, param: parameters);
                // Trả về cho client

                var response = StatusCode(200, position);
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
        /// Hàm thêm vị trí mới
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertPosition([FromBody] Position position)
        {
            try
            {           
                // Kiểm tra dữ liệu
                // 1. Mã phòng ban bắt buộc nhập
                if (position.PositionCode == "" || position.PositionCode == null)
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
                //Sinh id vị trí 
                position.PositionId = Guid.NewGuid();
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
                var properties = position.GetType().GetProperties();

                //Duyệt từng property:
                foreach (var prop in properties)
                {
                    //lấy tên của prop:
                    var propName = prop.Name;

                    //Lấy value của prop:
                    var propValue = prop.GetValue(position);

                    //Lấy kiểu dữ liệu của prop:
                    var propType = prop.PropertyType;

                    //thêm param tương ứng với mỗi property của đối tượng
                    dynamicParam.Add($"@{propName}", propValue);

                    columnsName += $"{propName},";
                    columnsParam += $"@{propName},";
                }
                columnsName = columnsName.Remove(columnsName.Length - 1, 1);
                columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);
                var sqlCommand = $"INSERT INTO Position ({columnsName}) VALUES ({columnsParam}) ";

                var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);
                // Trả về cho client
                if (rowEffects > 0)
                {
                    return StatusCode(201, rowEffects);                 
                }
                else return StatusCode(200);
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
        /// Hàm cập nhật vị trí
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns></returns>
        [HttpPut("{positionId}")]
        public IActionResult UpdatePosition(Guid positionId, Position position)
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
                var properties = position.GetType().GetProperties();

                //Duyệt từng property:
                foreach (var prop in properties)
                {
                    //lấy tên của prop:
                    var propName = prop.Name;

                    //Lấy value của prop:
                    var propValue = prop.GetValue(position);

                    //Lấy kiểu dữ liệu của prop:
                    var propType = prop.PropertyType;

                    //thêm param tương ứng với mỗi property của đối tượng
                    dynamicParam.Add($"@{propName}", propValue);

                    columnsName += $"{propName} = @{propName},";

                }
                columnsName = columnsName.Remove(columnsName.Length - 1, 1);

                var sqlCommand = $"UPDATE Position SET {columnsName} WHERE positionId = @PositionIdParam ";

                dynamicParam.Add("@PositionIdParam", positionId);
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
        /// <param name="positionId"></param>
        /// <returns></returns>
        [HttpDelete("{positionId}")]
        public IActionResult DeleteDepartment(Guid positionId)
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
                var sqlCommand = $"DELETE FROM Position WHERE PositionId = @PositionIdParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@PositionIdParam", positionId);
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
