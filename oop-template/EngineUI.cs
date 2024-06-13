using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string playerName = Microsoft.VisualBasic.Interaction.InputBox("Game Over! Napisi ime da bi ti se uneo highscore:");
            _engine.SaveHighScore(playerName, _difficulty);
            _highScoreManager.DisplayLeaderboard(_difficulty);
            DialogResult result = MessageBox.Show("Zelis li zapoceti novu igru?", "Game Over", MessageBoxButtons.YesNo);
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
            Controls.Remove(_tableLayoutPanel);
            _engine = new Engine(_size);
            _labels = new Label[_size, _size];
            InitializeUI();
            UpdateUI();
        }
    }
}
