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

        public GameManager()
        {
            _table = new GameTable();
            _player = new Player();
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

            int turnNum = 0;
            int shapeIndex;
            int colorIndex;

            // 단계 넣을 변수 선언
            int currentStage = 1;

            // A면 뒤로 이동
            // D면 앞으로 이동
            int posX;
            int posY;

            shapeIndex = rand.Next(0, (int)MarbleShape.None);

            // 플레이어가 ●, ★, ◆, ♠, ♥, ♣  이 중에서 랜덤하게 나오게 출력
            _player.setPlayerShape((MarbleShape)shapeIndex);

            colorIndex = rand.Next(0, (int)MarbleColor.None);
            _table.SetTable(tableArr, currentStage, _player.Score, marbleArr);
            _player.SetPlayerMarble((MarbleShape)shapeIndex, (MarbleColor)colorIndex);

            // 맨 처음 플레이어 위치 설정
            posX = _table.Width / 2 + 4;
            posY = _table.Height - 2;


            // 레벨에 따른 제약 조건
            _table.setAccordLevel(currentStage);

            for(int i=0; i<marbleArr.GetLength(0); i++)
            {
                for(int j=0; j<marbleArr.GetLength(1); j++)
                {
                    marbleArr[i, j] = new Marble();
                }
            }

            while (true)
            {
                // 플레이어가 져서 다음 스테이지로 못 가고 다시 시작
                if (_table.IsFinish)
                {
                    _player.Score = 0;
                    turnNum = 0;
                    Console.Clear();
                    _table.SetTable(tableArr, currentStage, _player.Score, marbleArr);
                }

                // 플레이어가 이겨서 다음 스테이지로 간다면
                if (_table.IsStageUp)
                {
                    // 한 턴이 끝날 때마다 스테이지 1씩 증가
                    currentStage += 1;
                    _player.Score = 0;
                    turnNum = 0;
                    // 스테이지 값 수정
                    //Console.SetCursorPosition(width + 5, height - 2);
                    //Console.WriteLine($"\t\t\t{currentStage}");
                    Console.Clear();
                    _table.SetTable(tableArr, currentStage, _player.Score, marbleArr);
                }

                // 입력한 키에 대해 게임에 출력하는 함수 
                KeyInputPrintOutput(tableArr, turnNum, marbleArr);

                // 턴이 끝나면 ■로 한줄을 채움
                if (_table.EliminateNum < 3)
                {
                    _table.MaxHeightIndex++;
                    _table.TurnEndBlock++;

                    for (int i = _table.MaxHeightIndex - _table.TurnEndBlock + 1; i > _table.TurnEndBlock - 1; i--)
                    {
                        for (int j = 0; j < tableArr.GetLength(1); j++)
                        {
                            if (tableArr[i, j] != block && tableArr[i, j] != space)
                            {
                                tableArr[i + 1, j] = tableArr[i, j];
                                marbleArr[i + 1, j] = marbleArr[i, j];
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
                        if (j == _table.Width / 2 - 1 && i == _table.Height - 2)
                        {
                            tableArr[i, j] = _player.PlayerShape;
                            marbleArr[i, j] = new Marble(_player.PlayerMarble.Shape, _player.PlayerMarble.Color);
                        }
                        if (tableArr[i,j] != block && tableArr[i,j] != space)
                        {
                            PrintShapeColor(marbleArr[i,j].Color);
                            Console.Write(_player.setPlayerShape(marbleArr[i, j].Shape));
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write(tableArr[i, j]);
                        }
                        // 스테이지, 플레이어 스코어 출력
                        if (i == _table.Height - 2 && j == _table.Width - 1)
                        {
                            Console.Write($"\tStage : {currentStage}");
                        }
                        if (i == _table.Height - 1 && j == _table.Width - 1)
                        {
                            Console.Write("\tScore : " + _player.Score);
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        void KeyInputPrintOutput(string[,] gameArr, int turnNum, Marble[,] marbles)
        {
            int posX = gameArr.GetLength(1) / 2 + 10;
            int posY = gameArr.GetLength(0) - 2;

            // 비어있는 공간의 Y인덱스 저장할 변수 선언
            int spaceYIndex;

            int playerIndex = 0;
            int colorIndex = 0;

            Vector2 marblePos;

            // 시간 측정하는 변수 선언
            Stopwatch stopwatch = new Stopwatch();
            ConsoleKeyInfo keyInput;

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

                    // 도형이 있는 공간의 Y좌표 인덱스 - 1 즉, 위에 있는 도형과 올린 도형과 색깔이 같으면
                    if (gameArr[spaceYIndex - 1, posX / 2] == _player.PlayerShape && marbles[spaceYIndex-1,posX/2].Color == _player.PlayerMarble.Color)
                    {
                        gameArr[spaceYIndex - 1, posX / 2] = "　";

                        marbles[spaceYIndex - 1, posX / 2] = new Marble(MarbleShape.None, MarbleColor.None);
                        // 일단 같은 도형 출력하고 나서
                        // 시간이 지나고 없어지게 만들기
                        Console.SetCursorPosition(posX, spaceYIndex);

                        // 색상 칠함
                        PrintShapeColor(_player.PlayerMarble.Color);
                        Console.Write(_player.PlayerShape);
                        Console.ResetColor();

                        // 생성된 블록 바로 아래에서 제거하면 +1씩 증가시키기
                        if (gameArr[spaceYIndex - 2, posX / 2] == "■")
                        {
                            _table.EliminateNum++;
                        }
                        while (true)
                        {
                            stopwatch.Start();
                            if (stopwatch.ElapsedMilliseconds / 1000 > 0.8f)
                            {
                                Console.SetCursorPosition(posX, spaceYIndex - 1);
                                Console.Write("　");
                                Console.SetCursorPosition(posX, spaceYIndex);
                                Console.Write("　");
                                stopwatch.Reset();
                                break;
                            }
                        }

                        // 도형 하나 없앨때마다 플레이어 Score 1씩 증가
                        _player.Score += 1;

                        // 플레이어 점수 수정
                        Console.SetCursorPosition(_table.Width + 39, _table.Height - 1);
                        Console.WriteLine($"{_player.Score}");
                        if (_player.Score > _table.StageUpScore)
                        {
                            _table.IsStageUp = true;
                            break;
                        }

                    }
                    // 위에 있는 도형과 올린 도형이 같지 않으면 그 아래에 플레이어 도형 출력
                    else
                    {
                        gameArr[spaceYIndex, posX / 2] = _player.PlayerShape;
                        Console.SetCursorPosition(posX, spaceYIndex);

                        marblePos = new Vector2(spaceYIndex, posX);

                        PrintShapeColor(_player.PlayerMarble.Color);
                        Console.Write(_player.PlayerShape);
                        Console.ResetColor();

                        marbles[spaceYIndex, posX / 2] = new Marble((MarbleShape)playerIndex, (MarbleColor)colorIndex, marblePos);
                    }
                    // 턴이 한 번 넘어갈 수록 
                    turnNum++;

                    // 플레이어 도형 다른 모양으로 랜덤하게 생성
                    playerIndex = rand.Next(0, (int)MarbleShape.None);

                    colorIndex = rand.Next(0, (int)MarbleColor.None);

                    _player.setPlayerShape((MarbleShape)playerIndex);
                    _player.SetPlayerMarble((MarbleShape)playerIndex, (MarbleColor)colorIndex);
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

                PrintShapeColor(_player.PlayerMarble.Color);
                Console.Write(_player.PlayerShape);
                Console.ResetColor();
            }
        }

        void PrintShapeColor(MarbleColor color)
        {
            switch(color)
            {
                case MarbleColor.Red:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case MarbleColor.Yellow:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case MarbleColor.Cyan:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case MarbleColor.DarkGreen:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
            }
        }

        void RandomSetMarbles(Marble[,] marbles)
        {

        }
    }

}
