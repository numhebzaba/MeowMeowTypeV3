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
    public LoopBg loopBg_1, loopBg_2, loopBg_3, loopBg_4;



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
            loopBg_1.IsMove = false;
            loopBg_2.IsMove = false;
            loopBg_3.IsMove = false;
            loopBg_4.IsMove = false;
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
