using System;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Windows;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Commands
{
    public class UpdateGroupsCodeCommand : CommandBase
    {
        public int id;
        public string code;
        public TeacherDashboardViewModel dashs;

        public UpdateGroupsCodeCommand(TeacherDashboardViewModel dash)
        {
            dashs = dash;
        }
        public UpdateGroupsCodeCommand()
        {

        }
        public override void Execute(object? parameter)
        {
            if (dashs.SelectedItem is not null) 
            {
                dashs.SelectedItem.RefreshCode();
                dashs.SelectedItem = dashs.SelectedItem;
            }
            
        }
        /*public override (int id, string code)
        {
            this.id = id;
            this.code = code;
        }*/
    }
}
