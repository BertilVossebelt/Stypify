using System;
using System.Windows;
using TypingApp.Services.DatabaseProviders;
using TypingApp.ViewModels;

namespace TypingApp.Commands;

public class DeleteTeacherCommand : CommandBase
{
    private readonly AdminDashboardViewModel _adminDashboardViewModel;

    public DeleteTeacherCommand(AdminDashboardViewModel adminDashboardViewModel)
    {
        _adminDashboardViewModel = adminDashboardViewModel;
    }

    /*
     * Delete teacher from database.
     * --------------------------------------
     * Method should only be used for admins.
     */
    public override void Execute(object? parameter)
    {
        string? message;
        
        // Check if email is empty.
        if (_adminDashboardViewModel.DeleteEmail.Length == 0)
        {
            message = "Het e-mailveld mag niet leeg zijn.";
            MessageBox.Show(message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        // Check if teacher exists and try to remove the teacher.
        var teacher = new TeacherProvider().GetByEmail(_adminDashboardViewModel.DeleteEmail);
        if (teacher != null)
        {
            try
            {
                // Remove teacher from database and notify user.
                new AdminProvider().DeleteTeacher(_adminDashboardViewModel.DeleteEmail);
                message = "Account is succesvol verwijderd.";
                MessageBox.Show(message, "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            return;
        }
        
        // Show error message if teacher doesn't exist.
        message = "Er is geen docent gevonden met dit e-mailadres.";
        MessageBox.Show(message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

    
