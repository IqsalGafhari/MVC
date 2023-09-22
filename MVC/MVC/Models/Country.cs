using System.Data.SqlClient;
using System.Transactions;
using System.Xml.Linq;
using LINQ;

namespace LINQ;

public class Country
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int RegionId { get; set; }

    public override string ToString()
    {
        return $"{Id} - {Name} - {RegionId} ";
    }


    // GET ALL: Country
    
    public List<Country> GetAll()
    {
        
        var Countries = new List<Country>();
        using var connection = Provider.GetConnection();
        using var command = connection.CreateCommand();

        command.CommandText = "SELECT * FROM countries";

        try
        {
            connection.Open();
            using var reader = command.ExecuteReader();           
            if (reader.HasRows)
            {               
                while (reader.Read())
                {                   
                    Countries.Add(new Country
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        RegionId = reader.GetInt32(2)
                    });
                }               
                reader.Close();
                connection.Close();                
                return Countries;
            }           
            reader.Close();
            connection.Close();            
            return new List<Country>();
        }        
        catch (Exception ex)
        {           
            Console.WriteLine($"Error: {ex.Message}");
        }       
        return new List<Country>();
    }

    // GET BY ID: Country    
    public Country GetById(string id)
    {       
        var Country = new Country();
        
        using var connection = Provider.GetConnection();
       
        using var command = connection.CreateCommand();
        
        command.CommandText = "SELECT * FROM countries WHERE id = @id";
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
                        Country.Id = reader.GetString(0);
                        Country.Name = reader.GetString(1);
                        Country.RegionId = reader.GetInt32(2);
                    }                   
                    reader.Close();
                    connection.Close();                   
                    return Country;
                }               
                reader.Close();
                connection.Close();                
                return Country;
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
        return Country;
    }

    // INSERT: Country
    
    public string Insert(string id, string name, int regionId )
    {       
        using var connection = Provider.GetConnection();       
        using var command = connection.CreateCommand();        
        command.CommandText = "INSERT INTO countries (id, name, region_id) VALUES (@id, @name, @regionId);";

        try
        {            
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@name", name));
            command.Parameters.Add(new SqlParameter("@regionId", regionId));            
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

    // UPDATE: Country
    
    public string Update(string id, string name, int regionId)
    {        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();      
        command.CommandText = "UPDATE countries SET name = @name, region_id = @regionID  WHERE id = @id;";

        try
        {          
            command.Parameters.Add(new SqlParameter("@name", name));
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@regionId", regionId));            
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

    // DELETE: Country
    
    public string Delete(string id)
    {        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();       
        command.CommandText = "DELETE FROM countries WHERE id = @id;";
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