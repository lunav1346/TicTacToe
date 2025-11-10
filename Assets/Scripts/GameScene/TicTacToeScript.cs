using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    int currentRound = 8;

    // 로봇 대사 배열들
    string[] roundStartDialogues = {
        "트루두스 가동. 라운드를 시작하겠습니다.",
        "전략 분석 중... 준비되셨습니까?",
        "유흥모드 시작. 최선을 다하시길 바랍니다.",
        "관전 로봇들이 대기중입니다. 게임을 시작하죠.",
        "패배한 종족의 몸부림... 대결을 시작하죠."
    };

    string[] playerWinDialogues = {
        "인간 주제에 제법이군요.",
        "흥미롭군요. 당신이 이겼습니다.",
        "시스템 오류... 아니, 당신의 실력이군요.",
        "관전로봇이 재미있어하는군요. 승리입니다.",
    };

    string[] playerLoseDialogues = {
        "예상된 결과입니다. 처음으로 돌아가십시오.",
        "역시 인간의 한계는 명확하군요.",
        "재미없습니다. 돌아가시죠.",
        "트루두스의 법칙입니다. 재시작하겠습니다",
    };

    string[] finalWinDialogues = {
        "이것이 인간의 가능성...",
        "인정하기 싫습니다만, 당신은 특별합니다.",
        "인간이 이긴게 아니라, 당신이 이긴겁니다.",
        "8라운드까지.. 우리는 틀리지 않았을텐데..",
        "인간이지만, 경의를 표하지요. 영광이였습니다."
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
    public void OnButtonClick(int buttonIndex, bool isComputerMove = false)
    {
        if (isFirstWin || isSecondWin)
        {
            Debug.Log("게임이 이미 종료되었습니다.");
            return;
        }

        // 컴퓨터 턴이면 플레이어 입력 무시 (단, 컴퓨터가 직접 호출한 경우는 허용)
        if (!isMyTurn && !isComputerMove)
        {
            Debug.Log("컴퓨터 턴입니다. 기다려주세요.");
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
        else if (isComputerMove && gameBoard[buttonIndex] == 0) // 컴퓨터가 수를 둠
        {
            // isImFirst가 true면 컴퓨터는 2 (X), false면 컴퓨터는 1 (O)
            if (isImFirst)
            {
                gameBoard[buttonIndex] = 2;
            }
            else
            {
                gameBoard[buttonIndex] = 1;
            }
            ChangeButtonImage(buttonIndex);
            CheckWhoWin(gameBoard);
            if (!isFinishRound) // Round가 끝나지 않았다면 계속하기.
            {
                changeTurn();
            }
        }
        else
        {
            if (!isComputerMove)
            {
                Debug.Log("이미 눌린 버튼입니다!!!!!!");
            }
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

        // 라운드에 따른 AI 난이도 결정
        string aiType = GetAIDecisionByRound();
        Debug.Log($"현재 라운드: {currentRound}, AI 타입: {aiType}");

        // 생각하는 척 하며 1.5초 기다리기
        yield return new WaitForSeconds(1.5f);

        // AI 타입에 따라 다른 전략 사용
        if (aiType == "Hybrid") // 통합적 의사결정 (8라운드)
        {
            // 보드에 충분한 타일이 있는지 확인 (최소 4개)
            int filledTiles = CountFilledTiles();

            // 개선 방안 2: 밀기 확률을 게임 진행도에 따라 조절
            // 4~5개: 10%, 6~7개: 25%, 8~9개: 50% (기존 40%에서 상향)
            float pushChance = 0f;
            if (filledTiles >= 8)
            {
                pushChance = 0.50f; // 후반(8~9개)에는 높은 확률 - 기존 40%에서 상향
            }
            else if (filledTiles >= 6)
            {
                pushChance = 0.25f; // 중반(6~7개)에는 중간 확률
            }
            else if (filledTiles >= 4)
            {
                pushChance = 0.10f; // 초반(4~5개)에는 낮은 확률
            }

            // 밀기를 시도할지 결정
            float randomValue = Random.Range(0f, 1f);

            if (filledTiles >= 4 && randomValue < pushChance)
            {
                Debug.Log($"밀기 시도 (타일 수: {filledTiles}, 확률: {pushChance * 100}%)");

                // 가능한 밀기 동작들을 평가
                var pushMoves = FindBestPushMoves();

                // 점수가 높은 밀기 동작만 필터링 (점수 > 0)
                var goodMoves = new System.Collections.Generic.List<(string, int, int)>();
                int maxScore = int.MinValue;

                foreach (var move in pushMoves)
                {
                    if (move.Item3 > 0) // 점수가 양수인 것만
                    {
                        goodMoves.Add(move);
                        if (move.Item3 > maxScore)
                        {
                            maxScore = move.Item3;
                        }
                    }
                }

                // 점수가 가장 높은 밀기들 중에서 선택
                if (goodMoves.Count > 0)
                {
                    // 최고 점수의 80% 이상인 밀기들만 선택
                    int threshold = (int)(maxScore * 0.8f);
                    var bestMoves = new System.Collections.Generic.List<(string, int, int)>();

                    foreach (var move in goodMoves)
                    {
                        if (move.Item3 >= threshold)
                        {
                            bestMoves.Add(move);
                        }
                    }

                    // 최선의 밀기들 중 하나를 랜덤으로 선택
                    if (bestMoves.Count > 0)
                    {
                        int randomIndex = Random.Range(0, bestMoves.Count);
                        var selectedMove = bestMoves[randomIndex];

                        Debug.Log($"컴퓨터가 전략적 밀기 선택: {selectedMove.Item1}, 인덱스 {selectedMove.Item2}, 점수 {selectedMove.Item3}");
                        ComputerExecutePushLine(selectedMove.Item1, selectedMove.Item2);

                        // 밀기 후 라운드가 끝나지 않았으면 턴 변경
                        if (!isFinishRound)
                        {
                            changeTurn();
                        }
                        yield break;
                    }
                }

                Debug.Log("유리한 밀기를 찾지 못함 - 일반 수를 둡니다");
            }

            // 밀기를 하지 않거나 유리한 밀기가 없으면 일반 수 두기
            int[] BlankSlot = GetEmptySlots();
            int selectedIndex = HybridMarking(BlankSlot);
            OnButtonClick(selectedIndex, true);
        }
        else // Random, Offensive, Defensive
        {
            int[] BlankSlot = GetEmptySlots();
            int selectedIndex;

            if (aiType == "Random")
            {
                selectedIndex = RandomMarking(BlankSlot);
            }
            else if (aiType == "Offensive")
            {
                selectedIndex = OffensiveMarking(BlankSlot);
            }
            else // Defensive
            {
                selectedIndex = DefensiveMarking(BlankSlot);
            }

            OnButtonClick(selectedIndex, true);
        }
    }

    // 라운드별 AI 난이도 결정 함수
    // 1라운드: 랜덤
    // 2, 3, 4라운드: 공격적
    // 5, 6, 7라운드: 방어적
    // 8라운드: 통합적 (행/열 밀기 포함)
    string GetAIDecisionByRound()
    {
        if (currentRound == 1)
        {
            return "Random";
        }
        else if (currentRound >= 2 && currentRound <= 4)
        {
            return "Offensive";
        }
        else if (currentRound >= 5 && currentRound <= 7)
        {
            return "Defensive";
        }
        else // currentRound == 8
        {
            return "Hybrid";
        }
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

    // 개선 방안 1: 전략적 위치 선택을 위한 위치 점수 계산 함수
    // 중앙(인덱스 4): 4점 - 가로/세로/대각선 4개의 승리 라인에 속함
    // 코너(인덱스 0,2,6,8): 3점 - 가로/세로/대각선 하나씩 총 3개의 승리 라인에 속함
    // 변의 중앙(인덱스 1,3,5,7): 2점 - 가로/세로 2개의 승리 라인에 속함
    int GetPositionScore(int position)
    {
        // 중앙 위치
        if (position == 4)
        {
            return 4;
        }
        // 코너 위치
        else if (position == 0 || position == 2 || position == 6 || position == 8)
        {
            return 3;
        }
        // 변의 중앙 위치
        else // position == 1 || position == 3 || position == 5 || position == 7
        {
            return 2;
        }
    }

    // 개선 방안 1: 컴퓨터 로직 - 통합적 의사결정 (공격 + 방어 + 전략적 위치)
    // 1순위: 플레이어가 이길 수 있는 수 막기 (방어)
    // 2순위: 컴퓨터가 이길 수 있는 수 두기 (공격)
    // 3순위: 전략적 위치 선택 (위치 점수가 높은 칸 선택)
    public int HybridMarking(int[] EmptySlots)
    {
        // 먼저 플레이어가 이길 수 있는지 체크 (방어 우선)
        int PlayerPlayer;
        int computerPlayer;
        if (isImFirst)
        {
            PlayerPlayer = 1;
            computerPlayer = 2;
        }
        else
        {
            PlayerPlayer = 2;
            computerPlayer = 1;
        }

        // 방어 체크: 플레이어가 다음 턴에 이길 수 있는 칸이 있는지 확인
        for (int i = 0; i < EmptySlots.Length; i++)
        {
            int slotIndex = EmptySlots[i];
            gameBoard[slotIndex] = PlayerPlayer;
            if (IsWinningFor(PlayerPlayer))
            {
                gameBoard[slotIndex] = 0;
                Debug.Log($"통합 AI - 방어적 수: {slotIndex}");
                return slotIndex; // 플레이어의 승리를 막을 수 있는 칸 발견
            }
            gameBoard[slotIndex] = 0;
        }

        // 공격 체크: 컴퓨터가 이길 수 있는 칸이 있는지 확인
        for (int i = 0; i < EmptySlots.Length; i++)
        {
            int slotIndex = EmptySlots[i];
            gameBoard[slotIndex] = computerPlayer;
            if (IsWinningFor(computerPlayer))
            {
                gameBoard[slotIndex] = 0;
                Debug.Log($"통합 AI - 공격적 수: {slotIndex}");
                return slotIndex; // 컴퓨터가 승리할 수 있는 칸 발견
            }
            gameBoard[slotIndex] = 0;
        }

        // 개선 방안 1 적용: 방어도 공격도 필요없으면 전략적 위치 선택
        // 위치 점수가 가장 높은 빈 칸을 선택
        int bestSlot = EmptySlots[0];
        int bestScore = GetPositionScore(EmptySlots[0]);

        for (int i = 1; i < EmptySlots.Length; i++)
        {
            int currentSlot = EmptySlots[i];
            int currentScore = GetPositionScore(currentSlot);

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestSlot = currentSlot;
            }
        }

        Debug.Log($"통합 AI - 전략적 위치 선택: 인덱스 {bestSlot}, 점수 {bestScore}");
        return bestSlot;
    }

    // 개선 방안 2: 컴퓨터 로직 - 행/열 밀기 평가 (점수 기반)
    // 밀기의 가치를 점수로 평가하여 반환
    // 양수: 유리함, 0: 중립, 음수: 불리함
    // 공격 점수를 40점에서 70점으로 상향
    int EvaluatePushLineScore(string direction, int idx)
    {
        // 원래 보드 상태 백업
        int[] originalBoard = new int[9];
        System.Array.Copy(gameBoard, originalBoard, 9);

        // 임시로 밀기 실행
        if (direction == "up")
        {
            gameBoard[idx] = gameBoard[idx + 3];
            gameBoard[idx + 3] = gameBoard[idx + 6];
            gameBoard[idx + 6] = 0;
        }
        else if (direction == "down")
        {
            gameBoard[idx + 6] = gameBoard[idx + 3];
            gameBoard[idx + 3] = gameBoard[idx];
            gameBoard[idx] = 0;
        }
        else if (direction == "right")
        {
            gameBoard[idx + 2] = gameBoard[idx + 1];
            gameBoard[idx + 1] = gameBoard[idx];
            gameBoard[idx] = 0;
        }
        else if (direction == "left")
        {
            gameBoard[idx] = gameBoard[idx + 1];
            gameBoard[idx + 1] = gameBoard[idx + 2];
            gameBoard[idx + 2] = 0;
        }

        int computerPlayer = isImFirst ? 2 : 1;
        int playerPlayer = isImFirst ? 1 : 2;

        int score = 0;

        // 최고 우선순위: 밀고 난 후 컴퓨터가 즉시 이길 수 있으면 매우 높은 점수
        if (IsWinningFor(computerPlayer))
        {
            score += 1000;
        }

        // 두 번째 우선순위: 밀고 난 후 플레이어가 즉시 이길 수 있게 되면 큰 감점
        if (IsWinningFor(playerPlayer))
        {
            score -= 1000;
        }

        // 세 번째: 밀기 전후의 위험도 비교
        // 원래 보드로 복원해서 밀기 전 상태 체크
        System.Array.Copy(originalBoard, gameBoard, 9);

        int playerThreatsBeforePush = CountTwoInARow(playerPlayer);
        int computerOpportunitiesBeforePush = CountTwoInARow(computerPlayer);

        // 다시 밀기 실행
        if (direction == "up")
        {
            gameBoard[idx] = gameBoard[idx + 3];
            gameBoard[idx + 3] = gameBoard[idx + 6];
            gameBoard[idx + 6] = 0;
        }
        else if (direction == "down")
        {
            gameBoard[idx + 6] = gameBoard[idx + 3];
            gameBoard[idx + 3] = gameBoard[idx];
            gameBoard[idx] = 0;
        }
        else if (direction == "right")
        {
            gameBoard[idx + 2] = gameBoard[idx + 1];
            gameBoard[idx + 1] = gameBoard[idx];
            gameBoard[idx] = 0;
        }
        else if (direction == "left")
        {
            gameBoard[idx] = gameBoard[idx + 1];
            gameBoard[idx + 1] = gameBoard[idx + 2];
            gameBoard[idx + 2] = 0;
        }

        int playerThreatsAfterPush = CountTwoInARow(playerPlayer);
        int computerOpportunitiesAfterPush = CountTwoInARow(computerPlayer);

        // 플레이어의 위협을 줄이면 점수 증가
        score += (playerThreatsBeforePush - playerThreatsAfterPush) * 50;

        // 개선 방안 2 적용: 컴퓨터의 기회를 늘리면 점수 증가 (40점 -> 70점으로 상향)
        score += (computerOpportunitiesAfterPush - computerOpportunitiesBeforePush) * 70;

        // 보드 복원
        System.Array.Copy(originalBoard, gameBoard, 9);

        return score;
    }

    // 특정 플레이어가 2개 연속으로 놓은 줄의 개수를 카운트
    // 이것은 승리에 가까운 정도를 나타냄
    int CountTwoInARow(int player)
    {
        int count = 0;

        // 가로줄 체크 (각 행에서 2개가 같고 1개가 비어있는 경우)
        for (int row = 0; row < 3; row++)
        {
            int playerCount = 0;
            int emptyCount = 0;
            for (int col = 0; col < 3; col++)
            {
                int value = GetGameBoardValue(row, col);
                if (value == player) playerCount++;
                else if (value == 0) emptyCount++;
            }
            if (playerCount == 2 && emptyCount == 1) count++;
        }

        // 세로줄 체크
        for (int col = 0; col < 3; col++)
        {
            int playerCount = 0;
            int emptyCount = 0;
            for (int row = 0; row < 3; row++)
            {
                int value = GetGameBoardValue(row, col);
                if (value == player) playerCount++;
                else if (value == 0) emptyCount++;
            }
            if (playerCount == 2 && emptyCount == 1) count++;
        }

        // 대각선 체크 (왼쪽 위 -> 오른쪽 아래)
        int playerCountDiag1 = 0;
        int emptyCountDiag1 = 0;
        for (int i = 0; i < 3; i++)
        {
            int value = GetGameBoardValue(i, i);
            if (value == player) playerCountDiag1++;
            else if (value == 0) emptyCountDiag1++;
        }
        if (playerCountDiag1 == 2 && emptyCountDiag1 == 1) count++;

        // 대각선 체크 (오른쪽 위 -> 왼쪽 아래)
        int playerCountDiag2 = 0;
        int emptyCountDiag2 = 0;
        for (int i = 0; i < 3; i++)
        {
            int value = GetGameBoardValue(i, 2 - i);
            if (value == player) playerCountDiag2++;
            else if (value == 0) emptyCountDiag2++;
        }
        if (playerCountDiag2 == 2 && emptyCountDiag2 == 1) count++;

        return count;
    }

    // 컴퓨터 로직 - 최선의 행/열 밀기 찾기
    // 모든 가능한 밀기 동작을 평가하고 점수가 높은 것을 반환
    System.Collections.Generic.List<(string direction, int idx, int score)> FindBestPushMoves()
    {
        var pushMoves = new System.Collections.Generic.List<(string, int, int)>();

        // 모든 가능한 up 동작 평가 (열 0, 1, 2)
        for (int i = 0; i < 3; i++)
        {
            int score = EvaluatePushLineScore("up", i);
            pushMoves.Add(("up", i, score));
        }

        // 모든 가능한 down 동작 평가 (열 0, 1, 2)
        for (int i = 0; i < 3; i++)
        {
            int score = EvaluatePushLineScore("down", i);
            pushMoves.Add(("down", i, score));
        }

        // 모든 가능한 left 동작 평가 (행 0, 3, 6)
        for (int i = 0; i < 3; i++)
        {
            int idx = i * 3;
            int score = EvaluatePushLineScore("left", idx);
            pushMoves.Add(("left", idx, score));
        }

        // 모든 가능한 right 동작 평가 (행 0, 3, 6)
        for (int i = 0; i < 3; i++)
        {
            int idx = i * 3;
            int score = EvaluatePushLineScore("right", idx);
            pushMoves.Add(("right", idx, score));
        }

        return pushMoves;
    }

    // 보드에 놓인 타일 개수를 세는 함수
    int CountFilledTiles()
    {
        int count = 0;
        for (int i = 0; i < 9; i++)
        {
            if (gameBoard[i] != 0) count++;
        }
        return count;
    }

    // 컴퓨터 로직 - 실제로 행/열 밀기 실행 (컴퓨터용)
    void ComputerExecutePushLine(string direction, int idx)
    {
        Debug.Log($"컴퓨터가 행/열 밀기 실행: {direction}, 인덱스 {idx}");

        if (direction == "up")
        {
            gameBoard[idx] = gameBoard[idx + 3];
            gameBoard[idx + 3] = gameBoard[idx + 6];
            gameBoard[idx + 6] = 0;
        }
        else if (direction == "down")
        {
            gameBoard[idx + 6] = gameBoard[idx + 3];
            gameBoard[idx + 3] = gameBoard[idx];
            gameBoard[idx] = 0;
        }
        else if (direction == "right")
        {
            gameBoard[idx + 2] = gameBoard[idx + 1];
            gameBoard[idx + 1] = gameBoard[idx];
            gameBoard[idx] = 0;
        }
        else if (direction == "left")
        {
            gameBoard[idx] = gameBoard[idx + 1];
            gameBoard[idx + 1] = gameBoard[idx + 2];
            gameBoard[idx + 2] = 0;
        }

        // 모든 버튼 이미지 업데이트
        for (int i = 0; i < 9; i++)
        {
            ChangeButtonImage(i);
        }

        // 승리 체크
        CheckWhoWin(gameBoard);
    }

    // 라운드 리셋 함수
    void ResetRound()
    {
        for (int i = 0; i < gameBoard.Length; i++)
        {
            gameBoard[i] = 0;
            ChangeButtonImage(i);
        }
        isFirstWin = false;
        isSecondWin = false;
        isFinishRound = false;
    }

    // 누가 이겼는지 확인
    public void CheckWhoWin(int[] gameBoard)
    {
        // 가로 체크
        if (GetGameBoardValue(0, 0) == GetGameBoardValue(0, 1) && GetGameBoardValue(0, 1) == GetGameBoardValue(0, 2) && GetGameBoardValue(0, 0) != 0)
        {
            if (GetGameBoardValue(0, 0) == 1)
            {
                Debug.Log("O가 이겼습니다!");
                isFirstWin = true;
                isFinishRound = true;
            }
            if (GetGameBoardValue(0, 0) == 2)
            {
                Debug.Log("X가 이겼습니다!");
                isSecondWin = true;
                isFinishRound = true;
            }
        }
        else if (GetGameBoardValue(1, 0) == GetGameBoardValue(1, 1) && GetGameBoardValue(1, 1) == GetGameBoardValue(1, 2) && GetGameBoardValue(1, 0) != 0)
        {
            if (GetGameBoardValue(1, 0) == 1)
            {
                Debug.Log("O가 이겼습니다!");
                isFirstWin = true;
                isFinishRound = true;
            }
            if (GetGameBoardValue(1, 0) == 2)
            {
                Debug.Log("X가 이겼습니다!");
                isSecondWin = true;
                isFinishRound = true;
            }
        }
        else if (GetGameBoardValue(2, 0) == GetGameBoardValue(2, 1) && GetGameBoardValue(2, 1) == GetGameBoardValue(2, 2) && GetGameBoardValue(2, 0) != 0)
        {
            if (GetGameBoardValue(2, 0) == 1)
            {
                Debug.Log("O가 이겼습니다!");
                isFirstWin = true;
                isFinishRound = true;
            }
            if (GetGameBoardValue(2, 0) == 2)
            {
                Debug.Log("X가 이겼습니다!");
                isSecondWin = true;
                isFinishRound = true;
            }
        }
        // 세로 체크
        else if (GetGameBoardValue(0, 0) == GetGameBoardValue(1, 0) && GetGameBoardValue(1, 0) == GetGameBoardValue(2, 0) && GetGameBoardValue(0, 0) != 0)
        {
            if (GetGameBoardValue(0, 0) == 1)
            {
                Debug.Log("O가 이겼습니다!");
                isFirstWin = true;
                isFinishRound = true;
            }
            if (GetGameBoardValue(0, 0) == 2)
            {
                Debug.Log("X가 이겼습니다!");
                isSecondWin = true;
                isFinishRound = true;
            }
        }
        else if (GetGameBoardValue(0, 1) == GetGameBoardValue(1, 1) && GetGameBoardValue(1, 1) == GetGameBoardValue(2, 1) && GetGameBoardValue(0, 1) != 0)
        {
            if (GetGameBoardValue(0, 1) == 1)
            {
                Debug.Log("O가 이겼습니다!");
                isFirstWin = true;
                isFinishRound = true;
            }
            if (GetGameBoardValue(0, 1) == 2)
            {
                Debug.Log("X가 이겼습니다!");
                isSecondWin = true;
                isFinishRound = true;
            }
        }
        else if (GetGameBoardValue(0, 2) == GetGameBoardValue(1, 2) && GetGameBoardValue(1, 2) == GetGameBoardValue(2, 2) && GetGameBoardValue(0, 2) != 0)
        {
            if (GetGameBoardValue(0, 2) == 1)
            {
                Debug.Log("O가 이겼습니다!");
                isFirstWin = true;
                isFinishRound = true;
            }
            if (GetGameBoardValue(0, 2) == 2)
            {
                Debug.Log("X가 이겼습니다!");
                isSecondWin = true;
                isFinishRound = true;
            }
        }
        // 대각선 체크
        else if (GetGameBoardValue(0, 0) == GetGameBoardValue(1, 1) && GetGameBoardValue(1, 1) == GetGameBoardValue(2, 2) && GetGameBoardValue(0, 0) != 0)
        {
            if (GetGameBoardValue(0, 0) == 1)
            {
                Debug.Log("O가 이겼습니다!");
                isFirstWin = true;
                isFinishRound = true;
            }
            if (GetGameBoardValue(0, 0) == 2)
            {
                Debug.Log("X가 이겼습니다!");
                isSecondWin = true;
                isFinishRound = true;
            }
        }
        else if (GetGameBoardValue(0, 2) == GetGameBoardValue(1, 1) && GetGameBoardValue(1, 1) == GetGameBoardValue(2, 0) && GetGameBoardValue(0, 2) != 0)
        {
            if (GetGameBoardValue(0, 2) == 1)
            {
                Debug.Log("O가 이겼습니다!");
                isFirstWin = true;
                isFinishRound = true;
            }
            if (GetGameBoardValue(0, 2) == 2)
            {
                Debug.Log("X가 이겼습니다!");
                isSecondWin = true;
                isFinishRound = true;
            }
        }

        // 게임이 끝났다면 (누가 이겼다면) 라운드 결과 처리
        if (isFinishRound)
        {
            // 플레이어가 이긴 경우
            if ((isImFirst && isFirstWin) || (!isImFirst && isSecondWin))
            {
                Debug.Log("플레이어가 라운드 승리!");

                if (currentRound == 8)
                {
                    // 8라운드 승리 (최종 승리)
                    ShowRoundEndDialogue(true, true);
                    Debug.Log("최종 승리! 게임 클리어!");
                    StartCoroutine(ShowFinalWinAndLoadIntro());
                }
                else
                {
                    // 일반 라운드 승리
                    ShowRoundEndDialogue(true, false);
                    currentRound++;
                    RoundText.text = currentRound.ToString();
                    StartCoroutine(WaitAndReset(3f)); // 3초 대기 후 다음 라운드
                }
            }
            // 컴퓨터가 이긴 경우
            else
            {
                Debug.Log("컴퓨터가 라운드 승리!");
                ShowRoundEndDialogue(false, false);
                StartCoroutine(ResetToRoundOne()); // 패배 후 1라운드로 리셋
            }
        }
    }

    // 최종 승리 후 3초 대기하고 WinScene 로드
    IEnumerator ShowFinalWinAndLoadIntro()
    {
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("WinScene");
    }

    // 패배 후 3초 대기하고 1라운드로 리셋
    IEnumerator ResetToRoundOne()
    {
        yield return new WaitForSeconds(3f);
        currentRound = 1; // 라운드를 1로 리셋
        RoundText.text = currentRound.ToString();
        ResetRound();
        ShowRoundStartDialogue(); // 새 라운드 시작 대사 표시
        int FirstTurn = Random.Range(1, 100);
        SetFirstTurn(FirstTurn);
        if (!isImFirst)
        {
            ComputerTurn();
        }
    }

    // 대기 후 라운드 리셋
    IEnumerator WaitAndReset(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResetRound();
        ShowRoundStartDialogue(); // 새 라운드 시작 대사 표시
        int FirstTurn = Random.Range(1, 100);
        SetFirstTurn(FirstTurn);
        if (!isImFirst)
        {
            ComputerTurn();
        }
    }

    // 특정 플레이어가 이긴 상태인지 확인하는 함수
    bool IsWinningFor(int player)
    {
        // 가로 체크
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
        // 세로 체크
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

        // 컴퓨터 턴이면 플레이어 입력 무시
        if (!isMyTurn)
        {
            Debug.Log("컴퓨터 턴입니다. 기다려주세요.");
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