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
    public bool forceIgnoreDropEffectOnNextPickupPoint = false;

    public GameObject dropEffect;

    [Header("Gizmos")]
    public Sprite unoccupiedIcon;
    public Sprite occupiedIcon;

    public bool CanPickupHere() { return canPickupHere; }
    public bool CanDropHere() { return canDropHere; }

    // Event declaration
    public static event PickupPointRecievedItemEvent OnPickupPointRecievedItemEvent;
    public delegate void PickupPointRecievedItemEvent(PickupPoint pickupPoint, Item item);

    public bool AddItem(GameObject itemObj, bool forceIgnoreDropEffect) {

        if (itemObj == null) return false;
        if (GetItem() != null) return false;

        itemObj.transform.SetParent(transform);
        itemObj.transform.SetPositionAndRotation(transform.position + itemOffset, Quaternion.identity);
        itemObj.transform.localScale = Vector3.one;
        pickupItemInstance = itemObj;

        Item item = itemObj.GetComponent<Item>();
        OnPickupPointRecievedItemEvent(this, item);

        if (dropEffect && !forceIgnoreDropEffect) {
            GameObject dropEffectInstance = Instantiate(dropEffect, itemObj.transform.position, Quaternion.identity);
            dropEffectInstance.transform.SetParent(itemObj.transform);
        }

        if (isOutput) {
            RemoveItem();
            StartCoroutine(SlideToPosition(itemObj, outputPosition.position));
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

                GetItem().GetComponent<Item>().SetItemType(recipe.output);

                return true;
            } else {
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

    public bool IsOccupied() {
        return pickupItemInstance != null;
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
