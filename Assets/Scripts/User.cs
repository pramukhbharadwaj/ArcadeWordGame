using System;

[Serializable]
public class User
{
    public string username;
    public int score;

    public User(string username, int score)
    {
        this.username = username;
        this.score = score;
    }
}
