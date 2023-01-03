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

    public override void Execute(object? parameter)
    {
        if (_adminDashboardViewModel.DeleteEmail.Length == 0)
            ShowDeleteEmailEmptyErrorMessage();
        else if (TeacherAccountExists())
            try
            {
                new AdminProvider().RemoveTeacher(_adminDashboardViewModel.DeleteEmail);
                ShowTeacherAccountDeletedMessage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        else
            ShowTeacherAccountNotFoundErrorMessage();
    }

    // Check of het docentaccount bestaat.
    private bool TeacherAccountExists()
    {
        var teacher = new TeacherProvider().GetByEmail(_adminDashboardViewModel.DeleteEmail);
        return teacher != null;
    }
    
    // Laat een informatiebericht zien als een docentaccount succesvol is verwijderd.
    private void ShowTeacherAccountDeletedMessage()
    {
        const string message = "Docentaccount is succesvol verwijderd.";
        const MessageBoxButton type = MessageBoxButton.OK;
        const MessageBoxImage icon = MessageBoxImage.Information;

        MessageBox.Show(message, "Account verwijderd", type, icon);
    }
    
    // Laat een errorbericht zien als het emailveld leeg is.
    private void ShowDeleteEmailEmptyErrorMessage()
    {
        const string message = "Het emailveld voor het verwijderen mag niet leeg zijn.";
        const MessageBoxButton type = MessageBoxButton.OK;
        const MessageBoxImage icon = MessageBoxImage.Error;

        MessageBox.Show(message, "Emailveld leeg", type, icon);
    }

    // Laat een errorbericht zien als een docentaccount niet gevonden kan worden.
    private void ShowTeacherAccountNotFoundErrorMessage()
    {
        const string message = "Docentaccount bestaat niet.";
        const MessageBoxButton type = MessageBoxButton.OK;
        const MessageBoxImage icon = MessageBoxImage.Error;

        MessageBox.Show(message, "Docentaccount niet gevonden", type, icon);
    }
}

    
