using System;
using System.Collections.Generic;
using System.Data;

namespace TypingApp.Services.DatabaseProviders;

public class LessonProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [Lesson] WHERE id = @id";
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader, "LessonProvider.GetById")?[0];
    }
    
    public  List<Dictionary<string, object>>? GetExercises(int lessonId)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT e.id, e.name, e.text FROM [Exercise] e JOIN [Lesson_Exercise] le ON e.id = le.exercise_id WHERE le.lesson_id = @lessonId";
        cmd.Parameters.Add("@lessonId", SqlDbType.Int).Value = lessonId;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader, "LessonProvider.GetExercises");
    }

    public Dictionary<string, object>? Create(string name, int teacherId)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "INSERT INTO [Lesson] (name, teacher_id) VALUES (@name, @teacherId); SELECT SCOPE_IDENTITY()";
        cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
        cmd.Parameters.Add("@teacherId", SqlDbType.Int).Value = teacherId;
        var id = (decimal)cmd.ExecuteScalar();
        
        return GetById((int)id);
    }
    
    public Dictionary<string, object>? LinkToGroup(int groupId, int lessonId)
    {
        // Create link between student and group.
        var cmd = GetSqlCommand();
        cmd.CommandText = "INSERT INTO [Group_Lesson] (group_id, lesson_id) VALUES (@groupId, @lessonId); SELECT SCOPE_IDENTITY()";
        cmd.Parameters.Add("@groupId", SqlDbType.Int).Value = groupId;
        cmd.Parameters.Add("@lessonId", SqlDbType.Int).Value = lessonId;
        var id = (decimal)cmd.ExecuteScalar();
        
        // Retrieve the newly created link.
        cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [Group_Lesson] WHERE id = @id";
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = (int)id;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader, "LessonProvider.LinkToGroup")?[0];
    }
        
        public override Dictionary<string, object>? GetById(int id)
        {

            var query = $"SELECT * FROM [Lesson] WHERE id = {id}";
            return DbInterface?.Select(query)?[0];
        }
        
        public int GetAmountOfExercisesFromTeacher(int id)
        {
            var query = $"SELECT COUNT(id) FROM [Exercise] WHERE [teacher_id] = {id} ";
            Dictionary<string, object>? result = DbInterface?.Select(query)?[0];
            return (int)result?[""];
        }
        
        public List<Dictionary<string, object>>? GetGroups(int id)
        {

            var query = $"SELECT * FROM [Group] WHERE [teacher_id] = {id}";
            return DbInterface?.Select(query);
        }
        
        public List<Dictionary<string, object>>? GetAllExercisesByLessonId(int LessonId)
        {
            var query = $"SELECT Exercise.* FROM [Exercise] JOIN [Lesson_Exercise] ON Lesson_Exercise.exercise_id = Exercise.id WHERE Lesson_Exercise.lesson_id ='{LessonId}'";
            return DbInterface?.Select(query);
        }
        
        public List<Dictionary<string, object>>? GetAllGroupsByLessonId(int LessonId)
        {
            var query = $"SELECT [Group].* FROM [Group] JOIN [Group_Lesson] ON Group_Lesson.group_id = [Group].id WHERE Group_Lesson.lesson_id ='{LessonId}'";
            return DbInterface?.Select(query);
        }
        
        public Dictionary<string, object>? CreateLesson(string Name, int teacher_id)
        {
            var query1 = $"INSERT INTO [Lesson] (name, teacher_id) VALUES ('{Name}','{teacher_id}')";
            return DbInterface?.Insert(query1);

        }

        public Dictionary<string, object>? CreateLessonWithID(int id, string Name, int teacher_id)
        {
            var query1 = $"INSERT INTO [Lesson] (id, name, teacher_id) VALUES ('{id}','{Name}','{teacher_id}')";
            return DbInterface?.Insert(query1);

        }
        
        public Dictionary<string, object>? CreateLessonExerciseLink(int lesson_id, int exercise_id)
        {
            var query1 = $"INSERT INTO [Lesson_Exercise] (lesson_id, exercise_id, place_number) VALUES ('{lesson_id}','{exercise_id}',0)";
            return DbInterface?.Insert(query1);

        }
        
        public Dictionary<string, object>? CreateGroupLessonLink(int group_id, int lesson_id)
        {
            var query1 = $"INSERT INTO [Group_Lesson] (group_id, lesson_id) VALUES ('{group_id}','{lesson_id}')";
            return DbInterface?.Insert(query1);

        }
        
        public void DeleteLesson(int LessonId)
        {
            var query1 = $"DELETE FROM Group_Lesson WHERE lesson_id ='{LessonId}'";
            DbInterface?.VoidExecuteRaw(query1);
            var query2 = $"DELETE FROM Lesson_Exercise WHERE lesson_id ='{LessonId}'";
            DbInterface?.VoidExecuteRaw(query2);
        }
        
        public void UpdateLesson(int LessonId, string name, int teacher_id)
        {
            var query = $"UPDATE [Lesson] SET name = '{name}', teacher_id = '{teacher_id}' WHERE id = '{LessonId}' ";
            DbInterface?.VoidExecuteRaw(query);
        }
}
