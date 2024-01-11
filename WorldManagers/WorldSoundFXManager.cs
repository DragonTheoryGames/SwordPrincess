using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour {

    public static WorldSoundFXManager singleton;

    [Header("DamageSFX")]
    public AudioClip[] physicalDamageSFX; 

    [Header("ActionSFX")]
    public AudioClip rollSFX;
    
    private void Awake() {
        if(singleton == null) {singleton = this;}
        else Destroy(gameObject);
    }

    private void Start() {
        DontDestroyOnLoad(this);
    }

    public AudioClip RandomSFX(AudioClip[] audioArray) {
        int index = Random.Range(0, audioArray.Length);
        return audioArray[index];
    }
}
