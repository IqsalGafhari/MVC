using System.Data.SqlClient;
using System.Transactions;
using System.Xml.Linq;
using LINQ;

namespace LINQ;

public class Region
{
    public int Id { get; set; }
    public string Name { get; set; }
    public override string ToString()
    {
        return $"{Id} - {Name}";
    }


    // GET ALL: Region   
    public List<Region> GetAll()
    {        
        var regions = new List<Region>();        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();        
        command.CommandText = "SELECT * FROM regions";

        try
        {
            
            connection.Open();            
            using var reader = command.ExecuteReader();
            
            if (reader.HasRows)
            {
              
                while (reader.Read())
                {
                    
                    regions.Add(new Region
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }                
                reader.Close();
                connection.Close();               
                return regions;
            }
            
            reader.Close();
            connection.Close();
            
            return new List<Region>();
        }
        
        catch (Exception ex)
        {
            
            Console.WriteLine($"Error: {ex.Message}");
        }
        
        return new List<Region>();
    }

    // GET BY ID: Region
    
    public Region GetById(int id)
    {        
        var region = new Region();        
        using var connection = Provider.GetConnection();       
        using var command = connection.CreateCommand();
        
        command.CommandText = "SELECT * FROM regions WHERE id = @id";

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
                        
                        region.Id = reader.GetInt32(0);
                        region.Name = reader.GetString(1);
                    }                    
                    reader.Close();
                    connection.Close();
                    
                    return region;
                }                
                reader.Close();
                connection.Close();
                
                return region;
            }
            catch (Exception ex)
            {
                
                transaction.Rollback();
                
                Console.WriteLine($"Error: {ex.Message}");
            }
        } catch ( Exception ex)
        {
            
            Console.WriteLine($"Error: {ex.Message}");
        }       
        return region;
    }

    // INSERT: Region
    
    public string Insert(string name)
    {
        
        using var connection = Provider.GetConnection();      
        using var command = connection.CreateCommand();
       
        command.CommandText = "INSERT INTO regions VALUES (@name);";

        try
        {           
            command.Parameters.Add(new SqlParameter("@name", name));            
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

    // UPDATE: Region
    
    public string Update(int id, string name)
    {        
        using var connection = Provider.GetConnection();       
        using var command = connection.CreateCommand();        
        command.CommandText = "UPDATE regions SET name = @name WHERE id = @id;";

        try
        {           
            command.Parameters.Add(new SqlParameter("@name", name));
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

    // DELETE: Region
   
    public string Delete(int id)
    {       
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();        
        command.CommandText = "DELETE FROM regions WHERE id = @id;";

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