using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBook : MonoBehaviour {

    public Text timerValue;
    public Text scoreValue;
    public Text targetScoreValue;
    public Text levelText;

    private float levelTimer = 0.0f;

    void Start() {
        timerValue.text = "12345";
        scoreValue.text = "54321";
        targetScoreValue.text = "99999";
        levelText.text = "Level  " + (Game.instance.GetLevel()+1);
    }

    void Update() {
        levelTimer += Time.deltaTime;

        timerValue.text = Mathf.Floor(levelTimer / 60).ToString("00") + ":" + Mathf.Floor(levelTimer % 60).ToString("00");
    }

    void OnEnable() {
        Game.OnChangeScoreEvent += OnChangeScoreEvent;
    }

    void OnDisable() {
        Game.OnChangeScoreEvent -= OnChangeScoreEvent;
    }

    void OnChangeScoreEvent(int amount, int newTotalScore) {
        scoreValue.text = newTotalScore.ToString();
    }
}
