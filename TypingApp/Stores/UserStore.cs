using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypingApp.Models;

namespace TypingApp.Stores;

public class UserStore
{
    public event Action<Student>? StudentCreated;
    public event Action<Student>? StudentUpdated;
    public event Action<Student>? StudentDeleted;
    
    public event Action<Teacher>? TeacherCreated;
    public event Action<Teacher>? TeacherUpdated;
    public event Action<Teacher>? TeacherDeleted;
    
    public event Action<Admin>? AdminCreated;
    public event Action<Admin>? AdminUpdated;
    public event Action<Admin>? AdminDeleted;
    
    public Student? Student { get; private set; }
    public Teacher? Teacher { get; private set; }
    public Admin? Admin { get; private set; }
 
    public void CreateStudent(Dictionary<string, object>? props)
    {
        var characters = new List<Character>
        {
            new('e'),
            new('n'),
            new('a'),
            new('t'),
        };
        
        Student = new Student(props, characters);
        OnStudentCreated();
    }
    
    /*
     * =================
     * Student methods
     * =================
     */
    private void OnStudentCreated()
    {
        if (Student != null) StudentCreated?.Invoke(Student);
    }
    
    public void UpdateStudent(Dictionary<string, object>? props)
    {
        var characters = new List<Character>
        {
            new('e'),
            new('n'),
            new('a'),
            new('t'),
        };

        Student = new Student(props, characters);
        OnStudentUpdated();
    }

    private void OnStudentUpdated()
    {
        if (Student != null) StudentUpdated?.Invoke(Student);
    }

    public void DeleteStudent()
    {
        Student = null;
        OnStudentDeleted();
    }
    private void OnStudentDeleted()
    {
        StudentDeleted?.Invoke(Student);
    }

    /*
     * =================
     * Teacher methods
     * =================
     */

    public void CreateTeacher(Dictionary<string, object>? props)
    {
        Teacher = new Teacher(props);
        OnTeacherCreated();
    }
    
    private void OnTeacherCreated()
    {
        if (Teacher != null) TeacherCreated?.Invoke(Teacher);
    }
    
    public void UpdateTeacher(Dictionary<string, object>? props)
    {
        Teacher = new Teacher(props);
        OnTeacherUpdated();
    }

    private void OnTeacherUpdated()
    {
        if (Teacher != null) TeacherUpdated?.Invoke(Teacher);
    }
    public void DeleteTeacher()
    {
        Teacher = null;
        OnTeacherDeleted();
    }
    private void OnTeacherDeleted()
    {
        TeacherDeleted?.Invoke(Teacher);
    }
    
    /*
     * =================
     * Admin methods
     * =================
     */

    public void CreateAdmin(Dictionary<string, object>? props)
    {
        Admin = new Admin(props);
        OnAdminCreated();
    }
    
    private void OnAdminCreated()
    {
        if (Admin != null) AdminCreated?.Invoke(Admin);
    }
    
    public void UpdateAdmin(Dictionary<string, object>? props)
    {
        Admin = new Admin(props);
        OnAdminUpdated();
    }

    private void OnAdminUpdated()
    {
        if (Admin != null) AdminUpdated?.Invoke(Admin);
    }

    public void DeleteAdmin()
    {
        Admin = null;

    }
    
    private void OnAdminDeleted()
    {
        AdminDeleted?.Invoke(Admin);
    }
}