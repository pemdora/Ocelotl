using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscores : MonoBehaviour
{
    // leaderboard link http://dreamlo.com/lb/nX-QEMnjlkGTsoZXLXlFCQRhtbMDXSrUGYMpq_DJpq5A
    private const string privateCode = "nX-QEMnjlkGTsoZXLXlFCQRhtbMDXSrUGYMpq_DJpq5A";
    private const string publicCode = "5a9a7b8758db5a1b84191fff";
    private const string webURL = "http://dreamlo.com/lb/";

    private int totalLvl = 2;

    public List<Highscore>[] highscoresByLvl; // Array of highscores list, highscoresByLvl[0] will be the highscores list of level 1
    DisplayHighscores highscoresDisplay;

    private void Awake()
    {/*
        AddNewHighscore("Tara", 1, 100);
        AddNewHighscore("Null", 1, 120);
        AddNewHighscore("Best", 1, 99);
        AddNewHighscore("C", 2, 120);
        AddNewHighscore("B", 2, 100);
        AddNewHighscore("A", 2, 90);
        DownloadHighscores();*/
        highscoresDisplay = GetComponent<DisplayHighscores>();
    }

    private void AddNewHighscore(string username, int lvl, int time)
    {
        StartCoroutine(UploadNewHighscore(username, lvl, time));
    }

    /// <summary>
    /// Upload score for a username
    /// </summary>
    /// <param name = username > The name of the player</param>
    /// <param name = lvl > Level played </param>
    /// <param name = time > Score of the player </param>
    /// <returns> Return an IEnumerator because web request is asynchronous (means that it is not instantaneous) </returns>
    private IEnumerator UploadNewHighscore(string username, int lvl, int time)
    {
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "|" + lvl + "/" + (1000-time) + "/" + time);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
            print("Upload Successful");
        else
            print("Error uploading :" + www.error);
    }

    public void DownloadHighscores()
    {
        StartCoroutine(DownloadHighscoresFromDB());
    }

    private IEnumerator DownloadHighscoresFromDB()
    {
        WWW www = new WWW(webURL + privateCode + "/pipe");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            FormatHighscores(www.text);
            if(highscoresDisplay!=null)
            highscoresDisplay.FetchHighscores(highscoresByLvl);
        }
        else
            print("Error donwloading :" + www.error);
    }

    public void FormatHighscores(string textStream)
    {
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscoresByLvl = new List<Highscore>[totalLvl];

        for (int i = 0; i < totalLvl; i++) // for each level, we instanciate a list 
        {
            highscoresByLvl[i] = new List<Highscore>(); // we can define a max number of highscores, eg : 50 max entries
        }

        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string username = entryInfo[0];
            int lvl = int.Parse(entryInfo[1]);
            int time = int.Parse(entryInfo[3]);

            highscoresByLvl[lvl - 1].Add(new Highscore(username, lvl, time));
        }
    }

}

public struct Highscore
{
    public string username;
    public int level;
    public int time;

    public Highscore(string username, int level, int time)
    {
        this.username = username;
        this.level = level;
        this.time = time;
    }
}