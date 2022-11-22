using System;
using System.Collections.Generic;

namespace TypingApp.Models;

public class Exercise
{
    public string Text { get; }
    
    // Generate exercise.
    public Exercise(IReadOnlyList<Character> subset)
    {
        var random = new Random();
        var words = 20;
        var text = "";
        
        for (int i = 0; i < words; i++)
        {
            var wordLength = random.Next(1, 15);
            for (int j = 0; j < wordLength; j++)
            {
                // Create random 'word' from subset based on random word length.
                var index = random.Next(subset.Count);
                var letter = subset[index].Char;
                text += letter;
            }

            text += " ";
        }

        Text = text;
    }
}

