using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour {

    public static TitleScreenManager singleton;
    
    [Header("Menus")]
    [SerializeField] GameObject titleScreenMainMenu;
    [SerializeField] GameObject titleScreenLoadMenu;

    [Header("Buttons")]
    [SerializeField] Button loadMenuReturnButton;
    [SerializeField] Button mainMenuLoadGameButton;
    [SerializeField] Button mainMenuNewGameButton;
    [SerializeField] Button deleteCharacterConfirmButton;

    [Header("Popups")]
    [SerializeField] GameObject noCharacterSlotsPopUp;
    [SerializeField] Button noCharavterSlotsOkayButton;
    [SerializeField] GameObject deleteCharacterSlotPopup;
    
    [Header("Character Save Slots")]
    public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

    void Awake() {
        if(singleton == null) {singleton = this;}
        else Destroy(gameObject);
    } 
    
    public void StartNetworkAsHost() {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame() {
        WorldSaveGameManager.singleton.CreateNewGame();
        StartCoroutine(WorldSaveGameManager.singleton.LoadWorld());
    }

    public void OpenLoadGameMenu() {
        titleScreenLoadMenu.SetActive(true);
        titleScreenMainMenu.SetActive(false);
        loadMenuReturnButton.Select();
    }

    public void CloseLoadGameMenu() {
        titleScreenMainMenu.SetActive(true);
        titleScreenLoadMenu.SetActive(false);
        mainMenuLoadGameButton.Select();
    }

    public void DisplayNoFreeCharacterSlots() {
        noCharacterSlotsPopUp.SetActive(true);
        noCharavterSlotsOkayButton.Select();
    }

    public void CloseNoFreeCharacterSlots() {
        noCharacterSlotsPopUp.SetActive(false);
        mainMenuNewGameButton.Select();
    }

    // Character Slots

    public void SelectCharacterSlot(CharacterSlot characterSlot) {
        currentSelectedSlot = characterSlot;
    }

    public void SelectNoSlot() {
        currentSelectedSlot = CharacterSlot.NO_SLOT;
    }

    public void AttemptToDeleteCharacterSlot() {
        deleteCharacterSlotPopup.SetActive(true);
        deleteCharacterConfirmButton.Select();
    }

    public void DeleteCharacterSlot() {
        WorldSaveGameManager.singleton.DeleteGame(currentSelectedSlot);
        //Refresh list of saved characters.
        titleScreenLoadMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);
        CloseDeleteCharacterPopUp();

    }

    public void CloseDeleteCharacterPopUp() {
        deleteCharacterSlotPopup.SetActive(false);
        loadMenuReturnButton.Select();
    }
}
