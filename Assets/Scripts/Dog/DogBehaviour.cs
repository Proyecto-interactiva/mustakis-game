using System.Collections;
using System.Collections.Generic;
using UnityEngine;




enum DogState { IDLE, WALKING }

public class DogBehaviour : MonoBehaviour
{
    // Configuración
    public float speed;
    public float cycleDuration;
    public float maxCycleVariance;
    // Estado de comportamiento del perro
    DogState currentState;
    // Interactua con jugador?
    bool isPlayerInteracting;
    // Ciclo
    IEnumerator currentCycle;
    bool isCycleActive;
    // Rigidbody
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6) // "Player"
        {
            isPlayerInteracting = false;
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
