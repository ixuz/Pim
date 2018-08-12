using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    public static Game instance;
    public ItemSpawner itemSpawner;

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

        if (itemSpawner.GetCurrentLevelIndex() < itemSpawner.GetLevelCount()) {
            if (score >= itemSpawner.GetCurrentLevel().targetScore) {
                OnNewLevelEvent(itemSpawner.GetCurrentLevelIndex() + 1);
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

    void OnEnable() {
        OnChangeScoreEvent += delegate { };
        OnClearScoreEvent += delegate { };
        OnNewLevelEvent += delegate { };
    }

    void OnDisable() {
        OnChangeScoreEvent -= delegate { };
        OnClearScoreEvent -= delegate { };
        OnNewLevelEvent -= delegate { };
    }
}
