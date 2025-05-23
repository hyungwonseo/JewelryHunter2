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

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;
    }

    // Update is called once per frame
    void Update()
    {
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
        // 그라운드에 서있는지 판정하는 코드 1
        //onGround = Physics2D.Linecast(transform.position,
        //    transform.position - (transform.up * 0.1f),
        //    groundLayer);
        // 그라운드에 서있는지 판정하는 코드 2
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
        onGround = hit.collider != null;

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
    }

    public void GameOver()
    {
        animator.Play(deadAnime);
    }
}
