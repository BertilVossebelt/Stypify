using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypingApp.Models;

namespace TypingApp.Stores;

public class UserStore
{
    public event Action<User>? UserCreated;
    public event Action<User>? StudentCreated;
    public event Action<User>? UserUpdated;
    public event Action<User>? StudentUpdated;
    
    public Student? Student { get; private set; }
    public User? User { get; private set; }
    
    public void CreateUser(List<Dictionary<string, object>>? props)
    {
        User = new User(props);
        OnUserCreated();
    }
    
    private void OnUserCreated()
    {
        if (User != null) UserCreated?.Invoke(User);
    }
    
    public void UpdateUser(List<Dictionary<string, object>>? props)
    {
        User = new User(props);
        OnUserUpdated();
    }

    private void OnUserUpdated()
    {
        if (User != null) UserUpdated?.Invoke(User);
    }
    
    public void CreateStudent(List<Dictionary<string, object>>? props)
    {
        var characters = new List<Character>
        {
            new('e'),
            new('n'),
            new('a'),
            new('t'),
        };
        
        Student = new Student(props, characters);
        User = new User(props);
        OnStudentCreated();
    }
    
    private void OnStudentCreated()
    {
        if (User != null) StudentCreated?.Invoke(User);
    }
    
    public void UpdateStudent(List<Dictionary<string, object>>? props)
    {
        var characters = new List<Character>
        {
            new('e'),
            new('n'),
            new('a'),
            new('t'),
        };

        Student = new Student(props, characters);
        User = new User(props);
        OnStudentUpdated();
    }

    private void OnStudentUpdated()
    {
        if (User != null) StudentUpdated?.Invoke(User);
    }
}