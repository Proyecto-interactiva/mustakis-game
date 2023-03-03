using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;




enum DogState { IDLE, WALKING }

public class DogBehaviour : MonoBehaviour
{
    //General
    GameManager gameManager;
    // Configuración
    public float speed;
    public float cycleDuration;
    public float maxCycleVariance;
    // Estado de comportamiento del perro
    DogState currentState;
    // Interactua con jugador?
    bool isPlayerInteracting;
    // Ciclo caminata-idle
    IEnumerator currentCycle;
    bool isCycleActive;
    // Rigidbody
    Rigidbody2D rb;
    // Tips
    GameObject tipsContainer;
    TMP_Text tipsBoxText;
    List<string> tips;
    int currentTipIndex;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        tipsContainer = this.gameObject.transform.Find("Tips").gameObject;
        if (tipsContainer == null) { Debug.LogError("DogBehaviour: No se encuentra contenedor \"Tips\"!"); }
        tipsBoxText = tipsContainer.GetComponentInChildren<TMP_Text>();
        if (tipsBoxText == null) { Debug.LogError("DogBehaviour: No se encuentra TMP_Text en \"Tips\"!"); }

        //tips = new List<string>() { "AAA A AAA", "SDGFDXGX SDFGVDG", "REEEEEEEEEE EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE", "BANANAAAAAAAAAAAAAAAY"};
        tips = gameManager.mustakisGameData.dialogues.tips;

        currentState = DogState.IDLE;
        isPlayerInteracting = false;
        isCycleActive = false;

        currentCycle = MoveCycle(cycleDuration);
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCycleActive && !isPlayerInteracting)
        {
            float actualDuration = cycleDuration + Random.Range(-maxCycleVariance, maxCycleVariance);
            currentCycle = MoveCycle(actualDuration);
            StartCoroutine(currentCycle);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6) // "Player"
        {
            isPlayerInteracting = true;
        }

        // Mostrar tips
        if (isPlayerInteracting && !tipsContainer.activeInHierarchy)
        {
            tipsContainer.SetActive(true);
            // Mostrar tip al azar
            int randomTipIndex = Random.Range(0, tips.Count);
            currentTipIndex = randomTipIndex;
            tipsBoxText.text = tips[currentTipIndex];
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6) // "Player"
        {
            isPlayerInteracting = false;
        }

        // Esconder tips y reiniciar variables
        if (!isPlayerInteracting && tipsContainer.activeInHierarchy)
        {
            RestartAndClose();
        }
    }

    // Muestra siguiente tip según índice, cíclico. Para usarse con BOTÓN "Siguiente".
    public void NextTip()
    {
        if (currentTipIndex == tips.Count - 1)
        {
            currentTipIndex = 0;
            tipsBoxText.text = tips[currentTipIndex];
        }
        else
        {
            currentTipIndex++;
            tipsBoxText.text = tips[currentTipIndex];
        }
    }

    // Reiniciar variables de tips (ej.: Al dejar de interactuar con perro)
    private void RestartAndClose()
    {
        currentTipIndex = 0;
        tipsBoxText.text = "";
        if (tipsContainer.activeInHierarchy) 
        {
            tipsContainer.SetActive(false);
        }
    }

    private IEnumerator MoveCycle(float duration)
    {
        isCycleActive = true;
        float remainingTime = duration;
        Vector2 currentVector = Vector2.zero;

        // Estados (definen movimiento)
        if (currentState == DogState.IDLE)
        {
            currentVector = Vector2.zero;
        }
        else if (currentState == DogState.WALKING)
        {
            // Dirección cardinal random y vector velocidad resultante
            List<Vector2> cardinalDirections = new List<Vector2>()
            {
                    Vector2.up,
                    Vector2.right,
                    Vector2.down,
                    Vector2.left
            };
            currentVector = cardinalDirections[Random.Range(0, cardinalDirections.Count)] * speed;
        }

        // Ciclo
        while (remainingTime > 0f)
        {
            if (!isPlayerInteracting)
            {
                rb.velocity = currentVector;
            }
            else
            {
                // Si interactúa con jugador, pasa a IDLE
                currentState = DogState.IDLE;
                rb.velocity = Vector2.zero;
                break;
            }
            remainingTime -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        // Alternancia de comportamiento (Si NO interactúa con jugador)
        if (!isPlayerInteracting)
        {
            if (currentState == DogState.IDLE)
            {
                currentState = DogState.WALKING;
            }
            else if (currentState == DogState.WALKING)
            {
                currentState = DogState.IDLE;
            }
        }

        isCycleActive = false;
    }
}
