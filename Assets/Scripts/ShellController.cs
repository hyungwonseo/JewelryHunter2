using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    public float deleteTime = 3.0f;

    void Start()
    {
        Destroy(gameObject, deleteTime); // ������ ����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("����");
        Destroy(gameObject); // ��� ����
    }
}
