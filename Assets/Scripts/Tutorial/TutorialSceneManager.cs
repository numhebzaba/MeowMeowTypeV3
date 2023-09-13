using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSceneManager : MonoBehaviour
{
    public void TutorialPart1()
    {
        SceneManager.LoadScene("TutorialPart1", LoadSceneMode.Single);
    }
    public void TutorialPart2()
    {
        SceneManager.LoadScene("Tutorial Part2", LoadSceneMode.Single);
    }
    public void TutorialPart3()
    {
        SceneManager.LoadScene("Tutorial Part3", LoadSceneMode.Single);
    }
    public void TutorialPart4()
    {
        SceneManager.LoadScene("Tutorial Part4", LoadSceneMode.Single);
    }
}
