using System.Collections.Generic;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;

namespace TypingApp.Services;

public class AuditExerciseService
{
    private List<Character> _audited = null!;
    private string _input = null!;
    private string _expected = null!;
    
    /*
    * Audits the user input and updates the exercise accordingly.
    */
    public List<Character> Audit(string input, string expected)
    {
        _input = input; // These need to be here for some reason, don't move them to the constructor.
        _expected = expected;
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
        
        
        return _audited;
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
    * Adds incorrect, missing character to the audited list and removes it from the expected string.
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