using System.Data.SqlClient;
using System.Transactions;
using System.Xml.Linq;
using LINQ;

namespace LINQ;

public class History
{
    public DateTime StartDate { get; set; }
    public int EmployeeId { get; set; }
    public DateTime? EndDate { get; set; }
    public int DepartmentId { get; set; }
    public string jobId { get; set; }


    // GET ALL: History
    
    public List<History> GetAll()
    {        
        var histories = new List<History>();        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();        
        command.CommandText = "SELECT * FROM histories";

        try
        {            
            connection.Open();            
            using var reader = command.ExecuteReader();
            
            if (reader.HasRows)
            {                
                while (reader.Read())
                {                    
                    histories.Add(new History
                    {
                        StartDate = reader.GetDateTime(2),
                        EmployeeId = reader.GetInt32(3),
                        EndDate = reader.GetDateTime(4),
                        DepartmentId = reader.GetInt32(5),
                        jobId = reader.GetString(6),

                    }); ;
                }                
                reader.Close();
                connection.Close();                
                return histories;
            }            
            reader.Close();
            connection.Close();
            
            return new List<History>();
        }
        
        catch (Exception ex)
        {            
            Console.WriteLine($"Error: {ex.Message}");
        }        
        return new List<History>();
    }

    // GET BY ID: History
    
    public History GetById(int id)
    {        
        var history = new History();        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();        
        command.CommandText = "SELECT * FROM histories WHERE employee_id = @id";

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
                        
                        history.StartDate = reader.GetDateTime(0);
                        history.EmployeeId = reader.GetInt32(1);
                        history.EndDate = reader.GetDateTime(2);
                        history.DepartmentId = reader.GetInt32(3);
                        history.jobId = reader.GetString(4);

                    }                    
                    reader.Close();
                    connection.Close();                    
                    return history;
                }                
                reader.Close();
                connection.Close();                
                return history;
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
        return history;
    }

    // INSERT: History
    
    public string Insert(DateTime startDate, int employeeId, DateTime? endDate, int departmentId, string jobId)
    {        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();        
        command.CommandText = "INSERT INTO histories (start_date, employee_id, end_date, department_id, job_id) VALUES (@StartDate, @EmployeeId, @EndDate, @DepartmentId, @JobId)";

        try
        {            
            command.Parameters.AddWithValue("@startDate", startDate);
            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@endDate", endDate ?? (object)DBNull.Value); 
            command.Parameters.AddWithValue("@departmentId", departmentId);
            command.Parameters.AddWithValue("@jobId", jobId);
            
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

    // UPDATE: History
    
    public string Update(int employeeId, DateTime? endDate)
    {        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();        
        command.CommandText = "UPDATE histories SET (end_date) VALUES (@endDate) WHERE employee_id = @id;";

        try
        {
            
            command.Parameters.AddWithValue("@id", employeeId);
            command.Parameters.AddWithValue("@endDate", endDate ?? (object)DBNull.Value);
            
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

    // DELETE: History
    
    public string Delete(int id)
    {        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();        
        command.CommandText = "DELETE FROM histories WHERE employee_id = @id;";

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