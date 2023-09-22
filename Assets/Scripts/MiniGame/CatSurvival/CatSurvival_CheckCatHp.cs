using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CatSurvival_CheckCatHp : MonoBehaviour
{
    public TMP_Text HP;
    public GameObject SummaryPanel;
    public GameObject GameOverPanel;

    private void Start()
    {
        SummaryPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (HP.text == "0")
        {
            GameObject.Find("Typer").GetComponent<CatSurvival_Typer>().enabled = false;
            GameOverPanel.SetActive(true);

        }
    }

    public void RestartCatSurvival()
    {
        SceneManager.LoadScene("Minigame_Cat_Survival");
    }
}
