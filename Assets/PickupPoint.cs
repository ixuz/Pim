using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPoint : MonoBehaviour {

    [Header("General properties")]
    public GameObject pickupItemInstance;

    public bool canPickupHere = true;
    public bool canDropHere = true;

    [Header("Gizmos")]
    public Sprite unoccupiedIcon;
    public Sprite occupiedIcon;

    public bool CanPickupHere() { return canPickupHere; }
    public bool CanDropHere() { return canDropHere; }

    public bool AddItem(GameObject item) {

        if (item == null) return false;
        if (GetItem() != null) return false;

        item.transform.SetParent(transform);
        item.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        item.transform.localScale = Vector3.one;
        pickupItemInstance = item;

        return true;
    }

    public bool MergeItem(GameObject item) {
        if (item == null) return false;
        if (GetItem() != null) {

            GameObject recipesObj = GameObject.FindGameObjectWithTag("Recipes");
            Recipes recipes = recipesObj.GetComponent<Recipes>();

            ItemType itemTypeA = GetItem().GetComponent<Item>().itemType;
            ItemType itemTypeB = item.GetComponent<Item>().itemType;

            Recipes.Recipe recipe = recipes.GetRecipe(itemTypeA, itemTypeB);
            if (recipe.output != null) {
                Debug.Log("This is a valid recipe! Output: " + recipe.output);

                GetItem().GetComponent<Item>().SetItemType(recipe.output);

                return true;
            } else {
                Debug.Log("This is not a valid recipe! Output: " + recipe.output);
                return false;
            }
        }
        return false;
    }

    public bool RemoveItem() {
        if (pickupItemInstance != null) {
            pickupItemInstance = null;
            return true;
        }

        return false;
    }

    public GameObject GetItem() {
        return pickupItemInstance;
    }

    void OnDrawGizmos() {
        if (pickupItemInstance == null && unoccupiedIcon) {
            Gizmos.DrawIcon(transform.position, unoccupiedIcon.name);
        }
        if (pickupItemInstance != null && occupiedIcon) {
            Gizmos.DrawIcon(transform.position, occupiedIcon.name);
        }
    }
}
