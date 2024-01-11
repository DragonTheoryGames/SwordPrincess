using System.Linq;
using UnityEngine;

public class UIInputManager : MonoBehaviour {
   
   PlayerControls playerControls;

    [Header("Title Screen Inputs")]
    [SerializeField] bool deleteCharacterSlot = false;
    
    void Update() {
        if (deleteCharacterSlot) {
            TitleScreenManager.singleton.AttemptToDeleteCharacterSlot();
        }
    }

    void OnEnable() {
        if(playerControls == null) {
            playerControls = new PlayerControls();
            playerControls.UI.AcceptSelect.performed += i => deleteCharacterSlot = true;
        }
        playerControls.Enable();
    }

    void OnDisable() {
        playerControls.Disable();
    }
}
