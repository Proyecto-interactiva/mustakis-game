using System.Collections.Generic;



public class MessagesResponse
{
    public List<string> dialogs;
    public bool quest;
    public List<Answer> answers;
}

public class Answer
{
    public string content;
    public bool correct;
    public int score; // may be float
}

public class PlayerData
{
    public string name;
    public string email;
    public string password;
}

public class Token
{
    public string token;
}

public class Save
{
    public string username;
    public List<Stage> stages;

    public class Stage
    {
        public int id;
        public bool unlocked;
        public int scene;
        public List<Character> characters;
        public List<Question> questions;

        public class Character
        {
            public int id;
            public bool talkedTo;
        }

        public class Question
        {
            public int id;
            public int score;
        }
    }
}








