using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {

    public AudioMixer audioMixer;
    public Sound[] sounds;
    public string initialBgm;

    public static AudioManager instance;

    Sound currentBgmSound;
    AudioSource audioSource;

    void Awake() {

        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        string outputMixer = "Sfx";
        
        foreach (Sound sound in sounds) {
            
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(outputMixer)[0];
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
        }
	}

    void Start() {
        if (initialBgm != "") {
            PlayBgm(initialBgm);
        }
    }

    public void PlaySfx(string soundName) {
        Sound sound = Array.Find(sounds, s => s.name == soundName);
        if (sound == null) {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        sound.audioSource.Play();
    }

    public void PlaySfx(AudioClip audioClip) {
        Sound sound = Array.Find(sounds, s => s.audioClip == audioClip);
        if (sound == null) {
            Debug.LogWarning("Sound: " + audioClip + " not found!");
            return;
        }
        sound.audioSource.Play();
    }

    public void PlaySfx(AudioClip audioClip, Vector3 position) {
        GameObject go = new GameObject();
        AudioSource goAudioSource = go.AddComponent<AudioSource>();
        goAudioSource.spatialBlend = 0.7f;

        go.transform.position = position;

        Sound sound = Array.Find(sounds, s => s.audioClip == audioClip);
        if (sound == null) {
            Debug.LogWarning("Sound: " + audioClip + " not found!");
            return;
        }

        string outputMixer = "Sfx";
        goAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(outputMixer)[0];
        goAudioSource.clip = sound.audioClip;
        goAudioSource.volume = sound.volume;
        goAudioSource.pitch = sound.pitch;
        goAudioSource.loop = sound.loop;

        goAudioSource.Play();
        Destroy(go, sound.audioClip.length);
    }

    public void PlayBgm(string soundName) {
        Sound sound = Array.Find(sounds, s => s.name == soundName);
        if (sound == null) {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }

        audioSource.clip = sound.audioClip;
        audioSource.volume = sound.volume;
        audioSource.pitch = sound.pitch;
        audioSource.loop = sound.loop;
        audioSource.Play();
        currentBgmSound = sound;
    }

    public string GetCurrentBgm() {
        return currentBgmSound.name;
    }
}
