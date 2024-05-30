using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    private const string filePath = "scores.txt";

    public List<HighScoreEntry> LoadHighScores()
    {
        List<HighScoreEntry> highScores = new List<HighScoreEntry>();

        try
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        int score = int.Parse(parts[0]);
                        string time = parts[1];
                        int coins = int.Parse(parts[2]);
                        string name = parts[3];

                        HighScoreEntry entry = new HighScoreEntry { score = score, time = time, coins = coins, name=name };
                        highScores.Add(entry);
                    }
                }
            }
            else
            {
                Debug.Log("HighScoreManager: No scores file found.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Błąd przy odczytywaniu wyników: " + e.Message);
        }

        return highScores.OrderByDescending(entry => entry.score).Take(7).ToList();
    }
}

