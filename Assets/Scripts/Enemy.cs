using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Transform player;
    private Animator anim;
    public float attackDistance=20;
    public float moveSpeed = 5;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < attackDistance)//玩家进入攻击距离
        {
            //攻击
            anim.SetBool("isRun", true);
            //敌人朝向
            if (player.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            //敌人移动
            Vector3 dis = player.position - transform.position;
            transform.position = transform.position + dis.normalized * moveSpeed * Time.deltaTime;
        }
        else
        {
            anim.SetBool("isRun", false);
        }
	}
}
