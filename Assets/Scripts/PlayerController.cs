using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("目前的水平速度")]
    public float speedX;

    [Header("目前的竖直速度")]
    public float speedY;

    [Header("跳跃的速度")]
    public float jumpSpeed = 500;

    public Rigidbody2D player;

    private Animator anim;

    [Header("是否在奔跑")]
    private bool isRun = false;//是否奔跑状态

    const string HORIZONTAL = "Horizontal";

    [Header("感应地板的距离")]
    [Range(0, 0.5f)]
    public float groundDistance;

    [Header("侦测地板的射线起点")]
    public Transform groundCheck;

    [Header("地面图层")]
    public LayerMask groundLayer;

    [Header("是否在地面")]
    public bool grounded;

    //在玩家底部射出一条很短的射线,如果射线有打到地板图层的话,代表正在踩着地板
    bool isGround
    {
        get
        {
            Vector2 start = groundCheck.position;
            Vector2 end = new Vector2(start.x, start.y - groundDistance);
            //把射线画出来
            Debug.DrawLine(start, end, Color.blue);
            grounded = Physics2D.Linecast(start, end, groundLayer);
            return grounded;
        }
    }

    public bool JumpKey
    {
        get
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }

    [Header("目前的水平方向")]
    public float horizontalDirection;

    [Header("水平方向(-1为左,1为右)")]
    public int turn = 1;//转向

    [Header("水平推力")]
    [Range(0, 200)]
    public float xForce;

    [Header("最大速度")]
    public float maxSpeedX;

    [Header("是否跳跃状态")]
    public bool isJumping;

    [Header("是否是二级跳跃状态")]
    public bool isDoubleJumping = false;//是否是二段跳

    [Header("感应墙的距离")]
    [Range(0, 0.5f)]
    public float wallDistance;

    [Header("侦测墙的射线起点")]
    public Transform wallCheck;

    [Header("墙图层")]
    public LayerMask wallLayer;

    [Header("是否在墙")]
    public bool walled;

    RaycastHit2D hit;

    [Header("跳墙的速度")]
    public float wallSpeed = 70f;

    [Header("是否是贴墙跳")]
    public bool isWallJump;

    bool isWall
    {
        get
        {

            hit = Physics2D.Linecast(wallCheck.position, new Vector2(wallCheck.position.x + transform.localScale.x * wallDistance, wallCheck.position.y), wallLayer);
            walled = hit.collider != null ? true : false;
            return walled;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x +  transform.localScale.x * wallDistance, wallCheck.position.y));

    }

    void Start () {
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update() {
            MoveMentX();
        wallJump();
        ControllSpeed();
            TryJump();

        animOperate();
        ////获得玩家的速度
        //Vector2 velocity = player.velocity;

        ////检测按键(方向键左右)
        //float horizontal = Input.GetAxis("Horizontal");
        ////检测按键(方向键上)
        ////float vertical = Input.GetAxis("Vertical");

        ////玩家左右移动
        //player.AddForce(Vector2.right * horizontal * moveSpeed * 10);
        ////玩家的跳跃
        //if (Input.GetKeyDown(KeyCode.Space))//在地面,并且按了方向键上
        //{
        //    if (!isJumping)
        //    {
        //        velocity.y = jumpSpeed;
        //        velocity.x = player.velocity.x;
        //        player.velocity = velocity;
        //        isJumping = true;
        //    }
        //     else  
        //    {
        //        if (isDoubleJumping)//判断是否在二段跳    
        //        {
        //            return;//否则不能二段跳    
        //        }
        //        else
        //        {
        //            isDoubleJumping = true;
        //            velocity.y = jumpSpeed;
        //            velocity.x = player.velocity.x;
        //            player.velocity = velocity;
        //        }
        //    }
        //}
        //anim.SetFloat("vertical", player.velocity.y);

        ////设置玩家状态为--奔跑
        //anim.SetBool("isRun", JudgePlayerMove(horizontal));

        ////设置玩家朝向
        //transform.localScale = JudgePlayerTurn(horizontal);

        //print("groundFlag=" + groundFlag);

    }

    public void wallJump()
    {
        if (isWall && !isGround && Input.GetKeyDown(KeyCode.Space)&& !isDoubleJumping)
        {
            //print("jjjjj=" + hit.normal.x + ",,,,,,,,,,,,=" + hit.normal.y);
            //playerRigidbody2D.velocity = new Vector2(0, 0);
            print("wallspeed=" + wallSpeed);
            player.velocity = new Vector2(wallSpeed * hit.normal.x, 80f);

            isWallJump = true;
            //wallCheck.position = new Vector3(turn*wallCheck.position.x, wallCheck.position.y, wallCheck.position.z);
            turn = -1 * turn; 
            transform.localScale = new Vector3(turn, 1, 1);

            //print("jump" + transform.localScale.x);
        }
    }

    /// <summary>
    /// 动画操作
    /// </summary>
    void animOperate()
    {
        anim.SetBool("isGround", grounded);
        anim.SetBool("isRun", JudgePlayerMove(horizontalDirection));
        anim.SetFloat("vertical",player.velocity.y);
        anim.SetBool("isWall", walled && !grounded);
    }

    /// <summary>
    /// 控制速度
    /// </summary>
    public void ControllSpeed()
    {
        speedX = player.velocity.x;
        speedY = player.velocity.y;
        float newSpeedX = Mathf.Clamp(speedX, -maxSpeedX, maxSpeedX);
        player.velocity = new Vector2(newSpeedX, speedY);
    }

    /// <summary>
    /// 水平移动
    /// </summary>
    void MoveMentX()
    {
        horizontalDirection = Input.GetAxis(HORIZONTAL);
        player.AddForce(new Vector2(xForce * horizontalDirection, 0));
        if (!isWallJump)
        {

            transform.localScale = JudgePlayerTurn(horizontalDirection);
        }
       
    }

    /// <summary>
    /// 跳跃和二段跳
    /// </summary>
    void TryJump()
    {
        if (JumpKey)
        {
            if (!isJumping)
            {
                player.AddForce(Vector2.up * jumpSpeed*10);
                isJumping = true;
            }
            else
            {
                if (JumpKey)
                {
                    if (isDoubleJumping || isWallJump)//判断是否在二段跳    
                    {
                        return;//否则不能二段跳    
                    }
                    //else if (walled) //判断是否贴墙,即是否可以贴墙跳
                    //{
                    //    print("wallspeed=" + wallSpeed);
                    //    player.velocity = new Vector2(wallSpeed * hit.normal.x, 80f);

                    //    isWallJump = true;
                    //    //wallCheck.position = new Vector3(turn*wallCheck.position.x, wallCheck.position.y, wallCheck.position.z);
                    //    transform.localScale = new Vector3(-turn, 1, 1);
                    //}
                    else
                    {
                        isDoubleJumping = true;
                        player.AddForce(Vector2.up * jumpSpeed*10);
                    }
                }
            }
        }
        if (isJumping)
        {
            grounded = false;
        }
    }



    //发生碰撞
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (isGround)
        {
            isJumping = false;
            isDoubleJumping = false;
            isWallJump = false;
        }
        //if (walled)
        //{
        //    isWallJump = false;
        //}
        //if(col.collider.tag == "Background" )//碰撞
        //{
        //    isGround = true;
        //    groundFlag++;
        //    isJumping = false;
        //    isDoubleJumping = false;
        //}
        //else if (col.collider.tag == "Wall")
        //{
        //    isWall = true;
        //    player.velocity = Vector2.zero;
        //    player.gravityScale = 5;
        //}
        ////设置玩家状态--在地面
        //anim.SetBool("isGround", isGround);
        ////设置玩家状态--在墙面
        //anim.SetBool("isWall", isWall);
    }

    //碰撞离开
    public void OnCollisionExit2D(Collision2D col)
    {
        //if(col.collider.tag == "Background" && groundFlag==1)
        //{
        //    isGround = false;
        //    groundFlag--;
        //}
        //else if(col.collider.tag == "Background" && groundFlag != 1)
        //{
        //    groundFlag--;
        //}
        //else if (col.collider.tag == "Wall")
        //{
        //    isWall = false;
        //    //player.gravityScale = 15;
        //}

        ////设置玩家状态--在地面
        //anim.SetBool("isGround", isGround);
        ////设置玩家状态--在墙面
        //anim.SetBool("isWall", isWall);
    }

    /// <summary>
    /// 对撞器的范围的偏移量
    /// </summary>
    /// <param name="isRun"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 对撞器的范围
    /// </summary>
    /// <param name="isRun"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 判断玩家的转向
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 判断玩家移动
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
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
