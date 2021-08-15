using Dapper;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        public int Add(Customer customer)
        {
            //sinh mã khách hàng mới
            customer.CustomerId = Guid.NewGuid();
            //Truy cập vào database:
            // 1.Khai báo đối tượng
            var connectionString = "Host = localhost;" +
               "Database = MISA.CukCuk_Demo;" +
               "User Id = root;" +
               "Password = 123456;" +
               "Allow User Variables=true;";
            // 2.Khởi tạo đối tượng kết nối với database
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            //khai báo dynamicParam:
            var dynamicParam = new DynamicParameters();
            // .2.1 Check mã trùng
            //var validateCommand = "SELECT * FROM Customer WHERE CustomerCode = @CustomerCodeParam";
            //DynamicParameters parameters = new DynamicParameters();
            //parameters.Add("@CustomerCodeParam", customer.CustomerCode);
            //var customerCheck = dbConnection.QueryFirstOrDefault<Customer>(validateCommand, param: parameters);
            //// Trả về cho client
            //if (customerCheck != null)
            //{
            //    var errorObj = new
            //    {
            //        userMsg = Properties.Resource.Duplicate_Code,
            //        errorCode = "misa-003",
            //        moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-003",
            //        traceId = ""
            //    };
            //    return BadRequest(errorObj);
            //}

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

                columnsName += $"{propName},";
                columnsParam += $"@{propName},";
            }
            columnsName = columnsName.Remove(columnsName.Length - 1, 1);
            columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);
            var sqlCommand = $"INSERT INTO Customer({columnsName}) VALUES({columnsParam}) ";
            var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);
            return rowEffects;
        }

        public int Delete(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public int Edit(Customer customer, Guid customerId)
        {
            throw new NotImplementedException();
        }

        public List<Customer> Get()
        {
            throw new NotImplementedException();
        }

        public Customer GetById(Guid customerId)
        {
            throw new NotImplementedException();
        }
    }
}
