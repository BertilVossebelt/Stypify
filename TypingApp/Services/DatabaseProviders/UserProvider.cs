using System;
using System.Collections.Generic;

namespace TypingApp.Services.Database;

public class UserProvider
{
    private readonly DatabaseService _dbConnectionService;

    public UserProvider()
    {
        _dbConnectionService = new DatabaseService();
    }
    
    public IEnumerable<Dictionary<string, object>> GetAllUsers()
    {
        const string query = "SELECT first_name, email FROM users";
        return _dbConnectionService.Select(query);
    }

    public List<Dictionary<string, object>> GetUserById(int id)
    {
        var query = $"SELECT * FROM users WHERE id = {id}";
        return _dbConnectionService.Select(query);
    }
}