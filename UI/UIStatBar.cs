using UnityEngine;
using UnityEngine.UI;

public class UIIStatBar : MonoBehaviour {

    Slider slider;
    RectTransform rectTransform;

    [Header("Bar Options")]
    [SerializeField] protected float widthScaleMultiplayer = 1;
    //TODO: SECONDARY YELLOW BAR

    protected virtual void Awake() {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void SetStat(int newValue) {
        slider.value = newValue;
    }

    public virtual void SetMaxStat(int maxValue) {
        slider.maxValue = maxValue;
        slider.value = maxValue;

        rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplayer, rectTransform.sizeDelta.y);
        PlayerUIManager.singleton.playerUIHUDManager.RefreshHUD();
    }
}