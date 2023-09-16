using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class CatSquidGameChecker : MonoBehaviour
{
    public TMP_Text HP;
    public GameObject SummaryPanel;
    public GameObject GameOverPanel;

    public bool IsTyper = true;
    private bool IsGameStart = false;
    private bool WarningColor;
    private bool IsFristInput = false;
    private bool IsgameOver = false;

    private int TimeToChange;
    private int TimeeFalseState;

    public VolumeProfile volumeProfile;
    private Vignette vignette;

    public AnimatorControllerState AnimationControllerEnemy;
    public GameObject EnemyCat;

    public Transform PlayerPostion;
    public Transform ForwardPosition;
    private float rotationSpeed = 2.0f;


    public Animator BGanimator;
    LoopBg loopBg_1, loopBg_2, loopBg_3, loopBg_4;
    public GameObject loopBgArray_1, loopBgArray_2, loopBgArray_3, loopBgArray_4;

    public AnimatorControllerState animationStateController;
    private GameObject animControllerObject;

    public GameObject MainCamera,Camera1,Camera2;
    public GameObject GameoverPanele,TyperPanel;


    private void Awake()
    {
        loopBg_1 = loopBgArray_1.GetComponent<LoopBg>();
        loopBg_2 = loopBgArray_2.GetComponent<LoopBg>();
        loopBg_3 = loopBgArray_3.GetComponent<LoopBg>();
        loopBg_4 = loopBgArray_4.GetComponent<LoopBg>();
    }

    private void Start()
    {
        Camera1.SetActive(false);
        Camera2.SetActive(false);
        GameoverPanele.SetActive(false);

        BGanimator.speed = 0;

        SummaryPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        AnimationControllerEnemy = EnemyCat.GetComponent<AnimatorControllerState>();
        SetReferenceAnimatorSkin();

    }

    private void SetReferenceAnimatorSkin()
    {
        animControllerObject = GameObject.FindWithTag("PlayerSkin");
        animationStateController = animControllerObject.GetComponent<AnimatorControllerState>();
    }

    private void Update()
    {
        if (IsgameOver == false)
        {
            CheckOnStartGame();
            AnimationEnemyControllerState();
            ChangScreenColor();
            IsRedState();

            if (TimeToChange == 0 && IsGameStart == true && IsTyper == true)
            {
                IsTyper = false;
                //Debug.Log(IsTyper);
            }
            if (IsTyper == false && TimeeFalseState == 0)
            {
                StartCoroutine(CountDownFalseStateTyper());
            }
            IsTyperState();
        }
       

    }

    private void IsRedState()
    {
        if(IsTyper == false)
        {
            animationStateController.animator.SetInteger(animationStateController.AnimationHash, 44);
        }
    }

    private void ChangeAnimationPlayer()
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(keyCode) && !(Input.GetKeyDown(KeyCode.LeftShift)))
            {
                loopBg_1.IsMove = true;
                loopBg_2.IsMove = true;
                loopBg_3.IsMove = true;
                loopBg_4.IsMove = true;

                animationStateController.animator.SetInteger(animationStateController.AnimationHash, 23);
                AnimationControllerEnemy.animator.SetInteger(AnimationControllerEnemy.AnimationHash, 21);

            }
        }

    }

    private void ChangScreenColor()
    {
        if (volumeProfile.TryGet<Vignette>(out vignette) && WarningColor == true && IsTyper == true)
        {
            vignette.color.Override(new Color(191f, 139f, 53f, 1f));
        }
        if (volumeProfile.TryGet<Vignette>(out vignette) && IsTyper == false && WarningColor == true)
        {
            vignette.color.Override(new Color(195f, 40f, 55f, 1f));
        }
        else if(IsTyper == true && WarningColor == false)
        {
            vignette.color.Override(new Color(53f, 112f, 191f, 1f));
        }
    }

    private void AnimationEnemyControllerState()
    {
        if(IsFristInput == false)
        {
            AnimationControllerEnemy.animator.SetInteger(AnimationControllerEnemy.AnimationHash, 33);
            CheckFirstInput();
        }
        else if (IsTyper == true && WarningColor == false && IsgameOver == false)
        {
            RotationEnemy(ForwardPosition);
            AnimationControllerEnemy.animator.SetInteger(AnimationControllerEnemy.AnimationHash, 1);
        }
        else if (IsTyper == false && WarningColor == true)
        {
            RotationEnemy(PlayerPostion);
            loopBg_1.IsMove = false;
            loopBg_2.IsMove = false;
            loopBg_3.IsMove = false;
            loopBg_4.IsMove = false;
            AnimationControllerEnemy.animator.SetInteger(AnimationControllerEnemy.AnimationHash, 1);

        }
    }

    private void RotationEnemy(Transform Tragey)
    {
        Vector3 targetDirection = Tragey.position - EnemyCat.transform.position;

        float singleStep = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(EnemyCat.transform.forward, targetDirection, singleStep, 0.0f);

        EnemyCat.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void CheckFirstInput()
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(keyCode))
            {
                IsFristInput = true;
            }
        }
    }

    private void OnFalseStateTyper()
    {
        if(TimeeFalseState == 0)
        {
            IsTyper = true;
            //Debug.Log("TRUE");
            StartCoroutine(CountDownTimeToChangeState());
        }
    }

    private IEnumerator CountDownFalseStateTyper()
    {
        TimeeFalseState = 4;
        while (TimeeFalseState > 0)
        {
            //Debug.Log("Time False"+TimeeFalseState);
            yield return new WaitForSeconds(1.0f);
            TimeeFalseState--;
        }
        OnFalseStateTyper();
    }

    private void CheckOnStartGame()
    {
        if (GameObject.Find("Typer").GetComponent<CatSquidGame_Typer>().enabled == !IsGameStart)
        {
            StartCoroutine(CountDownTimeToChangeState());
            IsGameStart = !IsGameStart;
        }
    }


    private IEnumerator CountDownTimeToChangeState()
    {
        TimeToChange = RandDomTimeToChange();
        WarningColor = false;
        while (TimeToChange > 0)
        {
           //Debug.Log(TimeToChange);
            yield return new WaitForSeconds(1.0f);
            TimeToChange--;

            if(TimeToChange == 3)
            {
                WarningColor = true;
            }
        }

    }

    private int RandDomTimeToChange()
    {
        int Time = Random.Range(5, 10);
        return Time;
    }

    private void IsTyperState()
    {
        if (IsTyper == false && WarningColor == true)
        {
            CheckAnyInput();
        }
        if (IsgameOver == false)
        {
            ChangeAnimationPlayer();
        }

    }

    private void StonpBG()
    {
        loopBg_1.IsMove = false;
        loopBg_2.IsMove = false;
        loopBg_3.IsMove = false;
        loopBg_4.IsMove = false;
    }

    private void CheckAnyInput()
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(keyCode))
            {
                GameObject.Find("Typer").GetComponent<CatSquidGame_Typer>().enabled = false;
                TyperPanel.SetActive(false);
                IsgameOver = true;
                RotationEnemy(PlayerPostion);
                StonpBG();
                AnimationControllerEnemy.animator.SetInteger(AnimationControllerEnemy.AnimationHash, 3);
                CameraEnd();
                StartCoroutine(ShowGameOverPanel());
            }
        }

    }

    private IEnumerator ShowGameOverPanel()
    {

        yield return new WaitForSeconds(4.0f);
        GameOverPanel.SetActive(true);

    }

    private void CameraEnd()
    {
        MainCamera.SetActive(false);
        Camera1.SetActive(true);

        StartCoroutine(WaitCamera());
    }

    private IEnumerator WaitCamera()
    {
        yield return new WaitForSeconds(2.0f);
        Camera2.SetActive(true);

    }

    public void RestartCatSquidGame()
    {
        SceneManager.LoadScene("Minigame_Cat_SquidGame");
    }
}
