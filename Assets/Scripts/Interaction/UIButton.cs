using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIButton : MonoBehaviour
{
    [Tooltip("Square Text reference")] public TMP_Text text;
    [Tooltip("Square reference")] Button button;
    [Tooltip("Index of Button in the 3x3 Grid")] [SerializeField] int buttonIndex;
    [Tooltip("Bool to verify if square has been clicked by a player")] [SerializeField] bool clicked = false;

    //Start Function, sets up OnClick listener
    void Start()
    {
        button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(this.PlayerClick);
    }

    //Listened event for player click. 
    //Will return out if square has been selected before, if not, will set the correct X or O and will update GameManager
    public void PlayerClick()
    {

        SoundManager.instance.PlaySound("click");
        
        if(clicked)
            return;

        switch(GameManager.instance.player)
        {
            case 1:
                text.text = "X";
                clicked = true;
                GameManager.instance.ButtonPressed(buttonIndex);
                break;
            case 2:
                text.text = "O";
                clicked = true;
                GameManager.instance.ButtonPressed(buttonIndex);
                break;
        }
    }

    //Sets the Square to be clickable
    public void InteractionToggle(bool toggle)
    {
        button.interactable = toggle;
    }

    //Resets square between games
    public void Reset()
    {
        text.text = "";
        clicked = false;
    }
}
