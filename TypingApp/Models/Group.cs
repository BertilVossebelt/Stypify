using System;
using TypingApp.Commands;

namespace TypingApp.Models
{
    public class Group
    {
        private readonly DatabaseConnection? _connection;

        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public int GroupId { get; set; }

        public Group(string groupName)
        {
            GroupName = groupName;
        }
        
        public Group(DatabaseConnection connection)
        {
            _connection = connection;
        }

        public void GroupCodeGeneratorMethod()
        {
            // Loop till a group code was found that wasn't in the database already.
            while (true)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var stringChars = new char[8];
                var random = new Random();

                for (var i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var newGroupCode = new string(stringChars);
                if (CheckCodeInDataBase(newGroupCode))
                {
                    GroupCode = newGroupCode;
                    break;
                }
            }
        }

        private bool CheckCodeInDataBase(string groupCode)
        {
            // Check if code is already used.
            var queryString = $"SELECT id FROM Groups WHERE code='{groupCode}';";

            var reader = _connection?.ExecuteSqlStatement(queryString);
            if (reader == null || reader.HasRows) return false;
            reader.Close();
            return true;
        }
    }
}
