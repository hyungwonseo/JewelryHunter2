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

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
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
        // �׶��忡 ���ִ��� �����ϴ� �ڵ� 1
        //onGround = Physics2D.Linecast(transform.position,
        //    transform.position - (transform.up * 0.1f),
        //    groundLayer);
        // �׶��忡 ���ִ��� �����ϴ� �ڵ� 2
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
        onGround = hit.collider != null;

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
    }
    
    public void Jump()
    {
        goJump = true;
    }
}
