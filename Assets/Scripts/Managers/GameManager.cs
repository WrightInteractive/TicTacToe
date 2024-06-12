using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [SerializeField] int Player = 0;
    [SerializeField] int[] board = {-1, -1, -1, -1, -1, -1, -1, -1, -1};
    [SerializeField] int winner = -1;
    [SerializeField] bool aI = false;
    [SerializeField] bool smartAI = false;
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
                    if(smartAI)
                        SmartAI();
                    else
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
        }
    }

    void DumbAI()
    {
        Debug.Log("DumbAI");
        int _index = Random.Range(0, board.Length - 1);

        if(board[_index] != -1)
            DumbAI();
        else
            buttons[_index].PlayerClick();
    }

    void SmartAI()
    {

        // Check for win
        if(LeftCrossCheck(true) != -1)
            buttons[LeftCrossCheck(true)].PlayerClick();
        else if(RightCrossCheck(true) != -1)
            buttons[RightCrossCheck(true)].PlayerClick(); 
        else if(HorizontalCheck(0, true) != -1)
            buttons[HorizontalCheck(0, true)].PlayerClick();   
        else if(HorizontalCheck(3, true) != -1)
            buttons[HorizontalCheck(3, true)].PlayerClick();   
        else if(HorizontalCheck(6, true) != -1)
            buttons[HorizontalCheck(6, true)].PlayerClick();  
        else if(VerticalCheck(0, true) != -1)
            buttons[VerticalCheck(0, true)].PlayerClick();   
        else if(VerticalCheck(1, true) != -1)
            buttons[VerticalCheck(1, true)].PlayerClick();   
        else if(VerticalCheck(2, true) != -1)
            buttons[VerticalCheck(2, true)].PlayerClick();  

        // Check for Block
        else if(LeftCrossCheck(false) != -1)
            buttons[LeftCrossCheck(false)].PlayerClick();
        else if(RightCrossCheck(false) != -1)
            buttons[RightCrossCheck(false)].PlayerClick();   
        else if(HorizontalCheck(0, false) != -1)
            buttons[HorizontalCheck(0, false)].PlayerClick();   
        else if(HorizontalCheck(3, false) != -1)
            buttons[HorizontalCheck(3, false)].PlayerClick();   
        else if(HorizontalCheck(6, false) != -1)
            buttons[HorizontalCheck(6, false)].PlayerClick();   
        else if(VerticalCheck(0, false) != -1)
            buttons[VerticalCheck(0, false)].PlayerClick();   
        else if(VerticalCheck(1, false) != -1)
            buttons[VerticalCheck(1, false)].PlayerClick();   
        else if(VerticalCheck(2, false) != -1)
            buttons[VerticalCheck(2, false)].PlayerClick();   
        else
            DumbAI();
    }

    bool EmptyCheck(int index)
    {
        if(board[index] == -1)
            return true;
        else
            return false;
    }

    bool WinCheck(int index1, int index2)
    {
        if(board[index1]  == 1 && board[index2] == 1)
            return true;
        
        return false;
    }

    int HorizontalCheck(int index, bool win)
    {
        Debug.Log("Horizontal: " + index + " | " + win);
        int _ret;
 
            if(board[index].Equals(board[index + 1]) && EmptyCheck(index + 2))
            {
                _ret = index + 2;

                if(win)
                {
                    if(WinCheck(index, index + 1))
                        return _ret;
                    else
                        return -1;
                }
                
                return _ret;
            }
            else if (board[index + 1].Equals(board[index + 2]) && EmptyCheck(index))
            {
                _ret = index;

                if(win)
                {
                    if(WinCheck(index + 1, index + 2))
                        return _ret;
                    else
                        return -1;
                }
                
                return _ret;
            }
            else if (board[index].Equals(board[index + 2]) && EmptyCheck(index + 1))
            {
                _ret = index + 1;

                if(win)
                {
                    if(WinCheck(index, index + 2))
                        return _ret;
                    else
                        return -1;
                }
                
                return _ret;
            }
            else
                return -1;
    }

    int VerticalCheck(int index, bool win)
    {
        Debug.Log("VerticalCheck: " + index + " | " + win);
        int _ret;

        if(board[index].Equals(board[index + 3]) && EmptyCheck(index + 6))
        {
                _ret = index + 6;

                if(win)
                {
                    if(WinCheck(index, index + 3))
                        return _ret;
                    else
                        return -1;
                }
                
                return _ret;
            }
        else if (board[index + 3].Equals(board[index + 6]) && EmptyCheck(index))
        {
                _ret = index ;

                if(win)
                {
                    if(WinCheck(index + 3, index + 6))
                        return _ret;
                    else
                        return -1;
                }
                
                return _ret;
            }
        else if (board[index].Equals(board[index + 6]) && EmptyCheck(index + 3))
        {
                _ret = index + 3;

                if(win)
                {
                    if(WinCheck(index, index + 6))
                        return _ret;
                    else
                        return -1;
                }
                
                return _ret;
            }
        else
            return -1;

    }

    int LeftCrossCheck(bool win)
    {
        Debug.Log("LeftCrossCheck: " + win);
        int _ret;

        if(board[0].Equals(board[4]) && EmptyCheck(8))
        {
                _ret = 8;

                if(win)
                {
                    if(WinCheck(0, 4))
                        return _ret;
                    else
                        return -1;
                }
                
                return _ret;
            }
        else if (board[0].Equals(board[8]) && EmptyCheck(4))
        {
                _ret = 4;

                if(win)
                {
                    if(WinCheck(0, 8))
                        return _ret;
                    else
                        return -1;
                }
                
                return _ret;
            }
        else if (board[4].Equals(board[8]) && EmptyCheck(0))
        {
                _ret = 0;

                if(win)
                {
                    if(WinCheck(4, 8))
                        return _ret;
                    else
                        return -1;
                }
                
                return _ret;
            }
        else
            return -1;

    }

    int RightCrossCheck(bool win)
    {
        Debug.Log("RightCrossCheck: " + win);
        int _ret;

        if(board[2].Equals(board[4]) && EmptyCheck(6))
        {
                _ret = 6;

                if(win)
                {
                    if(WinCheck(2, 4))
                        return _ret;
                    else
                        return -1;
                }
                
                return _ret;
        }
        else if (board[2].Equals(board[6]) && EmptyCheck(4))
        {
                _ret = 4;

                if(win)
                {
                    if(WinCheck(2, 6))
                        return _ret;
                    else
                        return -1;
                }
                
                return _ret;
            }
        else if (board[4].Equals(board[6]) && EmptyCheck(2))
        {
                _ret = 2;

                if(win)
                {
                    if(WinCheck(4, 6))
                        return _ret;
                    else
                        return -1;
                }
                
                return _ret;
            }
        else
            return -1;
    }

    void Clear()
    {
        foreach (UIButton button in buttons)
            button.Reset();
    }
}
