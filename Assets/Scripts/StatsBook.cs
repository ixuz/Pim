using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBook : MonoBehaviour {

    public FadeScreen fadeScreen;
    public Text timerValue;
    public Text scoreValue;
    public Text targetScoreValue;
    public Text levelText;

    private float levelTimer = 0.0f;
    private bool gameover = false;

    void Start() {
        gameover = false;
        levelTimer = Game.instance.GetLevelData(0).startTimer;
        timerValue.text = "12345";
        scoreValue.text = "0";
        targetScoreValue.text = "99999";
        levelText.text = "Level 1";
    }

    void Update() {
        levelTimer -= Time.deltaTime;


        if (levelTimer < 0) {
            levelTimer = 0;
            if (gameover == false) {
                gameover = true;
                fadeScreen.LoadScene("GameOver");
            }
        }
        timerValue.text = Mathf.Floor(levelTimer / 60).ToString("00") + ":" + Mathf.Floor(levelTimer % 60).ToString("00");
    }

    void OnEnable() {
        Game.OnChangeScoreEvent += OnChangeScoreEvent;
        Game.OnClearScoreEvent += OnClearScoreEvent;
        Game.OnNewLevelEvent += OnNewLevelEvent;
        Item.OnItemReachedOutputEvent += OnItemReachedOutputEvent;
    }

    void OnDisable() {
        Game.OnChangeScoreEvent -= OnChangeScoreEvent;
        Game.OnClearScoreEvent -= OnClearScoreEvent;
        Game.OnNewLevelEvent -= OnNewLevelEvent;
        Item.OnItemReachedOutputEvent -= OnItemReachedOutputEvent;
    }

    void OnChangeScoreEvent(int amount, int newTotalScore) {
        scoreValue.text = newTotalScore.ToString();
    }

    void OnClearScoreEvent(int newTotalScore) {
        scoreValue.text = newTotalScore.ToString();
    }

    void OnNewLevelEvent(int levelIndex) {
        if (Game.instance.GetLevel() < Game.instance.GetLevelCount()) {
            targetScoreValue.text = Game.instance.GetLevelData(levelIndex).targetScore.ToString();
            levelText.text = "Level  " + (levelIndex + 1);
            levelTimer = Game.instance.GetLevelData(levelIndex).startTimer;
        }
    }

    void OnItemReachedOutputEvent(Item item, ItemType itemType) {
        levelTimer += itemType.timeGain;
    }
}
