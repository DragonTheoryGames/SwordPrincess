using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveFileDataWriter {
    
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    public bool CheckToSeeIfFileExists() {
        return File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)) ? true : false;
    }

    public void DeleteSaveFile() {
        File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
    }

    public void CreateNewCharacterSaveFile(CharacterSaveData characterSaveData) {
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName); //Make Save Path

        try { //Create Directory to Save File
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("CreatingSave File @" + savePath);

            //Serialize the C# Game Data into JSON
            string dataToStore = JsonUtility.ToJson(characterSaveData, true);

            //Write File to Computer
            using (FileStream stream = new FileStream(savePath, FileMode.Create)) {
                using (StreamWriter fileWriter = new StreamWriter(stream)) {
                    fileWriter.Write(dataToStore);
                }
            }
        }
        catch (Exception ex) {Debug.LogError("SAVE GAME ERROR:" + savePath + "/n" + ex); }
    }

    public CharacterSaveData LoadSaveFile() {
        
        CharacterSaveData characterData = null;
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName); //Find Load Path

        if(File.Exists(loadPath)) {
            try {
                string dataToLoad = "";

                using(FileStream stream = new FileStream(loadPath, FileMode.Open)) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //Deserialize the data
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
            catch (Exception ex) {Debug.Log("FILE IS BLANK" + ex);}
        }

        return characterData;   
    }
}
