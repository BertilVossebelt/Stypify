using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TypingApp.Models;

namespace TypingApp.Services.DatabaseProviders;

public class GroupProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var query = $"SELECT * FROM [Groups] WHERE id = {id}";
        return DbInterface?.Select(query)?[0];
    }

    public Dictionary<string, object>? GetByCode(string groupCode)
    {
        var query = $"SELECT * FROM [Groups] WHERE code = '{groupCode}';";
        return DbInterface?.Select(query)?[0];
    }
    public int GetGroupCount(int groupsId)
    {
       /* var query = $"SELECT COUNT(Groups.id) FROM Groups JOIN Group_Student ON Groups.id = Group_Student.group_id WHERE teacher_id = {teacherId} GROUP BY Groups.id HAVING COUNT(Groups.id) > 1";*/
        var query = $"SELECT COUNT(group_id) FROM Group_Student WHERE group_id = {groupsId} GROUP BY group_id HAVING COUNT(group_id) > 1";
        var reader2 = DbInterface?.ExecuteRaw(query);
        var count =0;
        if (reader2 != null)
        {
            while (reader2.Read())
            {
                count = (int)reader2[0];
            }
        }
        reader2.Close();
        return count;



            //var query = $"SELECT Groups.id, Groups.name, Groups.code, t.* FROM Groups JOIN (SELECT Group_Student.group_id COUNT(Group_Student.group_id) as qnty FROM Group_Student GROUP BY Group_Student.group_id having count(Group_Student.group_id) > 1 ) t ON Group_Student.group_id = Groups.id";
            //var query = $"SELECT Groups.id, Groups.name, Groups.code FROM [Groups] s where 1 < (select count(*) from[stuff] i where i.city = s.city and i.name = s.name)";
            /*var reader2 = DbInterface?.ExecuteRaw(query);
            if (reader2 != null)
            {

                var PreviousId = 0;
                int count = 0;
                Group group = new Group(-1, "geen groep", "nog steeds geen groep");
                while (reader2.Read())
                {
                    Console.WriteLine("0");
                    while (reader2.Read())
                    {

                        Console.WriteLine("1: "+ (int)reader2[0]);
                            group.AmountOfStudents = (int)reader2[3];
                            Groups.Add(group);
                        *//*Console.WriteLine("2 " + (int)reader2[0]+ " =? " + PreviousId);
                        if (PreviousId == (int)reader2[0] || PreviousId == 0)
                        {
                            count++;
                            Console.WriteLine("3 count: " + count);
                        }
                        else
                        {
                            Console.WriteLine(group.GroupId + " " + group.GroupName);
                            groups.Add(group);
                            count = 0;
                            count++;


                            group.AmountOfStudents = count;
                            Console.WriteLine(group + " " + count);

                        }
                        group = new Group((int)reader2[0], (string)reader2[1], (string)reader2[2]);
                        PreviousId = (int)reader2[0];*//*
                    }
                }
                reader2.Close();
            }
            return Groups;*/
        }
    



    public Tuple<ObservableCollection<Student>,int> ? GetStudents(int groupId)
    {
        var query = $"SELECT s.id, s.email, s.first_name, s.preposition, s.last_name, s.teacher, s.admin, completed_excercises FROM [Users] s JOIN Group_Student g ON s.id = g.student_id LEFT JOIN Student t ON g.student_id = t.student_id WHERE g.group_id = {groupId}";
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

            var student =new Student((int)reader[0], (string)reader[1], (string)reader[2], preposition, (string)reader[4], Convert.ToBoolean(reader[5]), Convert.ToBoolean(reader[6]), amounOfExercices);
                students.Add(student);
                
            
        }
        reader.Close();
        return  Tuple.Create(students,count);
    }
    
    public List<Dictionary<string, object>>? GetStudent(int groupId, int studentId)
    {
        var query = $"SELECT id FROM Group_Student WHERE group_id='{groupId}' AND student_id = {studentId}";
        return DbInterface?.Select(query);
    }

    public Dictionary<string, object>? Create(int teacherId, string groupName, string groupCode)
    {
        var query = $"INSERT INTO [Groups] (teacher_id,name,code) VALUES ({teacherId}, '{groupName}', '{groupCode}')";
        return DbInterface?.Insert(query);
    }

    public Dictionary<string, object>? LinkStudent(int groupId, int studentId)
    {
        var query = $"INSERT INTO Group_Student (group_id,student_id) VALUES ({groupId} , {studentId})";
        return DbInterface?.Select(query)?[0];
    }
    public void UpdateGroupCode(int groupId, string groupCode)
    {
        Console.WriteLine("GroupId And Groupcode: "+groupId + " "+ groupCode);
        var query = $"UPDATE Groups SET Groups.code = '{groupCode}' WHERE Groups.id = {groupId}";
        DbInterface?.VoidExecuteRaw(query);
    }
}