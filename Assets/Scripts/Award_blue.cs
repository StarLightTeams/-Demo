using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Award_blue : MonoBehaviour {

    public float rotateSpeed = 100;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(-Vector3.forward * Time.deltaTime * rotateSpeed);
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            //加分数
            //放音效
            AudioManager._instance.GetAwardBlueCollectible();
            //销毁自己
            Destroy(this.gameObject);
        }
    }
}
