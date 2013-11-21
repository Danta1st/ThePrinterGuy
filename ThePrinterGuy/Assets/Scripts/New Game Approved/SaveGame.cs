using UnityEngine;
using System.Collections;
using System.Text;

public class SaveGame : MonoBehaviour
{
    public static int GetPlayerCurrency()
    {
        if(PlayerPrefs.HasKey("currency"))
        {
            return PlayerPrefs.GetInt("currency");
        }
        return 0;
    }

    public static int GetPlayerPremiumCurrency()
    {
        if(PlayerPrefs.HasKey("premiumCurrency"))
        {
            return PlayerPrefs.GetInt("premiumCurrency");
        }
        return 0;
    }

    public static int[] GetPlayerHighscores()
    {
        if(PlayerPrefs.HasKey("highscoresAsString"))
        {
            return HighscoresStringToArray(PlayerPrefs.GetString("highscoresAsString"));
        }
        return new int[1]{0};
    }

    public static void SavePlayerData( int currency, int premiumCurrency, int[] highscores)
    {
        PlayerPrefs.SetInt("currency", currency);
        PlayerPrefs.SetInt("premiumCurrency", premiumCurrency);
        PlayerPrefs.SetString("highscoresAsString", HighscoresArrayToString(highscores));
    }

    public static void ResetPlayerData()
    {
        PlayerPrefs.SetInt("currency", 0);
        PlayerPrefs.SetInt("premiumCurrency", 0);
        PlayerPrefs.SetString("highscoresAsString", "0;-1;-1;");
    }

    private static string HighscoresArrayToString(int[] highscores)
    {
        StringBuilder builder = new StringBuilder();
        foreach (int highscore in highscores)
        {
            // Append each int to the StringBuilder overload.
            builder.Append(highscore).Append(";");
        }
        builder.Remove(builder.Length-1,1);
        return builder.ToString();
    }

    private static int[] HighscoresStringToArray(string highscores)
    {
        string[] parts = highscores.Split(';');
        int length = parts.Length;
        int[] result = new int[length];
        int i = 0;

        foreach(string highscore in parts)
        {
            int.TryParse(highscore, out result[i]);
            i++;
        }
        return result;
    }
}