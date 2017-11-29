using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed=250;
    public float jumpSpeed = 100;
    public Rigidbody2D player;
    private Animator anim;
    //private BoxCollider2D playerCollider;
    private bool isRun = false;//是否奔跑状态
    private bool isGround = false;//是否在地面状态
    private int groundFlag = 0;
    private bool isWall = false;//是否在墙状态
    private int turn = 1;//转向
    private float jumpTime = 10;
    private bool isJumping = false;//是否在跳跃
    private bool isDoubleJumping = false;//是否是二段跳

	// Use this for initialization
	void Start () {
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //playerCollider = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update() {

        //获得玩家的速度
        Vector2 velocity = player.velocity;

        //检测按键(方向键左右)
        float horizontal = Input.GetAxis("Horizontal");
        //检测按键(方向键上)
        //float vertical = Input.GetAxis("Vertical");

        //玩家左右移动
        player.AddForce(Vector2.right * horizontal * moveSpeed * 10);
        //玩家的跳跃
        if (Input.GetKeyDown(KeyCode.Space))//在地面,并且按了方向键上
        {
            if (!isJumping)
            {
                velocity.y = jumpSpeed;
                velocity.x = player.velocity.x;
                player.velocity = velocity;
                isJumping = true;
            }
             else  
            {
                if (isDoubleJumping)//判断是否在二段跳    
                {
                    return;//否则不能二段跳    
                }
                else
                {
                    isDoubleJumping = true;
                    velocity.y = jumpSpeed;
                    velocity.x = player.velocity.x;
                    player.velocity = velocity;
                }
            }
        }
        anim.SetFloat("vertical", player.velocity.y);

        //设置玩家状态为--奔跑
        anim.SetBool("isRun", JudgePlayerMove(horizontal));

        //设置玩家朝向
        transform.localScale = JudgePlayerTurn(horizontal);

        print("groundFlag=" + groundFlag);
       
    }

    //发生碰撞
    public void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.tag == "Background" )//碰撞
        {
            isGround = true;
            groundFlag++;
            isJumping = false;
            isDoubleJumping = false;
        }
        //else if(col.collider.tag == "Wall")
        //{
        //    isWall = true;
        //player.velocity = Vector2.zero;
        //player.gravityScale = 5;
        //}
        //设置玩家状态--在地面
        anim.SetBool("isGround", isGround);
        ////设置玩家状态--在墙面
        //anim.SetBool("isWall", isWall);
    }

    //碰撞离开
    public void OnCollisionExit2D(Collision2D col)
    {
        if(col.collider.tag == "Background" && groundFlag==1)
        {
            isGround = false;
            groundFlag--;
        }
        else if(col.collider.tag == "Background" && groundFlag != 1)
        {
            groundFlag--;
        }

        //else if (col.collider.tag == "Wall")
        //{
        //    isWall = false;
        //    //player.gravityScale = 15;
        //}

        //设置玩家状态--在地面
        anim.SetBool("isGround", isGround);
        ////设置玩家状态--在墙面
        //anim.SetBool("isWall", isWall);
    }

    //对撞器的范围的偏移量
    Vector2 getPlayerColliderOffset(bool isRun)
    {
        Vector2 colOffset;
        if (isRun)
        {
            colOffset = new Vector2(0.23f,-0.74f);
        }
        else
        {
            colOffset = new Vector2(0.13f, -0.32f);
        }
        return colOffset;
    }

    //对撞器的范围
    Vector2 getPlayerColliderSize(bool isRun)
    {
        Vector2 colSize;
        if (isRun)
        {
            colSize = new Vector2(3.44f,2.49f);
        }
        else
        {
            colSize = new Vector2(1.97f, 3.35f);
        }
        return colSize;
    }

    //判断玩家的转向
    Vector3 JudgePlayerTurn(float x)
    {
        if (x > 0.05)
        {
            turn = 1;//朝右方向
        }
        else if(x<-0.05)
        {
            turn =-1;//朝左方向
        }
        
        return new Vector3(turn, 1, 1);
    }

    //判断玩家移动
    bool JudgePlayerMove(float x)
    {
        if (x > 0.05 || x < -0.05)
        {
            isRun = true;
        }
        else
        {
            isRun = false;
        }
        return isRun;
    }
}
