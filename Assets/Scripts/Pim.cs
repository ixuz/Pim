using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pim : MonoBehaviour {

    public Animator animator;
    public Transform itemHolder;
    public GameObject currentItem;

    public float movementSmoothing = 1.0f;

    public float speed = 2f;

    private Vector2 currentVelocity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // Handle inputs
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Vector2 desiredVelocity = input * speed;

        Vector2 actualVelocity = Vector2.Lerp(currentVelocity, desiredVelocity, 1/movementSmoothing * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space)) {
            Interact();
        }


        // Update position of player
        Vector2 position = transform.position;
        position += actualVelocity * Time.deltaTime;
        transform.position = position;

        currentVelocity = actualVelocity;

        animator.SetFloat("velocity", currentVelocity.magnitude);
        if (currentVelocity.x > 0) {
            animator.SetBool("facing_right", true);
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (currentVelocity.x < 0) {
            animator.SetBool("facing_right", false);
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void Interact() {
        Debug.Log("Try to grab closest item");
        List<GameObject> pickupPoints = GameObject.FindGameObjectsWithTag("PickupPoint").ToList<GameObject>();
        pickupPoints = pickupPoints.OrderBy(x => GetDistanceToObject(x)).ToList();

        foreach (GameObject pickupPointObj in pickupPoints) {
            PickupPoint pickupPoint = pickupPointObj.GetComponent<PickupPoint>();
            if (pickupPoint != null) {
                Collider2D pickupPointCollider = pickupPointObj.GetComponent<Collider2D>();
                if (pickupPointCollider != null) {
                    bool standingOnPickupPoint = pickupPointCollider.OverlapPoint(transform.position);

                    if (standingOnPickupPoint) {
                        Debug.Log("pickupPoint.GetItem(): " + pickupPoint.GetItem());
                        if (pickupPoint.GetItem() != null) {
                            Debug.Log("I can try to pick this item up!");
                            PickupItem(pickupPoint);
                        } else {
                            Debug.Log("I can try to drop an item here!");
                            DropItem(pickupPoint);
                        }
                    }
                }
            }
        }
    }

    bool PickupItem(PickupPoint pickupPoint) {
        if (IsHoldingItem()) return false;
        if (!pickupPoint.GetItem()) return false;

        currentItem = pickupPoint.GetItem();
        pickupPoint.RemoveItem();
        currentItem.transform.SetParent(itemHolder);
        currentItem.transform.SetPositionAndRotation(itemHolder.transform.position, Quaternion.identity);
        currentItem.transform.localScale = Vector3.one;

        return true;
    }

    bool DropItem(PickupPoint pickupPoint) {
        if (!IsHoldingItem()) return false;
        if (pickupPoint == null) return false;
        if (pickupPoint.GetItem()) return false;

        if (pickupPoint.AddItem(currentItem)) {
            currentItem = null;
        }

        return true;
    }

    bool IsHoldingItem() {
        return (currentItem != null);
    }

    float GetDistanceToObject(GameObject go) {
        return (go.transform.position - transform.position).magnitude;
    }
}
