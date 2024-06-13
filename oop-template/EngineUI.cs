using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Windows.Forms;

namespace oop_template
{
    public class EngineUI : Form
    {
        private Engine _engine;
        private Label[,] _labels;
        private int _size;
        private HighScoreManager _highScoreManager;
        private GameDifficulty _difficulty;
        private TableLayoutPanel _tableLayoutPanel;

        public EngineUI(int size, GameDifficulty difficulty)
        {
            _size = size;
            _difficulty = difficulty;
            _engine = new Engine(_size);
            _highScoreManager = new HighScoreManager();
            Text = "2048 Game";
            Size = new Size(600, 600);
            KeyDown += EngineUI_KeyDown;
            _labels = new Label[_size, _size];
            InitializeUI();
        }

        private void InitializeUI()
        {
            _tableLayoutPanel = new TableLayoutPanel
            {
                RowCount = _size,
                ColumnCount = _size,
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            for (int i = 0; i < _size; i++)
            {
                _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / _size));
                _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / _size));
                for (int j = 0; j < _size; j++)
                {
                    var label = new Label
                    {
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Arial", 24),
                        BackColor = GetBlockColor(_engine.Board[i, j]),
                        Text = _engine.Board[i, j] > 0 ? _engine.Board[i, j].ToString() : ""
                    };
                    _labels[i, j] = label;
                    _tableLayoutPanel.Controls.Add(label, j, i);
                }
            }

            Controls.Add(_tableLayoutPanel);
        }

        private Color GetBlockColor(int value)
        {
            switch (value)
            {
                case 2: return Color.LightYellow;
                case 4: return Color.LightGoldenrodYellow;
                case 8: return Color.Khaki;
                case 16: return Color.Gold;
                case 32: return Color.Orange;
                case 64: return Color.OrangeRed;
                case 128: return Color.LightSalmon;
                case 256: return Color.Salmon;
                case 512: return Color.DarkSalmon;
                case 1024: return Color.Coral;
                case 2048: return Color.Tomato;
                default: return Color.LightGray;
            }
        }

        private void EngineUI_KeyDown(object sender, KeyEventArgs e)
        {
            bool moved = false;

            switch (e.KeyCode)
            {
                case Keys.Up: moved = _engine.Move(Direction.Up); break;
                case Keys.Down: moved = _engine.Move(Direction.Down); break;
                case Keys.Left: moved = _engine.Move(Direction.Left); break;
                case Keys.Right: moved = _engine.Move(Direction.Right); break;
                case Keys.Z: moved = _engine.Undo(); break;
            }

            if (moved)
            {
                UpdateUI();
                if (_engine.IsGameOver())
                {
                    ShowGameOver();
                }
            }
        }

        private void UpdateUI()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _labels[i, j].Text = _engine.Board[i, j] > 0 ? _engine.Board[i, j].ToString() : "";
                    _labels[i, j].BackColor = GetBlockColor(_engine.Board[i, j]);
                }
            }
        }

        private void ShowGameOver()
        {
            string playerName = Interaction.InputBox("Game Over! Enter your name for the high score:");
            _engine.SaveHighScore(playerName, _difficulty);
            _highScoreManager.DisplayLeaderboard(_difficulty);
            DialogResult result = MessageBox.Show("Do you want to start a new game?", "Game Over", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                ChooseDifficulty();
            }
            else
            {
                Close();
            }
        }

        private void ChooseDifficulty()
        {
            DifficultySelectionForm difficultyForm = new DifficultySelectionForm();
            if (difficultyForm.ShowDialog() == DialogResult.OK)
            {
                int size = difficultyForm.SelectedSize;
                GameDifficulty difficulty = difficultyForm.SelectedDifficulty;
                StartGame(size, difficulty);
            }
        }

        private void StartGame(int size, GameDifficulty difficulty)
        {
            _size = size;
            _difficulty = difficulty;
            Controls.Remove(_tableLayoutPanel); // Remove the old table
            _engine = new Engine(_size);
            _labels = new Label[_size, _size];
            InitializeUI();
            UpdateUI();
        }
    }
}
