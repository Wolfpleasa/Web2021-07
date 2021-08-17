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
    public class PositionsController : ControllerBase
    {
        IPositionService _positionService;
        IBaseRepository<Position> _baseRepository;
        public PositionsController(IPositionService positionService, IBaseRepository<Position> baseRepository)
        {
            _positionService = positionService;
            _baseRepository = baseRepository;
        }
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
                var positions = _baseRepository.GetAll();
                return Ok(positions);
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
        /// Hàm lấy vị trí theo id
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns>Vị trí</returns>
        [HttpGet("{positionId}")]
        public IActionResult GetPositionById(Guid positionId)
        {
            try
            {
                var position = _baseRepository.GetById(positionId);
                return Ok(position);
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
                var serviceResult = _positionService.Add(position);

                // Trả về cho client
                if (serviceResult.isValid == true)
                {
                    return StatusCode(201, serviceResult.Data);
                }
                else {
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
        /// Hàm cập nhật vị trí
        /// Author: Ngọc 12/8/2021
        /// </summary>
        /// <returns></returns>
        [HttpPut("{positionId}")]
        public IActionResult UpdatePosition(Guid positionId, Position position)
        {
            try
            {
                var serviceResult = _positionService.Edit(position, positionId);
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
                var rowEffect = _baseRepository.Delete(positionId);
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
