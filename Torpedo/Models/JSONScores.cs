using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Torpedo.Models
{
    public class JSONScores
    {
        // (FirstPlayerName, FirstPlayerHits): Egy ketelemu tuple, amelynek elso eleme a jatekos neve, a masodik eleme a jatekos talalatainak a szama
        public (string, int) FirstPlayerProperties { get; set; }
        // (FirstPlayerName, FirstPlayerHits): Egy ketelemu tuple, amelynek elso eleme a jatekos neve, a masodik eleme a jatekos talalatainak a szama
        public (string, int) SecondPlayerProperties { get; set; }

        public int Rounds { get; set; }
        public string Winner { get; set; }

        public JSONScores((string, int) firstPlayerProperties, (string, int) secondPlayerProperties, int rounds, string winner)
        {
            FirstPlayerProperties = firstPlayerProperties;
            SecondPlayerProperties = secondPlayerProperties;
            Rounds = rounds;
            Winner = winner;
        }

        // Reads the whole JSON file and returns it as an array of the JSON lines (one line represents the Scores of one Game)
        public string[] GetScoresFromJSON()
        {
            StringBuilder sb = new StringBuilder();
            string[] scores;

            string ScoresJSONPath = Path.Combine(Directory.GetCurrentDirectory(), "\\scores.json");

            using (var f = new StreamReader("scores.json"))
            {
                string line;

                while ((line = f.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }

                scores = sb.ToString().Split(
                    new string[] { Environment.NewLine + "," },
                StringSplitOptions.None);

                scores[0] = scores[0].Replace("[", "");
                scores[scores.Length - 1] = scores[scores.Length - 1].Replace("]", "");
            }

            return scores;
        }

        // Saves the scores with the above mentioned properties (GameID, FirstPlayerProperties, SecondPlayerProperties, Rounds, Winner) to the default file location
        public void SaveScores(JSONScores scores)
        {
            string json = JsonConvert.SerializeObject(scores);

            if (new FileInfo("scores.json").Length == 0)
            {
                using (var g = new StreamWriter("scores.json"))
                {
                    g.WriteLine("[");
                    g.WriteLine(json);
                    g.WriteLine("]");
                }
            }
            else
            {
                string text = File.ReadAllText("scores.json");
                text = text.Replace("]", ",");
                File.WriteAllText("scores.json", text);

                using (var g = new StreamWriter("scores.json", append: true))
                {
                    g.WriteLine(json);
                    g.WriteLine("]");
                }
            }
        }
    }
}
