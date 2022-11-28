using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Stores;

namespace TypingApp.ViewModels;

public class TeacherDashboardViewModel : ViewModelBase
{
    public ICommand AddGroupButton { get; }
    public ICommand NextGroupButton { get; }
    public ICommand NewGroupCodeButton { get; } 

    private DatabaseConnection _connection;
    private User _user;
    private Group CurrentGroup;
    public readonly List<string[]> groupDataArray = new List<string[]>();

    private int _groupNumber;
    private string _groupID;
    private string _groupNameText2;
    private string _groupCodeText2;
    private Visibility _groupBoxVisibility;

    public Visibility GroupBoxVisibility { get { return _groupBoxVisibility; } set { _groupBoxVisibility = value; } }

    public int GroupNumber { get { return _groupNumber; } set { _groupNumber = value; OnPropertyChanged(); } }
    public string GroupNameText2 { get { return _groupNameText2; } set{ _groupNameText2 = value; OnPropertyChanged(); } }
    public string GroupCodeText2 { get { return _groupCodeText2; } set { _groupCodeText2 = value; OnPropertyChanged(); } }
    public string GroupID { get { return _groupID;  } set { _groupID = value; OnPropertyChanged();} }

    public TeacherDashboardViewModel(NavigationService addGroupNavigationService, User user, DatabaseConnection connection)
    {
        _connection = connection;
        _user = user;
        _groupBoxVisibility = Visibility.Hidden;
        _groupNumber = 0;
        CurrentGroup = new Group(connection);
        GetGroupsFromDatabase();
        
        AddGroupButton = new AddGroupCommand(user, connection, addGroupNavigationService);
        NextGroupButton = new NextGroupCommand(user, connection,this);
        NewGroupCodeButton = new UpdateGroupCodeCommand(CurrentGroup, user, connection,this);
    }

    //bool startup is voor als je de viewmodel in komt, als die false is werkt de functie als een refresh
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
            
            GroupNameText2 = GetGroupNameFromDatabase(GroupNumber);
            GroupCodeText2 = GetGroupCodeFromDatabase(GroupNumber);
            GroupID = GetGroupIdFromDatabase(GroupNumber);
        
            _groupBoxVisibility = Visibility.Visible;
        }
    }

    public string GetGroupIdFromDatabase(int groupNumber)
    {
        var groupNameCodeArray = groupDataArray[groupNumber];
        var groupID = groupNameCodeArray[0];
        CurrentGroup.GroupID = groupID;
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
