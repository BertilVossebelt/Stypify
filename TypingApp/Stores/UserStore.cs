using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypingApp.Models;

namespace TypingApp.Stores;

public class UserStore
{
    public Student? Student { get; private set; }
    public Teacher? Teacher { get; private set; }
    public Admin? Admin { get; private set; }
    public int? LessonId { get; private set; }
    
    public event Action<Student>? StudentCreated;
    public event Action<Student>? StudentDeleted;
    
    public event Action<Teacher>? TeacherCreated;
    public event Action<Teacher>? TeacherDeleted;
    
    public event Action<Admin>? AdminCreated;
    public event Action<Admin>? AdminDeleted;

    public UserStore()
    {
       LessonId = 78;
    }
    
    /*
     * =================
     * Student methods
     * =================
     */
    public void CreateStudent(Dictionary<string, object>? props)
    {
        // TODO: Characters should be queried from the database.
        var characters = new List<Character>
        {
            new('e'),
            new('n'),
            new('a'),
            new('t'),
        };
        
        Student = new Student(props, characters);
        if (Student != null) StudentCreated?.Invoke(Student);
    }
    
    public void DeleteStudent()
    {
        Student = null;
        if (Student != null) StudentDeleted?.Invoke(Student);
    }

    /*
     * =================
     * Teacher methods
     * =================
     */
    public void CreateTeacher(Dictionary<string, object>? props)
    {
        Teacher = new Teacher(props);
        if (Teacher != null) TeacherCreated?.Invoke(Teacher);
    }
    
    public void DeleteTeacher()
    {
        Teacher = null;
        if (Teacher != null) TeacherDeleted?.Invoke(Teacher);
    }
    
    /*
     * =================
     * Admin methods
     * =================
     */
    public void CreateAdmin(Dictionary<string, object>? props)
    {
        Admin = new Admin(props);
        if (Admin != null) AdminCreated?.Invoke(Admin);
    }
    
    public void DeleteAdmin()
    {
        Admin = null;
        if (Admin != null) AdminDeleted?.Invoke(Admin);
    }
}