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
        audioManager.SetMute(true); // Se mutea música de fondo al comenzar trailer

        videoPlayer.loopPointReached += loadNextSceneAfterVideoEnds; // Al terminar video, cambiar de escena

        // Asignar url del video y reproducir
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = Application.streamingAssetsPath + "/Trailer_Eduju.mp4"; // Video está en 'Assets/StreamingAssets'
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
        audioManager.SetMute(false); // Se desmutea música de fondo al terminar trailer
        FindObjectOfType<AudioManager>().Play("Open");

        if (gameManager.lastSceneBeforeTrailer == "Register")
        {
            SceneManager.LoadScene("Play Menu");
        }
        else
        {
            SceneManager.LoadScene("Menu"); // Por ahora, si NO se accedió desde el registro, se vuelve al principio
        }
    }
}
