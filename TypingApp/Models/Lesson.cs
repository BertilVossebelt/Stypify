using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypingApp.Models
{
    public class Lesson
    {
        public string LessonName { get; set; }
        public string Teacher { get; set; }
        public int LessonID { get; set; }
        public Lesson(string lessonName, string teacher, int id)
        {
            LessonName = lessonName;
            Teacher = teacher;
            LessonID = id;
        }
    }
}
