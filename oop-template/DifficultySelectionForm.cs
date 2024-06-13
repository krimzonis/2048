using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace oop_template
{
    public class DifficultySelectionForm : Form
    {
        public int SelectedSize { get; private set; }
        public GameDifficulty SelectedDifficulty { get; private set; }

        public DifficultySelectionForm()
        {
            Text = "Choose Difficulty";
            Size = new Size(300, 200);

            Button easyButton = new Button { Text = "LAKO [6x6]", Dock = DockStyle.Top };
            Button mediumButton = new Button { Text = "SREDNJE [5x5]", Dock = DockStyle.Top };
            Button hardButton = new Button { Text = "TESKO [4x4]", Dock = DockStyle.Top };

            easyButton.Click += (sender, e) => { SelectedSize = 6; SelectedDifficulty = GameDifficulty.Lako; DialogResult = DialogResult.OK; };
            mediumButton.Click += (sender, e) => { SelectedSize = 5; SelectedDifficulty = GameDifficulty.Srednje; DialogResult = DialogResult.OK; };
            hardButton.Click += (sender, e) => { SelectedSize = 4; SelectedDifficulty = GameDifficulty.Tesko; DialogResult = DialogResult.OK; };

            Controls.Add(easyButton);
            Controls.Add(mediumButton);
            Controls.Add(hardButton);
        }
    }
}
