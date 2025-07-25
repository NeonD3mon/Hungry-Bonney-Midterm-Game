using System;

[Serializable]
public class HighScoreElement
{
    public string PlayerName;
    public int time;

    public HighScoreElement(string name, int time)
    {
        PlayerName = name;
        this.time = time;
    }
}
