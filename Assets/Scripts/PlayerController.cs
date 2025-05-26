using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;
    float axisH = 0.0f;
    public float speed = 3.0f;

    public float jump = 9.0f;
    public LayerMask groundLayer;
    bool goJump = false;
    bool onGround = false;

    Animator animator;
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";

    public static string gameState = "Playing";

    // 이중점프 - 시작
    int jumpCount = 0;             // 현재 점프 횟수
    public int maxJumpCount = 2;   // 최대 점프 횟수 (2단 점프)
    // 이중점프 - 끝

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;

        gameState = "Playing";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != "Playing")
        {
            return;
        }

        axisH = Input.GetAxisRaw("Horizontal");
        if (axisH > 0.0f)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    // 
    private void FixedUpdate()
    {
        if (gameState != "Playing")
        {
            return;
        }

        // 그라운드에 서있는지 판정하는 코드 1
        //onGround = Physics2D.Linecast(transform.position,
        //    transform.position - (transform.up * 0.1f),
        //    groundLayer);
        // 그라운드에 서있는지 판정하는 코드 2
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
        onGround = hit.collider != null;

        // 이중점프 - 시작
        if (onGround)
        {
            jumpCount = 0;  // 착지 시 점프 횟수 초기화
        }
        // 이중점프 - 끝

        if (onGround || axisH != 0)
        {
            rbody.velocity = new Vector2(axisH * speed, rbody.velocity.y);
            // y의 값은 여기서 조정하지않고 다른곳에서 적용하는 y벡터값을 그대로 사용하라는 뜻
            // y는 기존값 유지!!
        }

        if (onGround && goJump)
        {
            rbody.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
            goJump = false;
        }
        // 이중점프 - 시작
        //if (goJump)
        //{
        //    rbody.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
        //    goJump = false;
        //}
        // 이중점프 - 끝

        if (onGround)
        {
            if (axisH == 0)
            {
                nowAnime = stopAnime;
            }else
            {
                nowAnime = moveAnime;
            }
        }
        else
        {
            nowAnime = jumpAnime;
        }

        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);
        }
    }
    
    public void Jump()
    {
        goJump = true;

        // 이중점프 - 시작
        //jumpCount++;  // 점프 횟수 증가
        //if (jumpCount < maxJumpCount)
        //{
        //    goJump = true;            
        //}
        // 이중점프 - 끝
    }

    // collider의 isTrigger 속성을 사용할 경우 발생하는 이벤트 함수
    // isTrigger는 충돌을 감지만하고 이동을 막지는 않음
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal();
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();
        }
    }

    // collider의 isTrigger 속성을 사용하지 않을 경우(디폴트) 발생하는 이벤트 함수
    // 충돌 감지와 더불어 이동을 막음(= 벽처럼 동작)
    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("OnCollisionEnter2D 충돌 이벤트 발생");
    //    if (collision.gameObject.tag == "Goal")
    //    {            
    //        Goal();
    //    }
    //    else if (collision.gameObject.tag == "Dead")
    //    {
    //        GameOver();
    //    }
    //}

    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear";
        GameStop();
    }

    public void GameOver()
    {
        gameState = "gameover";
        rbody.velocity = Vector2.zero;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        GetComponent<CapsuleCollider2D>().enabled = false;
        animator.Play(deadAnime);
    }

    public void GameStop()
    {
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.velocity = Vector2.zero;
    }
}
