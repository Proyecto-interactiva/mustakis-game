using System;
using System.Collections.Generic;
using UnityEngine;

// Manejo del orden general de las constelaciones en el mapa
public class ConstellationManager : MonoBehaviour
{
    public enum ConstellationPhase { INTRO, QMESSAGES, QUESTIONS, OUTRO };

    // Tipos de constelación. Ligado al sprite asociado a cada ConstellationNPC.
    public enum ConstellationType
    {
        Constellation1,
        Constellation2,
        Constellation3
    }

    // Variables
    public MessagesDisplay messagesDisplay;
    public UIQuestionBox questionBox;
    [SerializeField]
    private GameObject constellationPrefab;
    [NonSerialized]
    public bool isSpawned; // Indica si ya se realizó el spawneo
    private GameManager gameManager;
    private List<Constellation> constellations;
    private List<MustakisSaveData.ConstellationSave> constellationSaves;
    private List<ConstellationNPC> constellationNPCs;

    // Singleton
    public static ConstellationManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        constellations = gameManager.mustakisGameData.scenes;
        constellationSaves = gameManager.mustakisSaveData.questionPacks;
        constellationNPCs = new();
    }

    // Spawnear constelaciones (al azar)
    public void SpawnConstellations()
    {

        List<(float x, float y)> outerCoords = new List<(float, float)> {
            (-4.565f, -21.296f), (8.45f, -21.296f), (-7.56f, -18.31f), (11.44f, -19.32f), (1.979f, -21.788f),
            (5.95f, -15.85f), (2.021f, -15.85f), (-6.01f, -7.84f), (-2.01f, -7.84f), (1.98f, -4.88f),
            (7.92f, -5.84f), (12.91f, -13.84f), (-8.56f, -13.34f), (2.021f,-11.4f)
        };

        int type = 1;
        foreach (Constellation constellation in constellations)
        {
            // Buscar save correcto
            MustakisSaveData.ConstellationSave currConstellationSave = null;
            foreach (MustakisSaveData.ConstellationSave constellationSave in constellationSaves)
            {
                if (constellation.round == constellationSave.round)
                {
                    currConstellationSave = constellationSave;
                    break;
                }
            }
            if (currConstellationSave == null) { Debug.LogError("ConstellationManager: ConstellationSave NO encontrado para" + constellation.name); }

            int n_coords = outerCoords.Count;
            int indexChoosen = UnityEngine.Random.Range(0, n_coords);
            SpawnConstellation(constellation.name, constellation, currConstellationSave, outerCoords[indexChoosen].x, outerCoords[indexChoosen].y, type);
            outerCoords.RemoveAt(indexChoosen);
            type++;
            if (type > Enum.GetNames(typeof(ConstellationType)).Length) type = 1; // Asignación circular de 'tipo'. Produce asignación circular de 'Sprites'.
        }
        isSpawned = true; // Las constelaciones han sido efectivamente spawneadas
    }

    // Spawnear una constelación
    private void SpawnConstellation(string info, Constellation constellation, MustakisSaveData.ConstellationSave constellationSave, float x, float y, int type)
    {
        GameObject newConstellation = Instantiate(constellationPrefab, new Vector3(x, y, 0), Quaternion.identity);
        ConstellationNPC currConstellationNPC = newConstellation.GetComponent<ConstellationNPC>();
        switch (type)
        {
            case 1:
                currConstellationNPC.constellationType = ConstellationType.Constellation1;
                break;
            case 2:
                currConstellationNPC.constellationType = ConstellationType.Constellation2;
                break;
            case 3:
                currConstellationNPC.constellationType = ConstellationType.Constellation3;
                break;
            default:
                currConstellationNPC.constellationType = ConstellationType.Constellation1;
                break;
        }
        currConstellationNPC.content = info;
        currConstellationNPC.constellation = constellation;
        currConstellationNPC.constellationSave = constellationSave;
        currConstellationNPC.gameObject.SetActive(true); // Necesario pues template "constellationPrefab" está desactivado
        constellationNPCs.Add(currConstellationNPC); // Se agrega a la lista de constelaciones en el mapa
    }

    // Chequear si las constelaciones estan completadas
    public bool isConstellationsComplete()
    {
        foreach (ConstellationNPC constellation in constellationNPCs)
        {
            if (!constellation.isComplete)
            {
                return false;
            }
        }
        return true;
    }

    public List<ConstellationNPC> GetConstellationNPCs()
    {
        return constellationNPCs;
    }

    // Borra singleton. Usado afuera cuando se sale (Ej.: Para loguearse con otro usuario)
    public static void RestartStatic()
    {
        Instance = null;
    }
}
