using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScreen : MonoBehaviour {

    public static FadeScreen instance;
    public Animator animator;
    public bool ignoreSplash = false;

    void Awake() {

        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        animator.SetBool("ignoreSplash", ignoreSplash);
    }

    public void FadeIn() {
        animator.SetBool("fadeIn", true);
    }

    public void FadeOut() {
        animator.SetBool("fadeIn", false);
    }

    public void LoadScene(string sceneName) {
        StartCoroutine(TransitionToScene(sceneName));
    }

    IEnumerator TransitionToScene(string sceneName) {
        FadeOut();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(0.25f);
        FadeIn();
    }
}
