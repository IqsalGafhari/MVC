using System.Data.SqlClient;
using System.Transactions;
using System.Xml.Linq;
using LINQ;

namespace LINQ;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime HireDate { get; set; }
    public int Salary { get; set; }
    public decimal CommissionPct { get; set; }
    public int? ManagerId { get; set; }
    public string JobId { get; set; }
    public int DepartmentId { get; set; }

    public override string ToString()
    {
        return $"{Id} - {HireDate} - {FirstName} - {LastName} - {Email} - {PhoneNumber}";
    }
    // GET ALL: Employee    
    public List<Employee> GetAll()
    {        
        var employees = new List<Employee>();        
        using var connection = Provider.GetConnection();       
        using var command = connection.CreateCommand();        
        command.CommandText = "SELECT * FROM employees";

        try
        {            
            connection.Open();            
            using var reader = command.ExecuteReader();
            
            if (reader.HasRows)
            {               
                while (reader.Read())
                {                    
                    employees.Add(new Employee
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Email = reader.GetString(3),
                        PhoneNumber = reader.IsDBNull(4) ? null : reader.GetString(4),
                        HireDate = reader.GetDateTime(5),
                        Salary = reader.GetInt32(6),
                        CommissionPct = reader.GetDecimal(7),
                        ManagerId = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8),
                        JobId = reader.GetString(9),
                        DepartmentId = reader.GetInt32(10)
                    });
                }                
                reader.Close();
                connection.Close();                
                return employees;
            }            
            reader.Close();
            connection.Close();
            
            return new List<Employee>();
        }
        
        catch (Exception ex)
        {           
            Console.WriteLine($"Error: {ex.Message}");
        }        
        return new List<Employee>();
    }

    // GET BY ID: Employee
   
    public Employee GetById(int id)
    {        
        var employee = new Employee();        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();       
        command.CommandText = "SELECT * FROM employees WHERE id = @id";

        try
        {           
            command.Parameters.Add(new SqlParameter("@id", id));            
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;                
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {                       
                        employee.Id = reader.GetInt32(0);
                        employee.FirstName = reader.GetString(1);
                        employee.LastName = reader.IsDBNull(2) ? null : reader.GetString(2);
                        employee.Email = reader.GetString(3);
                        employee.PhoneNumber = reader.IsDBNull(4) ? null : reader.GetString(4);
                        employee.HireDate = reader.GetDateTime(5);
                        employee.Salary = reader.GetInt32(6);
                        employee.CommissionPct = reader.GetDecimal(7);
                        employee.ManagerId = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8);
                        employee.JobId = reader.GetString(9);
                        employee.DepartmentId = reader.GetInt32(10);
                    }                   
                    reader.Close();
                    connection.Close();                
                    return employee;
                }               
                reader.Close();
                connection.Close();                
                return employee;
            }
            catch (Exception ex)
            {                
                transaction.Rollback();                
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        catch (Exception ex)
        {            
            Console.WriteLine($"Error: {ex.Message}");
        }       
        return employee;
    }

    // INSERT: Employee
    
    public string Insert(Employee employee)
    {        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();        
        command.CommandText = "INSERT INTO employees (id, first_name, last_name, email, phone_number, hire_date, salary, commission_pct, manager_id, job_id, department_id) VALUES (@id, @firstName, @lastName, @email, @phoneNumber, @hireDate, @salary, @commissionPct, @managerId, @jobId, @departmentId);";

        try
        {            
            command.Parameters.Add(new SqlParameter("@id", employee.Id));
            command.Parameters.Add(new SqlParameter("@firstName", employee.FirstName));
            command.Parameters.Add(new SqlParameter("@lastName", employee.LastName ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@email", employee.Email));
            command.Parameters.Add(new SqlParameter("@phoneNumber", employee.PhoneNumber ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@hireDate", employee.HireDate));
            command.Parameters.Add(new SqlParameter("@salary", employee.Salary));
            command.Parameters.Add(new SqlParameter("@commissionPct", employee.CommissionPct));
            command.Parameters.Add(new SqlParameter("@managerId", employee.ManagerId ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@jobId", employee.JobId));
            command.Parameters.Add(new SqlParameter("@departmentId", employee.DepartmentId));
            
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;
                
                var result = command.ExecuteNonQuery();
                
                transaction.Commit();
                connection.Close();
              
                if (result > 0)
                {
                    return "Insert Success";
                }
                return "Insert Failed";
            }
            catch (Exception ex)
            {                
                transaction.Rollback();                
                return $"Error Transaction: {ex.Message}";
            }
        }
        catch (Exception ex)
        {            
            return $"Error: {ex.Message}";
        }
    }

    // UPDATE: Employee    
    public string Update(Employee employee)
    {        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();        
        command.CommandText = "UPDATE employees SET first_name = @firstName, last_name = @lastName, email = @email, phone_number = @phoneNumber, hire_date = @hireDate, salary = @salary, commission_pct = @commissionPct, manager_id = @managerId, job_id = @jobId, department_id = @departmentId WHERE id = @id;";

        try
        {            
            command.Parameters.Add(new SqlParameter("@firstName", employee.FirstName));
            command.Parameters.Add(new SqlParameter("@lastName", employee.LastName ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@email", employee.Email));
            command.Parameters.Add(new SqlParameter("@phoneNumber", employee.PhoneNumber ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@hireDate", employee.HireDate));
            command.Parameters.Add(new SqlParameter("@salary", employee.Salary));
            command.Parameters.Add(new SqlParameter("@commissionPct", employee.CommissionPct));
            command.Parameters.Add(new SqlParameter("@managerId", employee.ManagerId ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@jobId", employee.JobId));
            command.Parameters.Add(new SqlParameter("@departmentId", employee.DepartmentId));
            
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;               
                var result = command.ExecuteNonQuery();               
                transaction.Commit();
                connection.Close();
               
                if (result > 0)
                {
                    return "Update Success";
                }
                return "Update Failed";
            }
            catch (Exception ex)
            {                
                transaction.Rollback();              
                return $"Error Transaction: {ex.Message}";
            }
        }
        catch (Exception ex)
        {    
            return $"Error: {ex.Message}";
        }
    }

    // DELETE: Employee    
    public string Delete(int id)
    {        
        using var connection = Provider.GetConnection();       
        using var command = connection.CreateCommand();       
        command.CommandText = "DELETE FROM employees WHERE id = @id;";
        try
        {            
            command.Parameters.Add(new SqlParameter("@id", id));            
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;               
                var result = command.ExecuteNonQuery();                
                transaction.Commit();
                connection.Close();                
                if (result > 0)
                {
                    return "Delete Success";
                }
                return "Delete Failed";
            }
            catch (Exception ex)
            {                
                transaction.Rollback();                
                return $"Error Transaction: {ex.Message}";
            }
        }
        catch (Exception ex)
        {           
            return $"Error: {ex.Message}";
        }
    }
}