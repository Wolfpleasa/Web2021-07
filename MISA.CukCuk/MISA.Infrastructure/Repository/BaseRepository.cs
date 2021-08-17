using Dapper;
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
    public class BaseRepository<MISAEntity> : IBaseRepository<MISAEntity>
    {
        public List<MISAEntity> GetAll()
        {
            var className = typeof(MISAEntity).Name;
            //Truy cập vào database:
            // 1.Khai báo đối tượng
            var connectionString = "Host = localhost;" +
              "Database = MISA.CukCuk_Demo;" +
              "User Id = root;" +
              "Password = 123456";
            // 2.Khởi tạo đối tượng kết nối với database
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            // 3.Lấy dữ liệu
            var sqlCommand = $"SELECT * FROM {className}";
            var entities = dbConnection.Query<MISAEntity>(sqlCommand);
            return entities.ToList();
        }

        public MISAEntity GetById(Guid entityId)
        {
            var className = typeof(MISAEntity).Name;
            //Truy cập vào database:
            // 1.Khai báo đối tượng
            var connectionString = "Host = localhost;" +
                 "Database = MISA.CukCuk_Demo;" +
                 "User Id = root;" +
                 "Password = 123456";
            // 2.Khởi tạo đối tượng kết nối với database
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            // 3.Lấy dữ liệu
            var sqlCommand = $"SELECT * FROM {className} WHERE {className}Id = @EntityIdParam";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EntityIdParam", entityId);
            var entity = dbConnection.QueryFirstOrDefault<MISAEntity>(sqlCommand, param: parameters);
            return entity;
        }

        public int Add(MISAEntity entity)
        {
            var className = typeof(MISAEntity).Name;
       
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
            var properties = entity.GetType().GetProperties();


            //Duyệt từng property:
            foreach (var prop in properties)
            {
                //lấy tên của prop:
                var propName = prop.Name;
                // Nếu  tên của prop là EmployeeId,CustomerId...
             
                //Lấy value của prop:
                var propValue = prop.GetValue(entity);
                if (propName == $"{className}Id" && prop.PropertyType == typeof(Guid))
                {
                    //sinh id mới
                    propValue = Guid.NewGuid();
                }

                //Lấy kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                //thêm param tương ứng với mỗi property của đối tượng
                dynamicParam.Add($"@{propName}", propValue);

                columnsName += $"{propName},";
                columnsParam += $"@{propName},";
               
            }
          
            columnsName = columnsName.Remove(columnsName.Length - 1, 1);
            columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);

    
            var sqlCommand = $"INSERT INTO {className}({columnsName}) VALUES({columnsParam}) ";
            var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);
            return rowEffects;
        }

        public int Edit(MISAEntity entity, Guid entityId)
        {
            var className = typeof(MISAEntity).Name;
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
            //Customer customerCheck = dbConnection.QueryFirstOrDefault<Customer>(validateCommand, param: parameters);
            //// Trả về cho client
            //if (customerCheck != null && customerCheck.CustomerId != customer.CustomerId)
            //{
            //    var errorObj = new
            //    {
            //        userMsg = Properties.ResourcVN.Duplicate_Code,
            //        errorCode = "misa-003",
            //        moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-003",
            //        traceId = ""
            //    };
            //    return BadRequest(errorObj);
            //}

            // 3.Thêm dữ liệu vào database
            var columnsName = string.Empty;

            //Đọc từng property của object:
            var properties = entity.GetType().GetProperties();

            //Duyệt từng property:
            foreach (var prop in properties)
            {
                //lấy tên của prop:
                var propName = prop.Name;

                //Lấy value của prop:
                var propValue = prop.GetValue(entity);

                //Lấy kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                //thêm param tương ứng với mỗi property của đối tượng
                dynamicParam.Add($"@{propName}", propValue);

                columnsName += $"{propName} = @{propName},";

            }
            columnsName = columnsName.Remove(columnsName.Length - 1, 1);

            var sqlCommand = $"UPDATE {className} SET {columnsName} WHERE {className}Id = @EntityIdParam ";

            dynamicParam.Add("@EntityIdParam", entityId);
            var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);
            return rowEffects;
        }

        public int Delete(Guid entityId)
        {
            var className = typeof(MISAEntity).Name;
            //Truy cập vào database:
            // 1.Khai báo đối tượng
            var connectionString = "Host = localhost;" +
                 "Database = MISA.CukCuk_Demo;" +
                 "User Id = root;" +
                 "Password = 123456";
            // 2.Khởi tạo đối tượng kết nối với database
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3.Lấy dữ liệu
            var sqlCommand = $"DELETE FROM {className} WHERE {className}Id = @EntityIdParam";
            Console.WriteLine(sqlCommand);
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EntityIdParam", entityId);
            var rowEffects = dbConnection.Execute(sqlCommand, param: parameters);
            return rowEffects;
        }

    }
}
