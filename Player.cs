using System;
using System.Collections.Generic;

namespace PracticeGame
{
    class Player
    {
        int _score;
        int _currentStage;
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

        public int CurrentStage
        {
            get
            {
                return _currentStage;
            }
            set
            {  
                _currentStage = value; 
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

        public string setPlayerShape(MarbleShape shape)
        {
            switch (shape)
            {
                case MarbleShape.Circle:
                    PlayerShape = "●";
                    break;
                case MarbleShape.Star:
                    PlayerShape = "★";
                    break;
                case MarbleShape.Diamond:
                    PlayerShape = "◆";
                    break;
                case MarbleShape.Spade:
                    PlayerShape = "♠";
                    break;
                case MarbleShape.Heart:
                    PlayerShape = "♥";
                    break;
                case MarbleShape.Clover:
                    PlayerShape = "♣";
                    break;
            }
            return PlayerShape;
        }

        public void SetPlayerMarble(MarbleShape shape, MarbleColor color)
        {
            PlayerMarble.Shape = shape;
            PlayerMarble.Color = color;
        }
    }
}
