using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToeScript : MonoBehaviour
{
    public Sprite Osprite;
    public Sprite Xsprite;
    public Sprite Defaultsprite;
    Button[] tictactoeButtons = new Button[9];
    GameObject TurnText;
    // GameObject ScoreText;
    TextMeshProUGUI RoundText;
    TextMeshProUGUI RobotText;

    // 일시정지 메뉴 UI 요소들
    GameObject PauseMenuPanel;
    GameObject ConfirmQuitPanel;
    bool isPaused = false;

    // GameObject RoundTextObject;
    string[] buttonName = {
        "Button_0", "Button_1", "Button_2" ,
        "Button_3", "Button_4", "Button_5" ,
        "Button_6", "Button_7", "Button_8"
    };
    Button[] pushLineButtons = new Button[12];
    string[] pushLineButtonsName = {
    "UpArrow_0", "UpArrow_1", "UpArrow_2",
    "DownArrow_0", "DownArrow_1", "DownArrow_2",
    "LeftArrow_0", "LeftArrow_1", "LeftArrow_2",
    "RightArrow_0", "RightArrow_1", "RightArrow_2"
};

    bool isImFirst;
    bool isMyTurn;
    bool isFirstWin = false;
    bool isSecondWin = false;
    bool isFinishRound = false;
    int[] gameBoard = new int[9];
    int currentRound = 1;

    // 로봇 대사 배열들
    string[] roundStartDialogues = {
        "시스템 가동. 라운드를 시작하겠습니다.",
        "전략 분석 중... 준비되셨습니까?",
        "연산 시작. 최선을 다하시길 바랍니다.",
        "초기화 완료. 게임을 시작합니다.",
        "준비 완료. 대결을 시작하죠."
    };

    string[] playerWinDialogues = {
        "분석 결과... 당신의 승리입니다.",
        "흥미롭군요. 당신이 이겼습니다.",
        "시스템 오류... 아니, 당신의 실력이군요.",
        "예상 밖의 결과입니다. 승리를 축하합니다.",
        "계산 완료. 당신의 승리를 인정합니다."
    };

    string[] playerLoseDialogues = {
        "안타깝습니다. 처음부터 다시 시작하죠.",
        "오류 감지. 라운드 1로 돌아갑니다.",
        "재부팅이 필요합니다. 초기화합니다.",
        "실패를 분석 중... 다시 시작하겠습니다.",
        "시스템 복구 중. 처음부터 다시입니다."
    };

    string[] finalWinDialogues = {
        "모든 연산 종료. 당신의 완전한 승리입니다.",
        "시스템 종료... 당신은 진정한 승자입니다.",
        "분석 불가... 놀라운 실력입니다. 축하합니다!",
        "최종 결과 확정. 당신의 승리를 기록합니다.",
        "프로그램 종료. 경의를 표합니다."
    };


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

    // 로봇 대사 표시 함수 - 라운드 시작 시
    void ShowRoundStartDialogue()
    {
        int randomIndex = Random.Range(0, roundStartDialogues.Length);
        RobotText.text = roundStartDialogues[randomIndex];
        Debug.Log($"로봇 대사: {roundStartDialogues[randomIndex]}");
    }

    // 로봇 대사 표시 함수 - 라운드 종료 시
    void ShowRoundEndDialogue(bool playerWon, bool isFinalRound)
    {
        string dialogue;

        if (isFinalRound && playerWon)
        {
            // 8라운드 승리 (최종 승리)
            int randomIndex = Random.Range(0, finalWinDialogues.Length);
            dialogue = finalWinDialogues[randomIndex];
        }
        else if (playerWon)
        {
            // 일반 라운드 승리
            int randomIndex = Random.Range(0, playerWinDialogues.Length);
            dialogue = playerWinDialogues[randomIndex];
        }
        else
        {
            // 패배
            int randomIndex = Random.Range(0, playerLoseDialogues.Length);
            dialogue = playerLoseDialogues[randomIndex];
        }

        RobotText.text = dialogue;
        Debug.Log($"로봇 대사: {dialogue}");
    }

    // 틱택토시 버튼 클릭
    public void OnButtonClick(int buttonIndex)
    {
        if (isFirstWin || isSecondWin)
        {
            Debug.Log("게임이 이미 종료되었습니다.");
            return;
        }
        // Debug.Log($"{buttonIndex} is clicked");
        if (isImFirst && isMyTurn && gameBoard[buttonIndex] == 0) // 내가 O & 내턴 & 비어있는 칸
        {
            gameBoard[buttonIndex] = 1;
            ChangeButtonImage(buttonIndex);
            CheckWhoWin(gameBoard);
            if (!isFinishRound) // Round가 끝나지 않았다면 계속하기.
            {
                changeTurn();
                ComputerTurn();
            }
        }
        else if (!isImFirst && isMyTurn && gameBoard[buttonIndex] == 0) // 내가 X & 내턴 & 비어있는 칸
        {
            gameBoard[buttonIndex] = 2;
            ChangeButtonImage(buttonIndex);
            CheckWhoWin(gameBoard);
            if (!isFinishRound) // Round가 끝나지 않았다면 계속하기.
            {
                changeTurn();
                ComputerTurn();
            }
        }
        else if (isImFirst && !isMyTurn && gameBoard[buttonIndex] == 0) // 내가 O & 컴퓨터턴 & 비어있는 칸
        {
            gameBoard[buttonIndex] = 2;
            ChangeButtonImage(buttonIndex);
            CheckWhoWin(gameBoard);
            if (!isFinishRound) // Round가 끝나지 않았다면 계속하기.
            {
                changeTurn();
            }
        }
        else if (!isImFirst && !isMyTurn && gameBoard[buttonIndex] == 0) // 내가 X & 컴퓨터턴 & 비어있는 칸
        {
            gameBoard[buttonIndex] = 1;
            ChangeButtonImage(buttonIndex);
            CheckWhoWin(gameBoard);
            if (!isFinishRound) // Round가 끝나지 않았다면 계속하기.
            {
                changeTurn();
            }
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
        else
        {
            ButtonImage.sprite = Defaultsprite;
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

    // 승패 여부 체크
    void CheckWhoWin(int[] gameBoard)
    {
        // 가로줄 확인
        if (GetGameBoardValue(0, 0) == GetGameBoardValue(0, 1) && GetGameBoardValue(0, 1) == GetGameBoardValue(0, 2) && GetGameBoardValue(0, 0) != 0)
        {
            if (GetGameBoardValue(0, 0) == 1)
            {
                Debug.Log("First Win!");
                isFirstWin = true;
                isFinishRound = true;
                HandleRoundEnd(); // 라운드 종료 처리
                return;
            }
            else
            {
                Debug.Log("Second Win!");
                isSecondWin = true;
                isFinishRound = true;
                HandleRoundEnd(); // 라운드 종료 처리
                return;
            }
        }
        if (GetGameBoardValue(1, 0) == GetGameBoardValue(1, 1) && GetGameBoardValue(1, 1) == GetGameBoardValue(1, 2) && GetGameBoardValue(1, 0) != 0)
        {
            if (GetGameBoardValue(1, 0) == 1)
            {
                Debug.Log("First Win!");
                isFirstWin = true;
                isFinishRound = true;
                HandleRoundEnd(); // 라운드 종료 처리
                return;
            }
            else
            {
                Debug.Log("Second Win!");
                isSecondWin = true;
                isFinishRound = true;
                HandleRoundEnd(); // 라운드 종료 처리
                return;
            }
        }
        if (GetGameBoardValue(2, 0) == GetGameBoardValue(2, 1) && GetGameBoardValue(2, 1) == GetGameBoardValue(2, 2) && GetGameBoardValue(2, 0) != 0)
        {
            if (GetGameBoardValue(2, 0) == 1)
            {
                Debug.Log("First Win!");
                isFirstWin = true;
                isFinishRound = true;
                HandleRoundEnd(); // 라운드 종료 처리
                return;
            }
            else
            {
                Debug.Log("Second Win!");
                isSecondWin = true;
                isFinishRound = true;
                HandleRoundEnd(); // 라운드 종료 처리
                return;
            }
        }
        // 세로줄 확인
        if (GetGameBoardValue(0, 0) == GetGameBoardValue(1, 0) && GetGameBoardValue(1, 0) == GetGameBoardValue(2, 0) && GetGameBoardValue(0, 0) != 0)
        {
            if (GetGameBoardValue(0, 0) == 1)
            {
                Debug.Log("First Win!");
                isFirstWin = true;
                isFinishRound = true;
                HandleRoundEnd(); // 라운드 종료 처리
                return;
            }
            else
            {
                Debug.Log("Second Win!");
                isSecondWin = true;
                isFinishRound = true;
                HandleRoundEnd(); // 라운드 종료 처리
                return;
            }
        }
        if (GetGameBoardValue(0, 1) == GetGameBoardValue(1, 1) && GetGameBoardValue(1, 1) == GetGameBoardValue(2, 1) && GetGameBoardValue(0, 1) != 0)
        {
            if (GetGameBoardValue(0, 1) == 1)
            {
                Debug.Log("First Win!");
                isFirstWin = true;
                isFinishRound = true;
                HandleRoundEnd(); // 라운드 종료 처리
                return;
            }
            else
            {
                Debug.Log("Second Win!");
                isSecondWin = true;
                isFinishRound = true;
                HandleRoundEnd(); // 라운드 종료 처리
                return;
            }
        }
        if (GetGameBoardValue(0, 2) == GetGameBoardValue(1, 2) && GetGameBoardValue(1, 2) == GetGameBoardValue(2, 2) && GetGameBoardValue(0, 2) != 0)
        {
            if (GetGameBoardValue(0, 2) == 1)
            {
                Debug.Log("First Win!");
                isFirstWin = true;
                isFinishRound = true;
                HandleRoundEnd(); // 라운드 종료 처리
                return;
            }
            else
            {
                Debug.Log("Second Win!");
                isSecondWin = true;
                isFinishRound = true;
                HandleRoundEnd(); // 라운드 종료 처리
                return;
            }
        }
        // 대각선 체크
        if (GetGameBoardValue(0, 0) == GetGameBoardValue(1, 1) && GetGameBoardValue(1, 1) == GetGameBoardValue(2, 2) && GetGameBoardValue(0, 0) != 0)
        {
            if (GetGameBoardValue(0, 0) == 1)
            {
                Debug.Log("First Win!");
                isFirstWin = true;
                isFinishRound = true;
                HandleRoundEnd(); // 라운드 종료 처리
                return;
            }
            else
            {
                Debug.Log("Second Win!");
                isSecondWin = true;
                isFinishRound = true;
                HandleRoundEnd(); // 라운드 종료 처리
                return;
            }
        }
        if (GetGameBoardValue(0, 2) == GetGameBoardValue(1, 1) && GetGameBoardValue(1, 1) == GetGameBoardValue(2, 0) && GetGameBoardValue(0, 2) != 0)
        {
            if (GetGameBoardValue(0, 2) == 1)
            {
                Debug.Log("First Win!");
                isFirstWin = true;
                isFinishRound = true;
                HandleRoundEnd(); // 라운드 종료 처리
                return;
            }
            else
            {
                Debug.Log("Second Win!");
                isSecondWin = true;
                isFinishRound = true;
                HandleRoundEnd(); // 라운드 종료 처리
                return;
            }
        }
    }

    // 라운드 종료
    void HandleRoundEnd()
    {
        // 플레이어가 이긴 경우 체크
        bool playerWon = (isImFirst && isFirstWin) || (!isImFirst && isSecondWin);

        if (playerWon)
        {
            Debug.Log($"라운드 {currentRound} 승리!");

            // 8라운드 승리 시 WinScene으로 전환
            // if (currentRound >= 8)
            // {
            //     Debug.Log("8라운드 승리! WinScene으로 이동합니다.");
            //     ShowRoundEndDialogue(true, true); // 최종 승리 대사
            //     StartCoroutine(LoadWinSceneWithDelay());
            //     return;
            // }

            // 일반 라운드 승리 대사 표시 + 다음라운드 시작
            ShowRoundEndDialogue(true, false);
            currentRound++;
            RoundText.text = currentRound.ToString();
            Debug.Log($"다음 라운드: {currentRound}");
            StartCoroutine(ResetBoardWithDelay());
        }
        else // 플레이어가 진 경우
        {
            Debug.Log("패배! 1라운드로 되돌아갑니다.");

            // 패배 대사 표시
            ShowRoundEndDialogue(false, false);

            currentRound = 1;
            RoundText.text = currentRound.ToString();

            // 게임 초기화 후 1라운드로 돌아감
            StartCoroutine(ResetBoardWithDelay());
        }
    }

    // 게임 보드 초기화 함수
    void ResetBoard()
    {
        // 게임 보드 초기화
        for (int i = 0; i < gameBoard.Length; i++)
        {
            gameBoard[i] = 0;
            ChangeButtonImage(i);
        }

        // 플래그 초기화
        isFirstWin = false;
        isSecondWin = false;
        isFinishRound = false;

        // 턴 랜덤 선택
        int FirstTurn = Random.Range(1, 100);
        SetFirstTurn(FirstTurn);

        // 라운드 시작 대사 표시
        ShowRoundStartDialogue();

        // 컴퓨터가 선공이면 컴퓨터 턴 시작
        if (!isImFirst)
        {
            ComputerTurn();
        }

        Debug.Log("게임 보드가 초기화되었습니다.");
    }

    // 딜레이 후 보드 초기화하는 코루틴
    IEnumerator ResetBoardWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
        ResetBoard();
    }

    // IEnumerator LoadWinSceneWithDelay()
    // {
    //     // 2초 대기 (승리 메시지를 볼 시간)
    //     yield return new WaitForSeconds(2.0f);
    //     UnityEngine.SceneManagement.SceneManager.LoadScene("WinScene");
    // }


    // 컴퓨터 체크로직
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
    void pushLine(string direction, int idx)
    {
        if (isFirstWin || isSecondWin)
        {
            Debug.Log("게임이 이미 종료되었습니다.");
            return;
        }
        if (direction == "up")
        {
            gameBoard[idx] = gameBoard[idx + 3];
            gameBoard[idx + 3] = gameBoard[idx + 6];
            gameBoard[idx + 6] = 0;

        }
        if (direction == "down")
        {
            gameBoard[idx + 6] = gameBoard[idx + 3];
            gameBoard[idx + 3] = gameBoard[idx];
            gameBoard[idx] = 0;

        }
        if (direction == "right")
        {
            gameBoard[idx + 2] = gameBoard[idx + 1];
            gameBoard[idx + 1] = gameBoard[idx];
            gameBoard[idx] = 0;

        }
        if (direction == "left")
        {
            gameBoard[idx] = gameBoard[idx + 1];
            gameBoard[idx + 1] = gameBoard[idx + 2];
            gameBoard[idx + 2] = 0;

        }
        for (int i = 0; i < 9; i++)
        {
            ChangeButtonImage(i);
        }
        CheckWhoWin(gameBoard);
        if (!isFinishRound) // 라운드가 끝나지 않았을 때만 턴 변경
        {
            changeTurn();
            ComputerTurn();
        }
    }

    // ESC(일시정지) 관련 함수

    // 일시정지 메뉴 띄우기
    public void TogglePauseMenu()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // 게임 일시정지
    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        PauseMenuPanel.SetActive(true);
        Debug.Log("게임 일시정지");
    }

    // 게임 재개 (게임으로 돌아가기 버튼)
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        PauseMenuPanel.SetActive(false);
        ConfirmQuitPanel.SetActive(false);
        Debug.Log("게임 재개");
    }

    // 종료 버튼 클릭 시 확인 메시지 표시
    public void ShowQuitConfirmation()
    {
        PauseMenuPanel.SetActive(false); // 일시정지 메뉴 숨기기
        ConfirmQuitPanel.SetActive(true); // 확인 메시지 표시
        Debug.Log("종료 확인 메시지 표시");
    }

    // 확인 메시지에서 "예" 버튼 클릭 시 - IntroScene으로 이동
    public void QuitToIntro()
    {
        Time.timeScale = 1f; // 게임 시간 복원 (Scene 전환 전에 필수)
        Debug.Log("IntroScene으로 이동");
        UnityEngine.SceneManagement.SceneManager.LoadScene("IntroScene");
    }

    // 확인 메시지에서 "아니오" 버튼 클릭 시 - 일시정지 메뉴로 복귀
    public void CancelQuit()
    {
        ConfirmQuitPanel.SetActive(false); // 확인 메시지 숨기기
        PauseMenuPanel.SetActive(true); // 일시정지 메뉴 다시 표시
        Debug.Log("종료 취소");
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        TurnText = GameObject.Find("TurnText");
        RoundText = GameObject.Find("RoundText").GetComponent<TextMeshProUGUI>();
        RobotText = GameObject.Find("RobotText").GetComponent<TextMeshProUGUI>(); // RobotText 찾기

        // 일시정지 메뉴 UI 요소 찾기
        PauseMenuPanel = GameObject.Find("PauseMenuPanel");
        ConfirmQuitPanel = GameObject.Find("ConfirmQuitPanel");


        Button resumeButton = null;
        Button quitButton = null;
        Button yesButton = null;
        Button noButton = null;

        if (PauseMenuPanel != null)
        {

            resumeButton = PauseMenuPanel.transform.Find("ResumeButton")?.GetComponent<Button>();
            quitButton = PauseMenuPanel.transform.Find("QuitButton")?.GetComponent<Button>();
        }

        if (ConfirmQuitPanel != null)
        {
            yesButton = ConfirmQuitPanel.transform.Find("YesButton")?.GetComponent<Button>();
            noButton = ConfirmQuitPanel.transform.Find("NoButton")?.GetComponent<Button>();
        }

        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
            Debug.Log("ResumeButton 이벤트 연결 성공");
        }
        else
        {
            Debug.LogWarning("ResumeButton을 찾을 수 없습니다!");
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(ShowQuitConfirmation);
            Debug.Log("QuitButton 이벤트 연결 성공");
        }
        else
        {
            Debug.LogWarning("QuitButton을 찾을 수 없습니다!");
        }

        if (yesButton != null)
        {
            yesButton.onClick.AddListener(QuitToIntro);
            Debug.Log("YesButton 이벤트 연결 성공");
        }
        else
        {
            Debug.LogWarning("YesButton을 찾을 수 없습니다!");
        }

        if (noButton != null)
        {
            noButton.onClick.AddListener(CancelQuit);
            Debug.Log("NoButton 이벤트 연결 성공");
        }
        else
        {
            Debug.LogWarning("NoButton을 찾을 수 없습니다!");
        }

        // 이제 패널들을 비활성화
        if (PauseMenuPanel != null) PauseMenuPanel.SetActive(false);
        if (ConfirmQuitPanel != null) ConfirmQuitPanel.SetActive(false);

        // 초기 라운드 표시
        RoundText.text = currentRound.ToString();

        // 라운드 시작 대사 표시
        ShowRoundStartDialogue();

        for (int i = 0; i < buttonName.Length; i++)
        {
            GameObject pushButton = GameObject.Find(buttonName[i]);
            tictactoeButtons[i] = pushButton.GetComponent<Button>();

            int buttonIndex = i;
            tictactoeButtons[i].onClick.AddListener(() => OnButtonClick(buttonIndex));

        }

        for (int i = 0; i < pushLineButtons.Length; i++)
        {
            GameObject arrowButtonObj = GameObject.Find(pushLineButtonsName[i]);
            pushLineButtons[i] = arrowButtonObj.GetComponent<Button>();
        }
        for (int i = 0; i < 3; i++)
        {
            int index = i;
            pushLineButtons[i].onClick.AddListener(() => pushLine("up", index));
        }
        for (int i = 3; i < 6; i++)
        {
            int index = i % 3;
            pushLineButtons[i].onClick.AddListener(() => pushLine("down", index));
        }
        for (int i = 6; i < 9; i++)
        {
            int index = (i - 6) * 3; // 가로줄 시작 인덱스: 0, 3, 6
            pushLineButtons[i].onClick.AddListener(() => pushLine("left", index));
        }
        for (int i = 9; i < 12; i++)
        {
            int index = (i - 9) * 3; // 가로줄 시작 인덱스: 0, 3, 6
            pushLineButtons[i].onClick.AddListener(() => pushLine("right", index));
        }

        int FirstTurn = Random.Range(1, 100);
        SetFirstTurn(FirstTurn);
        if (isImFirst) isMyTurn = true;
        else
        {
            ComputerTurn();
        }
        Debug.Log($"내턴: {isImFirst}"); // isImFirst 값 출력
    }



    // Update is called once per frame
    void Update()
    {
        // ESC 키 입력 감지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }
}