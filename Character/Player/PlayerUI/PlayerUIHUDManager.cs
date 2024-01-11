using UnityEngine;

public class PlayerUIHUDManager : MonoBehaviour {
    
    [SerializeField] UIIStatBar healthBar;
    [SerializeField] UIIStatBar staminaBar;

    public void RefreshHUD() {
        healthBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);
        staminaBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(true);
    }

    //Health
    public void SetNewHealthValue(int oldValue, int newValue) {
        healthBar.SetStat(newValue);
    }

    public void SetMaxHealthValue(int maxHealth) {
        healthBar.SetMaxStat(maxHealth * 10);
        RefreshHUD();
    }

    //Stamina
    public void SetNewStaminaValue(int oldValue, int newValue) {
        staminaBar.SetStat(newValue);
    }

    public void SetMaxStaminaValue(int maxStamina) {
        staminaBar.SetMaxStat(maxStamina * 10);
        RefreshHUD();
    }
    
}
