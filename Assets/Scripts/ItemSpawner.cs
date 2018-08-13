using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    [Header("General properties")]
    public GameObject itemPrefab;
    public float spawnRate = 1.0f;

    public PickupPoint[] pickupPoints;
    public Level[] levels;
    public ItemType[] itemTypes;

    [Header("Gizmos")]
    public Sprite icon;

    private float spawnCooldown = 0.0f;
    private int currentLevelIndex = 0;
    private int currentItemIndex = -1;

    // Update is called once per frame
    void Update () {
        if (spawnCooldown < 0) {
            if (Game.instance.GetLevel() < Game.instance.GetLevelCount()) {
                spawnCooldown = 1 / Game.instance.GetCurrentLevelData().itemSpawnRate;

                GameObject itemObj = SpawnItem();
            }
        }

        spawnCooldown -= Time.deltaTime;
	}

    GameObject SpawnItem() {
        /*
        currentItemIndex++;

        if (currentLevelIndex >= levels.Length) {
            Debug.LogWarning("Can't spawn item for level " + currentLevelIndex + ". Doesn't exist!");
            return null;
        }
        if (currentItemIndex >= levels[currentLevelIndex].itemTypesToSpawn.Length) {
            Debug.LogWarning("Can't spawn item index (" + currentItemIndex + ") for level " + currentLevelIndex + ". Doesn't exist!");
            return null;
        }
        */
        ItemType[] selectFromTypes = Game.instance.GetCurrentLevelData().itemTypes;
        if (selectFromTypes.Length > 0) {
            GameObject itemInstance = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            Item item = itemInstance.GetComponent<Item>();
            if (item) {
                //ItemType itemType = levels[currentLevelIndex].itemTypesToSpawn[currentItemIndex];
                ItemType itemType = selectFromTypes[Random.Range(0, selectFromTypes.Length)];

                item.SetItemType(itemType);
                StartCoroutine(SlideItemToPickupPoint(itemInstance, 0));
            }

            if (pickupPoints.Length == 0) {
                Debug.LogWarning("Warning no pickup points assigned to the ItemSpawner");
                return itemInstance;
            }

            return itemInstance;
        }
        return null;
    }

    public int GetCurrentLevelIndex() {
        return currentLevelIndex;
    }

    public int GetLevelCount() {
        return levels.Length;
    }

    public Level GetCurrentLevel() {
        return levels[GetCurrentLevelIndex()];
    }

    void OnNewLevelEvent(int levelIndex) {
        currentLevelIndex = levelIndex;
        currentItemIndex = -1;
}

    void OnEnable() {
        Game.OnNewLevelEvent += OnNewLevelEvent;
    }

    void OnDisable() {
        Game.OnNewLevelEvent -= OnNewLevelEvent;
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

                pickupPoint.AddItem(item);

                item = null;
                AudioManager.instance.PlaySfx("Blip_Select17");
                EZCameraShake.CameraShaker.Instance.ShakeOnce(0.2f, 5.0f, 0.2f, 0.2f);
                break;
            } else {
                if (item.transform.position.x >= pickupPoint.transform.position.x - 1) {
                    if (pickupPoint.GetItem() != null) {

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
        public int targetScore;
        public ItemType[] itemTypesToSpawn;
    }
}
