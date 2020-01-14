using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanel : GamePanel
{
    [SerializeField] Text scoreText;

    public override void Close()
    {
        scoreText.gameObject.SetActive(false);
    }

    public override void Open()
    {
        scoreText.gameObject.SetActive(true);
    }
}
