using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIButton : MonoBehaviour
{
    public TMP_Text text;
    Button button;
    [SerializeField] int buttonIndex;
    [SerializeField] int playerClick;

    void Start()
    {
        button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(this.PlayerClick);
    }
    public void PlayerClick()
    {
        switch(GameManager.instance.player)
        {
            case 0:
                text.text = "X";
                playerClick = 0;
                GameManager.instance.ButtonPressed(buttonIndex);
                break;
            case 1:
                text.text = "O";
                playerClick = 1;
                GameManager.instance.ButtonPressed(buttonIndex);
                break;
        }
    }

    public void ButtonInteraction(bool toggle)
    {
        button.interactable = toggle;
    }

    public void Reset()
    {
        ButtonInteraction(true);
        text.text = "";
    }
}
