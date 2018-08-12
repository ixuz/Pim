using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipes : MonoBehaviour {

    public Recipe[] recipes;

    public List<Recipe> FindAllRecipesWithItem(ItemType itemType) {
        List<Recipe> recipeList = new List<Recipe>();

        foreach (Recipe recipe in recipes) {
            if (recipe.inputA == itemType || recipe.inputB == itemType || recipe.output == itemType) {
                recipeList.Add(recipe);
            }
        }

        return recipeList;
    }

    public Recipe GetRecipe(ItemType a, ItemType b) {

        foreach (Recipe recipe in recipes) {
            if (recipe.CheckInput(a, b)) {
                return recipe;
            }
        }

        Recipe noRecipe = new Recipe();
        return noRecipe;
    }

    [System.Serializable]
    public struct Recipe {
        public ItemType inputA;
        public ItemType inputB;
        public ItemType output;

        public bool CheckInput(ItemType a, ItemType b) {
            if (inputA == a && inputB == b) { return true; }
            if (inputA == b && inputB == a) { return true; }
            return false;
        }
    }
    
}
