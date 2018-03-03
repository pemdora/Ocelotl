using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayHighscores : MonoBehaviour
{

    public TextMeshProUGUI[] scoresTxt;
    Highscores highscoresManager;

    public int highscoreLvl = 2;

    public void Start()
    {
        for (int i = 0; i < scoresTxt.Length; i++)
        {
            scoresTxt[i].text = (i + 1) + ".  Fetching ...";
        }
        highscoresManager = GetComponent<Highscores>();

        StartCoroutine(RefreshHighscores());
    }

    public void FetchHighscores(List<Highscore>[] highscoreList)
    {
        for (int i = 0; i < scoresTxt.Length; i++)
        {
            scoresTxt[i].text = (i + 1) + ". ";
            if (highscoreList[highscoreLvl-1].Count > i)
            {
                scoresTxt[i].text += highscoreList[highscoreLvl-1][i].username + " - " + highscoreList[highscoreLvl-1][i].time + " sec";
            }
        }
    }

    IEnumerator RefreshHighscores() {
        while (true) // return every 30sec
        {
            highscoresManager.DownloadHighscores();
            yield return new WaitForSeconds(30);
        }
    }
}
