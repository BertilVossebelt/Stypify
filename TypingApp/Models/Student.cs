using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypingApp.Models
{
    public class Student
    {
        public string Name { get; set; }
        public uint Accuracy { get; set; }
        public uint AmountOfExercises { get; set; }

        public Student(string name, uint accuracy, uint amountOfExercises)
        {
            Name = name;
            Accuracy = accuracy;
            AmountOfExercises = amountOfExercises;

        }
    }
}
