using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;        // �̹����� ��Ƶδ� GameObject
    public Sprite gameOverSpr;          // GAME OVER �̹���
    public Sprite gameClearSpr;         // GAME CLEAR �̹���
    public GameObject panel;            // �г�
    public GameObject restartButton;    // RESTART ��ư
    public GameObject nextButton;       // NEXT ��ư
    Image titleImage;                   // �̹����� ǥ���ϰ��ִ� Image ������Ʈ

    // Start is called before the first frame update
    void Start()
    {
        // �̹��� �����
        Invoke("InactiveImage", 1.0f);
        // ��ư(�г�)�� �����
        panel.SetActive(false);

        // ��ư �̺�Ʈ ���
        restartButton.GetComponent<Button>().onClick.AddListener(HandleRestartButton);
        nextButton.GetComponent<Button>().onClick.AddListener(HandleNextButton);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == "gameclear")
        {
            // ���� Ŭ����
            mainImage.SetActive(true); // �̹��� ǥ��
            panel.SetActive(true); // ��ư(�г�)�� ǥ��
            // RESTART ��ư ��ȿȭ 
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameClearSpr;
            PlayerController.gameState = "gameend";
        }
        else if (PlayerController.gameState == "gameover")
        {
            // ���� ����
            mainImage.SetActive(true);      // �̹��� ǥ��
            panel.SetActive(true);          // ��ư(�г�)�� ǥ��
            // NEXT ��ư ��Ȱ��
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";
        }
        else if (PlayerController.gameState == "playing")
        {
            // DO NOTHING!!!
        }
    }

    private void OnDestroy()
    {
        // ��ư �̺�Ʈ ��� ����
        restartButton.GetComponent<Button>().onClick.RemoveListener(HandleRestartButton);
        nextButton.GetComponent<Button>().onClick.RemoveListener(HandleNextButton);
    }

    // �̹��� �����
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    void HandleRestartButton()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
        Debug.Log("Restart scene : " + currentScene);
    }

    void HandleNextButton()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int lastScene = SceneManager.sceneCountInBuildSettings - 1;
        if (currentScene == lastScene)
        {
            Debug.Log("���� ���������� ������ ���������Դϴ�.");
            SceneManager.LoadScene(0);
        }else
        {
            SceneManager.LoadScene(currentScene + 1);
        }
    }
}
