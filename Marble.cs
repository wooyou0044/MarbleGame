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

        public Marble()
        {
            // 기본으로 세팅
            _shape = MarbleShape.Circle;
            _color = MarbleColor.Red;
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
    }
}
