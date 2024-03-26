using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class WorldAIManager : MonoBehaviour {
    
    public static WorldAIManager singleton;

    [Header("DEBUG")]
    [SerializeField] bool despawnCharacters = false;
    [SerializeField] bool respawnCharacters = false;

    [Header("Characters")]
    [SerializeField] GameObject[] aiCharacters;
    [SerializeField] List<GameObject> spawnedCharacters;

    void Awake() {
        if (singleton == null) {singleton = this;}
        else Destroy(gameObject);
    }

    void Start() {
        if(NetworkManager.Singleton.IsServer) {
            StartCoroutine(WaitForSceneToLoadSpawnCharacters());
        }
    }

    void Update() {
        if (respawnCharacters) {
            respawnCharacters = false;
            SpawnAllCharacters();
        }
        if (despawnCharacters) {
            despawnCharacters = false;
            DespawnAllCharacters();
        }

    }

    IEnumerator WaitForSceneToLoadSpawnCharacters() {
        while (!SceneManager.GetActiveScene().isLoaded) {
            yield return null;
        }
        SpawnAllCharacters();
    }

    void SpawnAllCharacters() {
        foreach (var character in aiCharacters) {
            GameObject instantiatedCharacters = Instantiate(character);
            instantiatedCharacters.GetComponent<NetworkObject>().Spawn();
            spawnedCharacters.Add(instantiatedCharacters);
        }
    }

    void DespawnAllCharacters() {
        foreach (var character in spawnedCharacters) {
            character.GetComponent<NetworkObject>().Despawn();
        }
    }

    void DisableAllCharacters() {
        //SET FLAG AND SYNC WITH NETWORK
        //DISABLE GAMEOBJECTS
        //
    }
}
