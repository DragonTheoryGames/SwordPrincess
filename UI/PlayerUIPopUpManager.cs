using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIPopUpManager : MonoBehaviour {
    
    [Header("YOU DIED")]
    [SerializeField] GameObject popUpGameObject;
    [SerializeField] TextMeshProUGUI diedPopUpText;
    [SerializeField] TextMeshProUGUI popUpBackgroundText;
    [SerializeField] CanvasGroup popUpCanvasGroup;

    public void SendPopUp() {
        // ACTIVATE POST PROCESSING EFFECTS
        popUpGameObject.SetActive(true);
        popUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(popUpBackgroundText, 8, 18f));
        StartCoroutine(FadeInPopUpOverTime(popUpCanvasGroup, 5));
        StartCoroutine(WaitThenFadeOutOverTime(popUpCanvasGroup, 2, 5));
    }

    private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretch) {
        if (duration > 0f) {
            text.characterSpacing = 0;
            float timer = 0;
            yield return null;

            while (timer < duration) {
                timer += Time.deltaTime;
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretch, duration * (Time.deltaTime / 20));
                yield return null;
            }
        }
    }

    private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration) {
        if (duration > 0) {
            canvas.alpha = 0;
            float timer = 0;
            yield return null;

            while (timer < duration) {
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * (Time.deltaTime / 20)); 
                yield return null;
            }
        }
    }

    private IEnumerator WaitThenFadeOutOverTime(CanvasGroup canvas, float duration, float delay) {
        if (duration > 0) {
            while (delay > 0) {
                delay -= Time.deltaTime;
                yield return null;
            }

            canvas.alpha = 1;
            float timer = 0;
            yield return null;

            while (timer < duration) {
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime); 
                yield return null;
            }
        }
    }
}
