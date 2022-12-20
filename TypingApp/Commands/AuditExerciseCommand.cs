using System;
using System.Collections.Generic;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Commands;

public class AuditExerciseCommand : CommandBase
{
    private readonly LessonStore _lessonStore;
    private readonly UserStore _userStore;
    private readonly LessonViewModel _lessonViewModel;
    private List<Character> _audited;
    private string _input;
    private string _expected;

    public AuditExerciseCommand(LessonViewModel lessonViewModel, LessonStore lessonStore, UserStore userStore)
    {
        _lessonViewModel = lessonViewModel;
        _lessonStore = lessonStore;
        _userStore = userStore;
    }

    /*
     * Audits the user input and updates the exercise accordingly.
     */
    public override void Execute(object? parameter)
    {
        _input = _lessonViewModel
            .UserInputText; // These need to be here for some reason, don't move them to the constructor.
        _expected = _lessonViewModel.Exercise.Text;
        _audited = new List<Character>();
        var pos = 0;

        // Continue loop until there are no more characters to audit.
        while (_input.Length + _expected.Length > 0)
        {
            if (_input.Length > 0 && _expected.Length > 0 && _input[0] == _expected[0])
            {
                /* If character is correct, add it to the correct list and remove it from
                 * both strings. */
                HandleCorrectCharacters(pos);
            }
            else
            {
                if (_input.Length >= _expected.Length)
                {
                    /* If character is incorrect and input >= expected,
                     * add it to the incorrect list and remove it from the input string. */
                    HandleExtraCharacters(pos);
                }
                else
                {
                    /* If character is incorrect and input < expected,
                     * add it to the incorrect list and remove it from the expected string. */
                    HandleMissingCharacters(pos);
                }
            }

            pos++;
        }

        _lessonStore.CreateAuditedExercise(_audited); // Save the audited exercise.
        UpdateDatabase(); // Update place_number in the database.
    }

    /*
     * Updates the database with a new lesson or updates the existing one.
     */
    private void UpdateDatabase()
    {
        if (_userStore.Student == null) return; // If the user is not a student, do nothing.
        var lessonId = _lessonStore.CurrentLesson.Id;
        var studentId = _userStore.Student.Id;
        var currentExercise = _lessonStore.CurrentExercise;
        var placeNumber = currentExercise  <= _lessonStore.Lessons.Count ? currentExercise + 1 : 0;
        
        var studentProvider = new StudentProvider();
        var lesson = studentProvider.GetLessonById(lessonId, studentId);
        if (lesson == null) studentProvider.CreateLesson(lessonId, studentId, placeNumber); // Create a new lesson.
        else studentProvider.UpdateLesson(lessonId, studentId, placeNumber); // Update place_number.
    }

    /*
     * Adds the correct character to the audited list and remove it from both strings.
     */
    private void HandleCorrectCharacters(int pos)
    {
        var c = new Character(_input[0])
        {
            Pos = pos,
        };
        _audited.Add(c);

        // Remove first character from input and expected.
        _input = _input[1..];
        _expected = _expected[1..];
    }

    /*
     * Adds incorrect, extra character to the audited list and remove it from the input string.
     */
    private void HandleExtraCharacters(int pos)
    {
        var c = new Character(_input[0])
        {
            Extra = true,
            Pos = pos,
        };
        _audited.Add(c);
        _input = _input[1..]; // Remove first character from input.
    }

    /*
    * Adds incorrect, missing character to the audited list and remove it from the expected string.
    */
    private void HandleMissingCharacters(int pos)
    {
        var c = new Character(_expected[0])
        {
            Missing = true,
            Pos = pos,
        };
        _audited.Add(c);
        _expected = _expected[1..]; // Remove first character from expected.
    }
}