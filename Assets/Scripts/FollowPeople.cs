using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPeople : MonoBehaviour {

    public Transform people;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos.x = people.position.x;
        pos.y = people.position.y;
        transform.position = pos;
    }
}
