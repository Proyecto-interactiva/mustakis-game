using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


// DEBUGGING - ***Desactivar cuando NO sea necesario!!!***
class ApiConnection : MonoBehaviour
{
    private void Start()
    {
        MustakisGameData debugGameTest = new();
        MustakisSaveData debugSaveTest = new();
        //StartCoroutine(debugGameTest.debugJsonToMustakisGame());
        StartCoroutine(debugSaveTest.debugJsonToMustakisSave());
    }
}
// FIN DEBUGGING

// Clases originales (Lucas)
//================================================
//================================================
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
//================================================
//================================================

// Gabo - Macro-objeto con info del juego (Planeta Mustakis)
[System.Serializable]
public class MustakisGameData
{
    public string gameName;
    public List<Constellation> scenes;
    public string description;
    public NpcDialogue dialogues;
    public int maxScore;

    [System.Serializable]
    public class NpcDialogue
    {
        public List<string> introDialogues;
        public List<string> generalDialogues;
        public List<string> finalDialogues;
        public List<string> tips;
    }

    // Gabo - DEBUG: Paso de 'gameTest.json' a objeto MustakisGameData.
    public IEnumerator debugJsonToMustakisGame()
    {
        MustakisGameData testGame;
        string jsonPath = Application.streamingAssetsPath + "/gameTest.json";
        UnityWebRequest www = UnityWebRequest.Get(jsonPath);
        yield return www.SendWebRequest();

        testGame = JsonUtility.FromJson<MustakisGameData>(www.downloadHandler.text);

        // Logs
        Debug.Log(testGame.scenes[0].intro[0]);
        Debug.Log(JsonUtility.ToJson(testGame));
    }
}

// Gabo - Constelación (Planeta Mustakis)
[System.Serializable]
public class Constellation
{
    public List<string> intro; // Diálogos. Antes de preguntas.
    public List<string> outro; // Diálogos. Después de preguntas.
    public string name; // Sub-dimensión
    public int round; // Identificador de la constelación (pero téc. no tienen orden) 
    public List<Question> questionPacks; // Preguntas **ARREGLAR NOMENCLATURA CON KAT

    [System.Serializable]
    public class Question
    {
        public List<string> messages;
        public int order;
        public string question;
        public List<Answer> answers;

        [System.Serializable]
        public class Answer
        {
            public string content;
            public int score;
        }
    }
}


// Gabo - Objeto de guardado de partida para un usuario
[System.Serializable]
public class MustakisSaveData
{
    public string userName;
    public string gameName;
    public string email;
    public List<ConstellationSave> questionPacks;
    public int finalScore;

    [System.Serializable]
    public class ConstellationSave
    {
        public int round;
        public List<QuestionSave> questions;

        [System.Serializable]
        public class QuestionSave
        {
            public int order;
            public int score;
            public string answer;
            public bool unlocked;
        }
    }

    // Gabo - DEBUG: Paso de 'saveTest.json' a objeto MustakisSaveData.
    public IEnumerator debugJsonToMustakisSave()
    {
        MustakisSaveData testSave;
        string jsonPath = Application.streamingAssetsPath + "/saveTest.json";
        UnityWebRequest www = UnityWebRequest.Get(jsonPath);
        yield return www.SendWebRequest();

        testSave = JsonUtility.FromJson<MustakisSaveData>(www.downloadHandler.text);

        // Logs
        Debug.Log(JsonUtility.ToJson(testSave));
    }
}










