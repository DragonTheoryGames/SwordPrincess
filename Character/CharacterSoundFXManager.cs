using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour {

    private AudioSource audioSource;

    protected virtual void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip sfx, float volume = 1, bool randomizePitch = true, float randomPitch = .1f) {
        if (randomizePitch) {
            audioSource.pitch += Random.Range(-randomPitch, randomPitch);
        }
        
        audioSource.PlayOneShot(sfx, volume);
        audioSource.pitch = 1; //RESET PITCH
    }

    public void PlayRollSoundFX() {
        audioSource.PlayOneShot(WorldSoundFXManager.singleton.rollSFX);
    }

}
