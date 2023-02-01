using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReturnToGuardianArrow : MonoBehaviour
{
    private Image image;
    private bool isPlaying;

    // Start is called before the first frame update
    void Start()
    {
        isPlaying = false;
        image = GetComponent<Image>();
    }

    // Ejecuta parpadeo de la flecha un número de repeticiones
    public void Play(int repetitions, float delay)
    {
        // Chequeo de rango de valores
        if (repetitions <= 0 || delay <= 0f)
        {
            throw new System.Exception("UIReturnToGuardianArrow: ¡Valores inválidos!");
        }

        if (!isPlaying)
        {
            isPlaying = true;
            StartCoroutine(_Play(repetitions, delay));
        }
    }
    private IEnumerator _Play(int repetitions, float delay)
    {

        int currentCounter = repetitions;
        while (currentCounter > 0)
        {
            ToggleImage();
            yield return new WaitForSeconds(delay);
            ToggleImage();
            yield return new WaitForSeconds(delay);
            currentCounter--;
        }

        // Al salir...
        isPlaying = false;
    }

    private void ToggleImage()
    {
        if (!image.isActiveAndEnabled) { gameObject.GetComponent<Image>().enabled = true; }
        else if (image.isActiveAndEnabled) { gameObject.GetComponent<Image>().enabled = false; }
    }
}
