using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_template
{
    public class HighScoreManager
    {
        private const string EasyFilePath = "highscores_easy.txt";
        private const string MediumFilePath = "highscores_medium.txt";
        private const string HardFilePath = "highscores_hard.txt";

        private string GetFilePath(GameDifficulty difficulty)
        {
            switch (difficulty)
            {
                case GameDifficulty.Lako: return EasyFilePath;
                case GameDifficulty.Srednje: return MediumFilePath;
                case GameDifficulty.Tesko: return HardFilePath;
                default: throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }

        public void SaveHighScore(string playerName, int score, GameDifficulty difficulty)
        {
            var highScores = LoadHighScores(difficulty);
            highScores.Add(new KeyValuePair<string, int>(playerName, score));
            highScores = highScores.OrderByDescending(x => x.Value).ToList();
            SaveHighScores(highScores, difficulty);
        }
        public List<KeyValuePair<string, int>> LoadHighScores(GameDifficulty difficulty)
        {
            var highScores = new List<KeyValuePair<string, int>>();
            string filePath = GetFilePath(difficulty);

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                    {
                        highScores.Add(new KeyValuePair<string, int>(parts[0], score));
                    }
                }
            }

            return highScores;
        }

        private void SaveHighScores(List<KeyValuePair<string, int>> highScores, GameDifficulty difficulty)
        {
            string filePath = GetFilePath(difficulty);
            var lines = highScores.Select(x => $"{x.Key}|{x.Value}");
            File.WriteAllLines(filePath, lines);
        }

        public void DisplayLeaderboard(GameDifficulty difficulty)
        {
            var highScores = LoadHighScores(difficulty);
            string leaderboard = $"Leaderboard ({difficulty}):\n";
            foreach (var highScore in highScores)
            {
                leaderboard += $"{highScore.Key}: {highScore.Value}\n";
            }
            MessageBox.Show(leaderboard, "Leaderboard");
        }
    }
}
