using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Fish fish;
    [SerializeField] GameObject pipes;

    [SerializeField] GamePanel gameReadyPanel;
    [SerializeField] GamePanel gamePlayPanel;
    [SerializeField] GamePanel gameOverPanel;
    [SerializeField] CreateUsernamePanel createUsernamePanel;

    public int score;
    public int bestScore;

#if UNITY_ANDROID       //안드로이드면 살리고 아니면 무시한다.
    string gameId = "3382788";
#elif UNITY_IOS
    string gameId = "3382789";
#endif
    
    string placementId = "Banner";
    string serverId;

    public static GameManager Instance;

    void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    enum State
    {
        NONE, READY, PLAY, OVER
    }
    State state = State.NONE;


    void Start()
    {
        GameReady();
        Advertisement.Initialize(gameId, true);
        StartCoroutine(ShowBannerWhenReady());

        //플레이어 아이디 확인
        serverId = PlayerPrefs.GetString("id");
        if (serverId == "")
        {
            //유저명 입력창 표시
            createUsernamePanel.Open(() =>
            {
                state = State.READY;
            });
        }
        else
        {
            state = State.READY;
        }
    }

    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(placementId))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(placementId);
    }

    void Update()
    {
        
        switch (state)
        {
            case State.READY:
                if (Input.GetButtonDown("Fire1"))
                {
                    GameStart();
                }
                break;
            case State.PLAY:
                if (fish.IsDead) GameOver();
                break;
            case State.OVER:

                break;
        }
    }

    void GameReady()
    {
        pipes.SetActive(false);
        fish.SetKinematic(true);
    }

    void GameStart()
    {
        state = State.PLAY;

        fish.SetKinematic(false);
        pipes.SetActive(true);

        gameReadyPanel.Close();
        gamePlayPanel.Open();
    }

    void GameOver()
    {
        state = State.OVER;

        ScrollObject[] scrollObjects = GameObject.FindObjectsOfType<ScrollObject>();

        foreach (ScrollObject scrollObject in scrollObjects)
        {
            scrollObject.enabled = false;
        }
        SaveScore();
        gamePlayPanel.Close();
        gameOverPanel.Open();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void OnClickQuit()
    {
        gameOverPanel.Close();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void SaveScore()
    {
        if (bestScore < score)
        {
            bestScore = score;
            PlayerPrefs.SetInt("bestscore", bestScore);

            //서버에 베스트 스코어 등록
            string id = PlayerPrefs.GetString("id");
            if (id != "")
            {
                Network.Instance.UpdateBestScore(id, score);
            }
        }
    }
    public void LoadScore()
    {
        bestScore = PlayerPrefs.GetInt("bestscore", 0);
    }
}
