using System.Data.SqlClient;
using System.Transactions;
using System.Xml.Linq;
using LINQ;

namespace LINQ;

public class Job
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int? MinSalary { get; set; }
    public int? MaxSalary { get; set; }


    // GET ALL: Job
    
    public List<Job> GetAll()
    {        
        var jobs = new List<Job>();        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();       
        command.CommandText = "SELECT * FROM jobs";

        try
        {            
            connection.Open();
            
            using var reader = command.ExecuteReader();
            
            if (reader.HasRows)
            {
                
                while (reader.Read())
                {
                    
                    jobs.Add(new Job
                    {
                        Id = reader.GetString(0),
                        Title = reader.GetString(1),
                        MinSalary = reader.GetInt32(2),
                        MaxSalary = reader.GetInt32(3),
                    });
                }                
                reader.Close();
                connection.Close();                
                return jobs;
            }
            
            reader.Close();
            connection.Close();
           
            return new List<Job>();
        }
        
        catch (Exception ex)
        {
            
            Console.WriteLine($"Error: {ex.Message}");
        }
        
        return new List<Job>();
    }

    // GET BY ID: Job
    
    public Job GetById(int id)
    {        
        var job = new Job();        
        using var connection = Provider.GetConnection();       
        using var command = connection.CreateCommand();        
        command.CommandText = "SELECT * FROM jobs WHERE id = @id";

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
                       
                        job.Id = reader.GetString(0);
                        job.Title = reader.GetString(1);
                        job.MinSalary = reader.GetInt32(2);
                        job.MaxSalary = reader.GetInt32(3);
                    }                    
                    reader.Close();
                    connection.Close();
                    
                    return job;
                }
                
                reader.Close();
                connection.Close();
                
                return job;
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
        
        return job;
    }

    // INSERT: Job
    
    public string Insert(string id, string title, int minSalary, int maxSalary)
    {
        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();       
        command.CommandText = "INSERT INTO jobs (id, title, min_salary, max_salary) VALUES (@id, @title, @minSalary , @maxSalary);";

        try
        {           
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@title", title));
            command.Parameters.Add(new SqlParameter("@minSalary", minSalary));
            command.Parameters.Add(new SqlParameter("@maxSalary", maxSalary));
          
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

    // UPDATE: Job
    
    public string Update(string id, string title, int minSalary, int maxSalary)
    {        
        using var connection = Provider.GetConnection();       
        using var command = connection.CreateCommand();        
        command.CommandText = "UPDATE jobs SET title = @title, min_salary = @minSalary, max_salary = @maxSalary WHERE id = @id;";

        try
        {            
            command.Parameters.Add(new SqlParameter("@title", title));
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@minSalary", minSalary));
            command.Parameters.Add(new SqlParameter("@maxSalary", maxSalary));
           
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

    // DELETE: Job
    
    public string Delete(int id)
    {        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();       
        command.CommandText = "DELETE FROM jobs WHERE id = @id;";
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