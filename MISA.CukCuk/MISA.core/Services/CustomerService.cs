using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class CustomerService : ICustomerService
    {
        ICustomerRepository _customerRepository;
        ServiceResult _serviceResult;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _serviceResult = new ServiceResult();
            _customerRepository = customerRepository;
        }
        

        public ServiceResult Add(Customer customer)
        {
            //Xử lý nghiệp vụ

            // Kiểm tra dữ liệu
            // 1. Mã khách hàng bắt buộc nhập
            if (customer.CustomerCode == "" || customer.CustomerCode == null)
            {
                var errorObj = new
                {
                    userMsg = Properties.ResourceVN.Error_Message_UserVN,
                    errorCode = "misa-001",
                    moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                _serviceResult.isValid = false;
                _serviceResult.Data = errorObj;
            }

            // 2. Email phải đúng định dạng
            var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
            var isMatch = Regex.IsMatch(customer.Email, emailFormat, RegexOptions.IgnoreCase);
            if (isMatch == false)
            {
                var errorObj = new
                {
                    userMsg = Properties.ResourceVN.Error_Email,
                    errorCode = "misa-003",
                    moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-003",
                    traceId = ""
                };
                _serviceResult.isValid = false;
                _serviceResult.Data = errorObj;
            }
            // 3. Check mã trùng

            //Tương tác với db
            _serviceResult.Data = _customerRepository.Add(customer);
            return _serviceResult;
            throw new NotImplementedException();
        }

        public ServiceResult Edit(Customer customer, Guid customerId)
        {
            _serviceResult.Data = _customerRepository.Edit(customer, customerId);
            return _serviceResult;
            throw new NotImplementedException();
        }
    }
}
