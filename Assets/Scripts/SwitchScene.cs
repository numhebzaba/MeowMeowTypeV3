using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
    public void CustomeGame()
    {
        SceneManager.LoadScene("Custom_Mode");
    }
    public void Tutorial()
    {
        SceneManager.LoadScene("TutorialPart1");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Challenge()
    {
        SceneManager.LoadScene("ChallengeMode");
    }
    public void GamePlayRank()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
