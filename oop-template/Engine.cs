using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using oop_template;

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

        public bool Move(Direction direction)
        {
            SaveState();
            bool moved = false;
            bool merged = false;
            switch (direction)
            {
                case Direction.Up: (moved, merged) = MoveUp(); break;
                case Direction.Down: (moved, merged) = MoveDown(); break;
                case Direction.Left: (moved, merged) = MoveLeft(); break;
                case Direction.Right: (moved, merged) = MoveRight(); break;
            }
            if (moved)
            {
                if (merged)
                {
                    _soundEffectEngine.PlayMergeSound();
                }
                else
                {
                    _soundEffectEngine.PlayMoveSound();
                }
                AddRandomBlock();
                OnBoardChanged();
            }
            return moved;
        }

        private void SaveState()
        {
            _history.Add((int[,])_board.Clone());
            if (_history.Count > 4)
            {
                _history.RemoveAt(0);
            }
        }

        public bool Undo()
        {
            if (_history.Count > 1)
            {
                _board = (int[,])_history[_history.Count - 1].Clone();
                _history.RemoveAt(_history.Count - 1);
                OnBoardChanged();
                return true;
            }
            return false;
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

        private (bool moved, bool merged) MoveUp()
        {
            bool moved = false;
            bool merged = false;
            for (int j = 0; j < _size; j++)
            {
                for (int i = 1; i < _size; i++)
                {
                    if (_board[i, j] != 0)
                    {
                        int k = i;
                        while (k > 0 && _board[k - 1, j] == 0)
                        {
                            _board[k - 1, j] = _board[k, j];
                            _board[k, j] = 0;
                            k--;
                            moved = true;
                        }
                        if (k > 0 && _board[k - 1, j] == _board[k, j] && _board[k - 1, j] != 2048)
                        {
                            _board[k - 1, j] *= 2;
                            _board[k, j] = 0;
                            merged = true;
                        }
                    }
                }
            }
            return (moved, merged);
        }

        private (bool moved, bool merged) MoveDown()
        {
            bool moved = false;
            bool merged = false;
            for (int j = 0; j < _size; j++)
            {
                for (int i = _size - 2; i >= 0; i--)
                {
                    if (_board[i, j] != 0)
                    {
                        int k = i;
                        while (k < _size - 1 && _board[k + 1, j] == 0)
                        {
                            _board[k + 1, j] = _board[k, j];
                            _board[k, j] = 0;
                            k++;
                            moved = true;
                        }
                        if (k < _size - 1 && _board[k + 1, j] == _board[k, j] && _board[k + 1, j] != 2048)
                        {
                            _board[k + 1, j] *= 2;
                            _board[k, j] = 0;
                            merged = true;
                        }
                    }
                }
            }
            return (moved, merged);
        }

        private (bool moved, bool merged) MoveLeft()
        {
            bool moved = false;
            bool merged = false;
            for (int i = 0; i < _size; i++)
            {
                for (int j = 1; j < _size; j++)
                {
                    if (_board[i, j] != 0)
                    {
                        int k = j;
                        while (k > 0 && _board[i, k - 1] == 0)
                        {
                            _board[i, k - 1] = _board[i, k];
                            _board[i, k] = 0;
                            k--;
                            moved = true;
                        }
                        if (k > 0 && _board[i, k - 1] == _board[i, k] && _board[i, k - 1] != 2048)
                        {
                            _board[i, k - 1] *= 2;
                            _board[i, k] = 0;
                            merged = true;
                        }
                    }
                }
            }
            return (moved, merged);
        }

        private (bool moved, bool merged) MoveRight()
        {
            bool moved = false;
            bool merged = false;
            for (int i = 0; i < _size; i++)
            {
                for (int j = _size - 2; j >= 0; j--)
                {
                    if (_board[i, j] != 0)
                    {
                        int k = j;
                        while (k < _size - 1 && _board[i, k + 1] == 0)
                        {
                            _board[i, k + 1] = _board[i, k];
                            _board[i, k] = 0;
                            k++;
                            moved = true;
                        }
                        if (k < _size - 1 && _board[i, k + 1] == _board[i, k] && _board[i, k + 1] != 2048)
                        {
                            _board[i, k + 1] *= 2;
                            _board[i, k] = 0;
                            merged = true;
                        }
                    }
                }
            }
            return (moved, merged);
        }

        public bool IsGameOver()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_board[i, j] == 0)
                        return false;
                    if (i < _size - 1 && _board[i, j] == _board[i + 1, j])
                        return false;
                    if (j < _size - 1 && _board[i, j] == _board[i, j + 1])
                        return false;
                }
            }
            return true;
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
