using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pim : MonoBehaviour {

    public Animator animator;
    public Transform itemHolder;
    public Item currentItem;

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
            //grabClosestItem();
        
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

    public void grabClosestItem() {
        Debug.Log("Try to grab closest item");

        List<GameObject> gos = GameObject.FindGameObjectsWithTag("Item").ToList<GameObject>();
        if (gos.Count() == 0) {
            return;
        }

        gos = gos.OrderBy(x => GetDistanceToObject(x)).ToList();

        if (gos.Count > 0) {
            GameObject closestObject = gos[0];
            Item closestItem = closestObject.GetComponent<Item>();
            Debug.Log("Distance to closest closestObject:" + GetDistanceToObject(closestObject));
        }
    }

    float GetDistanceToObject(GameObject go) {
        return (go.transform.position - transform.position).magnitude;
    }
}
