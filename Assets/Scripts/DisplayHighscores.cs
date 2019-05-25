using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayHighscores : MonoBehaviour
{

    [Header("UI Elements variables")]
    public TextMeshProUGUI[] scoresTxt;
    public TMP_Dropdown dropdown;
    public TextMeshProUGUI dropdownTxt;

    public Highscores highscoresManager;

    public int highscoreLvl = 1;

    public void Start()
    {
        for (int i = 0; i < scoresTxt.Length; i++)
        {
            scoresTxt[i].text = (i + 1) + ".  Fetching ...";
        }
        highscoresManager = GetComponent<Highscores>();

        // StartCoroutine(RefreshHighscores());
        highscoresManager.DownloadHighscores();

        //Add listener for when the value of the Dropdown changes, to take action
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
    }

    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged(TMP_Dropdown change)
    {
        // Test to display value
        // Debug.Log("New Value : " + change.value);
        highscoreLvl = change.value + 1;
        dropdownTxt.text = change.options[change.value].text +"  :";
        for (int i = 0; i < scoresTxt.Length; i++)
        {
            scoresTxt[i].text = (i + 1) + ".  Fetching ...";
        }
        highscoresManager.DownloadHighscores();
    }

    public void FetchHighscores(List<Highscore>[] highscoreList)
    {
        print(highscoreLvl - 1);
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
