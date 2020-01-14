using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameReadyPanel : GamePanel
{
    [SerializeField] Animator logoAnimator;
    [SerializeField] Animator touchAnimator;
    [SerializeField] Text bestScoreText;
    [SerializeField] GameObject bestScoreObj;

    public override void Close()
    {
        logoAnimator.SetTrigger("hide");
        touchAnimator.SetTrigger("hide");
        bestScoreObj.SetActive(false);
    }
    private void Start()
    {
        GameManager.Instance.LoadScore();
        bestScoreText.text = GameManager.Instance.bestScore.ToString();
    }

    public override void Open()
    {
    //    logoAnimator.SetTrigger("show");
    //    touchAnimator.SetTrigger("show");
    //    bestScoreObj.SetActive(true);
    }


}
