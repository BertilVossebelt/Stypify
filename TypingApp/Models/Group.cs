using System;
using System.Collections.Generic;
using TypingApp.Commands;
using TypingApp.Services;
using TypingApp.Services.DatabaseProviders;

namespace TypingApp.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public int AmountOfStudents { get; set; } = 0;

        public Group(int groupId, string groupName, string groupCode)
        {
            GroupId = groupId;
            GroupName = groupName;
            GroupCode = groupCode;
        }

        public Group(IReadOnlyDictionary<string, object> props)
        {
            GroupId = (int)props["id"];
            GroupName = (string)props["name"];
            GroupCode = (string)props["code"];

            var groupProvider = new GroupProvider();
            AmountOfStudents = groupProvider.GetGroupCount(GroupId);
        }

        public void RefreshCode()
        {
            GroupCode = new GroupCodeService().GenerateCode();
            new GroupCodeService().updateCodeInDatabase(GroupId, GroupCode);
        }
    }
}