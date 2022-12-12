using System;
using TypingApp.Services.DatabaseProviders;

namespace TypingApp.Services;

public class GroupCodeService
{
    public string GenerateCode()
    {
        string groupCode;
        
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

            if (!VerifyCode(newGroupCode)) continue;
            groupCode = newGroupCode;

            break;
        }

        return groupCode;
    }

    public bool VerifyCode(string groupCode)
    {
        var group = new GroupProvider().GetByCode(groupCode);
        return group == null;
    }
    public void updateCodeInDatabase(int groupId, string groupCode)
    {
        
        new GroupProvider().UpdateGroupCode(groupId, groupCode);
    }
}