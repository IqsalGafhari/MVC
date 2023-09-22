using System.Data.SqlClient;

namespace LINQ;

public class Provider
{
    private static readonly string connectionString = "Data Source=IQSALGAFHARI;Integrated Security=True;Database=db_hr_dts;Connect Timeout=30;";

    public static SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }
    
    public static SqlCommand GetCommand()
    {
        return new SqlCommand();
    }
    
    public static SqlParameter SetParameter(string name, object value)
    {
        return new SqlParameter(name, value);
    }
}
