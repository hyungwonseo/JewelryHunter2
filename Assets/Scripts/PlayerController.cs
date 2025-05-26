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

    // �������� - ����
    int jumpCount = 0;             // ���� ���� Ƚ��
    public int maxJumpCount = 2;   // �ִ� ���� Ƚ�� (2�� ����)
    // �������� - ��

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

        // �׶��忡 ���ִ��� �����ϴ� �ڵ� 1
        //onGround = Physics2D.Linecast(transform.position,
        //    transform.position - (transform.up * 0.1f),
        //    groundLayer);
        // �׶��忡 ���ִ��� �����ϴ� �ڵ� 2
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
        onGround = hit.collider != null;

        // �������� - ����
        if (onGround)
        {
            jumpCount = 0;  // ���� �� ���� Ƚ�� �ʱ�ȭ
        }
        // �������� - ��

        if (onGround || axisH != 0)
        {
            rbody.velocity = new Vector2(axisH * speed, rbody.velocity.y);
            // y�� ���� ���⼭ ���������ʰ� �ٸ������� �����ϴ� y���Ͱ��� �״�� ����϶�� ��
            // y�� ������ ����!!
        }

        if (onGround && goJump)
        {
            rbody.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
            goJump = false;
        }
        // �������� - ����
        //if (goJump)
        //{
        //    rbody.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
        //    goJump = false;
        //}
        // �������� - ��

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

        // �������� - ����
        //jumpCount++;  // ���� Ƚ�� ����
        //if (jumpCount < maxJumpCount)
        //{
        //    goJump = true;            
        //}
        // �������� - ��
    }

    // collider�� isTrigger �Ӽ��� ����� ��� �߻��ϴ� �̺�Ʈ �Լ�
    // isTrigger�� �浹�� �������ϰ� �̵��� ������ ����
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

    // collider�� isTrigger �Ӽ��� ������� ���� ���(����Ʈ) �߻��ϴ� �̺�Ʈ �Լ�
    // �浹 ������ ���Ҿ� �̵��� ����(= ��ó�� ����)
    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("OnCollisionEnter2D �浹 �̺�Ʈ �߻�");
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
