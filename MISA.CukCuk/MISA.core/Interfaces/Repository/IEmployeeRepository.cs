using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repository
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {   
        /// <summary>
        /// Phân trang nhân viên
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="searchContent"></param>
        /// <param name="positionId"></param>
        /// <param name="departmentId"></param>
        /// <returns>Danh sách nhân viên</returns>
        /// Created By: Ngọc 19/8/2021
        public PagingResult<Employee> Pagination(int? pageSize, int? pageNumber, string searchContent, Guid? positionId, Guid? departmentId);

        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        /// Created By: Ngọc 19/8/2021
        public string GetNewCode();
    }
}
