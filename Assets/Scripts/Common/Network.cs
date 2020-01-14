using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

struct UserID
{
    public string id;
    public string username;
}
struct UserScore
{
    public string id;
    public int score;
}
public class Network : MonoBehaviour
{
    const string serverAddr = "myfishserverok.herokuapp.com";
    public static Network Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void GetServerID(string username, Action success, Action fail)
    {
        StartCoroutine(GetServerIDCoroutine(username, success, fail));
    }
    IEnumerator GetServerIDCoroutine(string username, Action success, Action fail)
    {
        UnityWebRequest www = UnityWebRequest.Get(serverAddr + "/users/new/" + username);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            fail();
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            string result = www.downloadHandler.text;
            UserID resultObj = JsonUtility.FromJson<UserID>(result);
            //ID 저장
            PlayerPrefs.SetString("id", resultObj.id);
            PlayerPrefs.SetString("username", resultObj.username);
            success();

        }
    }

    public void UpdateBestScore(string id, int bestScore)
    {
        StartCoroutine(UpdateBestScoreCoroutine(id, bestScore));
    }
    IEnumerator UpdateBestScoreCoroutine(string id, int bestScore)
    {
        UserScore userScore = new UserScore { id = id, score = bestScore };

        string postData = JsonUtility.ToJson(userScore);

        UnityWebRequest www = UnityWebRequest.Put(serverAddr + "/score/add", postData);

        www.SetRequestHeader("Content-Type", "application/json");
        www.method = "POST";

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }
}
