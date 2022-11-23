using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class TrailerManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // VideoPlayer del trailer
    private GameManager gameManager;
    private AudioManager audioManager;

    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.SetMute(true); // Se mutea m�sica de fondo al comenzar trailer

        videoPlayer.loopPointReached += loadNextSceneAfterVideoEnds; // Al terminar video, cambiar de escena

        // Asignar url del video y reproducir
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = Application.streamingAssetsPath + "/Trailer_Eduju.mp4"; // Video est� en 'Assets/StreamingAssets'
        videoPlayer.Play();
    }

    // Ejecutada cuando termina video
    public void loadNextSceneAfterVideoEnds(VideoPlayer vp)
    {
        loadNextSceneBasedOnLast();
    }

    // Determina siguiente escena basado en escena previa
    public void loadNextSceneBasedOnLast()
    {
        audioManager.SetMute(false); // Se desmutea m�sica de fondo al terminar trailer
        FindObjectOfType<AudioManager>().Play("Open");

        if (gameManager.lastSceneBeforeTrailer == "Register")
        {
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            SceneManager.LoadScene("Menu"); // Por ahora, si NO se accedi� desde el registro, se vuelve al principio
        }
    }
}
