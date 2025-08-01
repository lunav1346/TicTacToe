using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToeScript : MonoBehaviour
{
    public Sprite Osprite;
    public Sprite Xsprite;
    Button[] tictactoeButtons = new Button[9];
    GameObject TurnText;
    GameObject ScoreText;
    TextMeshProUGUI RoundText;
    string[] buttonName = {
        "Button_0", "Button_1", "Button_2",
        "Button_3", "Button_4", "Button_5",
        "Button_6", "Button_7", "Button_8"
    };
    bool isImFirst;
    bool isMyTurn;
    int[] gameBoard = new int[9];


    // 순서 정하기
    void SetFirstTurn(int FirstTurn)
    {
        if (FirstTurn % 2 == 0)
        {
            isImFirst = true;
            TurnText.GetComponent<TextMeshProUGUI>().text = "당신";
        }
        else
        {
            isImFirst = false;
            TurnText.GetComponent<TextMeshProUGUI>().text = "COM";
        }
    }


    // 턴 바꾸기
    public void changeTurn()
    {
        if (isMyTurn)
        {
            TurnText.GetComponent<TextMeshProUGUI>().text = "COM";
            isMyTurn = !isMyTurn;
        }
        else
        {
            TurnText.GetComponent<TextMeshProUGUI>().text = "당신";
            isMyTurn = !isMyTurn;
        }
    }

    // 틱택토시 버튼 클릭
    public void OnButtonClick(int buttonIndex)
    {
        // Debug.Log($"{buttonIndex} is clicked");
        if (isMyTurn && gameBoard[buttonIndex] == 0)
        {
            gameBoard[buttonIndex] = 1;
            ChangeButtonImage(buttonIndex);
            CheckWhoWin(gameBoard);
            changeTurn();
            ComputerTurn();
        }
        else if (!isMyTurn && gameBoard[buttonIndex] == 0)
        {
            gameBoard[buttonIndex] = 2;
            ChangeButtonImage(buttonIndex);
            CheckWhoWin(gameBoard);
            changeTurn();
            ComputerTurn();
        }
        else
        {
            Debug.Log("이미 눌린 버튼입니다!!!!!!");
        }
    }


    // 버튼 클릭 시 이미지 바꾸기
    public void ChangeButtonImage(int buttonIndex)
    {
        Image ButtonImage = tictactoeButtons[buttonIndex].GetComponent<Image>();
        if (gameBoard[buttonIndex] == 1)
        {
            ButtonImage.sprite = Osprite;
        }
        else if (gameBoard[buttonIndex] == 2)
        {
            ButtonImage.sprite = Xsprite;
        }
    }


    // 컴퓨터 로직 - 메인
    public void ComputerTurn()
    {
        if (isMyTurn) return; // 사용자 턴이면 패스
        else // 컴퓨터 턴일 때 로직
        {
            int[] BlankSlot = GetEmptySlots();
            int selectedIndex = RandomMarking(BlankSlot);
            OnButtonClick(selectedIndex);
        }
    }

    // 컴퓨터 로직 - 상황분석(리스트 반환)
    public int[] GetEmptySlots()
    {
        int EmptyCount = 0;
        int currentIndex = 0;
        for (int i = 0; i < gameBoard.Length; i++)
        {
            if (gameBoard[i] == 0) EmptyCount++;
        }
        int[] EmptySlots = new int[EmptyCount];
        for (int j = 0; j < gameBoard.Length; j++)
        {
            if (gameBoard[j] == 0)
            {
                EmptySlots[currentIndex] = j;
                currentIndex++;
            }
        }
        return EmptySlots;
    }


    // 컴퓨터 로직 - 랜덤 의사결정
    public int RandomMarking(int[] EmptySlots)
    {
        int r = Random.Range(0, EmptySlots.Length);
        return EmptySlots[r];
    }

    // 컴퓨터 로직 - 공격적 의사결정
    public int OffensiveMarking(int[] EmptySlots)
    {
        for (int i = 0; i < EmptySlots.Length; i++)
        {
            if
        }
    }

    // 컴퓨터 로직 - 방어적 의사결정
    public int DefensiveMarking(int[] EmptySlots)
    {

    }

    // 컴퓨터 로직 - 복합적 의사결정
    public int HybridMarking(int[] EmptySlots)
    {

    }

    // 틱택토 체크 로직
    public void CheckWhoWin(int[] gameBoard)
    {
        // 가로줄 확인
        if (gameBoard[0] == gameBoard[1] && gameBoard[1] == gameBoard[2] && gameBoard[0] != 0)
        {
            if (gameBoard[0] == 1)
            {
                Debug.Log("First Win!");
            }
            else
            {
                Debug.Log("Second Win!");
            }
        }
        else if (gameBoard[3] == gameBoard[4] && gameBoard[4] == gameBoard[5] && gameBoard[3] != 0)
        {
            if (gameBoard[3] == 1)
            {
                Debug.Log("First Win!");
            }
            else
            {
                Debug.Log("Second Win!");
            }
        }
        else if (gameBoard[6] == gameBoard[7] && gameBoard[7] == gameBoard[8] && gameBoard[6] != 0)
        {
            if (gameBoard[6] == 1)
            {
                Debug.Log("First Win!");
            }
            else
            {
                Debug.Log("Second Win!");
            }
        }
        // 세로줄 확인
        else if (gameBoard[0] == gameBoard[3] && gameBoard[3] == gameBoard[6] && gameBoard[0] != 0)
        {
            if (gameBoard[0] == 1)
            {
                Debug.Log("First Win!");
            }
            else
            {
                Debug.Log("Second Win!");
            }
        }
        else if (gameBoard[1] == gameBoard[4] && gameBoard[4] == gameBoard[7] && gameBoard[1] != 0)
        {
            if (gameBoard[1] == 1)
            {
                Debug.Log("First Win!");
            }
            else
            {
                Debug.Log("Second Win!");
            }
        }
        else if (gameBoard[2] == gameBoard[5] && gameBoard[5] == gameBoard[8] && gameBoard[2] != 0)
        {
            if (gameBoard[2] == 1)
            {
                Debug.Log("First Win!");
            }
            else
            {
                Debug.Log("Second Win!");
            }
        }
        // 대각선 체크
        else if (gameBoard[0] == gameBoard[4] && gameBoard[4] == gameBoard[8] && gameBoard[0] != 0)
        {
            if (gameBoard[0] == 1)
            {
                Debug.Log("First Win!");
            }
            else
            {
                Debug.Log("Second Win!");
            }
        }
        else if (gameBoard[2] == gameBoard[4] && gameBoard[4] == gameBoard[6] && gameBoard[2] != 0)
        {
            if (gameBoard[2] == 1)
            {
                Debug.Log("First Win!");
            }
            else
            {
                Debug.Log("Second Win!");
            }
        }

    }

    void Start()
    {
        Application.targetFrameRate = 60;
        TurnText = GameObject.Find("TurnText");


        for (int i = 0; i < buttonName.Length; i++)
        {
            GameObject pushButton = GameObject.Find(buttonName[i]);
            tictactoeButtons[i] = pushButton.GetComponent<Button>();

            int buttonIndex = i;
            tictactoeButtons[i].onClick.AddListener(() => OnButtonClick(buttonIndex));

        }


        int FirstTurn = Random.Range(1, 100);
        SetFirstTurn(FirstTurn);
        if (isImFirst) isMyTurn = true;
        else
        {
            ComputerTurn();
        }
        Debug.Log($"내턴: {isImFirst}"); // isImFirst 값 출력

        RoundText = GameObject.Find("RoundText").GetComponent<TextMeshProUGUI>();
    }



    // Update is called once per frame
    void Update()
    {
    }
}