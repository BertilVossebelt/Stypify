namespace TypingApp.Models;

public class Character
{
    public Character(char c)
    {
        Char = c;
    }

    public uint User_id { get; set; }

    public char Char { get; }
    
    public byte Accuracy { get; set; }
    
    public bool Wrong { get; set; }
    
    public bool Correct { get; set; }
}