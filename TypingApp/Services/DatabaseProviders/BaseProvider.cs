using System.Collections.Generic;

namespace TypingApp.Services.DatabaseProviders;

public abstract class BaseProvider
{
    protected static DatabaseService? DbInterface;

    protected BaseProvider()
    {
        DbInterface ??= new DatabaseService();
    }
    
    public abstract Dictionary<string, object>? GetById(int id);
}