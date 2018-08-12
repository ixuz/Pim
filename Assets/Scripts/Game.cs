using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Game : MonoBehaviour {

    public static Game instance;
    public AudioMixer audioMixer;
    public ItemSpawner itemSpawner;

    public Level[] levels;

    public GameObject newLevelEffect;

    private int score = 0;
    private int currentLevel = 0;

    // Event declaration
    public static event ChangeScoreEvent OnChangeScoreEvent;
    public delegate void ChangeScoreEvent(int amount, int newTotalScore);
    public static event ClearScoreEvent OnClearScoreEvent;
    public delegate void ClearScoreEvent(int newTotalScore);
    public static event NewLevelEvent OnNewLevelEvent;
    public delegate void NewLevelEvent(int levelIndex);

    void Awake() {

        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        OnNewLevelEvent(0);
    }

    public void LoadLevel(int levelIndex) {

    }

    public Level GetCurrentLevelData() {
        return levels[currentLevel];
    }

    public Level GetLevelData(int index) {
        return levels[index];
    }

    public int GetLevelCount() {
        return levels.Length;
    }

    public int GetLevel() {
        return currentLevel;
    }

    public int GetLevelTargetScore() {
        if (itemSpawner.GetCurrentLevelIndex() < itemSpawner.GetLevelCount()) {
            return itemSpawner.GetCurrentLevel().targetScore;
        } else {
            return 0;
        }
    }

    public void AddScore(int amount) {
        score += amount;
    }

    public void ChangeScore(int amount) {
        score += amount;
        OnChangeScoreEvent(amount, score);

        if (currentLevel < GetLevelCount()) {
            if (score >= Game.instance.GetCurrentLevelData().targetScore) {
                currentLevel++;
                OnNewLevelEvent(currentLevel);
                ClearScore();
            }
        }
        
    }

    public int GetScore() {
        return score;
    }

    public void ClearScore() {
        score = 0;
        OnClearScoreEvent(score);
    }

    public void QuitApplication() {
        Application.Quit();
    }

    public void SetMasterVolume(float volume) {
        audioMixer.SetFloat("Master", volume);
    }

    public void SetBgmVolume(float volume) {
        audioMixer.SetFloat("Bgm", volume);
    }

    public void SetSfxVolume(float volume) {
        audioMixer.SetFloat("Sfx", volume);
    }

    public void PauseGame() {
        if (Time.timeScale != 0) { Time.timeScale = 0; }
        else { Time.timeScale = 1; }
    }

    void OnEnable() {
        OnChangeScoreEvent += delegate { };
        OnClearScoreEvent += delegate { };
        OnNewLevelEvent += NewLevelEventEffect;
    }

    void OnDisable() {
        OnChangeScoreEvent -= delegate { };
        OnClearScoreEvent -= delegate { };
        OnNewLevelEvent -= NewLevelEventEffect;
    }

    void NewLevelEventEffect(int levelIndex) {
        if (levelIndex == 0) return;
        Instantiate(newLevelEffect, Vector3.zero, Quaternion.identity);
    }

    [System.Serializable]
    public struct Level {
        public int targetScore;
        public int startTimer;
        public float itemSpawnRate;
        public ItemType[] itemTypes;
    }
}
