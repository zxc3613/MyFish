using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameOverPanel : GamePanel, IUnityAdsListener
{
    [SerializeField] Animator popupAnimator;
    [SerializeField] Button adsButton;
    [SerializeField] Text bestScoreText;
    [SerializeField] Text currentScoreText;

#if UNITY_ANDROID       //안드로이드면 살리고 아니면 무시한다.
    string gameId = "3382788";
#elif UNITY_IOS
    string gameId = "3382789";
#endif

    string myPlacementId = "rewardedVideo";

    private void Start()
    {
        adsButton.interactable = Advertisement.IsReady(myPlacementId);
        Advertisement.AddListener(this);
    }

    public void ShowRewardedViedeo()
    {
        Advertisement.Show(myPlacementId);
    }

    public override void Close()
    {
        popupAnimator.SetTrigger("hide");
    }

    public override void Open()
    {
        popupAnimator.SetTrigger("show");
        bestScoreText.text = GameManager.Instance.bestScore.ToString();
        currentScoreText.text = GameManager.Instance.score.ToString();
    }

    public void Finish()
    {
        GameManager.Instance.ReloadScene();
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("레디");
        if (placementId == myPlacementId)
        {
            adsButton.interactable = true;
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("OnUbityAdsDidError: " + message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("OnUbityAdsDidError: " + placementId);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log("OnUbityAdsDidError: " + placementId + ", " + showResult);
        if(showResult == ShowResult.Finished)
        {
            Debug.Log("d");
        }
    }
}
