using System.Collections.Generic;



public class MessagesResponse
{
    public List<string> dialogs;
    public bool quest;
    public List<string> answers;
    public bool lastScene;
}

public class FeedbackResponse
{
    public string feedbackMessage;
    public bool progress;
}

public class Answer
{
    public string content;
}

public class PlayerData
{
    public string name;
    public string email;
    public string password;
}

public class GameData
{
    public string gameName;
}

public class Token
{
    public string token;
}

public class Save
{
    public string userName;
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

[System.Serializable] // Parece ser necesario???
// Gabo - Stage con Packs de Preguntas por libro
public class QuestionPack
{   
    public List<Question> questions = new(); // **Habia que inicializarlo para que no fuese null?? Y por q las del Lucas arriba no?

    [System.Serializable]
    public class Question
    {
        public int id;
        public string text;
        public List<string> answers;
    }
}










