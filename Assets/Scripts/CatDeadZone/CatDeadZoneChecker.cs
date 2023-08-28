using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CatDeadZoneChecker : MonoBehaviour
{
    public TMP_Text HP;
    public GameObject SummaryPanel;
    public GameObject GameOverPanel;

    public GameObject HPBar;
    public AnimatorControllerState animationStateController;
    private GameObject animControllerObject;


    private void Start()
    {
        SummaryPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    private void SetReferenceAnimatorSkin()
    {
        animControllerObject = GameObject.FindWithTag("PlayerSkin");
        animationStateController = animControllerObject.GetComponent<AnimatorControllerState>();
    }

    private void Update()
    {
        CheckSpawnPlayer();
        if (HPBar.transform.localScale.x == 0)
        {
            GameObject.Find("Typer").GetComponent<CatDeadZone_Typer>().enabled = false;
            animationStateController.animator.SetInteger(animationStateController.AnimationHash, 6);

            GameOverPanel.SetActive(true);
        }
    }

    private bool CheckSpawnPlayer()
    {
        if(GameObject.Find("Typer").GetComponent<CatDeadZone_Typer>().enabled == true)
        {
            SetReferenceAnimatorSkin();
            return false;
        }
        return true;
    }

    public void RestartCatSurvival()
    {
        SceneManager.LoadScene("Minigame_Cat_DeadZone");
    }
}
