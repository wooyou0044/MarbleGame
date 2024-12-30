using System;
using System.Collections.Generic;

namespace PracticeGame
{
    class Player
    {
        int _score;
        string _player;
        Marble _playerMarble;

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

        public string PlayerShape
        {
            get
            {
                return _player;
            }
            set
            {
                _player = value;
            }
        }

        public Marble PlayerMarble
        {
            get
            {
                return _playerMarble;
            }
            set
            {
                _playerMarble = value;
            }
        }

        public Player()
        {
            _playerMarble = new Marble();
        }

        public string SetPlayerShape(MarbleShape shape)
        {
            switch (shape)
            {
                case MarbleShape.Circle:
                    return "●";
                case MarbleShape.Star:
                    return "★";
                case MarbleShape.Diamond:
                    return "◆";
                case MarbleShape.Spade:
                    return "♠";
                case MarbleShape.Heart:
                    return "♥";
                case MarbleShape.Clover:
                    return "♣";
            }
            return string.Empty;
        }

        public void SetPlayerMarble(MarbleShape shape, MarbleColor color)
        {
            PlayerMarble.Shape = shape;
            PlayerShape = SetPlayerShape(shape);
            PlayerMarble.Color = color;
        }
    }
}
