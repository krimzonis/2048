using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_template
{
    public class Engine
    {
        public event Action BoardChanged;
        private int[,] _board;
        private List<int[,]> _history;
        private Random _random;
        private int _size;
        private SoundEffectEngine _soundEffectEngine;
        private HighScoreManager _highScoreManager;
        public int[,] Board => _board;

        public Engine(int size)
        {
            _size = size;
            _board = new int[_size, _size];
            _history = new List<int[,]>();
            _random = new Random();
            _soundEffectEngine = new SoundEffectEngine();
            _highScoreManager = new HighScoreManager();
            NewGame();
        }

        public void NewGame()
        {
            Array.Clear(_board, 0, _board.Length);
            _history.Clear();
            AddRandomBlock();
            AddRandomBlock();
            OnBoardChanged();
        }

        private void AddRandomBlock()
        {
            List<Point> emptyPoints = new List<Point>();
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_board[i, j] == 0)
                    {
                        emptyPoints.Add(new Point(i, j));
                    }
                }
            }

            if (emptyPoints.Count > 0)
            {
                Point p = emptyPoints[_random.Next(emptyPoints.Count)];
                _board[p.X, p.Y] = _random.NextDouble() < 0.9 ? 2 : 4;
            }
        }

        public void SaveHighScore(string playerName, GameDifficulty difficulty)
        {
            int score = CalculateScore();
            _highScoreManager.SaveHighScore(playerName, score, difficulty);
        }

        private int CalculateScore()
        {
            int score = 0;
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    score += _board[i, j];
                }
            }
            return score;
        }

        private void OnBoardChanged()
        {
            BoardChanged?.Invoke();
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum GameDifficulty
    {
        Lako,
        Srednje,
        Tesko
    }
}
