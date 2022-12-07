namespace TypingApp.Models;

public class Character
{
    public Character(char c)
    {
        Char = c;
    }
    
    public char Char { get; }
    
    public byte Accuracy { get; set; }
    
    public int TimeToType { get; set; }
    
    // Not in database
    public bool Wrong { get; set; }
    
    public bool Correct { get; set; }
}