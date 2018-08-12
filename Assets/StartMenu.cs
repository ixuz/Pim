using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour {

    public FadeScreen fadeScreen;
    public string gameScene;

    public void StartGame() {
        fadeScreen.LoadScene(gameScene);
    }

    public void QuitApplication() {
        Application.Quit();
    }
}
