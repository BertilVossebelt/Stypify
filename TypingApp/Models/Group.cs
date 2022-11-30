using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypingApp.Commands;

namespace TypingApp.Models
{
    public class Group
    {
        private string? _groupCode;
        private string? _name;
        private int _groupID;
        private DatabaseConnection? _connection;

        public string GroupName
        {
            get { return _name; }
            set { _name = value; }
        }
        public string GroupCode
        {
            get { return _groupCode; }
            set { _groupCode = value; }
        }
        
        public int? AmountOfStudents { get; set; }
        public int? Id { get; set; }

        public int GroupID { get { return _groupID; } set { _groupID = value; } }

        public Group(string _groupName, int amount, int id)
        {
            GroupName = _groupName;
            AmountOfStudents = amount;
            Id = id;
        }
        
        public Group(DatabaseConnection connection)
        {
            _connection = connection;
        }

        public void GroupCodeGeneratorMethod()
        {
            // loop tot de code heeft gecheckt of dezelfde code niet al voorkomt in database
            while (true)
            {
                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                char[] stringChars = new char[8];
                Random random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                string NewGroupCode = new string(stringChars);
                if (CheckCodeInDataBase(NewGroupCode))
                {
                    _groupCode = NewGroupCode;
                    break;
                }
            }
        }

        public bool CheckCodeInDataBase(string groupCode)
        {
            //check of de code al is gebruikt
            string QueryString = $"SELECT id FROM Groups WHERE code='{groupCode}';";

            var reader = _connection.ExecuteSqlStatement(QueryString);
            if(reader.HasRows == false)
            {
                reader.Close();
                return true;
            }
            return false;
        }
    }
}
