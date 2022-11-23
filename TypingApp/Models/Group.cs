using System;

namespace TypingApp.Models
{
    public class Group
    {
        private string _groupCode;
        private string _groupName;

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

        public Group()
        {
            GroupCodeGeneratorMethod();
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
            //Hier komt de check of de code al is gebruikt

            return true;
        }



    }
}