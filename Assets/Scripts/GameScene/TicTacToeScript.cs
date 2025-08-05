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
        "Button_0", "Button_1", "Button_2" ,
        "Button_3", "Button_4", "Button_5" ,
        "Button_6", "Button_7", "Button_8"
    };
    bool isImFirst;
    bool isMyTurn;
    int[] gameBoard = new int[9];

    // gameBoard를 2차원 배열로 바꿔주는 함수
    int GetGameBoardValue(int row, int col)
    {
        int index = row * 3 + col;
        return gameBoard[index];
    }


    // 순서 정하기
    void SetFirstTurn(int FirstTurn)
    {
        // FirstTurn이 0이면 내가 먼저, 1이면 컴퓨터가 먼저.
        if (FirstTurn % 2 == 0)
        {
            isImFirst = true;
            isMyTurn = true;
            TurnText.GetComponent<TextMeshProUGUI>().text = "당신";
        }
        else
        {
            isImFirst = false;
            isMyTurn = false;
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
        if (isImFirst && isMyTurn && gameBoard[buttonIndex] == 0) // 내가 O & 내턴 & 비어있는 칸
        {
            gameBoard[buttonIndex] = 1;
            ChangeButtonImage(buttonIndex);
            CheckWhoWin(gameBoard);
            changeTurn();
            ComputerTurn();
        }
        else if (!isImFirst && isMyTurn && gameBoard[buttonIndex] == 0) // 내가 X & 내턴 & 비어있는 칸
        {
            gameBoard[buttonIndex] = 2;
            ChangeButtonImage(buttonIndex);
            CheckWhoWin(gameBoard);
            changeTurn();
            ComputerTurn();
        }
        else if (isImFirst && !isMyTurn && gameBoard[buttonIndex] == 0) // 내가 O & 컴퓨터턴 & 비어있는 칸
        {
            gameBoard[buttonIndex] = 2;
            ChangeButtonImage(buttonIndex);
            CheckWhoWin(gameBoard);
            changeTurn();
            // ComputerTurn();
        }
        else if (!isImFirst && !isMyTurn && gameBoard[buttonIndex] == 0) // 내가 X & 컴퓨터턴 & 비어있는 칸
        {
            gameBoard[buttonIndex] = 1;
            ChangeButtonImage(buttonIndex);
            CheckWhoWin(gameBoard);
            changeTurn();
            // ComputerTurn();
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


    // 컴퓨터 로직 - 코루틴(생각하는 척 하면서 기다리기)
    public IEnumerator ComputerTurnCoroutine()
    {
        if (isMyTurn) yield break; // 사용자 턴이면 코루틴 종료

        // 컴퓨터 로직 실행
        int[] BlankSlot = GetEmptySlots();

        // int selectedIndex = RandomMarking(BlankSlot);
        // int selectedIndex = OffensiveMarking(BlankSlot);

        int selectedIndex = DefensiveMarking(BlankSlot);

        // 생각하는 척 하며 1.5초 기다리기
        yield return new WaitForSeconds(1.5f);

        // 실제로 수 두기
        OnButtonClick(selectedIndex);
    }

    // 컴퓨터 로직 - 메인
    public void ComputerTurn()
    {
        if (isMyTurn) return; // 사용자 턴이면 패스
        else // 컴퓨터 턴일 때 로직
        {
            // int[] BlankSlot = GetEmptySlots();
            // int selectedIndex = RandomMarking(BlankSlot);
            StartCoroutine(ComputerTurnCoroutine());
            // OnButtonClick(selectedIndex);
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
    // isImFirst를 이용해 true면 컴퓨터가 1인 공간을 찾아서 선택.
    // isImFirst가 false면 2인 공간을 찾아서 선택
    // 만약 로직에 만족하는 칸이 없다면 랜덤 의사결정
    public int OffensiveMarking(int[] EmptySlots)
    {
        int computerPlayer;
        if (isImFirst)
        {
            computerPlayer = 2;
        }
        else
        {
            computerPlayer = 1;
        }

        for (int i = 0; i < EmptySlots.Length; i++)
        {
            int slotIndex = EmptySlots[i];

            gameBoard[slotIndex] = computerPlayer;

            if (IsWinningFor(computerPlayer))
            {
                gameBoard[slotIndex] = 0;
                return slotIndex;
            }

            gameBoard[slotIndex] = 0;
        }
        return RandomMarking(EmptySlots);
    }

    // 컴퓨터 로직 - 방어적 의사결정
    public int DefensiveMarking(int[] EmptySlots)
    {
        int PlayerPlayer;
        if (isImFirst)
        {
            PlayerPlayer = 1;
        }
        else
        {
            PlayerPlayer = 2;
        }
        for (int i = 0; i < EmptySlots.Length; i++)
        {
            int slotIndex = EmptySlots[i];
            gameBoard[slotIndex] = PlayerPlayer;
            if (IsWinningFor(PlayerPlayer))
            {
                gameBoard[slotIndex] = 0;
                return slotIndex;
            }
            gameBoard[slotIndex] = 0;
        }
        return RandomMarking(EmptySlots);
    }

    // 컴퓨터 로직 - 복합적 의사결정
    // public int HybridMarking(int[] EmptySlots)
    // {

    // }

    // 틱택토 체크 로직
    public void CheckWhoWin(int[] gameBoard)
    {
        // 가로줄 확인
        if (GetGameBoardValue(0, 0) == GetGameBoardValue(0, 1) && GetGameBoardValue(0, 1) == GetGameBoardValue(0, 2) && GetGameBoardValue(0, 0) != 0)
        {
            if (GetGameBoardValue(0, 0) == 1)
            {
                Debug.Log("First Win!");
            }
            else
            {
                Debug.Log("Second Win!");
            }
        }
        else if (GetGameBoardValue(1, 0) == GetGameBoardValue(1, 1) && GetGameBoardValue(1, 1) == GetGameBoardValue(1, 2) && GetGameBoardValue(1, 0) != 0)
        {
            if (GetGameBoardValue(1, 0) == 1)
            {
                Debug.Log("First Win!");
            }
            else
            {
                Debug.Log("Second Win!");
            }
        }
        else if (GetGameBoardValue(2, 0) == GetGameBoardValue(2, 1) && GetGameBoardValue(2, 1) == GetGameBoardValue(2, 2) && GetGameBoardValue(2, 0) != 0)
        {
            if (GetGameBoardValue(2, 0) == 1)
            {
                Debug.Log("First Win!");
            }
            else
            {
                Debug.Log("Second Win!");
            }
        }
        // 세로줄 확인
        else if (GetGameBoardValue(0, 0) == GetGameBoardValue(1, 0) && GetGameBoardValue(1, 0) == GetGameBoardValue(2, 0) && GetGameBoardValue(0, 0) != 0)
        {
            if (GetGameBoardValue(0, 0) == 1)
            {
                Debug.Log("First Win!");
            }
            else
            {
                Debug.Log("Second Win!");
            }
        }
        else if (GetGameBoardValue(0, 1) == GetGameBoardValue(1, 1) && GetGameBoardValue(1, 1) == GetGameBoardValue(2, 1) && GetGameBoardValue(0, 1) != 0)
        {
            if (GetGameBoardValue(0, 1) == 1)
            {
                Debug.Log("First Win!");
            }
            else
            {
                Debug.Log("Second Win!");
            }
        }
        else if (GetGameBoardValue(0, 2) == GetGameBoardValue(1, 2) && GetGameBoardValue(1, 2) == GetGameBoardValue(2, 2) && GetGameBoardValue(0, 2) != 0)
        {
            if (GetGameBoardValue(0, 2) == 1)
            {
                Debug.Log("First Win!");
            }
            else
            {
                Debug.Log("Second Win!");
            }
        }
        // 대각선 체크
        else if (GetGameBoardValue(0, 0) == GetGameBoardValue(1, 1) && GetGameBoardValue(1, 1) == GetGameBoardValue(2, 2) && GetGameBoardValue(0, 0) != 0)
        {
            if (GetGameBoardValue(0, 0) == 1)
            {
                Debug.Log("First Win!");
            }
            else
            {
                Debug.Log("Second Win!");
            }
        }
        else if (GetGameBoardValue(0, 2) == GetGameBoardValue(1, 1) && GetGameBoardValue(1, 1) == GetGameBoardValue(2, 0) && GetGameBoardValue(0, 2) != 0)
        {
            if (GetGameBoardValue(0, 2) == 1)
            {
                Debug.Log("First Win!");
            }
            else
            {
                Debug.Log("Second Win!");
            }
        }
    }


    // 컴퓨터의 체크로직
    private bool IsWinningFor(int player)
    {

        // 가로줄 확인
        if (GetGameBoardValue(0, 0) == GetGameBoardValue(0, 1) && GetGameBoardValue(0, 1) == GetGameBoardValue(0, 2) && GetGameBoardValue(0, 0) != 0)
        {
            if (GetGameBoardValue(0, 0) == player)
            {
                return true;
            }
        }
        else if (GetGameBoardValue(1, 0) == GetGameBoardValue(1, 1) && GetGameBoardValue(1, 1) == GetGameBoardValue(1, 2) && GetGameBoardValue(1, 0) != 0)
        {
            if (GetGameBoardValue(1, 0) == player)
            {
                return true;
            }
        }
        else if (GetGameBoardValue(2, 0) == GetGameBoardValue(2, 1) && GetGameBoardValue(2, 1) == GetGameBoardValue(2, 2) && GetGameBoardValue(2, 0) != 0)
        {
            if (GetGameBoardValue(2, 0) == player)
            {
                return true;
            }
        }
        // 세로줄 확인
        else if (GetGameBoardValue(0, 0) == GetGameBoardValue(1, 0) && GetGameBoardValue(1, 0) == GetGameBoardValue(2, 0) && GetGameBoardValue(0, 0) != 0)
        {
            if (GetGameBoardValue(0, 0) == player)
            {
                return true;
            }
        }
        else if (GetGameBoardValue(0, 1) == GetGameBoardValue(1, 1) && GetGameBoardValue(1, 1) == GetGameBoardValue(2, 1) && GetGameBoardValue(0, 1) != 0)
        {
            if (GetGameBoardValue(0, 1) == player)
            {
                return true;
            }
        }
        else if (GetGameBoardValue(0, 2) == GetGameBoardValue(1, 2) && GetGameBoardValue(1, 2) == GetGameBoardValue(2, 2) && GetGameBoardValue(0, 2) != 0)
        {
            if (GetGameBoardValue(0, 2) == player)
            {
                return true;
            }
        }
        // 대각선 체크
        else if (GetGameBoardValue(0, 0) == GetGameBoardValue(1, 1) && GetGameBoardValue(1, 1) == GetGameBoardValue(2, 2) && GetGameBoardValue(0, 0) != 0)
        {
            if (GetGameBoardValue(0, 0) == player)
            {
                return true;
            }
        }
        else if (GetGameBoardValue(0, 2) == GetGameBoardValue(1, 1) && GetGameBoardValue(1, 1) == GetGameBoardValue(2, 0) && GetGameBoardValue(0, 2) != 0)
        {
            if (GetGameBoardValue(0, 2) == player)
            {
                return true;
            }
        }

        // 승자가 없는 경우 false 반환
        return false;
    }

    // 틱택토를 1줄 미는 함수

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