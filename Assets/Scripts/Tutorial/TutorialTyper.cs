using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class TutorialTyper : MonoBehaviour
{
    public TutorialWordList wordList = null;
    public TMP_Text wordOutput = null;
    public TMP_Text nextWordOutput = null;
    public TMP_Text wordTotalUI = null;
    public TMP_Text TimeSpent = null;
    public TMP_Text WordPerminuteText = null;

    public TMP_Text A_count = null;
    public TMP_Text A_error = null;

    public GameObject SummaryUI;
    public TMP_Text[] data = null;

    bool IsGameFinish = false;


    public float accuracy = 0;
    public int wordTotal = 0;
    public int allTypedEntries = 0;
    public int unCorrectedError = 0;
    public int wordPerMinute = 0;
    public TimeSpan delayTimeSpan = new TimeSpan(0, 0, 0);
    public int TimeInMinute;

    private string remainWord = string.Empty;
    private string currentWord = string.Empty;
    private string nextWord = string.Empty;
    public List<ListLetters> DataLetterList = new List<ListLetters>();

    //public animationStateControllerByType animationStateController;
    public AnimatorControllerState animationStateController;
    private GameObject animControllerObject;

    public Animator BGanimator;


    LoopBg loopBg_1, loopBg_2, loopBg_3, loopBg_4;
    public GameObject loopBgArray_1, loopBgArray_2, loopBgArray_3, loopBgArray_4;

    public Animator animator;
    public GameObject Keyboard;
    public bool IsKeyboardActive;

    private int selectedOption = 0;
    public CharacterDataBase CharacterDB;
    public GameObject AtworkSkin;

    public GameObject PlayerPosition;

    public GameObject SummaryPanel;

    private float CountWordIsTrue = 0;
    private float CountWordIsFalse = 0;
    private double CountAccuracy;

    public TMP_Text SummaryAccuracyText;
    public TMP_Text SummaryTimeText;
    public TMP_Text SummaryCorrect;
    public TMP_Text SummaryInCorrect;

    [SerializeField] SondFxkeyboardManager KeyboardFx;

    private bool canDoAction = true;

    private void Awake()
    {
        loopBg_1 = loopBgArray_1.GetComponent<LoopBg>();
        loopBg_2 = loopBgArray_2.GetComponent<LoopBg>();
        loopBg_3 = loopBgArray_3.GetComponent<LoopBg>();
        loopBg_4 = loopBgArray_4.GetComponent<LoopBg>();
    }
    void Start()
    {
        // BGanimator = GetComponent<Animator>();
        BGanimator.speed = 0;
        SummaryUI.SetActive(false);
        SummaryPanel.SetActive(false);
        SetCurrentWord();
        AddEngLetterlist();
        IsKeyboardActive = true;
        LoadSkinWithSkinSelect();
        SetReferenceAnimatorSkin();

    }

    private void SetReferenceAnimatorSkin()
    {
        animControllerObject = GameObject.FindWithTag("PlayerSkin");
        animationStateController = animControllerObject.GetComponent<AnimatorControllerState>();
    }

    private void LoadSkinWithSkinSelect()
    {
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }
        else
        {
            Load();
        }

        SpawnCharacter(selectedOption);
    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }

    public void SpawnCharacter(int CharacterIndex)
    {
        Character character = CharacterDB.GetCharacter(CharacterIndex);
        AtworkSkin = character.CharacterPrefab;
        Instantiate(AtworkSkin, PlayerPosition.transform.position, Quaternion.identity);
        Debug.Log(AtworkSkin.name);
    }

    private void SetCurrentWord()
    {
        currentWord = wordList.getWord();
        SetRemainWord(currentWord);
        nextWord = wordList.getNextWord();
        SetNextRemainWord(nextWord);
    }
    private void SetRemainWord(string newString)
    {
        remainWord = newString;
        wordOutput.text = remainWord;
    }
    private void SetNextRemainWord(string newString)
    {
        nextWord = newString;
        nextWordOutput.text = nextWord;
    }
    void Update()
    {
        if (!IsGameFinish)
        {
            CheckInput();
            SetAnimationKeyboard(currentWord);
        }

        if (!wordList.IsWordLeft() && IsWordComplete())
        {
            delayTimeSpan = delayTimeSpan;
            if (IsGameFinish == false)
            {
                SummaryPanel.SetActive(true);
                ShowDataLetterSummary();
                canDoAction = false;
                IsGameFinish = true;
            }
        }

        else
            delayTimeSpan += TimeSpan.FromSeconds(Time.deltaTime);

        TimeSpent.text = delayTimeSpan.ToString(@"hh\:mm\:ss");
        wordTotalUI.text = "word :" + wordTotal.ToString();

        double TimeSpentTotalSec = delayTimeSpan.TotalSeconds;
        if (TimeSpentTotalSec <= 1)
            TimeSpentTotalSec = 1;
        TimeSpentTotalSec = TimeSpentTotalSec / 60;

        wordPerMinute = (int)Math.Round(((((allTypedEntries / 5) - unCorrectedError)) / TimeSpentTotalSec));

        WordPerminuteText.text = "WPM :" + wordPerMinute.ToString();

    }

    public void ShowDataLetter()
    {
        int i = 0;
        foreach (var item in DataLetterList)
        {
            data[i].text = item.GetAllData.ToString();
            Debug.Log(item.GetAllData);
            i++;
        }
    }

    private void CheckInput()
    {
        if (Input.anyKeyDown && canDoAction == true && !(Input.GetMouseButtonDown(0)
            || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {
            string keyPressed = Input.inputString;

            if (keyPressed.Length == 1)
                EnterLetter(keyPressed);
            allTypedEntries++;
        }
    }

    private void CheckLetter(string keyinput)
    {
        foreach (var letter in DataLetterList)
        {
            if (keyinput.ToLower() == letter.getName)
            {
                letter.UpdateData();
                letter.UpdateAccuracy();
            }
        }
    }

    private void EnterLetter(string typedLetter)
    {
        if (IsCorrectLetter(typedLetter))
        {
            CountWordIsTrue++;
            KeyboardFx.PlyerAudioKeyboardIsTrue();
            loopBg_1.IsMove = true;
            loopBg_2.IsMove = true;
            loopBg_3.IsMove = true;
            loopBg_4.IsMove = true;

            CheckLetter(typedLetter);
            RemoveLetter();
            BGanimator.speed = 1; //play background animation//
            animationStateController.animator.SetInteger(animationStateController.AnimationHash, 23);
            //animationStateController.animator.SetBool(animationStateController.isWalkingHash, true);

            if (IsWordComplete())
            {
                wordTotal = wordTotal + 1;
                SetCurrentWord();
            }
            return;
        }
        IsFalse(typedLetter);

    }

    public void IsFalse(string keyinput)
    {
        CountWordIsFalse++;
        KeyboardFx.PlyerAudioKeyboardIsFalse();
        BGanimator.speed = 0; //Pause background animation//
       //animationStateController.animator.SetBool(animationStateController.isSittingHash, true);
        animationStateController.animator.SetInteger(animationStateController.AnimationHash, 14);


        foreach (var letter in DataLetterList)
        {
            //string temp = remainWord.ToLower();
            if (remainWord.Substring(0, 1) == letter.getName)
            {
                letter.UpdateWrongLetterData();
            }
        }
        loopBg_1.IsMove = false;
        loopBg_2.IsMove = false;
        loopBg_3.IsMove = false;
        loopBg_4.IsMove = false;

        unCorrectedError++;
    }

    private bool IsCorrectLetter(string letter)
    {
        return remainWord.IndexOf(letter) == 0;
    }
    private void RemoveLetter()
    {
        string newString = remainWord.Remove(0, 1);
        SetRemainWord(newString);
    }
    private bool IsWordComplete()
    {
        return remainWord.Length == 0;
    }
    public void ShowDataLetterSummary()
    {
        double TimeSeccond = Math.Round(double.Parse(delayTimeSpan.TotalSeconds.ToString()));
        double TimeMinut = Math.Round(double.Parse(delayTimeSpan.TotalMinutes.ToString()));

        CalculateAccuracy();
        SummaryAccuracyText.text = "Accuracy : " + CountAccuracy + " %";
        SummaryTimeText.text = "Time : " + delayTimeSpan.ToString(@"hh\:mm\:ss");
        SummaryCorrect.text = "Correct : " + CountWordIsTrue;
        SummaryInCorrect.text = "Incorrect : " + CountWordIsFalse;
    }

    private void CalculateAccuracy()
    {
        float AllCount = CountWordIsTrue + CountWordIsFalse;
        CountAccuracy = Math.Round((CountWordIsTrue * 100) / AllCount);
    }
    private void SetAnimationKeyboard(string newWord)
    {
        switch (newWord)
        {
            case "ffff":
                SetParametorHandAnimator();
                animator.SetBool("F", true);
                break;
            case "jjjj":
                SetParametorHandAnimator();
                animator.SetBool("J", true);
                break;
            case "dddd":
                SetParametorHandAnimator();
                animator.SetBool("D", true);
                break;
            case "kkkk":
                SetParametorHandAnimator();
                animator.SetBool("K", true);
                break;
            case "ssss":
                SetParametorHandAnimator();
                animator.SetBool("S", true);
                break;
            case "llll":
                SetParametorHandAnimator();
                animator.SetBool("L", true);
                break;
            case "aaaa":
                SetParametorHandAnimator();
                animator.SetBool("A", true);
                break;
            case ";;;;":
                SetParametorHandAnimator();
                animator.SetBool("Colon", true);
                break;
        }
    }

    private void SetParametorHandAnimator()
    {
        for (int i = 0; i < animator.parameterCount; i++)
        {
            animator.SetBool(animator.parameters[i].name, false);
        }
    }

    public void SetActiveKeyboard()
    {
        IsKeyboardActive = !IsKeyboardActive;
        Keyboard.SetActive(IsKeyboardActive);
    }

    private void AddEngLetterlist()
    {
        DataLetterList.Add(new ListLetters("a", 0, 0, 0));
        DataLetterList.Add(new ListLetters("b", 0, 0, 0));
        DataLetterList.Add(new ListLetters("c", 0, 0, 0));
        DataLetterList.Add(new ListLetters("d", 0, 0, 0));
        DataLetterList.Add(new ListLetters("e", 0, 0, 0));
        DataLetterList.Add(new ListLetters("f", 0, 0, 0));
        DataLetterList.Add(new ListLetters("g", 0, 0, 0));
        DataLetterList.Add(new ListLetters("h", 0, 0, 0));
        DataLetterList.Add(new ListLetters("i", 0, 0, 0));
        DataLetterList.Add(new ListLetters("j", 0, 0, 0));
        DataLetterList.Add(new ListLetters("k", 0, 0, 0));
        DataLetterList.Add(new ListLetters("l", 0, 0, 0));
        DataLetterList.Add(new ListLetters("m", 0, 0, 0));
        DataLetterList.Add(new ListLetters("n", 0, 0, 0));
        DataLetterList.Add(new ListLetters("o", 0, 0, 0));
        DataLetterList.Add(new ListLetters("p", 0, 0, 0));
        DataLetterList.Add(new ListLetters("q", 0, 0, 0));
        DataLetterList.Add(new ListLetters("r", 0, 0, 0));
        DataLetterList.Add(new ListLetters("s", 0, 0, 0));
        DataLetterList.Add(new ListLetters("t", 0, 0, 0));
        DataLetterList.Add(new ListLetters("u", 0, 0, 0));
        DataLetterList.Add(new ListLetters("v", 0, 0, 0));
        DataLetterList.Add(new ListLetters("w", 0, 0, 0));
        DataLetterList.Add(new ListLetters("x", 0, 0, 0));
        DataLetterList.Add(new ListLetters("y", 0, 0, 0));
        DataLetterList.Add(new ListLetters("z", 0, 0, 0));
        DataLetterList.Add(new ListLetters(";", 0, 0, 0));
    }
}
