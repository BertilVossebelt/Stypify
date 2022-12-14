using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypingApp.Services.DatabaseProviders
{
    public class LessonProvider : BaseProvider
    {
        public override Dictionary<string, object>? GetById(int id)
        {
            var query = $"SELECT * FROM [Lessons] WHERE id = {id}";
            return DbInterface?.Select(query)?[0];
        }

        public Dictionary<string, object>? Create(int teacherId, string lessonName)
        {
            var query = $"INSERT INTO [Lessons] (teacher_id, lesson_name,) VALUES ({teacherId}, '{lessonName}')";
            return DbInterface?.Insert(query);
        }

        public List<Dictionary<string, object>>? GetAll(int teacherId)
        {
            var query = $"SELECT * FROM [Lessons] WHERE teacher_id = '{teacherId}'";
            return DbInterface?.Select(query);
        }
    }
}
