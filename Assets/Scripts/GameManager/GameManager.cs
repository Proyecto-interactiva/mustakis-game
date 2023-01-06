using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public MustakisGameData mustakisGameData;
    public MustakisSaveData mustakisSaveData;
    private string jwt;
    private string auxUserNameForSave; // Guarda redundantemente el username para newSave() y getSave(),
                                       // cuando todavia no se tiene mustakisSaveData
    private string generalUri = "https://planeta-backend.onrender.com/api"; 
    public GameObject bookPrefab;

    [NonSerialized]
    public string lastSceneBeforeTrailer = ""; // Tag de escena previa al trailer, para determinar escena posterior

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("Menu");
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

    //public IEnumerator NextMessage(int characterId, Action<MessagesResponse> CallBackSuccess, Action CallbackError)
    //{
    //    using (UnityWebRequest www = UnityWebRequest.Get($"{generalUri}/game/messages?userName={userName}&gameName={gameName}&stageId={stageId}&character={characterId}"))
    //    {
    //        Debug.Log("Next messages requested");
    //        www.SetRequestHeader("Authorization", $"Bearer {jwt}");
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log("Error");
    //            Debug.Log(www.error);

    //            CallbackError();
    //        }
    //        else
    //        {
    //            Debug.Log("Messages received!");
    //            var json = www.downloadHandler.text;
    //            var response = JsonUtility.FromJson<MessagesResponse>(json);
    //            Debug.Log(response);
    //            CallBackSuccess(response);
    //        }
    //    }
    //}

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

    public void SpawnBooks(List<string> answers)
    {
        
        List<(int x, int y)> outerCoords = new List<(int, int)> {
            (-8, 9), (-6, 9), (-4, 9), (-2, 9), (-0, 9), (2, 9), (4, 9), (6, 9), (8, 9),
            (11, 9), (11, 11), (11, 13), (11, 15), (11, 17), (11, 19), (11, 21), (11, 23), (11, 25),
            (11, 27), (-6, 27), (-4, 27), (-2, 27), (-0, 27), (2, 27), (4, 27), (6, 27), (8, 27),
            (-8, 27), (-8, 11), (-8, 13), (-8, 15), (-8, 17), (-8, 19), (-8, 21), (-8, 23), (-8, 25),
        };
        
        int type = 1;
        foreach (string answer in answers)
        {
            int n_coords = outerCoords.Count;
            int indexChoosen = UnityEngine.Random.Range(0, n_coords - 1);
            SpawnBook(answer, outerCoords[indexChoosen].x, outerCoords[indexChoosen].y, type);
            outerCoords.RemoveAt(indexChoosen);
            type++;
            if (type > 5) type = 1;
        }
    }

    private void SpawnBook(string answer, float x, float y, int type)
    {
        GameObject newBook = Instantiate(bookPrefab, new Vector3(x, y, 0), Quaternion.identity);
        Item item = newBook.GetComponent<Item>();
        switch (type)
        {
            case 1:
                item.itemType = Item.ItemType.Book1;
                break;
            case 2:
                item.itemType = Item.ItemType.Book2;
                break;
            case 3:
                item.itemType = Item.ItemType.Book3;
                break;
            case 4:
                item.itemType = Item.ItemType.Book4;
                break;
            case 5:
                item.itemType = Item.ItemType.Book5;
                break;
            default:
                item.itemType = Item.ItemType.Book1;
                break;
        }
        newBook.GetComponent<Item>().content = answer;
    }
}