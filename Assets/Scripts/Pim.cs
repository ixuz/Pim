using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pim : MonoBehaviour {

    public Transform itemHolder;

    public float speed = 2f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // Handle inputs
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Vector2 velocity = input * speed;

        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space)) {
            grabClosestItem();
        }

        // Update position of player
        Vector2 position = transform.position;
        position += velocity * Time.deltaTime;
        transform.position = position;
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

        /*foreach (GameObject go in gos) {
            Debug.Log("Distance to closest item:" + GetDistanceToObject(go));
        }*/
    }

    float GetDistanceToObject(GameObject go) {
        return (go.transform.position - transform.position).magnitude;
    }
}
