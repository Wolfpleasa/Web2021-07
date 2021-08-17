using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
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
        IDepartmentService _departmentService;
        IBaseRepository<Department> _baseRepository;
        public DepartmentsController(IDepartmentService departmentService, IBaseRepository<Department> baseRepository)
        {
            _departmentService = departmentService;
            _baseRepository = baseRepository;
        }
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
                var departments = _baseRepository.GetAll();
                return Ok(departments);
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
                var department = _baseRepository.GetById(departmentId);
                return Ok(department);
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
                var serviceResult = _departmentService.Add(department);

                if (serviceResult.isValid)
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
                var serviceResult = _departmentService.Edit(department, departmentId);

                if(serviceResult.isValid == true)
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
        /// Hàm xóa phòng ban 
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{departmentId}")]
        public IActionResult DeleteDepartment(Guid departmentId)
        {
            try
            {
                var rowEffect = _baseRepository.Delete(departmentId);
                return Ok(rowEffect);
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
