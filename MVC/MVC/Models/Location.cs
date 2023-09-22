using System.Data.SqlClient;
using System.Transactions;
using System.Xml.Linq;
using LINQ;

namespace LINQ;

public class Location
{
    public int Id { get; set; }
    public string StreetAddress { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string StateProvince { get; set; }
    public string CountryId { get; set; }

    public override string ToString()
    {
        return $"{Id} - {StreetAddress} - {PostalCode} - {City} - {StateProvince} - {CountryId}";
    }


    // GET ALL: Location
    
    public List<Location> GetAll()
    {
        
        var Locations = new List<Location>();        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();       
        command.CommandText = "SELECT * FROM locations";

        try
        {            
            connection.Open();           
            using var reader = command.ExecuteReader();
            
            if (reader.HasRows)
            {                
                while (reader.Read())
                {                    
                    Locations.Add(new Location
                    {
                        Id = reader.GetInt32(0),
                        StreetAddress = reader.GetString(1),
                        PostalCode = reader.GetString(2),
                        City = reader.GetString(3),
                        StateProvince = reader.GetString(4),
                        CountryId = reader.GetString(5)
                    });
                }                
                reader.Close();
                connection.Close();                
                return Locations;
            }            
            reader.Close();
            connection.Close();           
            return new List<Location>();
        }
        
        catch (Exception ex)
        {            
            Console.WriteLine($"Error: {ex.Message}");
        }        
        return new List<Location>();
    }

    // GET BY ID: Location
    
    public Location GetById(int id)
    {        
        var Location = new Location();        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();        
        command.CommandText = "SELECT * FROM locations WHERE id = @id";

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
                        Location.Id = reader.GetInt32(0);
                        Location.StreetAddress = reader.GetString(1);
                        Location.PostalCode = reader.GetString(2);
                        Location.City = reader.GetString(3);
                        Location.StateProvince = reader.GetString(4);
                        Location.CountryId = reader.GetString(5);

                    }                    
                    reader.Close();
                    connection.Close();                  
                    return Location;
                }                
                reader.Close();
                connection.Close();                
                return Location;
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
        return Location;
    }

    // INSERT: Location
   
    public string Insert(int id, string streetAdd, string postalCode, string city, string stateProvince, string countryId )
    {        
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();        
        command.CommandText = "INSERT INTO locations (id, street_address, postal_code, city, state_province, country_id) VALUES (@id, @streedAdd, @postalCode, @city, @stateProvince, @countryId);";

        try
        {            
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@streetAdd", streetAdd));
            command.Parameters.Add(new SqlParameter("@postalCode", postalCode));
            command.Parameters.Add(new SqlParameter("@city", city));
            command.Parameters.Add(new SqlParameter("@stateProvince", stateProvince));
            command.Parameters.Add(new SqlParameter("@countryId", countryId));            
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

    // UPDATE: Location
    
    public string Update(int id, string streetAdd, string postalCode, string city, string stateProvince, string countryId)
    {       
        using var connection = Provider.GetConnection();        
        using var command = connection.CreateCommand();       
        command.CommandText = "UPDATE locations SET street_address = @streedAdd, postal_code = @postalCode, city = @city, state_province = @stateProvince,  country_id = @countryId WHERE id = @id;";

        try
        {            
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@streetAdd", streetAdd));
            command.Parameters.Add(new SqlParameter("@postalCode", postalCode));
            command.Parameters.Add(new SqlParameter("@city", city));
            command.Parameters.Add(new SqlParameter("@stateProvince", stateProvince));
            command.Parameters.Add(new SqlParameter("@countryId", countryId));
            
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

    // DELETE: Location
    
    public string Delete(int id)
    {        
        using var connection = Provider.GetConnection();       
        using var command = connection.CreateCommand();       
        command.CommandText = "DELETE FROM locations WHERE id = @id;";

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