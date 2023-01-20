using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Phase { INTRO, GENERAL, FINAL };

    [NonSerialized]
    public MustakisGameData mustakisGameData;
    [NonSerialized]
    public MustakisSaveData mustakisSaveData;
    private string jwt;
    private string auxUserNameForSave; // Guarda redundantemente el username para newSave() y getSave(),
                                       // cuando todavia no se tiene mustakisSaveData
    private string generalUri = "https://planeta-backend.onrender.com/api";

    [NonSerialized]
    public string lastSceneBeforeTrailer = ""; // Tag de escena previa al trailer, para determinar escena posterior
    // Gabo - Fases
    [NonSerialized]
    public Phase currentPhase;
    // Gabo - FIN DE JUEGO
    [NonSerialized]
    public bool isGameFinished;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("Menu");

        currentPhase = Phase.INTRO; // Se activa fase inicial
    }

    public IEnumerator PostForm(string specificUri, WWWForm form, Action FallbackSuccess, Action FallbackError)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(generalUri + specificUri, form))
        {
            Debug.Log("Post started");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error");
                Debug.Log(www.error);
                FallbackError();
            }
            else
            {
                Debug.Log("Form upload complete!");
                var json = www.downloadHandler.text;
                var playerData = JsonUtility.FromJson<PlayerData>(json);
                auxUserNameForSave = playerData.name;
                StartCoroutine(Auth(form, FallbackSuccess, FallbackError));

            }
        }
    }

    public IEnumerator CheckGame(string uncheckedGameName, Action FallbackSuccess, Action FallbackError)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{generalUri}/game/checkGame?gameName={uncheckedGameName}"))
        {
            Debug.Log("Checking game");
            www.SetRequestHeader("Authorization", $"Bearer {jwt}");
            yield return www.SendWebRequest();
            

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error, game not found");
                Debug.Log(www.error);
                FallbackError();
            }
            else
            {
                var json = www.downloadHandler.text;
                var gameData = JsonUtility.FromJson<MustakisGameData>(json);
                mustakisGameData = gameData;
                Debug.Log("Game found!: ->" + mustakisGameData.gameName + "<-");
                Debug.Log(JsonUtility.ToJson(mustakisGameData));
                FallbackSuccess();

            }
        }
    }

    public IEnumerator Auth(WWWForm form, Action CallbackSuccess, Action CallbackError)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(generalUri + "/auth", form))
        {
            Debug.Log("Auth started");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error");
                Debug.Log(www.error);
                CallbackError();
            }
            else
            {
                
                var json = www.downloadHandler.text;
                var response = JsonUtility.FromJson<Token>(json);
                Debug.Log(response.token);
                Debug.Log("Auth Successful");

                jwt = response.token;
                CallbackSuccess();
            }
        }
    }

    public IEnumerator PostAnswer(WWWForm form, Action<FeedbackResponse> CallBackSuccess, Action CallbackError)
    {
        using (UnityWebRequest www = UnityWebRequest.Post($"{generalUri}/game/answer?email={mustakisSaveData.email}&gameName={mustakisGameData.gameName}", form))
        {
            Debug.Log("Posting answer");
            www.SetRequestHeader("Authorization", $"Bearer {jwt}");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error");
                Debug.Log(www.error);

                CallbackError();
            }
            else
            {
                Debug.Log("Answers posted!");
                string json = www.downloadHandler.text; 
                var response = JsonUtility.FromJson<FeedbackResponse>(json);                
                Debug.Log(json);
                CallBackSuccess(response);
            }
        }
    }

    public IEnumerator newSave(Action<MustakisSaveData> CallbackSuccess, Action CallbackError)
    {
        WWWForm form = new WWWForm();
        form.AddField("gameName", mustakisGameData.gameName);
        form.AddField("userName", auxUserNameForSave);
        using (UnityWebRequest www = UnityWebRequest.Post($"{generalUri}/game/newEmpty", form))
        {
            Debug.Log("Creating new save");
            www.SetRequestHeader("Authorization", $"Bearer {jwt}");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error");
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("New save created!");
                var json = www.downloadHandler.text;
                mustakisSaveData = JsonUtility.FromJson<MustakisSaveData>(json);
                Debug.Log(JsonUtility.ToJson(mustakisSaveData));
                CallbackSuccess(mustakisSaveData);

            }
        }
    }

    public IEnumerator getSave(Action<MustakisSaveData> CallbackSuccess, Action CallbackError)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{generalUri}/game?userName={auxUserNameForSave}&gameName={mustakisGameData.gameName}"))
        {
            Debug.Log("Getting save");
            www.SetRequestHeader("Authorization", $"Bearer {jwt}");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error");
                Debug.Log(www.error);
                CallbackError();
            }
            else
            {
                Debug.Log("Save found!");
                var json = www.downloadHandler.text;
                mustakisSaveData = JsonUtility.FromJson<MustakisSaveData>(json);
                Debug.Log(JsonUtility.ToJson(mustakisSaveData));
                CallbackSuccess(mustakisSaveData);
            }
        }
    }

    // REINICIO de variables PERSISTENTES o ESTÁTICAS relevantes
    public void RestartEverything()
    {
        _Restart();
        UIQuestionBox.RestartStatic();
        ItemAssets.RestartStatic();
        Debug.Log("GameManager: ¡Reiniciando variables!"); 
    }
    private void _Restart()
    {

        mustakisGameData = null;
        mustakisSaveData = null;
        jwt = null;
        auxUserNameForSave = null;
        lastSceneBeforeTrailer = "";
        currentPhase = Phase.INTRO;
        isGameFinished = false;
    }
}