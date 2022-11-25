using System;
using TypingApp.Commands;

namespace TypingApp.Models
{
    public class Group
    {
        private string _groupCode;
        private string _groupName;
        private string _groupID;
        private DatabaseConnection _connection;

        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }
        public string GroupCode
        {
            get { return _groupCode; }
            set { _groupCode = value; }
        }

        public string GroupID { get { return _groupID; } set { _groupID = value; } }

        public Group(DatabaseConnection connection)
        {
            _connection = connection;
            GroupCodeGeneratorMethod();
        }

        public void GroupCodeGeneratorMethod()
        {
            // loop tot de code heeft gecheckt of dezelfde code niet al voorkomt in database
            //Dit kan misschien nog wat anders
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

        public void newGroupCodeUpdater()
        {

        }



    }
}