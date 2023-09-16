using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEditor.Animations;

public class CatSurvival_Typer : MonoBehaviour
{
    public CatSurvival_wordList CatSurvivalWordList = null;
    public TMP_Text wordOutput = null;
    public TMP_Text wordOutputIsTrue = null;
    public TMP_Text nextWordOutput = null;
    public TMP_Text wordTotalUI = null;
    public TMP_Text TimeSpent = null;
    public TMP_Text WordPerminuteText = null;

    public TMP_Text A_count = null;
    public TMP_Text A_error = null;
    public GameObject TyperPanel;
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
    private string remainWordIsTrue = string.Empty;
    public List<ListLetters> DataLetterList = new List<ListLetters>();

    public AnimatorControllerState animationStateController;
    public Animator BGanimator;


    LoopBg loopBg_1, loopBg_2, loopBg_3, loopBg_4;
    public GameObject loopBgArray_1, loopBgArray_2, loopBgArray_3, loopBgArray_4;

    //public GameObject Keyboard;
    public bool IsKeyboardActive;

    public CharacterDataBase CharacterDB;
    public GameObject AtworkSkin;
    private int selectedOption = 0;

    public GameObject playerPositionSpawn;
    private GameObject animControllerObject;

    private int Cat_HP = 3;
    public TMP_Text CatTextHP;

    private float CountWordIsTrue = 0;
    private float CountWordIsFalse = 0;
    private double CountAccuracy;

    public TMP_Text SummaryAccuracyText;
    public TMP_Text SummaryTimeText;
    public TMP_Text SummaryCorrect;
    public TMP_Text SummaryInCorrect;


    private void Awake()
    {
        loopBg_1 = loopBgArray_1.GetComponent<LoopBg>();
        loopBg_2 = loopBgArray_2.GetComponent<LoopBg>();
        loopBg_3 = loopBgArray_3.GetComponent<LoopBg>();
        loopBg_4 = loopBgArray_4.GetComponent<LoopBg>();
    }
    void Start()
    {
        BGanimator.speed = 0;
        SummaryUI.SetActive(false);
        SetCurrentWord();
        AddEngLetterlist();
        IsKeyboardActive = true;

        LoadSkinWithSkinSelect();
        SetReferenceAnimatorSkin();
        CatTextHP.text = Cat_HP.ToString();
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
        Instantiate(AtworkSkin, playerPositionSpawn.transform.position, Quaternion.identity);
        Debug.Log(AtworkSkin.name);
    }

    public void SetCurrentWord()
    {
        currentWord = CatSurvivalWordList.getWord();
        SetRemainWord(currentWord);
        nextWord = CatSurvivalWordList.getNextWord();
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
        CheckInput();
        //SetAnimationKeyboard(currentWord);

        if (/*!CatSurvivalWordlist.IsWordLeft() &&*/ IsWordComplete())
        {
            TyperPanel.SetActive(false);
            SummaryUI.SetActive(true);
            delayTimeSpan = delayTimeSpan;
            if (IsGameFinish == false)
            {
                ShowDataLetter();
                IsGameFinish = true;
            }
        }

        else
            delayTimeSpan += TimeSpan.FromSeconds(Time.deltaTime);

        TimeSpent.text = delayTimeSpan.ToString();
        wordTotalUI.text = "word :" + wordTotal.ToString();
            
        int TimeInIntValue = int.Parse(delayTimeSpan.Minutes.ToString());
        if (TimeInIntValue <= 0)
            TimeInIntValue = 1;
        if ((allTypedEntries / 5) <= unCorrectedError)
            wordPerMinute = 0;
        else
            wordPerMinute = (((allTypedEntries / 5) - unCorrectedError)) / TimeInIntValue;
        WordPerminuteText.text = "WPM : " + wordPerMinute.ToString();

    }

    public void ShowDataLetter()
    {
        double TimeSeccond = Math.Round(double.Parse(delayTimeSpan.TotalSeconds.ToString()));
        double TimeMinut = Math.Round(double.Parse(delayTimeSpan.TotalMinutes.ToString()));

        CalculateAccuracy();
        SummaryAccuracyText.text = "Accuracy : " + CountAccuracy + " %";
        SummaryTimeText.text = "Time : " + TimeMinut + ":" + TimeSeccond ;
        SummaryCorrect.text = "Correct : " + CountWordIsTrue;
        SummaryInCorrect.text = "Incorrect : " + CountWordIsFalse;
    }

    private void CalculateAccuracy()
    {
        float AllCount = CountWordIsTrue + CountWordIsFalse;
        CountAccuracy = Math.Round((CountWordIsTrue * 100) / AllCount);
    }

    private void CheckInput()
    {
        if (Input.anyKeyDown)
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

            loopBg_1.IsMove = true;
            loopBg_2.IsMove = true;
            loopBg_3.IsMove = true;
            loopBg_4.IsMove = true;

            CheckLetter(typedLetter);
            RemoveLetter();
            BGanimator.speed = 1; //play background animation//
            //animationStateController.animator.SetBool(animationStateController.isSittingHash, false);
            animationStateController.animator.SetInteger(animationStateController.AnimationHash, 23);

            if (IsWordComplete())//loop word
            {
                wordTotal = wordTotal + 1;
                //SetCurrentWord();
            }
            return;
        }
        IsFalse(typedLetter);

    }

    public void IsFalse(string keyinput)
    {
        CountWordIsFalse++;

        BGanimator.speed = 0; //Pause background animation//
        //animationStateController.animator.SetBool(animationStateController.isSittingHash, true);
        ReducedHP();
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
        //Color color = Color.gray;

        // Add the character with the new color to the modified text
        //modifiedText += "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + textString[0] + "</color>";

        //wordOutput.text = modifiedText;
        string newString = remainWord.Remove(0, 1);
        if (remainWord[0].ToString() == " ")
        {
            wordOutputIsTrue.text += "_";
            //"<alpha=#00>F<alpha=#FF>"
        }
        else if (remainWord[0].ToString() != " ")
        {
            wordOutputIsTrue.text += remainWord[0];
        }

        if (wordOutputIsTrue.text.Length > 14)
        {
            DeleteWordIstrue();
        }
        SetRemainWord(newString);
    }

    private void DeleteWordIstrue()
    {
        remainWordIsTrue = wordOutputIsTrue.text;
        string newStrings = remainWordIsTrue.Remove(0, 1);
        wordOutputIsTrue.text += remainWordIsTrue[0];

        nextWord = newStrings;
        wordOutputIsTrue.text = nextWord;
    }

    private bool IsWordComplete()
    {
        return remainWord.Length == 1;
    }

    public void SetActiveKeyboard()
    {
        IsKeyboardActive = !IsKeyboardActive;
        //Keyboard.SetActive(IsKeyboardActive);
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
    }
    
    private void ReducedHP()
    {
        Cat_HP--;
        CatTextHP.text = Cat_HP.ToString();
    }

}
