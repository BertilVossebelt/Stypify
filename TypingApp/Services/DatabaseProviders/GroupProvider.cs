using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TypingApp.Models;

namespace TypingApp.Services.DatabaseProviders;

public class GroupProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var query = $"SELECT * FROM [Group] WHERE id = {id}";
        return DbInterface?.Select(query)?[0];
    }

    public Dictionary<string, object>? GetByCode(string groupCode)
    {
        var query = $"SELECT * FROM [Group] WHERE code = '{groupCode}';";
        return DbInterface?.Select(query)?[0];
    }

    public List<Dictionary<string, object>>? GetLessons(int groupId)
    {
        var query = $"SELECT l.id, l.name, l.teacher_id, l.image FROM [Lesson] l JOIN [Group_Lesson] gl ON l.id = gl.lesson_id WHERE gl.group_id = '{groupId}'";
        return DbInterface?.Select(query);
    }

    public int GetGroupCount(int groupId)
    {
        /* var query = $"SELECT COUNT(Group.id) FROM Group JOIN Group_Student ON Groups.id = Group_Student.group_id WHERE teacher_id = {teacherId} GROUP BY Groups.id HAVING COUNT(Groups.id) > 1";*/
        var query = $"SELECT COUNT(group_id) FROM Group_Student WHERE group_id = '{groupId}' GROUP BY group_id HAVING COUNT(group_id) > 0";
        var reader = DbInterface?.ExecuteRaw(query);
        var count = 0;
        if (reader != null)
        {
            while (reader.Read())
            {
                count = (int)reader[0];
            }
        }

        reader?.Close();
        return count;
    }


    public Tuple<ObservableCollection<Student>, int>? GetStudents(int groupId)
    {
        var query = $"SELECT s.id, s.email, s.first_name, s.preposition, s.last_name, s.teacher, s.admin, completed_excercises FROM [User] s JOIN Group_Student g ON s.id = g.student_id LEFT JOIN Student t ON g.student_id = t.student_id WHERE g.group_id = {groupId}";
        var reader = DbInterface?.ExecuteRaw(query);
        ObservableCollection<Student> students = new ObservableCollection<Student>();
        string preposition;
        int amounOfExercices;
        int count = 0;

        while (reader.Read())
        {
            count++;
            if (!reader.IsDBNull(3))
            {
                preposition = (string)reader[3];
            }
            else
            {
                preposition = "";
            }

            if (!reader.IsDBNull(7))
            {
                amounOfExercices = (int)reader[7];
            }
            else
            {
                amounOfExercices = 0;
            }

            var student = new Student((int)reader[0], (string)reader[1], (string)reader[2], preposition,
                (string)reader[4], Convert.ToBoolean(reader[5]), Convert.ToBoolean(reader[6]), amounOfExercices);
            students.Add(student);
        }

        reader.Close();
        return Tuple.Create(students, count);
    }

    public List<Dictionary<string, object>>? GetStudent(int groupId, int studentId)
    {
        var query = $"SELECT id FROM Group_Student WHERE group_id='{groupId}' AND student_id = {studentId}";
        return DbInterface?.Select(query);
    }

    public Dictionary<string, object>? Create(int teacherId, string groupName, string groupCode)
    {
        var query = $"INSERT INTO [Group] (teacher_id,name,code) VALUES ({teacherId}, '{groupName}', '{groupCode}')";
        return DbInterface?.Insert(query);
    }

    public Dictionary<string, object>? LinkStudent(int groupId, int studentId)
    {
        var query = $"INSERT INTO Group_Student (group_id,student_id) VALUES ({groupId} , {studentId})";
        return DbInterface?.Select(query)?[0];
    }

    public void UpdateGroupCode(int groupId, string groupCode)
    {
        var query = $"UPDATE [Group] SET code = '{groupCode}' WHERE id = {groupId}";
        var reader = DbInterface?.ExecuteRaw(query);
        reader?.Close();
    }
}