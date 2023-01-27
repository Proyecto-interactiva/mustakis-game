using UnityEngine;

public class ConstellationAssets : MonoBehaviour
{
    // Singleton
    public static ConstellationAssets Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public Sprite constellation1OFF;
    public Sprite constellation2OFF;
    public Sprite constellation3OFF;

    public Sprite constellation1ON;
    public Sprite constellation2ON;
    public Sprite constellation3ON;

    // Obtener sprite correcto para la constelación. Recibe tipo y estado (apagado/prendido)
    public Sprite GetSprite(ConstellationManager.ConstellationType constellationType, bool isOn)
    {
        switch (constellationType)
        {
            case ConstellationManager.ConstellationType.Constellation1:
                if (isOn) { return constellation1ON; }
                else { return constellation1OFF; }

            case ConstellationManager.ConstellationType.Constellation2:
                if (isOn) { return constellation2ON; }
                else { return constellation2OFF; }

            case ConstellationManager.ConstellationType.Constellation3:
                if (isOn) { return constellation3ON; }
                else { return constellation3OFF; }
        }
        Debug.LogError("GetSprite(): No existe un sprite para aquel tipo.");
        return null;
    }

    public static void RestartStatic()
    {
        Instance = null;
    }
}
