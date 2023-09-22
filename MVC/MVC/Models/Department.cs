using System.Data.SqlClient;
using System.Transactions;
using System.Xml.Linq;
using LINQ;

namespace LINQ;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int LocationId { get; set; }
    public int ManagerId { get; set; }

    public override string ToString()
    {
        return $"{Id} - {Name} - {LocationId} - {ManagerId} ";
    }


    // GET ALL: Department 
    public List<Department> GetAll()
    {       
        var departments = new List<Department>();        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();       
        command.CommandText = "SELECT * FROM departments";
        try
        {            
            connection.Open();            
            using var reader = command.ExecuteReader();            
            if (reader.HasRows)
            {               
                while (reader.Read())
                {                   
                    departments.Add(new Department
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        LocationId = reader.GetInt32(2),
                        ManagerId = reader.GetInt32(3),
                });
                }
              
                reader.Close();
                connection.Close();               
                return departments;
            }
            
            reader.Close();
            connection.Close();           
            return new List<Department>();
        }        
        catch (Exception ex)
        {           
            Console.WriteLine($"Error: {ex.Message}");
        }
        
        return new List<Department>();
    }

    // GET BY ID: Department
    
    public Department GetById(int id)
    {        
        var department = new Department();              
        using var connection = Provider.GetConnection();      
        using var command = connection.CreateCommand();       
        command.CommandText = "SELECT * FROM departments WHERE id = @id";
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
                        department.Id = reader.GetInt32(0);
                        department.Name = reader.GetString(1);
                        department.LocationId = reader.GetInt32(2);
                        department.ManagerId = reader.GetInt32(3);
                    }                    
                    reader.Close();
                    connection.Close();                    
                    return department;
                }             
                reader.Close();
                connection.Close();               
                return department;
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
        return department;
    }

    // INSERT: Department
    
    public string Insert(int id, string name, int locationId, int? managerId)
    {        
        using var connection = Provider.GetConnection();       
        using var command = connection.CreateCommand();        
        command.CommandText = "INSERT INTO departments (id, name, location_id, manager_id) VALUES (@id, @name, @locationId, @managerId);";

        try
        {            
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@name", name));
            command.Parameters.Add(new SqlParameter("@locationId", locationId));
            command.Parameters.Add(new SqlParameter("@managerId", managerId ?? (object)DBNull.Value));
           
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

    // UPDATE: Department
    
    public string Update(int id, string name, int locationId, int managerId)
    {        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();        
        command.CommandText = "UPDATE departments SET name = @name, location_id = @locationId, manager_id = @managerId WHERE id = @id;";

        try
        {            
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@name", name));
            command.Parameters.Add(new SqlParameter("@locationId", locationId));
            command.Parameters.Add(new SqlParameter("@managerId", managerId));          
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

    // DELETE: Department
   
    public string Delete(int id)
    {        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();        
        command.CommandText = "DELETE FROM departments WHERE id = @id;";
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