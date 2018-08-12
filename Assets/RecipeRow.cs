using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeRow : MonoBehaviour {

    public Image itemInputA;
    public Image itemInputB;
    public Image itemOutput;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetItems(ItemType inputA, ItemType inputB, ItemType output) {
        itemInputA.sprite = inputA.sprite;
        itemInputB.sprite = inputB.sprite;
        itemOutput.sprite = output.sprite;
    }
}
