using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TypingApp.Models;

namespace TypingApp.Stores;

public class ExerciseStore
{
    private readonly Lazy<Task> _initLazy;
    public event Action<List<Character>> ExerciseCreated;
    public event Action<List<Character>> ExerciseUpdated;

    public List<Character> TextAsCharList { get; private set; }

    public ExerciseStore()
    {
        TextAsCharList = new List<Character>();
        _initLazy = new Lazy<Task>(Initialize);
    }

    public async Task Load()
    {
        await _initLazy.Value;
    }

    public Task Initialize()
    {
        return Task.CompletedTask;
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