using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    [Header("General properties")]
    public GameObject itemPrefab;
    public float spawnRate = 1.0f;

    public PickupPoint[] pickupPoints;
    public Level[] levels;

    [Header("Gizmos")]
    public Sprite icon;

    private float spawnCooldown = 0.0f;
    private int currentLevelIndex = 0;
    private int currentItemIndex = -1;


    // Update is called once per frame
    void Update () {
        if (spawnCooldown < 0) {
            spawnCooldown = 1/spawnRate;

            GameObject itemObj = SpawnItem();
        }

        spawnCooldown -= Time.deltaTime;
	}

    GameObject SpawnItem() {

        currentItemIndex++;

        if (currentLevelIndex >= levels.Length) {
            Debug.LogWarning("Can't spawn item for level " + currentLevelIndex + ". Doesn't exist!");
            return null;
        }
        if (currentItemIndex >= levels[currentLevelIndex].itemTypesToSpawn.Length) {
            Debug.LogWarning("Can't spawn item index (" + currentItemIndex + ") for level " + currentLevelIndex + ". Doesn't exist!");
            return null;
        }

        GameObject itemInstance = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Item item = itemInstance.GetComponent<Item>();
        if (item) {
            ItemType itemType = levels[currentLevelIndex].itemTypesToSpawn[currentItemIndex];

            item.SetItemType(itemType);
        }

        if (pickupPoints.Length == 0) {
            Debug.LogWarning("Warning no pickup points assigned to the ItemSpawner");
            return itemInstance;
        }

        StartCoroutine(SlideItemToPickupPoint(itemInstance, 0));

        return itemInstance;
    }

    void OnDrawGizmos() {
        if (icon) {
            Gizmos.DrawIcon(transform.position, icon.name);
        }
    }

    IEnumerator SlideItemToPickupPoint(GameObject item, int pickupPointIndex) {
        while (true) {
            if (item == null) {
                break;
            }
            if (pickupPointIndex >= pickupPoints.Length) break;

            PickupPoint pickupPoint = pickupPoints[pickupPointIndex];

            if (item.transform.position.x >= pickupPoint.transform.position.x) {
                // The current Item have reached the first pickupPoint!
                Debug.Log("Yay!");

                pickupPoint.AddItem(item);

                item = null;
                break;
            } else {
                if (item.transform.position.x >= pickupPoint.transform.position.x - 1) {
                    if (pickupPoint.GetItem() != null) {
                        Debug.Log("Push!");

                        int nextPickupPointIndex = pickupPointIndex+1;
                        StartCoroutine(SlideItemToPickupPoint(pickupPoint.GetItem(), nextPickupPointIndex));

                        pickupPoint.RemoveItem();
                    }
                }

                Vector3 currentItemPos = item.transform.position;
                currentItemPos.x += Time.deltaTime;
                item.transform.position = currentItemPos;
            }

            // Yield execution of this coroutine and return to the main loop until next frame
            yield return null;
        }
    }

    [System.Serializable]
    public struct Level {
        public ItemType[] itemTypesToSpawn;
    }
}
