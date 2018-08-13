using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopStats : MonoBehaviour {

    public Animator animator;

    void OnEnable() {
        Game.OnNewLevelEvent += NewLevelEventEffect;
    }

    void OnDisable() {
        Game.OnNewLevelEvent -= NewLevelEventEffect;
    }

    void NewLevelEventEffect(int levelIndex) {
        if (levelIndex == Game.instance.GetLevelCount()) {
            return;
        }
        if (levelIndex == 0) return;

        animator.SetTrigger("replay");
    }
}
