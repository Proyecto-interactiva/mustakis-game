using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;




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
    Vector2 currentMovementVector;
    Vector2 lastNonIdleUnitaryMovementVector;
    List<Vector2> cardinalDirections;
    List<Tuple<Collider2D, Vector2>> forbiddenDirectionTuples;
    // Rigidbody
    Rigidbody2D rb;
    // Animation components
    Animator animator;
    // Tips
    GameObject tipsContainer;
    TMP_Text tipsBoxText;
    List<string> tips;
    int currentTipIndex;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
        tipsContainer = this.gameObject.transform.Find("Tips").gameObject;
        if (tipsContainer == null) { Debug.LogError("DogBehaviour: No se encuentra contenedor \"Tips\"!"); }
        tipsBoxText = tipsContainer.GetComponentInChildren<TMP_Text>();
        if (tipsBoxText == null) { Debug.LogError("DogBehaviour: No se encuentra TMP_Text en \"Tips\"!"); }
        rb = gameObject.GetComponent<Rigidbody2D>();

        tips = new List<string>() { "AAA A AAA", "SDGFDXGX SDFGVDG", "REEEEEEEEEE EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE", "BANANAAAAAAAAAAAAAAAY"};
        //tips = gameManager.mustakisGameData.dialogues.tips;

        currentState = DogState.IDLE;
        isPlayerInteracting = false;
        isCycleActive = false;
        currentMovementVector = Vector2.zero;
        cardinalDirections = new List<Vector2>()
            {
                    Vector2.up,
                    Vector2.right,
                    Vector2.down,
                    Vector2.left
            };
        forbiddenDirectionTuples = new List<Tuple<Collider2D, Vector2>>();

        currentCycle = MoveCycle(cycleDuration);
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

        // Actualizar animación
        UpdateAnimation();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Se setea inmediatamente en IDLE
        currentState = DogState.IDLE;

        // Revisa si jugador está colisionando y lo toma como interacción
        if (collision.gameObject.layer == 6) // "Player"
        {
            isPlayerInteracting = true;
        }

        // Mostrar tips
        if (isPlayerInteracting && !tipsContainer.activeInHierarchy)
        {
            tipsContainer.SetActive(true);
            // Mostrar tip al azar
            int randomTipIndex = UnityEngine.Random.Range(0, tips.Count);
            currentTipIndex = randomTipIndex;
            tipsBoxText.text = tips[currentTipIndex];
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Revisa si objeto con el que termina la colisión es el jugador y termina interacción
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
        currentMovementVector = Vector2.zero;

        // Estados (definen movimiento)
        if (currentState == DogState.IDLE)
        {
            currentMovementVector = Vector2.zero;
        }
        else if (currentState == DogState.WALKING)
        {
            //// Evitar caminar contra una pared a la que se está pegada
            //// -------------------
            ContactFilter2D wallsCF = new ContactFilter2D();
            wallsCF.SetLayerMask(LayerMask.GetMask("Dog Walls"));
            List<Collider2D> currColliders = new List<Collider2D>();
            rb.OverlapCollider(wallsCF, currColliders); // Agrega colisionadores actuales

            // CASO 1: Paredes NUEVAS
            if (currColliders.Count > forbiddenDirectionTuples.Count)
            {
                // Obtener nueva pared
                Collider2D newWall = null;
                Collider2D auxCollider = null;
                for (int i = 0; i < currColliders.Count; i++)
                {
                    auxCollider = currColliders[i];
                    bool found = false;
                    foreach(var tuple in forbiddenDirectionTuples)
                    {
                        if (tuple.Item1 == auxCollider)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found == false) { newWall = auxCollider; break; }
                }
                // ERROR: Pared nueva no encontrada
                if (newWall == null) { Debug.LogError("DogBehaviour: Pared no encontrada" +
                    "entre prohibidas"); }
                // Agregar nueva pared
                var newForbiddenTuple = new Tuple<Collider2D, Vector2>(newWall,
                    lastNonIdleUnitaryMovementVector);
                forbiddenDirectionTuples.Add(newForbiddenTuple);
                // Eliminar dirección nueva prohibida
                bool result = cardinalDirections.Remove(lastNonIdleUnitaryMovementVector);
                // ERROR: Dirección no encontrada
                if (!result) { Debug.LogError("DogBehaviour:Dirección prohibida nueva" +
                    " no encontrada"); }
            }

            // CASO 2: MENOS paredes
            else if (currColliders.Count < forbiddenDirectionTuples.Count)
            {
                // Obtener tupla con pared abandonada
                Tuple<Collider2D, Vector2> abandonedWallTuple = null;
                Collider2D auxCollider = null;
                for (int i = 0; i < forbiddenDirectionTuples.Count; i++)
                {
                    bool gone = true;
                    auxCollider = forbiddenDirectionTuples[i].Item1;
                    foreach (var currCollider in currColliders)
                    {
                        if (auxCollider == currCollider)
                        {
                            gone = false;
                            break;
                        }
                    }
                    if (gone) { abandonedWallTuple = forbiddenDirectionTuples[i]; break; }
                }
                // ERROR: Pared abandonada no encontrada
                if (abandonedWallTuple == null)
                {
                    Debug.LogError("DogBehaviour: Pared abandonada no está entre prohibidas");
                }
                // Eliminar pared abandonada de prohibidas                
                forbiddenDirectionTuples.Remove(abandonedWallTuple);
                // Permitir nuevamente dirección prohibida
                cardinalDirections.Add(abandonedWallTuple.Item2);
            }
            //// -------------------

            // Dirección cardinal random (permitidas) y vector velocidad resultante
            int randIndex = Random.Range(0, cardinalDirections.Count);
            currentMovementVector = cardinalDirections[randIndex] * speed;
            lastNonIdleUnitaryMovementVector = currentMovementVector.normalized;
            // ERROR: Vector NonIdle es cero
            if (lastNonIdleUnitaryMovementVector == Vector2.zero)
            {
                Debug.LogError("DogBehaviour: lastNonIdleUnitaryMovementVector es CERO!");
            }
        }

        // Ciclo
        while (remainingTime > 0f)
        {
            // Si no interactúo y no IDLE, me muevo
            if (!isPlayerInteracting && currentState != DogState.IDLE)
            {
                rb.velocity = currentMovementVector;
            }
            // Si IDLE EXTERNO: Detiene pero deja que termina ciclo.
            else if (currentState == DogState.IDLE)
            {
                currentMovementVector = Vector2.zero;
                rb.velocity = Vector2.zero;
            }
            // Detiene y termina ciclo
            else
            {
                // Si interactúa con jugador, pasa a IDLE
                currentState = DogState.IDLE;
                currentMovementVector = Vector2.zero;
                rb.velocity = Vector2.zero;
                break;
            }
            remainingTime -= Time.deltaTime;
            yield return null;
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

        // Termina ciclo
        isCycleActive = false;
    }

    // Actualizar parámetros de animación
    private void UpdateAnimation()
    {
        // Actualizar parámetros de animación
        const float ARBITRARY_LOW_NUMBER = 0.000001f; // Margen de error, para prevenir bugs con el 'cero float' (0f).
        if (currentMovementVector.x > ARBITRARY_LOW_NUMBER) { animator.SetInteger("Horizontal", 1); }
        if (currentMovementVector.x > -ARBITRARY_LOW_NUMBER && currentMovementVector.x < ARBITRARY_LOW_NUMBER) { animator.SetInteger("Horizontal", 0); }
        if (currentMovementVector.x < -ARBITRARY_LOW_NUMBER) { animator.SetInteger("Horizontal", -1); }

        if (currentMovementVector.y > ARBITRARY_LOW_NUMBER) { animator.SetInteger("Vertical", 1); }
        if (currentMovementVector.y > -ARBITRARY_LOW_NUMBER && currentMovementVector.y < ARBITRARY_LOW_NUMBER) { animator.SetInteger("Vertical", 0); }
        if (currentMovementVector.y < -ARBITRARY_LOW_NUMBER) { animator.SetInteger("Vertical", -1); }
    }
}
