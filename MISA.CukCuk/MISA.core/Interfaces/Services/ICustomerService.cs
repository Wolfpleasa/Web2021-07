using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Services
{
    public interface ICustomerService
    {   
        /// <summary>
        /// Nghiệp vụ thêm mới khách hàng 
        /// Author: Ngọc 13/8/2021
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>ServiceResult: Kết quả xử lý nghiệp vụ</returns>
        ServiceResult Add(Customer customer);

        /// <summary>
        /// Nghiệp vụ sửa thông tin khách hàng 
        /// Author: Ngọc 13/8/2021
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>ServiceResult: Kết quả xử lý nghiệp vụ</returns>
        ServiceResult Edit(Customer customer, Guid customerId);
    }
}
