using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGaameSceneManager : MonoBehaviour
{
    public void CatServivalButton()
    {
        SceneManager.LoadScene("Minigame_Cat_Survival",LoadSceneMode.Single);
    }

    public void CatSquidGameButton()
    {
        SceneManager.LoadScene("Minigame_Cat_SquidGame", LoadSceneMode.Single);
    }

    public void CatDeadZoneButton()
    {
        SceneManager.LoadScene("Minigame_Cat_DeadZone", LoadSceneMode.Single);
    }
}
