using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICharacterSaveSlot : MonoBehaviour {
    
    SaveFileDataWriter saveFileWriter;

    [Header("Game Slot")]
    public CharacterSlot characterSlot;

    [Header("Character Info")]
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI timePlayed;

    void OnEnable() {
        LoadSaveSlots();
    }

    void LoadSaveSlots() {
        saveFileWriter = new SaveFileDataWriter();
        saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

        switch (characterSlot) {
            case CharacterSlot.CharacterSlot0:
                saveFileWriter.saveFileName = WorldSaveGameManager.singleton.DecideCharacterFileName(characterSlot);
                if (saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.singleton.characterSlot0.characterName;
                    //TODO add timePlayed;
                } 
                else gameObject.SetActive(false); 
                break;
            case CharacterSlot.CharacterSlot1:
                saveFileWriter.saveFileName = WorldSaveGameManager.singleton.DecideCharacterFileName(characterSlot);
                if (saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.singleton.characterSlot1.characterName;
                    //TODO add timePlayed;
                } 
                else gameObject.SetActive(false); 
                break;
            case CharacterSlot.CharacterSlot2:
                saveFileWriter.saveFileName = WorldSaveGameManager.singleton.DecideCharacterFileName(characterSlot);
                if (saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.singleton.characterSlot2.characterName;
                    //TODO add timePlayed;
                } 
                else gameObject.SetActive(false); 
                break;
            case CharacterSlot.CharacterSlot3:
                saveFileWriter.saveFileName = WorldSaveGameManager.singleton.DecideCharacterFileName(characterSlot);
                if (saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.singleton.characterSlot3.characterName;
                    //TODO add timePlayed;
                } 
                else gameObject.SetActive(false); 
                break;
            case CharacterSlot.CharacterSlot4:
                saveFileWriter.saveFileName = WorldSaveGameManager.singleton.DecideCharacterFileName(characterSlot);
                if (saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.singleton.characterSlot4.characterName;
                    //TODO add timePlayed;
                } 
                else gameObject.SetActive(false); 
                break;
            case CharacterSlot.CharacterSlot5:
                saveFileWriter.saveFileName = WorldSaveGameManager.singleton.DecideCharacterFileName(characterSlot);
                if (saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.singleton.characterSlot5.characterName;
                    //TODO add timePlayed;
                } 
                else gameObject.SetActive(false); 
                break;
            case CharacterSlot.CharacterSlot6:
                saveFileWriter.saveFileName = WorldSaveGameManager.singleton.DecideCharacterFileName(characterSlot);
                if (saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.singleton.characterSlot6.characterName;
                    //TODO add timePlayed;
                } 
                else gameObject.SetActive(false); 
                break;
            case CharacterSlot.CharacterSlot7:
                saveFileWriter.saveFileName = WorldSaveGameManager.singleton.DecideCharacterFileName(characterSlot);
                if (saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.singleton.characterSlot7.characterName;
                    //TODO add timePlayed;
                } 
                else gameObject.SetActive(false); 
                break;
            case CharacterSlot.CharacterSlot8:
                saveFileWriter.saveFileName = WorldSaveGameManager.singleton.DecideCharacterFileName(characterSlot);
                if (saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.singleton.characterSlot8.characterName;
                    //TODO add timePlayed;
                } 
                else gameObject.SetActive(false); 
                break;
            case CharacterSlot.CharacterSlot9:
                saveFileWriter.saveFileName = WorldSaveGameManager.singleton.DecideCharacterFileName(characterSlot);
                if (saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.singleton.characterSlot9.characterName;
                    //TODO add timePlayed;
                } 
                else gameObject.SetActive(false); 
                break;
            default:
                break;
        }
    }

    public void LoadGameFromCharacterSlot() {
        WorldSaveGameManager.singleton.currentCharacterSlot = characterSlot;
        WorldSaveGameManager.singleton.LoadGame();
    }

    public void SelectCurrentSlot() {
        TitleScreenManager.singleton.SelectCharacterSlot(characterSlot);
    }
}