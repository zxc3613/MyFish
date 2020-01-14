using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CreateUsernamePanel : GamePanel
{
    [SerializeField] Animator popupAnimator;
    [SerializeField] InputField nameInputField;
    [SerializeField] Button okButton;

    Action closeAction;

    public void OnClickOKButton()
    {
        string username = nameInputField.text;

        if (nameInputField.text != "")
        {
            //TODO: 서버에 아이디 요청하기.
            okButton.interactable = false;
            nameInputField.interactable = false;
            Network.Instance.GetServerID(username,
                () =>
            {
                Close();
            },
                () =>
            {
                okButton.interactable = true;
                nameInputField.interactable = true;
                nameInputField.text = "";
            });
        }
    }

    public override void Close()
    {
        popupAnimator.SetTrigger("hide");
    }

    public override void Open()
    {
        popupAnimator.SetTrigger("show");
    }

    public void Open(Action didFinished)
    {
        gameObject.SetActive(true);
        popupAnimator.SetTrigger("show");
        closeAction = didFinished;
    }

    public void Finish()
    {
        closeAction();
        gameObject.SetActive(false);
    }
}
