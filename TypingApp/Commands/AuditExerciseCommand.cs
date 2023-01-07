using TypingApp.Services;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Commands;

public class AuditExerciseCommand : CommandBase
{
    private readonly LessonViewModel _lessonViewModel;
    private readonly LessonStore _lessonStore;
    private readonly UserStore _userStore;

    public AuditExerciseCommand(LessonViewModel lessonViewModel, LessonStore lessonStore, UserStore userStore)
    {
        _lessonViewModel = lessonViewModel;
        _lessonStore = lessonStore;
        _userStore = userStore;
    }

    /*
     * Method is used to audit an exercise and updates the place number of the user.
     * -----------------------------------------------------------------------------
     * Method is only used for students.
     */
    public override void Execute(object? parameter)
    {
        var input = _lessonViewModel.UserInputText;
        var expected = _lessonViewModel.Exercise.Text;
        
        // Check if the input is correct using the AuditExerciseService.
        var auditedExercise = new AuditExerciseService().Audit(input, expected);
        
        _lessonStore.CreateAuditedExercise(auditedExercise); // Save the audited exercise.
        UpdateDatabase(); // Update place_number in the database.
    }
    
    /*
    * Updates the database with a new lesson or updates the existing one.
    */
    private void UpdateDatabase()
    {
        // If the user is not a student, do nothing.
        if (_userStore.Student == null) return;

        // Initialize variables.
        var lessonId = _lessonStore.CurrentLesson.Id;
        var studentId = _userStore.Student.Id;
        var currentExercise = _lessonStore.CurrentExercise;
        
        var placeNumber = currentExercise < _lessonStore.CurrentLesson.Exercises.Count - 1 ? currentExercise + 1 : 0;
        
        // Get the lesson from the database.
        var studentProvider = new StudentProvider();
        var lesson = studentProvider.GetLessonById(lessonId, studentId);
        
        // If the lesson is null, create a new one.
        if (lesson == null) studentProvider.CreateLesson(lessonId, studentId, placeNumber); // Create a new lesson.
        else studentProvider.UpdateLesson(lessonId, studentId, placeNumber); // Update place_number.
    }
}