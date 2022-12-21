using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypingApp.Models;

namespace TypingApp.Stores;

public class ExerciseStore
{
    public event Action<List<Character>>? ExerciseCreated;
    public event Action<List<Character>>? ExerciseUpdated;
    public List<Character> TextAsCharList { get; private set; }

    public ExerciseStore()
    {
        TextAsCharList = new List<Character>();
    }
    
    public void CreateExercise(string text)
    {
        TextAsCharList = text.Select(c => new Character(c)).ToList();
        OnExerciseCreated();
    }
    
    private void OnExerciseCreated()
    {
        ExerciseCreated?.Invoke(TextAsCharList);
    }
    
    public void UpdateExercise(List<Character> characters)
    {
        TextAsCharList = characters;
        OnExerciseUpdated();
    }

    private void OnExerciseUpdated()
    {
        ExerciseUpdated?.Invoke(TextAsCharList);
    }
}