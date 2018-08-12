using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectBeltObjects : MonoBehaviour {

    public Animator animator;
    public Sprite[] sprites;
    public Image itemImage;

    private bool changeSpriteTriggered = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        AnimatorStateInfo animatorStateInfo = animator.GetNextAnimatorStateInfo(0);
        if (!changeSpriteTriggered && animatorStateInfo.IsName("ChangeSprite")) {
            changeSpriteTriggered = true;
            itemImage.sprite = sprites[Random.Range(0, sprites.Length)];
        } else if (changeSpriteTriggered && !animatorStateInfo.IsName("ChangeSprite")) {
            changeSpriteTriggered = false;
        }
    }
}
