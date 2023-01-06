using TypingApp.Stores;

namespace TypingApp.Commands;

public class NextExerciseCommand : CommandBase
{
    private readonly LessonStore _lessonStore;

    public NextExerciseCommand(LessonStore lessonStore)
    {
        _lessonStore = lessonStore;
    }

    /*
     * Moves to the next exercise in the lesson.
     * -----------------------------------------
     * Method is only used for students.
     */
    public override void Execute(object? parameter)
    {
        _lessonStore.GoToNextExercise();
    }
}