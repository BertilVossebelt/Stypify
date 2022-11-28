using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Stores;

namespace TypingApp.ViewModels;

public class TeacherDashboardViewModel : ViewModelBase
{
    public ICommand AddGroupButton { get; }
    public ICommand NextGroupButton { get; }
    public ICommand NewGroupCodeButton { get; } 

    private NavigationStore _navigationStore;
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



    public TeacherDashboardViewModel(User user, NavigationStore navigationStore,DatabaseConnection connection)
    {
        _connection = connection;
        _user = user;
        _navigationStore = navigationStore;
        _groupBoxVisibility = Visibility.Hidden;
        _groupNumber = 0;
        CurrentGroup = new Group(connection);
        getGroupsFromDatabase();

        GroupNameText2 = getGroupNameFromDatabase(GroupNumber);
        GroupCodeText2 = getGroupCodeFromDatabase(GroupNumber);
        GroupID = getGroupIDFromDatabase(GroupNumber);

        AddGroupButton = new AddGroupCommand(user, navigationStore,connection);
        NextGroupButton = new NextGroupCommand(user, navigationStore, connection,this);
        NewGroupCodeButton = new UpdateGroupCodeCommand(CurrentGroup,navigationStore, user, connection,this);
    }

    //bool startup is voor als je de viewmodel in komt, als die false is werkt de functie als een refresh
    public void getGroupsFromDatabase()
    {
        string QueryString3 = $"SELECT id,name,code FROM Groups WHERE teacher_id='{_user.Id}'";
        var reader = _connection.ExecuteSqlStatement(QueryString3);
        if (reader.HasRows)
        {
            groupDataArray.Clear();
            while (reader.Read())
            {
                String[] groupNameCodeArray = new string[] { $"{reader["id"]}", $"{reader["name"]}", $"{reader["code"]}" };
                groupDataArray.Add(groupNameCodeArray);
            }
            reader.Close();
            _groupBoxVisibility = Visibility.Visible;
        }
    }
    

    public string getGroupIDFromDatabase(int groupNumber)
    {
        string[] GroupNameCodeArray = groupDataArray[groupNumber];
        string groupID = GroupNameCodeArray[0];
        CurrentGroup.GroupID = groupID;
        return groupID;
    }
    public string getGroupNameFromDatabase(int groupNumber)
    {
        string[] GroupNameCodeArray = groupDataArray[groupNumber];
        string groupName = GroupNameCodeArray[1];
        return groupName;
    }
    public string getGroupCodeFromDatabase(int groupNumber)
    {
        string[] testArray2 = groupDataArray[groupNumber];
        string groupCode = testArray2[2];
        return groupCode;
    }

}
