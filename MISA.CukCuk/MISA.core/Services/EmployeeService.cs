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
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        IEmployeeRepository _employeeRepository;
        ServiceResult _serviceResult;

        public EmployeeService(IEmployeeRepository employeeRepository, IBaseRepository<Employee> baseRepository) : base(baseRepository)
        {
            _serviceResult = new ServiceResult();
            _employeeRepository = employeeRepository;
        }

        public override ServiceResult Add(Employee entity)
        {
            try
            {
                //validate định dạng mã nv
                var propValue = entity.GetType().GetProperty("EmployeeCode").GetValue(entity).ToString().Trim();

                if (string.IsNullOrEmpty(propValue))
                {
                    _serviceResult.Message = Properties.ResourceVN.Empty_EmployeeCode;
                    _serviceResult.isValid = false;
                    return _serviceResult;
                }
                                             
                var employeeCodeFormat = @"^(NV-\d+)$"; 
                var isMatch = Regex.IsMatch(propValue, employeeCodeFormat, RegexOptions.IgnoreCase);
                if (!isMatch)
                {
                    _serviceResult.Message = Properties.ResourceVN.EmployeeCode;
                    _serviceResult.isValid = false;
                    return _serviceResult;
                }
                    
                return base.Add(entity);
            }
            catch(Exception)
            {
                _serviceResult.isValid = false;
                _serviceResult.Message = Properties.ResourceVN.Error_Message_UserVN;
                return _serviceResult;
            }               
        }

        public override ServiceResult Edit(Employee entity, Guid entityId)
        {
            try
            {
                //validate định dạng mã nv
                var propValue = entity.GetType().GetProperty("EmployeeCode").GetValue(entity).ToString().Trim();

                if (string.IsNullOrEmpty(propValue))
                {
                    _serviceResult.Message = Properties.ResourceVN.Empty_EmployeeCode;
                    _serviceResult.isValid = false;
                    return _serviceResult;
                }

                var employeeCodeFormat = @"^(NV-\d+)$";
                var isMatch = Regex.IsMatch(propValue, employeeCodeFormat, RegexOptions.IgnoreCase);

                if (!isMatch)
                {
                    _serviceResult.Message = Properties.ResourceVN.EmployeeCode;
                    _serviceResult.isValid = false;
                    return _serviceResult;
                }

                return base.Edit(entity, entityId);
            }
            catch (Exception)
            {
                _serviceResult.isValid = false;
                _serviceResult.Message = Properties.ResourceVN.Error_Message_UserVN;
                return _serviceResult;
            }

        }
    }

}
