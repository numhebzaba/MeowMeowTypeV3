using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIChallenge : MonoBehaviour
{
    public bool IsShowKeyboard = false;
    public UIGameManager _UIGameManager;
    public DataManager _DataManager;
    public Button[] ChallengeButton;
    // Start is called before the first frame update
    public void ShowUIChallenge()
    {
        IsShowKeyboard = !IsShowKeyboard;
        if (IsShowKeyboard)
        {
            _UIGameManager.showUI(3);// show challenge screen
            StartCoroutine(_DataManager.LoadChallengeData());

        }
        else
        {
            _UIGameManager.ClearScreen();
            _UIGameManager.showUI(6);// show Main screen
            return;
        }

    }
    public void SetChallengeButton(int numberOfCompleteLevel)
    {
        for (int i =0;i<=numberOfCompleteLevel;i++)
        {
            ChallengeButton[i].interactable = true;
            ChallengeButton[i].GetComponent<Image>().color = Color.white;

        }
    }
    public void Level1_button()
    {
        PlayerPrefs.SetInt("challenge_level", 1);
        SceneManager.LoadScene("ChallengeMode");

    }
    public void Level2_button()
    {
        PlayerPrefs.SetInt("challenge_level", 2);
        SceneManager.LoadScene("ChallengeMode");

    }
    public void Level3_button()
    {
        PlayerPrefs.SetInt("challenge_level", 3);
        SceneManager.LoadScene("ChallengeMode");

    }
    public void Level4_button()
    {
        PlayerPrefs.SetInt("challenge_level", 4);
        SceneManager.LoadScene("ChallengeMode");

    }
    public void Level5_button()
    {
        PlayerPrefs.SetInt("challenge_level", 5);
        SceneManager.LoadScene("ChallengeMode");

    }
    public void Level6_button()
    {
        PlayerPrefs.SetInt("challenge_level", 6);
        SceneManager.LoadScene("ChallengeMode");

    }
    public void Level7_button()
    {
        PlayerPrefs.SetInt("challenge_level", 7);
        SceneManager.LoadScene("ChallengeMode");

    }
    public void Level8_button()
    {
        PlayerPrefs.SetInt("challenge_level", 8);
        SceneManager.LoadScene("ChallengeMode");

    }
    public void Level9_button()
    {
        PlayerPrefs.SetInt("challenge_level", 9);
        SceneManager.LoadScene("ChallengeMode");

    }
    public void Level10_button()
    {
        PlayerPrefs.SetInt("challenge_level", 10);
        SceneManager.LoadScene("ChallengeMode");

    }

}
