using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour {
    
    public static WorldSaveGameManager singleton;

    public PlayerManager player;


    [Header("SAVE/LOAD")]
    [SerializeField] bool saveGame;
    [SerializeField] bool loadGame;

    [Header("World Scene Index")]
    [SerializeField] int worldSceneIndex = 1;

    [Header("Save Data Writer")]
    SaveFileDataWriter saveFileDataWriter;

    [Header("Current Character Data")]
    public CharacterSlot currentCharacterSlot;
    public CharacterSaveData currentCharacterData;
    string fileName;

    [Header("Character Slots")]
    public CharacterSaveData characterSlot0;
    public CharacterSaveData characterSlot1;
    public CharacterSaveData characterSlot2;
    public CharacterSaveData characterSlot3;
    public CharacterSaveData characterSlot4;
    public CharacterSaveData characterSlot5;
    public CharacterSaveData characterSlot6;
    public CharacterSaveData characterSlot7;
    public CharacterSaveData characterSlot8;
    public CharacterSaveData characterSlot9;

    void Awake() {
        if(singleton == null) {singleton = this;}
        else Destroy(gameObject);
    }

    void Start() {
        DontDestroyOnLoad(gameObject);
        LoadAllCharacterProfiles();
    }

    void Update() {
        if(saveGame) {
            saveGame = false;
            SaveGame();
        }
        
        if(loadGame) {
            loadGame = false;
            LoadGame();
        }
    }

    public string DecideCharacterFileName(CharacterSlot characterSlot) {
        //string filename = "";
        switch (characterSlot) {
            case CharacterSlot.CharacterSlot0:
                fileName = "CharacterSlot0";
                break;
            case CharacterSlot.CharacterSlot1:
                fileName = "CharacterSlot1";
                break;
            case CharacterSlot.CharacterSlot2:
                fileName = "CharacterSlot2";
                break;
            case CharacterSlot.CharacterSlot3:
                fileName = "CharacterSlot3";
                break;
            case CharacterSlot.CharacterSlot4:
                fileName = "CharacterSlot4";
                break;
            case CharacterSlot.CharacterSlot5:
                fileName = "CharacterSlot5";
                break;
            case CharacterSlot.CharacterSlot6:
                fileName = "CharacterSlot6";
                break;
            case CharacterSlot.CharacterSlot7:
                fileName = "CharacterSlot7";
                break;
            case CharacterSlot.CharacterSlot8:
                fileName = "CharacterSlot8";
                break;
            case CharacterSlot.CharacterSlot9:
                fileName = "CharacterSlot9";
                break;
            default:
                break;
        }
        return fileName;
    }

    public void CreateNewGame() {
        //TEMP
        player.playerNetworkManager.vitality.Value = 9;
        player.playerNetworkManager.endurance.Value = 9;

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot0);
        if (!saveFileDataWriter.CheckToSeeIfFileExists()) {
            currentCharacterSlot = CharacterSlot.CharacterSlot0;
            currentCharacterData = new CharacterSaveData();
            StartNewGame();
            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot1);
        if (!saveFileDataWriter.CheckToSeeIfFileExists()) {
            currentCharacterSlot = CharacterSlot.CharacterSlot1;
            currentCharacterData = new CharacterSaveData();
            StartNewGame();
            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot2);
        if (!saveFileDataWriter.CheckToSeeIfFileExists()) {
            currentCharacterSlot = CharacterSlot.CharacterSlot2;
            currentCharacterData = new CharacterSaveData();
            StartNewGame();
            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot3);
        if (!saveFileDataWriter.CheckToSeeIfFileExists()) {
            currentCharacterSlot = CharacterSlot.CharacterSlot3;
            currentCharacterData = new CharacterSaveData();
            StartNewGame();
            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot4);
        if (!saveFileDataWriter.CheckToSeeIfFileExists()) {
            currentCharacterSlot = CharacterSlot.CharacterSlot4;
            currentCharacterData = new CharacterSaveData();
            StartNewGame();
            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot5);
        if (!saveFileDataWriter.CheckToSeeIfFileExists()) {
            currentCharacterSlot = CharacterSlot.CharacterSlot5;
            currentCharacterData = new CharacterSaveData();
            StartNewGame();
            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot6);
        if (!saveFileDataWriter.CheckToSeeIfFileExists()) {
            currentCharacterSlot = CharacterSlot.CharacterSlot6;
            currentCharacterData = new CharacterSaveData();
            StartNewGame();
            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot7);
        if (!saveFileDataWriter.CheckToSeeIfFileExists()) {
            currentCharacterSlot = CharacterSlot.CharacterSlot7;
            currentCharacterData = new CharacterSaveData();
            StartNewGame();
            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot8);
        if (!saveFileDataWriter.CheckToSeeIfFileExists()) {
            currentCharacterSlot = CharacterSlot.CharacterSlot8;
            currentCharacterData = new CharacterSaveData();
            StartNewGame();
            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot9);
        if (!saveFileDataWriter.CheckToSeeIfFileExists()) {
            currentCharacterSlot = CharacterSlot.CharacterSlot9;
            currentCharacterData = new CharacterSaveData();
            StartNewGame();
            return;
        }

        //Notify Player to delete files if full
        TitleScreenManager.singleton.DisplayNoFreeCharacterSlots();
    }

    public void StartNewGame() {
        SaveGame();
        StartCoroutine(WorldSaveGameManager.singleton.LoadWorld());
    }

    public void LoadGame() {
        fileName = DecideCharacterFileName(currentCharacterSlot);

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = fileName;
        currentCharacterData = saveFileDataWriter.LoadSaveFile();

        StartCoroutine(LoadWorld());
    }

    public void DeleteGame(CharacterSlot characterSlot) {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = DecideCharacterFileName(characterSlot);

        saveFileDataWriter.DeleteSaveFile();    
    }

    void LoadAllCharacterProfiles() {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        
        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot0);
        characterSlot0 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot1);
        characterSlot1 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot2);
        characterSlot2 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot3);
        characterSlot3 = saveFileDataWriter.LoadSaveFile();
        DeleteGame(CharacterSlot.CharacterSlot0);

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot4);
        characterSlot4 = saveFileDataWriter.LoadSaveFile();
        DeleteGame(CharacterSlot.CharacterSlot0);

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot5);
        characterSlot5 = saveFileDataWriter.LoadSaveFile();
        DeleteGame(CharacterSlot.CharacterSlot0);

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot6);
        characterSlot6 = saveFileDataWriter.LoadSaveFile();
        DeleteGame(CharacterSlot.CharacterSlot0);

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot7);
        characterSlot7 = saveFileDataWriter.LoadSaveFile();
        DeleteGame(CharacterSlot.CharacterSlot0);

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot8);
        characterSlot8 = saveFileDataWriter.LoadSaveFile();
        DeleteGame(CharacterSlot.CharacterSlot0);

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot9);
        characterSlot9 = saveFileDataWriter.LoadSaveFile();
        DeleteGame(CharacterSlot.CharacterSlot0);
    }

    public void SaveGame() {
        fileName  = DecideCharacterFileName(currentCharacterSlot);

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = fileName;
        //TODO: SAVE CHARACTER DATA
        player.SavePlayerGame(ref currentCharacterData);
        saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
    }

    public IEnumerator LoadWorld() {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
        //AsyncOperation loadOperation = SceneManager.LoadSceneAsync(currentCharacterData.sceneIndex); //custom scene
        player.LoadPlayerGame(ref currentCharacterData);
        yield return null;
    }

    public int GetWorldSceneIndex() {
        return worldSceneIndex;
    }
}
