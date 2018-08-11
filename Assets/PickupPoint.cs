using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPoint : MonoBehaviour {

    private GameObject pickupItemInstance;

    [Header("Gizmos")]
    public Sprite unoccupiedIcon;
    public Sprite occupiedIcon;

    public bool AddItem(GameObject itemPrefab) {
        if (pickupItemInstance != null) {
            return false;
        }

        GameObject go = Instantiate(itemPrefab);
        return true;
    }

    public bool RemoveItem() {
        if (pickupItemInstance != null) {
            Destroy(pickupItemInstance);
            pickupItemInstance = null;
            return true;
        }

        return false;
    }

    void OnDrawGizmos() {
        if (unoccupiedIcon) {
            Gizmos.DrawIcon(transform.position, unoccupiedIcon.name);
        }
        if (occupiedIcon) {
            Gizmos.DrawIcon(transform.position, occupiedIcon.name);
        }
    }
}
