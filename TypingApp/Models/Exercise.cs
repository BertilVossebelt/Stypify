using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace TypingApp.Models;

public class Exercise
{
    public string Name { get; }
    public string Text { get; }
    public int AmountOfCharacters { get; }

    public Exercise(string text, string name)
    {
        Name = name;
        Text = text;
        AmountOfCharacters = Text.Length;
    }

    // Generate exercise.
    public Exercise(IReadOnlyList<Character> subset)
    {
        var random = new Random();
        const int words = 20;
        var text = "";

        for (var i = 0; i < words; i++)
        {
            var wordLength = random.Next(1, 15);
            for (var j = 0; j < wordLength; j++)
            {
                // Create random 'word' from subset based on random word length.
                var index = random.Next(subset.Count);
                var letter = subset[index].Char;
                text += letter;
            }

            text += " ";
        }

        Text = text.TrimEnd(' ');
    }
}

