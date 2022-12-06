using System.Data.SqlClient;
using System.Windows;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Services.Database;
using Group = TypingApp.Models.Group;

namespace TypingApp.Commands
{
    internal class LinkToGroupSaveCommand : CommandBase
    {
        private readonly User _user;
        private readonly DatabaseService _connection;
        private readonly Group _code;
        private int _groupId;
        private readonly NavigationService _studentDashboardNavigationService;

        public LinkToGroupSaveCommand(Group code, User user, DatabaseService connection,
            NavigationService studentDashboardNavigationService)
        {
            _code = code;
            _user = user;
            _connection = connection;
            _studentDashboardNavigationService = studentDashboardNavigationService;
        }

        public override void Execute(object? parameter)
        {
            // Ask for user verification.
            var linkGroupMessageBox = AskUserVerification();
            if (linkGroupMessageBox != MessageBoxResult.Yes) return;

            GetGroupId();
            
            // Prevent linking if user is already linked.
            if (CheckIfUserIsLinked(out var reader)) return;

            // Link to group.
            reader.Close();
            var queryString3 = $"INSERT INTO Group_Student (group_id,student_id) VALUES ('{_groupId}','{_user.Id}')";
            _connection.ExecuteSqlStatement2(queryString3);
            
            // Navigate to student dashboard.
            var navigateCommand = new NavigateCommand(_studentDashboardNavigationService);
            navigateCommand.Execute(this);
        }

        private bool CheckIfUserIsLinked(out SqlDataReader reader)
        {
            // Check if student is already linked to group.
            var queryString = $"SELECT id FROM Group_Student WHERE group_id='{_groupId}' AND student_id='{_user.Id}';";
            reader = _connection.ExecuteSqlStatement(queryString);

            // Check if student is already linked
            if (!reader.HasRows) return false;
            ShowAlreadyLinkedError();
            return true;
        }

        /*
         * Checks if group code exists and finds the group id.
         */
        private void GetGroupId()
        {
            var queryString = $"SELECT id FROM Groups WHERE code='{_code.GroupCode}'";
            var reader = _connection.ExecuteSqlStatement(queryString);

            // Close connection if reader doesn't have rows.
            if (!reader.HasRows)
            {
                reader.Close();
                ShowGroupDoesNotExistError();
                return;
            }

            // If reader has rows, get group id.
            while (reader.Read()) _groupId = (int)reader["id"];
            reader.Close();
        }
        
        /*
         * Notifies the user the group code doesn't exist.
         * ---------------------------------------------
         * Show OK messagebox
         */
        private static void ShowGroupDoesNotExistError()
        {
            const string message = "Deze groep code bestaat niet";
            const MessageBoxButton type = MessageBoxButton.OK;
            const MessageBoxImage icon = MessageBoxImage.Error;
            
            MessageBox.Show(message, "Fout", type, icon);
        }

        /*
         * Notifies the user he is already linked to the group.
         * ---------------------------------------------
         * Show OK messagebox
         */
        private static void ShowAlreadyLinkedError()
        {
            const string message = "Je bent al gekoppeld aan deze groep";
            const MessageBoxButton type = MessageBoxButton.OK;
            const MessageBoxImage icon = MessageBoxImage.Error;

            MessageBox.Show(message, "Fout", type, icon);
        }

        /*
         * Ask the user if wants to link to the group.
         * ---------------------------------------------
         * Show Yes/No messagebox
         */
        private static MessageBoxResult AskUserVerification()
        {
            const string message = "Weet je zeker dat je  aan deze groep wilt koppelen";
            const MessageBoxButton type = MessageBoxButton.YesNo;
            const MessageBoxImage icon = MessageBoxImage.Question;

            var linkGroupMessageBox = MessageBox.Show(message, "Koppelen", type, icon);
            return linkGroupMessageBox;
        }
    }
}