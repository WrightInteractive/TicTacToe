using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [SerializeField] int Player = 0;
    [SerializeField] int[] board = {-1, -1, -1, -1, -1, -1, -1, -1, -1};
    [SerializeField] int winner = -1;
    [SerializeField] bool aI = false;
    public int player
    {
        get { return Player; }
    }
    [SerializeField] List<UIButton> buttons = new List<UIButton>();

    void Start()
    {

    }

    void Update()
    {
        switch(Player)
        {
            case 0:
                break;
            case 1:
                if(aI)
                    DumbAI();
                break;
            case 2:
                break;
        }
    }

    void TogglePlayer()
    {
        CheckGame();

        if(Player == 0)
            Player = 1;
        else
            Player = 0;
    }

    public void ButtonPressed(int buttonIndex)
    {
        board[buttonIndex] = Player;
        TogglePlayer();
    }

    void CheckGame()
    {
        if((board[0].Equals(board[1]) && board[0].Equals(board[2]) && !board[0].Equals(-1)) ||
           (board[0].Equals(board[3]) && board[0].Equals(board[6]) && !board[0].Equals(-1)) ||
           (board[0].Equals(board[4]) && board[0].Equals(board[8]) && !board[0].Equals(-1)) ||
           (board[1].Equals(board[4]) && board[1].Equals(board[7]) && !board[1].Equals(-1)) ||
           (board[2].Equals(board[5]) && board[2].Equals(board[8]) && !board[2].Equals(-1)) ||
           (board[2].Equals(board[4]) && board[2].Equals(board[6]) && !board[2].Equals(-1)) ||
           (board[3].Equals(board[4]) && board[3].Equals(board[5]) && !board[3].Equals(-1)) ||
           (board[6].Equals(board[7]) && board[6].Equals(board[8]) && !board[6].Equals(-1)))
           {
                winner = Player;
                Debug.Log("Winner!");
                Player = 2;
                foreach(UIButton buttons in buttons)
                    buttons.ButtonInteraction(false);
           }
        else
        {
            Debug.Log("No winner yet");
        }
    }

    void DumbAI()
    {
        int _index = Random.Range(0, board.Length - 1);

        if(board[_index] != -1)
        {
            DumbAI();
            return;
        }
        else
        {
            buttons[_index].GetComponent<UIButton>().PlayerClick();
        }
    }

/*     int RandomIndex()
    {
        int _index = Random.Range(0, board.Length - 1);

        if(board[_index] != -1)
            return _index;
        else
            RandomIndex();
        
        return -1;
    } */

    void Clear()
    {
        foreach (UIButton button in buttons)
            button.Reset();
    }
}
