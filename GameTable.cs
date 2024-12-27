using System;
using System.Collections.Generic;

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


        // stage에 따른 제약조건
        public void setAccordLevel(int stage)
        {
            switch (stage)
            {
                case 1:
                    _turnMaxNum = 10;
                    _stageUpScore = 50;
                    break;
            }
        }

        public void SetTable(string[,] gameArr, string player, int stage, int score)
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
                    }
                    //else if (j == gameArr.GetLength(1) / 2 - 1 && i == gameArr.GetLength(0) - 2)
                    //{
                    //    gameArr[i, j] = player;
                    //}
                    else
                    {
                        gameArr[i, j] = space;
                    }
                    Console.Write(gameArr[i, j]);

                    // 스테이지, 플레이어 스코어 출력
                    if (i == gameArr.GetLength(0) - 2 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write($"\tStage : {stage}");
                    }
                    if (i == gameArr.GetLength(0) - 1 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write("\tScore : " + score);
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
