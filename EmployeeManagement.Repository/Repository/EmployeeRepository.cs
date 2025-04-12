using Dapper;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Repository.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DBContext _dbContext;
        public EmployeeRepository(DBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var sql = "SELECT ID, Name, Email, Position, Salary FROM Employee";

            var result = await _dbContext.Connection.QueryAsync<dynamic>(sql);
            var reimbursements = result.Select(r => new Employee
            {
                ID = r.ID,
                Name = r.Name,
                Email = r.Email,
                Position = r.Position,
                Salary = r.Salary
            }).ToList();

            return reimbursements;
        }

        public async Task<Employee?> GetByIdAsync(string id)
        {
            return await _dbContext.Connection.QueryFirstOrDefaultAsync<Employee>(
                "SELECT * FROM Employee WHERE ID = @Id", new { ID = id });
        }
        public async Task<int> AddAsync(Employee employee)
        {
            employee.ID = Guid.NewGuid().ToString();
            var sql = "INSERT INTO Employee (ID, Name, Email, Position, Salary) VALUES (@ID, @Name, @Email, @Position, @Salary); SELECT SCOPE_IDENTITY();";
            return await _dbContext.Connection.ExecuteScalarAsync<int>(sql, employee);
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            var sql = "UPDATE Employee SET Name = @Name, Email = @Email, Position = @Position, Salary = @Salary  WHERE ID = @ID";
            var rowsAffected = await _dbContext.Connection.ExecuteAsync(sql, employee);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            var sql = "DELETE FROM Employee WHERE ID = @ID";
            var rowsAffected = await _dbContext.Connection.ExecuteAsync(sql, new { ID = id });
            return rowsAffected > 0;
        }

        public async Task<List<string>> GetPositions()
        {
            var positions = new List<string>
            {
                "Staf", "Manager", "Supervisor", "General Manager", "Product Manager"
            };

            return positions;
        }
    }
}
