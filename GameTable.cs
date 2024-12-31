using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PracticeGame
{
    class GameTable
    {
        int _width;
        int _height;
        int _maxHeightIndex;
        int _stageUpScore;
        int _turnMaxNum;
        int _turnEndBlock;
        bool _isFinish;
        bool _isStageUp;
        int _eliminateNum;
        int _setMarbleNum;

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public int MaxHeightIndex
        {
            get { return _maxHeightIndex; }
            set { _maxHeightIndex = value; }
        }

        public int TurnMaxNum
        {
            get { return _turnMaxNum; }
        }

        public int StageUpScore
        {
            get { return _stageUpScore; }
        }

        public int TurnEndBlock
        {
            get { return _turnEndBlock; }
            set { _turnEndBlock = value; }
        }
        public bool IsFinish
        {
            get { return _isFinish; }
            set { _isFinish = value; }
        }
        public int EliminateNum
        {
            get { return _eliminateNum; }
            set { _eliminateNum = value; }
        }

        public bool IsStageUp
        {
            get { return _isStageUp; }
            set { _isStageUp = value; }
        }

        public int SetMarbleNum
        {
            get { return _setMarbleNum; }
        }

        // stage에 따른 제약조건
        public void setAccordLevel(int stage)
        {
            switch (stage)
            {
                case 1:
                    _turnMaxNum = 10;
                    _stageUpScore = 20;
                    _setMarbleNum = 5;
                    break;
                case 2:
                    _turnMaxNum = 8;
                    _stageUpScore = 120;
                    _setMarbleNum = 7;
                    break;
                case 3:
                    _turnMaxNum = 7;
                    _stageUpScore = 150;
                    _setMarbleNum = 8;
                    break;
            }
        }

        public void SetTable(string[,] gameArr, int stage, int score, Marble[,] marbles)
        {
            string space = "　";
            string block = "■";
            bool isBlock = false;

            for (int i = 0; i < gameArr.GetLength(0); i++)
            {
                for (int j = 0; j < gameArr.GetLength(1); j++)
                {
                    isBlock = (i <= 0) || (j == 0) || (j == gameArr.GetLength(1) - 1) || (i == gameArr.GetLength(0) - 1);
                    if (isBlock)
                    {
                        gameArr[i, j] = block;
                        marbles[i, j] = new Marble(MarbleShape.None, MarbleColor.None);
                        Console.Write(gameArr[i, j]);
                    }
                    //else
                    else if (!isBlock && i >= SetMarbleNum + 1)
                    {
                        gameArr[i, j] = space;
                        marbles[i, j] = new Marble(MarbleShape.None, MarbleColor.None);
                        Console.Write(gameArr[i, j]);
                    }
                    else if (i >= 0 && i <= SetMarbleNum)
                    {
                        // 색상 나오게
                        PrintShapeColor(marbles[i, j].Color);
                        Console.Write(gameArr[i, j]);
                        Console.ResetColor();
                    }
                    //Console.Write(gameArr[i, j]);

                    // 스테이지, 플레이어 스코어 출력
                    if (i == gameArr.GetLength(0) - 3 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write($"\t『   Stage : {stage}");
                    }
                    if (i == gameArr.GetLength(0) - 2 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write("\t　   Score : " + score + "   』");
                    }

                    // 숫자, 모양에 따른 점수 옆에 나오게 하기
                    if (i == gameArr.GetLength(0) - 27 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write("\t▣▣▣▣▣▣▣▣▣▣▣▣");
                        Console.Write("\t▣▣▣▣▣▣▣▣");
                    }
                    if (i == gameArr.GetLength(0) - 26 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write("\t▣");
                        Console.Write("\t\t      ▣");
                        Console.Write("\t▣");
                        Console.Write("\t      ▣");
                    }
                    if (i == gameArr.GetLength(0) - 25 && j == gameArr.GetLength(1) - 1)
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
                    if (i == gameArr.GetLength(0) - 24 && j == gameArr.GetLength(1) - 1)
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
                    if (i == gameArr.GetLength(0) - 23 && j == gameArr.GetLength(1) - 1)
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
                    if (i == gameArr.GetLength(0) - 22 && j == gameArr.GetLength(1) - 1)
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
                    if (i == gameArr.GetLength(0) - 21 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write("\t▣");
                        Console.Write("\t\t      ▣");

                        Console.Write("\t▣");
                        Console.Write("  ♥ : ");
                        Console.Write("3점");
                        Console.Write("  ▣");
                    }
                    if (i == gameArr.GetLength(0) - 20 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write("\t▣▣▣▣▣▣▣▣▣▣▣▣");

                        Console.Write("\t▣");
                        Console.Write("  ♣ : ");
                        Console.Write("2점");
                        Console.Write("  ▣");
                    }
                    if (i == gameArr.GetLength(0) - 19 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write("\t\t\t\t\t▣");
                        Console.Write("\t      ▣");
                    }
                    if (i == gameArr.GetLength(0) - 18 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("\t  ★  ");
                        Console.Write("노란색 별 : ");
                        Console.ResetColor();
                        Console.Write("10점");
                        Console.Write("\t\t▣▣▣▣▣▣▣▣");
                    }

                    if (i == gameArr.GetLength(0) - 13 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write($"\t■■■■■■");
                    }
                    if (i == gameArr.GetLength(0) - 12 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write($"\t■　　　　■");
                    }
                    if (i == gameArr.GetLength(0) - 11 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write($"\t■　 ");
                        // 여기에 다음에 나올 것이 들어가야 함
                        Console.Write("　   ■");
                    }
                    if (i == gameArr.GetLength(0) - 10 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write($"\t■　　　　■");
                    }
                    if (i == gameArr.GetLength(0) - 9 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write($"\t■■■■■■");
                    }

                    // 도달까지 필요한 점수 출력
                    if (i == gameArr.GetLength(0) - 6 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write($"\t▶　{stage}단계 통과 시 필요한 score : {StageUpScore}");
                    }
                }
                Console.WriteLine();
            }

        }

        public void PrintShapeColor(MarbleColor color)
        {
            switch (color)
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
    }
}
