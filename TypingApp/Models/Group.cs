using System.Collections.Generic;
using TypingApp.Services;
using TypingApp.Services.DatabaseProviders;

namespace TypingApp.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        
        // Not in database.
        public int AmountOfStudents { get; set; }

        public Group(int groupId, string groupName, string groupCode)
        {
            GroupId = groupId;
            GroupName = groupName;
            GroupCode = groupCode;
        }

        public Group(IReadOnlyDictionary<string, object>? props)
        {
            GroupId = (int)props["id"];
            GroupName = (string)props["name"];
            GroupCode = (string)props["code"];
            AmountOfStudents = new GroupProvider().GetStudents(GroupId)?.Count ?? 0;
        }

        public void RefreshCode()
        {
            GroupCode = new GroupCodeService().GenerateCode();
            new GroupCodeService().UpdateCodeInDatabase(GroupId, GroupCode);
        }
    }
}