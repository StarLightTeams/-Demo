using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPeople : MonoBehaviour {

    public Transform people;

    public Vector2 vector = new Vector2(-86.6f, 240f);

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(people.position.x, vector.x,vector.y);
        //pos.x = people.position.x;
        pos.y = people.position.y;
        transform.position = pos;
    }
}
