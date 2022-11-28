
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypingApp.Models
{
    public class Group
    {
        public string Name { get; set; }
        public int AmountOfStudents {get; set; }
        
        public int Id { get; set; }

        public Group(string name, int amountofStudents, int id)
        {
            Name = name;
            amountofStudents = AmountOfStudents;
            Id = id;
        }
        
    }
}
