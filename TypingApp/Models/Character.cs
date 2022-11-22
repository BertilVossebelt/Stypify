namespace TypingApp.Models;

public class Character
{
    public Character(uint userId, char c, byte accuracy)
    {
        User_id = userId;
        Accuracy = accuracy;
        Char = c;
    }

    public uint User_id { get; set; }
    
    public char Char { get; }
    
    public byte Accuracy { get; }
}