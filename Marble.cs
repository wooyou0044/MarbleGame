using System;
using System.Collections.Generic;
namespace PracticeGame
{
    enum MarbleColor
    {
        Red,
        Yellow,
        Cyan,
        DarkGreen,
        None
    }

    enum MarbleShape
    {
        Circle,
        Star,
        Diamond,
        Spade,
        Heart,
        Clover,
        None
    }

    struct Vector2
    {
        public int posX;
        public int posY;
        public Vector2(int x, int y)
        {
            posX = x;
            posY = y;
        }
    }

    class Marble
    {
        MarbleShape _shape;
        MarbleColor _color;
        Vector2 _location;
        int _score;

        public MarbleShape Shape
        {
            get
            {
                return _shape;
            }
            set
            {
                _shape = value;
            }
        }

        public MarbleColor Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        public Vector2 Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
            }
        }

        public Marble()
        {
            // 기본으로 세팅
            _shape = MarbleShape.None;
            _color = MarbleColor.None;
        }

        public Marble(MarbleShape shape, MarbleColor color)
        {
            _shape = shape;
            _color = color;
        }

        public Marble(MarbleShape shape, MarbleColor color, Vector2 location)
        {
            _shape = shape;
            _color = color;
            _location = location;
        }

        public int GetMarbleScore(MarbleColor color, MarbleShape shape)
        {
            int colorScore = 0;
            int shapeScore = 0;

            switch (color)
            {
                case MarbleColor.Red:
                    colorScore = 1;
                    break;
                case MarbleColor.Yellow:
                    colorScore = 3;
                    break;
                case MarbleColor.Cyan:
                    colorScore = 2;
                    break;
                case MarbleColor.DarkGreen:
                    colorScore = 1;
                    break;
            }

            switch (shape)
            {
                case MarbleShape.Circle:
                    shapeScore = 1;
                    break;
                case MarbleShape.Star:
                    shapeScore = 5;
                    break;
                case MarbleShape.Diamond:
                    shapeScore = 2;
                    break;
                case MarbleShape.Spade:
                    shapeScore = 3;
                    break;
                case MarbleShape.Heart:
                    shapeScore = 3;
                    break;
                case MarbleShape.Clover:
                    shapeScore = 2;
                    break;
            }

            Score = shapeScore + colorScore;

            if (shape == MarbleShape.Star && color == MarbleColor.Yellow)
            {
                Score = 10;
            }

            return Score;
        }

        public void ChangePosition(Vector2 pos)
        {
            _location.posX = pos.posX;
            _location.posY = pos.posY;
        }
    }
}
