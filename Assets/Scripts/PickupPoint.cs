using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPoint : MonoBehaviour {

    [Header("General properties")]
    public GameObject pickupItemInstance;
    public Vector3 itemOffset;

    public bool canPickupHere = true;
    public bool canDropHere = true;
    public bool isOutput = false;
    public Transform outputPosition;

    [Header("Gizmos")]
    public Sprite unoccupiedIcon;
    public Sprite occupiedIcon;

    public bool CanPickupHere() { return canPickupHere; }
    public bool CanDropHere() { return canDropHere; }

    public bool AddItem(GameObject item) {

        if (item == null) return false;
        if (GetItem() != null) return false;

        item.transform.SetParent(transform);
        item.transform.SetPositionAndRotation(transform.position + itemOffset, Quaternion.identity);
        item.transform.localScale = Vector3.one;
        pickupItemInstance = item;

        if (isOutput) {
            RemoveItem();
            StartCoroutine(SlideToPosition(item, outputPosition.position));
        }

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

    IEnumerator SlideToPosition(GameObject go, Vector3 position) {

        Item item = go.GetComponent<Item>();

        while (true) {

            if (go.transform.position.x >= position.x) {
                // The current Item have reached the first pickupPoint!
                Debug.Log("Output sent!");
                Item.TriggerItemReachedOutputEvent(item, item.itemType);
                Destroy(go);

                break;
            } else {
                Vector3 currentItemPos = go.transform.position;
                currentItemPos.x += Time.deltaTime;
                go.transform.position = currentItemPos;
            }

            yield return null;
        }

        // Yield execution of this coroutine and return to the main loop until next frame
        yield return null;
    }
}
