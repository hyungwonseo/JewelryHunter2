using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float leftLimit = 0.0f;      // 왼쪽 스크롤 제한
    public float rightLimit = 0.0f;     // 오른쪽 스크롤 제한 
    public float topLimit = 0.0f;       // 위 스크롤 제한 
    public float bottomLimit = 0.0f;    // 아래 스크롤 제한

    public GameObject subScreen;

    public bool isForceScrollX = false;     // X 축 강제 스크롤 플래그
    public float forceScrollSpeedX = 0.5f;  // 1초간 움직일 X의 거리
    public bool isForceScrollY = false;     // Y 축 강제 스크롤 플래그
    public float forceScrollSpeedY = 0.5f;  // 1초간 움직일 Y의 거리

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.deltaTime);

        GameObject player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 찾기
        if (player != null)
        {
            // 카메라의 좌표 갱신
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = transform.position.z;
            // 가로 방향 동기
            if (isForceScrollX)
            {
                // 가로 강제 스크롤
                x = transform.position.x + (forceScrollSpeedX * Time.deltaTime);
            }
            // 양 끝에 이동 제한 적용
            if (x < leftLimit)
            {
                x = leftLimit;
            }
            else if (x > rightLimit)
            {
                x = rightLimit;
            }
            // 세로 방향 동기
            if (isForceScrollY)
            {
                // 세로 강제 스크롤
                y = transform.position.y + (forceScrollSpeedY * Time.deltaTime);
            }
            // 위 아래에 이동 제한 적용
            if (y < bottomLimit)
            {
                y = bottomLimit;
            }
            else if (y > topLimit)
            {
                y = topLimit;
            }
            // 카메라 위치의 Vector3 만들기
            Vector3 v3 = new Vector3(x, y, z);
            transform.position = v3;

            if (subScreen != null)
            {
                y = subScreen.transform.position.y;
                z = subScreen.transform.position.z;
                subScreen.transform.position = new Vector3(x / 2.0f, y, z);
            }
        }
    }
}
