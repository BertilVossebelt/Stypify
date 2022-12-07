using System.Collections.Generic;

namespace TypingApp.Services.DatabaseProviders;

public abstract class BaseProvider
{
    protected static DatabaseService? DbConnection;

    protected BaseProvider()
    {
        DbConnection ??= new DatabaseService();
    }
    
    public abstract IEnumerable<Dictionary<string, object>>? GetById(int id);
}