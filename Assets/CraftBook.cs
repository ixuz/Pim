using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftBook : MonoBehaviour {

    public Recipes recipes;
    public GameObject recipeRowTemplatePrefab;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LookupRecipesForItem(ItemType itemType) {
        List<Recipes.Recipe> recipeList = recipes.FindAllRecipesWithItem(itemType);

        foreach (Recipes.Recipe recipe in recipeList) {
            AddRow(recipe.inputA, recipe.inputB, recipe.output);
        }
    }

    void AddRow(ItemType inputA, ItemType inputB, ItemType output) {
        GameObject go = Instantiate(recipeRowTemplatePrefab, transform.position, Quaternion.identity);
        go.transform.SetParent(transform);

        RecipeRow recipeRow = go.GetComponent<RecipeRow>();
        recipeRow.SetItems(inputA, inputB, output);
        recipeRow.transform.localScale = Vector3.one;
    }

    void OnPimPickedUpItemEvent(Pim pim, Item item) {
        Debug.Log("Refresh CraftBook");

        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        LookupRecipesForItem(item.itemType);
    }
    void OnPimDroppedItemEvent(Pim pim, Item item) {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    void OnEnable() {
        Pim.OnPimPickedUpItemEvent += OnPimPickedUpItemEvent;
        Pim.OnPimDroppedItemEvent += OnPimDroppedItemEvent;
    }

    void OnDisable() {
        Pim.OnPimPickedUpItemEvent -= OnPimPickedUpItemEvent;
        Pim.OnPimDroppedItemEvent -= OnPimDroppedItemEvent;
    }
}
