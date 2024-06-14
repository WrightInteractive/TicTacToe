using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [Header("Game Variables")]
    [Tooltip("Active Player")][SerializeField] int Player = 0;
    [Tooltip("Board Square Values for the Player that selected the square. 0 is unplayed")][SerializeField] int[] board = {0, 0, 0, 0, 0, 0, 0, 0, 0};
    [Tooltip("Winner of last match, -1 is no active game")][SerializeField] int winner = -1;
    [Tooltip("Is the AI active")][SerializeField] bool aI = false;
    [Tooltip("Is the Smart AI active")][SerializeField] bool smartAI = false;
    [Tooltip("How many games Player 1 has won")][SerializeField] int playerOneWins;
    [Tooltip("How many games Player 2 has won")][SerializeField] int playerTwoWins;
    [Tooltip("How many games have gone to stalemates")][SerializeField] int stalemates;
    [Tooltip("Winning Game array: -1 is no winning game")][SerializeField] int[] winningArray = {-1, -1, -1};
    [Tooltip("Color for MiniBoard font for winning player")][SerializeField] Color32 resultColor = new Color(0,0,0,0);

    [Header("GameObject References")]
    
    [Tooltip("Reference for the Buttons that make up the Game Squares")][SerializeField] List<UIButton> buttons = new List<UIButton>();
    [Tooltip("Reference for the Buttons that make up the MiniGame Squares")][SerializeField] List<UIButton> miniButtons = new List<UIButton>();
    [Tooltip("Reference for ResultMenu gameResult lines")] [SerializeField] List<GameObject> resultLines = new List<GameObject>();
    [Tooltip("Reference for StartMenu")][SerializeField] GameObject startMenu;
    [Tooltip("Reference for ResultMenu")][SerializeField] GameObject resulttMenu;
    [Tooltip("Reference for Player toggle text on StartMenu")][SerializeField] TMP_Text playerToggleText;
    [Tooltip("Reference for AI Difficulty toggle text on StartMenu")][SerializeField] Button aILevelButton;
    [Tooltip("Reference for Ai Level text on StartMenu")][SerializeField] TMP_Text aILevelText;
    [Tooltip("Reference for Player One Win text on ResultMenu")][SerializeField] TMP_Text playerOneWinText;
    [Tooltip("Reference for Player Two Win text on ResultMenu")][SerializeField] TMP_Text playerTwoWinText;
    [Tooltip("Reference for Stalemate text on ResultMenu")][SerializeField] TMP_Text stalemateText;
    [Tooltip("Reference for ResultHeader text on ResultMenu")][SerializeField] TMP_Text resultHeaderText;

    public int player {get { return Player; }}      //Getter for Player variable
    
    //Checks to see if a Player has won the game, if not, switched active player.
    // If AI is selected, it will either call Smart or Dumb AI depending on User selection
    void TogglePlayer()
    {
        if(CheckGame())
            return;

        if(Player == 1)
        {
            Player = 2;
            
            if(aI)
                if(smartAI)
                    SmartAI();
                else
                    DumbAI();
        }
        else
            Player = 1;
    }

    //Called when Squares are selected. Sets board to active player then switchs active player
    public void ButtonPressed(int buttonIndex)
    {
        board[buttonIndex] = Player;
        if(Player == 1)
            miniButtons[buttonIndex].GetComponentInChildren<TMP_Text>().text = "X";
        else
             miniButtons[buttonIndex].GetComponentInChildren<TMP_Text>().text = "O";
        TogglePlayer();
    }

    //Returns true if either player won game or stalemate. 
    //Checks win positions in Horizontal, Vertical, LeftCross, RightCross order.
    //Then checks stalemate
    bool CheckGame()
    {
        if(WinConfrim(0,1,2) ||
            WinConfrim(0,3,6) ||
            WinConfrim(0,4,8) ||
            WinConfrim(1,4,7) ||
            WinConfrim(2,5,8) ||
            WinConfrim(2,4,6) ||
            WinConfrim(3,4,5) ||
            WinConfrim(6,7,8))
           {
                winner = Player - 1;
                Player = 1;

                if(winner == 0)
                {
                    playerOneWins++;
                    resultHeaderText.text = "X's Won!";
                }
                else if(winner == 1)
                {
                    playerTwoWins++;
                    resultHeaderText.text = "O's Won!";
                }

                UpdateScores();
                return true;
           }
        else if(StaleMateCheck())
        {
            winner = -2;
            Player = 1;

            stalemates++;
            resultHeaderText.text = "Stalemate";
            

            UpdateScores();

            return true;
        }
        return false;
    }

    //Updates ResultMenu with new win and then toggles ResultMenu to open
    void UpdateScores()
    {
        playerOneWinText.text = playerOneWins.ToString();
        playerTwoWinText.text = playerTwoWins.ToString();
        stalemateText.text = stalemates.ToString();

        foreach(UIButton b in buttons)
            b.InteractionToggle(false);

        ResultHighlight();

        ResultMenuToggle();
    }

    //Sets MiniBoard squares to winning Color if part of the winning array
    //Sets active the correct line depending on winning array

    void ResultHighlight()
    {
        if(winner == -2)
            return;

        foreach(int r in winningArray)
            miniButtons[r].GetComponentInChildren<TMP_Text>().color = resultColor;

        if(winningArray[0] == 0)
            if(winningArray[2] == 2)
                resultLines[0].SetActive(true);
            else if(winningArray[2] == 6)
                resultLines[3].SetActive(true);
            else if(winningArray[2] == 8)
                resultLines[6].SetActive(true);
        
        if(winningArray[0] == 1)
            if(winningArray[2] == 7)
                resultLines[4].SetActive(true);
        
        if(winningArray[0] == 2)
            if(winningArray[2] == 8)
                resultLines[5].SetActive(true);
            else if(winningArray[2] == 6)
                resultLines[7].SetActive(true);
        
        if(winningArray[0] == 3)
            if(winningArray[2] == 5)
                resultLines[1].SetActive(true);    
        
        if(winningArray[0] == 6)
            if(winningArray[2] == 8)
                resultLines[2].SetActive(true);
    }

    //Returns true if all squares are filled but there is no winner
    //Returns false if any square is empty
    bool StaleMateCheck()
    {
        foreach(int b in board)
            if(b == 0)
                return false;

        return true;
    }

    //Randomly selects a square, checks to see if it has been selected, and if not, chooses that square.
    //If square has been selected, call same function in recursion
    void DumbAI()
    {
        if(winner != -1)
            return;

        int _index = Random.Range(0, board.Length - 1);

        if(board[_index] != 0)
            DumbAI();
        else
            buttons[_index].PlayerClick();
    }

    //Checks for any winning square, if one is found, it will play the winning square.
    //If no winning square is found, it will check for and blocks to keep opponent from winning, and will play the blocking square
    //If no blocking square is found, it will randomly select an available square
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

    //Wiil return true if any block is empty
    //Will return false if all blocks are filled
    bool EmptyCheck(int index)
    {
        if(board[index] == 0)
            return true;
        else
            return false;
    }

    //Will return true if the two square parameters match player selection
    //Will return false if the tqo square parameters do not match player
    bool WinCheck(int index1, int index2)
    {
        if(board[index1]  == 1 && board[index2] == 1)
            return true;
        
        return false;
    }

    //Will return true if all square parameters match
    //Will return false if any square parameter doesnt match
    bool WinConfrim(int a, int b, int c)
    {
        if(board[a].Equals(board[b]) && board[a].Equals(board[c]) && !board[a].Equals(0))
        {
            winningArray = new int[3]{a, b, c};
            return true;
        }

        return false;
    }

    //Checks the three parameter squares to see if the values match
    //Returns player number if matching, or -1 if there is no match
    //Will check Win move vs block moves for AI
    int Check(int a, int b, int c, bool w)
    {
        int _i;

        if(board[a].Equals(board[b]) && EmptyCheck(c))
        {
            _i = c;

            if(w)
                {
                    if(WinCheck(a, b))
                        return _i;
                    else
                        return -1;
                }
            
            return _i;
        }
        else if(board[b].Equals(board[c]) && EmptyCheck(a))
        {
            _i = a;

            if(w)
                {
                    if(WinCheck(b, c))
                        return _i;
                    else
                        return -1;
                }
            
            return _i;
        }
        else if(board[c].Equals(board[a]) && EmptyCheck(b))
        {
            _i = b;

            if(w)
                {
                    if(WinCheck(c, a))
                        return _i;
                    else
                        return -1;
                }
            
            return _i;
        }
        else
            return -1;
    }

    //Checks all Horizontal rows for a player to have selected all squares and returns player 
    //Will return -1 if a single player has not selected all the squares, or if there are empty squares
    int HorizontalCheck(int index, bool win)
    {
        return Check(index, index + 1, index + 2, win);
    }

    //Checks all Vertical columns for a player to have selected all squares and returns player 
    //Will return -1 if a single player has not selected all the squares, or if there are empty squares
    int VerticalCheck(int index, bool win)
    {
        return Check(index, index + 3, index + 6, win);
    }

    //Checks Left Diaginal squars for a player to have selected all squares and returns player 
    //Will return -1 if a single player has not selected all the squares, or if there are empty squares
    int LeftCrossCheck(bool win)
    {
        return Check(0, 4, 8, win);
    }

    //Checks Right Diaginal squars for a player to have selected all squares and returns player 
    //Will return -1 if a single player has not selected all the squares, or if there are empty squares
    int RightCrossCheck(bool win)
    {
        return Check(2, 4, 6, win);
    }

    //Toggles between Solo Play vs AI, or Two player mode
    //Sets AI difficulty button to active if Solo play is selected
    //Called by Button OnClick 
    public void PlayerToggle()
    {
        aI = !aI;
        
        if(aI)
        {
            playerToggleText.text = "AI";
            aILevelButton.gameObject.SetActive(true);
            aILevelText.gameObject.SetActive(true);
        }
        else
        {
            playerToggleText.text = "2 Player";
            aILevelButton.gameObject.SetActive(false);
            aILevelText.gameObject.SetActive(false);
        }
    }

    //Toggles between Beginner and Expert AI modes
    //Called by Button OnClick 
    public void AILevelToggle()
    {
        smartAI = !smartAI;

        if(smartAI)
            aILevelButton.GetComponentInChildren<TMP_Text>().text = "Expert";
        else
            aILevelButton.GetComponentInChildren<TMP_Text>().text = "Beginner";
    }

    //Sets Animation bool to transition MainMenu between Open and Closed state
    public void MainMenuToggle()
    {
        Animator anim = startMenu.GetComponent<Animator>();
        if(anim != null)
        {
            bool isOpen = anim.GetBool("open");
            anim.SetBool("open", !isOpen);
        }
    }

    //Sets Animation bool to transition ResultMenu between Open and Closed state
    public void ResultMenuToggle()
    {
        Animator anim = resulttMenu.GetComponent<Animator>();
        if(anim != null)
        {
            bool isOpen = anim.GetBool("open");
            anim.SetBool("open", !isOpen);
        }
    }

    //Resets board between games
    public void Clear(bool clearLines = false)
    {
        winner = -1;
        Player = 1;

        for(int i = 0; i < winningArray.Length; i++)
            winningArray[i] = -1;

        foreach(UIButton b in buttons)
            b.InteractionToggle(true);

        for(int i = 0; i < board.Length; i++)
            board[i] = 0;

        foreach (UIButton button in buttons)
            button.Reset();

        foreach(UIButton m in miniButtons)
            m.GetComponentInChildren<TMP_Text>().color = Color.black;

        if(clearLines)
            foreach(GameObject g in resultLines)
                g.SetActive(false);

    }

    //Transitions between the ResultMenu and the StartMenu
    public void RestartGame()
    {
        ResultMenuToggle();
        MainMenuToggle();
    }

    //Transitions the StartMenu to its closed position and starts the game
    public void StartGame()
    {
        Clear(true);
        MainMenuToggle();
    }
}
