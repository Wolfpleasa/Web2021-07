using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class BaseService<MISAEntity> : IBaseService<MISAEntity>
    {
        IBaseRepository _baseRepository;
        ServiceResult _serviceResult;

        public BaseService(IBaseRepository baseRepository)
        {
            _serviceResult = new ServiceResult();
            _baseRepository = baseRepository;         
        }
        public ServiceResult Add(MISAEntity entity)
        {
            //validate dữ liệu và xử lí nghiệp vụ
            //    // Kiểm tra dữ liệu
            //    // 1. Mã khách hàng bắt buộc nhập
            //    if (customer.CustomerCode == "" || customer.CustomerCode == null)
            //    {
            //        var errorObj = new
            //        {
            //            userMsg = Properties.ResourceVN.Error_Message_UserVN,
            //            errorCode = "misa-001",
            //            moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
            //            traceId = ""
            //        };
            //        _serviceResult.isValid = false;
            //        _serviceResult.Data = errorObj;
            //    }

            //    // 2. Email phải đúng định dạng
            //    var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
            //    var isMatch = Regex.IsMatch(customer.Email, emailFormat, RegexOptions.IgnoreCase);
            //    if (isMatch == false)
            //    {
            //        var errorObj = new
            //        {
            //            userMsg = Properties.ResourceVN.Error_Email,
            //            errorCode = "misa-003",
            //            moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-003",
            //            traceId = ""
            //        };
            //        _serviceResult.isValid = false;
            //        _serviceResult.Data = errorObj;
            //    }
            //    // 3. Check mã trùng
            //Thực hiện thêm mới
            _serviceResult.Data = _baseRepository.Add<MISAEntity>(entity);
            return _serviceResult;
            
        }

        public ServiceResult Edit(MISAEntity entity, Guid entityId)
        {
            //validate dữ liệu và xử lí nghiệp vụ

            //Thực hiện sửa
            _serviceResult.Data = _baseRepository.Edit<MISAEntity>(entity, entityId);
            return _serviceResult;
        }
    }
}
