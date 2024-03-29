﻿namespace TypingApp.Models;

public class Character
{
    public char Char { get; }
    
    public byte? Accuracy { get; set; }
    
    public int? TimeToType { get; set; }
    
    // Not in database
    public bool Wrong { get; set; }
    public bool Correct { get; set; }
    public int Pos { get; set; }
    public bool Extra { get; set; }
    public bool Missing { get; set; }
    
    public Character(char c)
    {
        Char = c;
    }
}