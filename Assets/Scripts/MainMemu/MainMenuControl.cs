using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    public UIGameManager MainMenuManager;
    public DataManager _DataManager;
    public UIChallenge _UIChallenge;

    private void Start()
    {
        MainMenuManager.ClearScreen();
        MainMenuManager.showUI(6);
    }

    public void ShowMainMenu()
    {
        MainMenuManager.ClearScreen();
        MainMenuManager.showUI(6);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void ShopButton()
    {
        SceneManager.LoadScene("Select_Character",LoadSceneMode.Single);
    }

    public void CustomModeButton()
    {
        SceneManager.LoadScene("Custom_Mode", LoadSceneMode.Single);
    }

    public void RankingMode()
    {
        SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
    }

    public void ShowLeaderBoard()
    {
        _DataManager.LeaderboardButton();
    }

    public void ShowChalengeUI()
    {
        _UIChallenge.ShowUIChallenge();
    }

    public void ShowTutorialUi()
    {
        MainMenuManager.showUI(7);
    }

    public void ShowMinigameUi()
    {
        MainMenuManager.showUI(8);
    }

    public void ShowProfileUI()
    {
        MainMenuManager.showUI(9);

    }

}
