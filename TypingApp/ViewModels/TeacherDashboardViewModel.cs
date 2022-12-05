using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Services;

namespace TypingApp.ViewModels;

public class TeacherDashboardViewModel : ViewModelBase
{
    public ICommand AddGroupButton { get; }
    public ICommand NextGroupButton { get; }
    public ICommand NewGroupCodeButton { get; }

    public readonly List<string[]> groupDataArray = new List<string[]>();

    private readonly DatabaseConnection _connection;
    private User _user;
    private Group CurrentGroup;
    private int _groupNumber;
    private int _groupID;
    private string _groupNameText;
    private string _groupCodeText;
    private Visibility _groupBoxVisibility;

    public Visibility GroupBoxVisibility { get => _groupBoxVisibility; set { _groupBoxVisibility = value; }}
    public int GroupNumber { get => _groupNumber; set { _groupNumber = value; OnPropertyChanged(); }}
    public string GroupNameText { get => _groupNameText; set { _groupNameText = value; OnPropertyChanged(); }}
    public string GroupCodeText { get => _groupCodeText; set { _groupCodeText = value; OnPropertyChanged(); }}
    public int GroupID { get => _groupID; set { _groupID = value; OnPropertyChanged(); }}

    public TeacherDashboardViewModel(NavigationService addGroupNavigationService, User user, DatabaseConnection connection)
    {
        _connection = connection;
        _user = user;
        _groupBoxVisibility = Visibility.Hidden;
        _groupNumber = 0;
        CurrentGroup = new Group(connection);
        GetGroupsFromDatabase();
        
        AddGroupButton = new AddGroupCommand(connection, addGroupNavigationService);
        NextGroupButton = new NextGroupCommand(this);
        NewGroupCodeButton = new UpdateGroupCodeCommand(CurrentGroup, connection,this);
    }

    public void GetGroupsFromDatabase()
    {
        var queryString3 = $"SELECT id, name, code FROM Groups WHERE teacher_id='{_user.Id}'";
        var reader = _connection.ExecuteSqlStatement(queryString3);

        if (reader.HasRows)
        {
            groupDataArray.Clear();
            while (reader.Read())
            {
                string[] groupNameCodeArray = { $"{reader["id"]}", $"{reader["name"]}", $"{reader["code"]}" };
                groupDataArray.Add(groupNameCodeArray);
            }
            reader.Close();
            
            GroupNameText = GetGroupNameFromDatabase(GroupNumber);
            GroupCodeText = GetGroupCodeFromDatabase(GroupNumber);
            GroupID = GetGroupIdFromDatabase(GroupNumber);
        
            _groupBoxVisibility = Visibility.Visible;
        }
        reader.Close();
    }

    public int GetGroupIdFromDatabase(int groupNumber)
    {
        var groupNameCodeArray = groupDataArray[groupNumber];
        int groupID = Int32.Parse(groupNameCodeArray[0]);
        CurrentGroup.GroupId = groupID;
        return groupID;
    }
    public string GetGroupNameFromDatabase(int groupNumber)
    {
        string[] GroupNameCodeArray = groupDataArray[groupNumber];
        string groupName = GroupNameCodeArray[1];
        return groupName;
    }
    public string GetGroupCodeFromDatabase(int groupNumber)
    {
        string[] testArray2 = groupDataArray[groupNumber];
        string groupCode = testArray2[2];
        return groupCode;
    }
}
