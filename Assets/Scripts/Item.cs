using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public SpriteRenderer spriteRenderer;

    public ItemType itemType;

    // Event declaration
    public static event ItemReachedOutputEvent OnItemReachedOutputEvent;
    public delegate void ItemReachedOutputEvent(Item item, ItemType itemType);

    // Use this for initialization
    void Start () {
        SetItemType(itemType);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable() {
        OnItemReachedOutputEvent += ItemReachedOutput;
    }

    void OnDisable() {
        OnItemReachedOutputEvent -= ItemReachedOutput;
    }

    public void SetItemType(ItemType itemType) {
        this.itemType = itemType;
        spriteRenderer.sprite = itemType.sprite;
    }

    public static void TriggerItemReachedOutputEvent(Item item, ItemType itemType) {
        OnItemReachedOutputEvent(item, itemType);
    }

    public void ItemReachedOutput(Item item, ItemType itemType) {
        if (item == this) {
            EZCameraShake.CameraShaker.Instance.ShakeOnce(0.4f, 5.0f, 0.4f, 0.4f);
            AudioManager.instance.PlaySfx("Blip_Select14");
            Game.instance.ChangeScore(itemType.value);
        }
    }
}
