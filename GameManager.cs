using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;

namespace PracticeGame
{

    class GameManager
    {
        GameTable _table;
        Player _player;

        Random rand = new Random();
        Queue<Marble> _qMarble;

        public GameManager()
        {
            _table = new GameTable();
            _player = new Player();

            _qMarble = new Queue<Marble>();
        }

        public void Play()
        {
            // 가로, 세로의 개수 변수 선언
            _table.Width = 25;
            _table.Height = 30;

            // 게임 테이블 안에 내용을 저장할 배열 선언
            // 가로 첫번쨰와 마지막, 세로 첫번째와 마지막에 ■ 들어가기 때문에 내용을 저장할 때는 -2씩 크기만큼 선언
            string[,] tableArr = new string[_table.Height, _table.Width];
            string space = "　";
            string block = "■";

            Marble[,] marbleArr = new Marble[_table.Height, _table.Height];
            Marble nextMarble;

            int turnNum = 0;

            // 단계 넣을 변수 선언
            int currentStage = 1;

            // A면 뒤로 이동
            // D면 앞으로 이동
            int posX;
            int posY;

            //shapeIndex = rand.Next(0, (int)MarbleShape.None);
            //// 플레이어가 ●, ★, ◆, ♠, ♥, ♣  이 중에서 랜덤하게 나오게 출력
            //_player.SetPlayerShape((MarbleShape)shapeIndex);

            //colorIndex = rand.Next(0, (int)MarbleColor.None);

            SettingStartMarble(tableArr, marbleArr, currentStage);

            // 맨 처음 플레이어 위치 설정
            posX = _table.Width / 2 + 4;
            posY = _table.Height - 2;

            // 레벨에 따른 maxHeightIndex 수정
            //_table.MaxHeightIndex = _table.SetMarbleNum;

            // 다 세팅되고 나서 테이블 출력
            //_table.SetTable(tableArr, currentStage, _player.Score, marbleArr);

            while (true)
            {
                // 플레이어가 져서 다음 스테이지로 못 가고 다시 시작
                if (_table.IsFinish)
                {
                    _player.Score = 0;
                    turnNum = 0;
                    _table.TurnEndBlock = 0;
                    Console.Clear();
                    SettingStartMarble(tableArr, marbleArr, currentStage);
                    _table.IsFinish = false;
                }

                // 플레이어가 이겨서 다음 스테이지로 간다면
                if (_table.IsStageUp)
                {
                    // 한 턴이 끝날 때마다 스테이지 1씩 증가
                    currentStage += 1;
                    _player.Score = 0;
                    _table.TurnEndBlock = 0;
                    turnNum = 0;
                    // 스테이지 값 수정
                    Console.SetCursorPosition(_table.Width + 9, _table.Height - 3);
                    Console.WriteLine($"\t\t\t{currentStage}");
                    Console.Clear();
                    SettingStartMarble(tableArr, marbleArr, currentStage);
                    _table.IsStageUp = false;
                }

                // 입력한 키에 대해 게임에 출력하는 함수 
                KeyInputPrintOutput(tableArr, turnNum, marbleArr);

                // 턴이 끝나면 ■로 한줄을 채움
                if (_table.EliminateNum < 3)
                {
                    _table.MaxHeightIndex += 2;
                    _table.TurnEndBlock++;

                    for (int i = _table.MaxHeightIndex - _table.TurnEndBlock + 1; i > _table.TurnEndBlock - 1; i--)
                    {
                        for (int j = 0; j < tableArr.GetLength(1); j++)
                        {
                            if (tableArr[i, j] != block && tableArr[i, j] != space)
                            {
                                tableArr[i + 1, j] = tableArr[i, j];
                                marbleArr[i + 1, j] = marbleArr[i, j];
                                marbleArr[i + 1, j].ChangePosition(new Vector2(j, i + 1));
                            }
                        }
                    }
                    for (int i = 0; i < tableArr.GetLength(1); i++)
                    {
                        tableArr[_table.TurnEndBlock, i] = block;
                        marbleArr[_table.TurnEndBlock, i] = new Marble(MarbleShape.None, MarbleColor.None);
                    }
                }
                // block이 여러 줄로 쌓여 있을 때 같은 도형으로 몇 번 서로 없애는 것에 성공하면 한 줄 없애기
                else
                {
                    // 턴이 끝나게 되면 플레이어가 이겨서 한 줄 늘어나 있던 블록을 하나씩 없애서 배열에 저장하면 됨
                    if (_table.TurnEndBlock > 0)
                    {
                        for (int i = 0; i < tableArr.GetLength(1); i++)
                        {
                            if (i == 0 || i == _table.Width - 1)
                            {
                                continue;
                            }
                            tableArr[_table.TurnEndBlock, i] = space;
                            marbleArr[_table.TurnEndBlock, i] = new Marble(MarbleShape.None, MarbleColor.None);
                        }
                        for (int i = _table.TurnEndBlock; i < _table.MaxHeightIndex + _table.TurnEndBlock; i++)
                        {
                            for (int j = 0; j < tableArr.GetLength(1); j++)
                            {
                                if (tableArr[i, j] != block && tableArr[i, j] != space)
                                {
                                    tableArr[i - 1, j] = tableArr[i, j];
                                    marbleArr[i - 1, j] = marbleArr[i, j];
                                    marbleArr[i - 1, j].ChangePosition(new Vector2(j, i - 1));
                                    tableArr[i, j] = space;
                                    marbleArr[i, j] = new Marble(MarbleShape.None, MarbleColor.None);
                                }
                            }
                        }
                        _table.TurnEndBlock--;
                        _table.MaxHeightIndex--;
                    }
                    else
                    {
                        _table.TurnEndBlock = 0;
                    }

                }

                _table.EliminateNum = 0;
                turnNum = 0;

                posX = _table.Width / 2 + 4;
                posY = _table.Height - 2;

                // player가 도형을 쌓는데 블록이 한줄 안 늘어나고 기존과 같다면 동일하게 재생성하고
                // 블록이 한 줄 늘어나면 생긴 블록 뒤에 한 줄이 느는것 뿐만 아니라 그 동안 넣었던 도형들은 다음 줄에 그대로 나와야 함
                Console.WriteLine();
                Console.Clear();
                for (int i = 0; i < tableArr.GetLength(0); i++)
                {
                    for (int j = 0; j < tableArr.GetLength(1); j++)
                    {
                        if (j == (_table.Width / 2) - 1 && i == _table.Height - 2)
                        {
                            tableArr[i, j] = _player.PlayerShape;
                            marbleArr[i, j] = new Marble(_player.PlayerMarble.Shape, _player.PlayerMarble.Color, new Vector2(j, i));
                        }
                        if (tableArr[i, j] != block && tableArr[i, j] != space)
                        {
                            _table.PrintShapeColor(marbleArr[i, j].Color);
                            Console.Write(_player.SetPlayerShape(marbleArr[i, j].Shape));
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write(tableArr[i, j]);
                        }
                        // 스테이지, 플레이어 스코어 출력
                        if (i == _table.Height - 3 && j == _table.Width - 1)
                        {
                            Console.Write($"\t『   Stage : {currentStage}");
                        }
                        if (i == _table.Height - 2 && j == _table.Width - 1)
                        {
                            Console.Write("\t　   Score : " + _player.Score + "   』");
                        }

                        // 숫자, 모양에 따른 점수 옆에 나오게 하기
                        if (i == _table.Height - 27 && j == _table.Width - 1)
                        {
                            Console.Write("\t▣▣▣▣▣▣▣▣▣▣▣▣");
                            Console.Write("\t▣▣▣▣▣▣▣▣");
                        }
                        if (i == _table.Height - 26 && j == _table.Width - 1)
                        {
                            Console.Write("\t▣");
                            Console.Write("\t\t      ▣");
                            Console.Write("\t▣");
                            Console.Write("\t      ▣");
                        }
                        if (i == _table.Height - 25 && j == _table.Width - 1)
                        {
                            Console.Write("\t▣  ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("■  ");
                            Console.Write("빨간색 : ");
                            Console.ResetColor();
                            Console.Write("1점");
                            Console.Write("  ▣");

                            Console.Write("\t▣");
                            Console.Write("  ● : ");
                            Console.Write("1점");
                            Console.Write("  ▣");
                        }
                        if (i == _table.Height - 24 && j == _table.Width - 1)
                        {
                            Console.Write("\t▣  ");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("■  ");
                            Console.Write("노란색 : ");
                            Console.ResetColor();
                            Console.Write("3점");
                            Console.Write("  ▣");

                            Console.Write("\t▣");
                            Console.Write("  ★ : ");
                            Console.Write("5점");
                            Console.Write("  ▣");
                        }
                        if (i == _table.Height - 23 && j == _table.Width - 1)
                        {
                            Console.Write("\t▣  ");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("■  ");
                            Console.Write("하늘색 : ");
                            Console.ResetColor();
                            Console.Write("2점");
                            Console.Write("  ▣");

                            Console.Write("\t▣");
                            Console.Write("  ◆ : ");
                            Console.Write("2점");
                            Console.Write("  ▣");
                        }
                        if (i == _table.Height - 22 && j == _table.Width - 1)
                        {
                            Console.Write("\t▣  ");
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write("■  ");
                            Console.Write("초록색 : ");
                            Console.ResetColor();
                            Console.Write("1점");
                            Console.Write("  ▣");

                            Console.Write("\t▣");
                            Console.Write("  ♠ : ");
                            Console.Write("3점");
                            Console.Write("  ▣");
                        }
                        if (i == _table.Height - 21 && j == _table.Width - 1)
                        {
                            Console.Write("\t▣");
                            Console.Write("\t\t      ▣");

                            Console.Write("\t▣");
                            Console.Write("  ♥ : ");
                            Console.Write("3점");
                            Console.Write("  ▣");
                        }
                        if (i == _table.Height - 20 && j == _table.Width - 1)
                        {
                            Console.Write("\t▣▣▣▣▣▣▣▣▣▣▣▣");

                            Console.Write("\t▣");
                            Console.Write("  ♣ : ");
                            Console.Write("2점");
                            Console.Write("  ▣");
                        }
                        if (i == _table.Height - 19 && j == _table.Width - 1)
                        {
                            Console.Write("\t\t\t\t\t▣");
                            Console.Write("\t      ▣");
                        }
                        if (i == _table.Height - 18 && j == _table.Width - 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("\t  ★  ");
                            Console.Write("노란색 별 : ");
                            Console.ResetColor();
                            Console.Write("10점");
                            Console.Write("\t\t▣▣▣▣▣▣▣▣");
                        }

                        if (i == _table.Height - 13 && j == _table.Width - 1)
                        {
                            Console.Write($"\t■■■■■■");
                        }
                        if (i == _table.Height - 12 && j == _table.Width - 1)
                        {
                            Console.Write($"\t■　　　　■");
                        }
                        if (i == _table.Height - 11 && j == _table.Width - 1)
                        {
                            Console.Write($"\t■　 ");
                            // 여기에 다음에 나올 것이 들어가야 함
                            Console.Write("　   ■");
                        }
                        if (i == _table.Height - 10 && j == _table.Width - 1)
                        {
                            Console.Write($"\t■　　　　■");
                        }
                        if (i == _table.Height - 9 && j == _table.Width - 1)
                        {
                            Console.Write($"\t■■■■■■");
                        }

                        // 도달까지 필요한 점수 출력
                        if (i == _table.Height - 6 && j == _table.Width - 1)
                        {
                            Console.Write($"\t▶　{currentStage}단계 통과 시 필요한 score : {_table.StageUpScore}");
                        }

                        // 다음에 나올 도형과 색상 나오게 하기
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();

                nextMarble = _qMarble.Peek();
                Console.SetCursorPosition(_table.Width + 36, _table.Height - 11);
                _table.PrintShapeColor(nextMarble.Color);
                Console.Write($"{_player.SetPlayerShape(nextMarble.Shape)}");
                Console.ResetColor();
            }
        }

        void KeyInputPrintOutput(string[,] gameArr, int turnNum, Marble[,] marbles)
        {
            int posX = gameArr.GetLength(1) / 2 + 10;
            int posY = gameArr.GetLength(0) - 2;

            // 비어있는 공간의 Y인덱스 저장할 변수 선언
            int spaceYIndex;

            // 검사해야 하는 라인 변수 선언
            int examineCount = 0;
            // 같은 것이 있어서 삭제하기 위한 bool형 변수 선언
            bool isXSame = false;
            bool isYSame = false;
            bool isLeft = false;

            int playerIndex = 0;
            int colorIndex = 0;

            Marble nextMarble;
            Marble playerMarble;
            Marble popMarble;

            // 시간 측정하는 변수 선언
            Stopwatch stopwatch = new Stopwatch();
            ConsoleKeyInfo keyInput;

            Stack<Marble> sMarbles = new Stack<Marble>();

            Console.SetCursorPosition(posX, posY);
            while (turnNum < _table.TurnMaxNum)
            {
                keyInput = Console.ReadKey(true);
                if (keyInput.Key == ConsoleKey.Enter)
                {
                    // 나중에 함수 사용
                    // 놓은 위치에 도형이 없으면 posY = 1에서 생성
                    spaceYIndex = _table.TurnEndBlock + 1;

                    // 비어 있지 않은 공간 확인할 때까지 반복
                    while (gameArr[spaceYIndex, posX / 2] != "　")
                    {
                        spaceYIndex++;
                    }
                    if (spaceYIndex >= _table.Height - 4)
                    {
                        _table.IsFinish = true;
                        break;
                    }
                    if (spaceYIndex > _table.MaxHeightIndex)
                    {
                        _table.MaxHeightIndex = spaceYIndex;
                    }

                    // 내가 쏜 도형이 있는 공간의 X좌표 인덱스 즉, 옆에 있는 도형과 색깔과 모양이 같으면 삭제
                    // 옆옆에도 같은게 있는지 없는지 확인 필요(이미 세팅된 도형 중에 같은게 있을 수 있기 때문) => 만약 옆에 있는 도형과 같은데 옆에 있는 도형 바로 위의 도형도 같으면??
                    // 일단 내가 쏜 공간의 양 옆에 같은게 있는지 확인 먼저
                    if (marbles[spaceYIndex, posX / 2 + 1].Shape == _player.PlayerMarble.Shape && marbles[spaceYIndex, posX / 2 + 1].Color == _player.PlayerMarble.Color)
                    {
                        examineCount = 0;
                        while (marbles[spaceYIndex, (posX / 2) + (examineCount + 1)].Shape == _player.PlayerMarble.Shape && marbles[spaceYIndex, (posX / 2) + (examineCount + 1)].Color == _player.PlayerMarble.Color)
                        {
                            // 옆에 있는 마블 밑에 도형들이 달려 있으면 점수 추가하고 밑에 것들 없애기
                            if (marbles[spaceYIndex + 1, (posX / 2) + (examineCount + 1)].Shape != MarbleShape.None)
                            {
                                int lineMarble = 0;
                                while (marbles[spaceYIndex + (lineMarble + 1), (posX / 2) + (examineCount + 1)].Shape != MarbleShape.None)
                                {
                                    sMarbles.Push(marbles[spaceYIndex + (lineMarble + 1), (posX / 2) + (examineCount + 1)]);
                                    gameArr[spaceYIndex + (lineMarble + 1), (posX / 2) + (examineCount + 1)] = "　";
                                    marbles[spaceYIndex + (lineMarble + 1), (posX / 2) + (examineCount + 1)] = new Marble(MarbleShape.None, MarbleColor.None);
                                    lineMarble++;
                                }
                            }
                            gameArr[spaceYIndex, (posX / 2) + (examineCount + 1)] = "　";
                            marbles[spaceYIndex, (posX / 2) + (examineCount + 1)] = new Marble(MarbleShape.None, MarbleColor.None);
                            examineCount++;
                        }

                        // 일단 같은 도형 출력하고 나서
                        // 시간이 지나고 없어지게 만들기
                        Console.SetCursorPosition(posX, spaceYIndex);

                        // 색상 칠함
                        _table.PrintShapeColor(_player.PlayerMarble.Color);
                        Console.Write(_player.PlayerShape);
                        Console.ResetColor();

                        _player.Score += _player.PlayerMarble.GetMarbleScore(_player.PlayerMarble.Color, _player.PlayerMarble.Shape) * examineCount;

                        isXSame = true;
                        isLeft = false;
                    }

                    if (marbles[spaceYIndex, posX / 2 - 1].Shape == _player.PlayerMarble.Shape && marbles[spaceYIndex, posX / 2 - 1].Color == _player.PlayerMarble.Color)
                    {
                        examineCount = 0;
                        while (marbles[spaceYIndex, (posX / 2) - (examineCount + 1)].Shape == _player.PlayerMarble.Shape && marbles[spaceYIndex, (posX / 2) - (examineCount + 1)].Color == _player.PlayerMarble.Color)
                        {
                            // 옆에 있는 마블 밑에 도형들이 달려 있으면 점수 추가하고 밑에 것들 없애기
                            if (marbles[spaceYIndex + 1, (posX / 2) - (examineCount + 1)].Shape != MarbleShape.None)
                            {
                                int lineMarble = 0;
                                while (marbles[spaceYIndex + (lineMarble + 1), (posX / 2) - (examineCount + 1)].Shape != MarbleShape.None)
                                {
                                    sMarbles.Push(marbles[spaceYIndex + (lineMarble + 1), (posX / 2) - (examineCount + 1)]);
                                    gameArr[spaceYIndex + (lineMarble + 1), (posX / 2) - (examineCount + 1)] = "　";
                                    marbles[spaceYIndex + (lineMarble + 1), (posX / 2) - (examineCount + 1)] = new Marble(MarbleShape.None, MarbleColor.None);
                                    lineMarble++;
                                }
                            }
                            gameArr[spaceYIndex, (posX / 2) - (examineCount + 1)] = "　";
                            marbles[spaceYIndex, (posX / 2) - (examineCount + 1)] = new Marble(MarbleShape.None, MarbleColor.None);
                            examineCount++;
                        }

                        // 일단 같은 도형 출력하고 나서
                        // 시간이 지나고 없어지게 만들기
                        Console.SetCursorPosition(posX, spaceYIndex);

                        // 색상 칠함
                        _table.PrintShapeColor(_player.PlayerMarble.Color);
                        Console.Write(_player.PlayerShape);
                        Console.ResetColor();

                        _player.Score += _player.PlayerMarble.GetMarbleScore(_player.PlayerMarble.Color, _player.PlayerMarble.Shape) * examineCount;

                        isXSame = true;
                        isLeft = true;
                    }

                    // 도형이 있는 공간의 Y좌표 인덱스 - 1 즉, 위에 있는 도형과 올린 도형과 색깔이 같으면
                    if (marbles[spaceYIndex - 1, posX / 2].Shape == _player.PlayerMarble.Shape && marbles[spaceYIndex - 1, posX / 2].Color == _player.PlayerMarble.Color)
                    {
                        examineCount = 0;
                        while (marbles[spaceYIndex - (examineCount + 1), posX / 2].Shape == _player.PlayerMarble.Shape && marbles[spaceYIndex - (examineCount + 1), posX / 2].Color == _player.PlayerMarble.Color)
                        {
                            gameArr[spaceYIndex - (examineCount + 1), posX / 2] = "　";
                            marbles[spaceYIndex - (examineCount + 1), posX / 2] = new Marble(MarbleShape.None, MarbleColor.None);
                            examineCount++;
                        }

                        // 일단 같은 도형 출력하고 나서
                        // 시간이 지나고 없어지게 만들기
                        Console.SetCursorPosition(posX, spaceYIndex);

                        // 색상 칠함
                        _table.PrintShapeColor(_player.PlayerMarble.Color);
                        Console.Write(_player.PlayerShape);
                        Console.ResetColor();

                        // 생성된 블록 바로 아래에서 제거하면 +1씩 증가시키기
                        if (gameArr[spaceYIndex - 2, posX / 2] == "■")
                        {
                            _table.EliminateNum++;
                        }

                        _player.Score += _player.PlayerMarble.GetMarbleScore(_player.PlayerMarble.Color, _player.PlayerMarble.Shape) * examineCount;

                        isYSame = true;
                    }

                    // 위에 있는 도형과 올린 도형이 같지 않으면 그 아래에 플레이어 도형 출력
                    if (!isXSame && !isYSame)
                    {
                        gameArr[spaceYIndex, posX / 2] = _player.PlayerShape;
                        Console.SetCursorPosition(posX, spaceYIndex);

                        _table.PrintShapeColor(_player.PlayerMarble.Color);
                        Console.Write(_player.PlayerShape);
                        Console.ResetColor();

                        marbles[spaceYIndex, posX / 2] = new Marble(_player.PlayerMarble.Shape, _player.PlayerMarble.Color, new Vector2(posX / 2, spaceYIndex));
                    }



                    if (isYSame)
                    {
                        while (true)
                        {
                            stopwatch.Start();
                            if (stopwatch.ElapsedMilliseconds / 1000 > 0.8f)
                            {
                                for (int i = 0; i < examineCount + 1; i++)
                                {
                                    Console.SetCursorPosition(posX, spaceYIndex - i);
                                    Console.Write("　");
                                }
                                stopwatch.Reset();
                                break;
                            }
                        }

                        // 플레이어 점수 수정
                        Console.SetCursorPosition(_table.Width + 44, _table.Height - 2);
                        Console.WriteLine($"{_player.Score}");
                        if (_player.Score > _table.StageUpScore)
                        {
                            _table.IsStageUp = true;
                            break;
                        }
                    }


                    if (isXSame)
                    {
                        int x = 0;
                        while (true)
                        {
                            stopwatch.Start();
                            if (stopwatch.ElapsedMilliseconds / 1000 > 0.8f)
                            {
                                for (int i = 0; i < examineCount + 1; i++)
                                {
                                    x = isLeft ? posX - (i * 2) : posX + (i * 2);
                                    Console.SetCursorPosition(x, spaceYIndex);
                                    Console.Write("　");
                                }

                                if (sMarbles.Count > 0)
                                {
                                    while (sMarbles.Count > 0)
                                    {
                                        //left일 때 삭제 됐음
                                        popMarble = sMarbles.Pop();
                                        _player.Score += _player.PlayerMarble.GetMarbleScore(popMarble.Color, popMarble.Shape);
                                        Console.SetCursorPosition(popMarble.Location.posX * 2, popMarble.Location.posY);
                                        Console.Write("　");
                                    }
                                }
                                stopwatch.Reset();
                                break;
                            }
                        }

                        // 플레이어 점수 수정
                        Console.SetCursorPosition(_table.Width + 44, _table.Height - 2);
                        Console.WriteLine($"{_player.Score}");
                        if (_player.Score > _table.StageUpScore)
                        {
                            _table.IsStageUp = true;
                            break;
                        }
                    }

                    isYSame = false;
                    isXSame = false;
                    isLeft = false;

                    // 턴이 한 번 넘어갈 수록 
                    turnNum++;

                    // 플레이어 도형 다른 모양으로 랜덤하게 생성
                    playerIndex = rand.Next(0, (int)MarbleShape.None);

                    colorIndex = rand.Next(0, (int)MarbleColor.None);

                    _qMarble.Enqueue(new Marble((MarbleShape)playerIndex, (MarbleColor)colorIndex));
                    //_player.SetPlayerShape((MarbleShape)playerIndex);
                    //_player.SetPlayerMarble((MarbleShape)playerIndex, (MarbleColor)colorIndex);

                    playerMarble = _qMarble.Dequeue();
                    //_player.SetPlayerShape(playerMarble.Shape);
                    _player.SetPlayerMarble(playerMarble.Shape, playerMarble.Color);

                    // 고쳐야 함
                    nextMarble = _qMarble.Peek();
                    Console.SetCursorPosition(_table.Width + 36, _table.Height - 11);
                    _table.PrintShapeColor(nextMarble.Color);
                    Console.Write($"{_player.SetPlayerShape(nextMarble.Shape)}");
                    Console.ResetColor();
                }
                else if (keyInput.Key == ConsoleKey.A)
                {
                    posX -= 2;
                    Console.SetCursorPosition(posX + 2, posY);
                    Console.WriteLine("　");

                }
                else if (keyInput.Key == ConsoleKey.D)
                {
                    posX += 2;
                    Console.SetCursorPosition(posX - 2, posY);
                    Console.WriteLine("　");
                }

                if (posX < 4)
                {
                    posX = 2;
                }
                // ■ 문자표가 2칸을 차지하므로 10 * 2
                if (posX > 46)
                {
                    posX = 46;
                }

                // A, D로 움직이지 않고도 첫 시작에 바로 나오게 하는 방법
                Console.SetCursorPosition(posX, posY);

                _table.PrintShapeColor(_player.PlayerMarble.Color);
                Console.Write(_player.PlayerShape);
                Console.ResetColor();
            }
        }


        public void SettingStartMarble(String[,] tableArr, Marble[,] marbleArr, int currentStage)
        {
            // 게임 시작 전 단계에 따른 난이도 조절을 위해 미리 마블을 세팅하기 위한 변수
            float count = 0;
            int shapeIndex = 0;
            int colorIndex = 0;
            string space = "　";
            Marble playerMarble;
            Marble nextMarble;

            _table.setAccordLevel(currentStage);

            for (int i = 0; i < tableArr.GetLength(0); i++)
            {
                for (int j = 0; j < tableArr.GetLength(1); j++)
                {
                    tableArr[i, j] = string.Empty;
                    marbleArr[i, j] = new Marble();
                }
            }

            count = ((_table.Width - 2) / _table.SetMarbleNum) / 2.0f;
            if (count - (int)count == 0.5f)
            {
                count += (count - (int)count);
            }

            for (int i = 0; i < _table.SetMarbleNum; i++)
            {
                for (int j = 1; j < _table.Width - 1; j++)
                {
                    if (j >= (i * count) + 1 && j < _table.Width - ((i * count) + 1))
                    {
                        shapeIndex = rand.Next(0, (int)MarbleShape.None);
                        colorIndex = rand.Next(0, (int)MarbleColor.None);
                        tableArr[i + 1, j] = _player.SetPlayerShape((MarbleShape)shapeIndex);
                        marbleArr[i + 1, j] = new Marble((MarbleShape)shapeIndex, (MarbleColor)colorIndex, new Vector2(j, i + 1));
                    }
                    else
                    {
                        tableArr[i + 1, j] = space;
                    }
                }
            }

            _table.SetTable(tableArr, currentStage, _player.Score, marbleArr);

            for (int i = 0; i < 3; i++)
            {
                shapeIndex = rand.Next(0, (int)MarbleShape.None);
                colorIndex = rand.Next(0, (int)MarbleColor.None);
                _qMarble.Enqueue(new Marble((MarbleShape)shapeIndex, (MarbleColor)colorIndex));
            }

            playerMarble = _qMarble.Dequeue();
            _player.SetPlayerMarble(playerMarble.Shape, playerMarble.Color);

            nextMarble = _qMarble.Peek();
            Console.SetCursorPosition(_table.Width + 36, _table.Height - 11);
            _table.PrintShapeColor(nextMarble.Color);
            Console.Write($"{_player.SetPlayerShape(nextMarble.Shape)}");
            Console.ResetColor();
        }
    }

}
