using System.Collections.Generic;
using System.Data.SqlClient;

namespace TypingApp.Services.DatabaseProviders;

public abstract class BaseProvider
{
    protected static DatabaseService? DatabaseService;

    protected BaseProvider()
    {
        DatabaseService ??= new DatabaseService();
    }
    
    public abstract Dictionary<string, object>? GetById(int id);

    public static void ResetDatabaseConnection()
    {
        DatabaseService?.CloseConnection();
        DatabaseService = null;
    }
    
    protected SqlCommand GetSqlCommand()
    {
        return new SqlCommand()
        {
            Connection = DatabaseService?.GetConnection()
        };
    }

    protected List<Dictionary<string, object>?>? ConvertToList(SqlDataReader reader)
    {
        if (!reader.HasRows)
        {
            reader.Close();
            return null;
        }

        var list = new List<Dictionary<string, object>>();
        while (reader.Read())
        {
            var dict = new Dictionary<string, object>();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                dict.Add(reader.GetName(i), reader[i]);
            }

            list.Add(dict);
        }
        
        reader.Close();
        return list;
    }
}