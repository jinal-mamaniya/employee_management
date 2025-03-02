using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        public EmployeeRepository(AppDbContext context) 
        {
            _context = context;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
           await _context.Employees.AddAsync(employee);
           await _context.SaveChangesAsync();

        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employeeInDB = await _context.Employees.FindAsync(id);

            if (employeeInDB != null)
            {
                _context.Employees.Remove(employeeInDB);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Employee with ${id} was not found");
            }
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            var trackedEntity = await _context.Employees.FindAsync(employee.Id);
            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}
