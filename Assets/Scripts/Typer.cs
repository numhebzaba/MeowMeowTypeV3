using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class Typer : MonoBehaviour
{
    public WordList wordList = null;
    public TMP_Text wordOutput = null;
    public TMP_Text nextWordOutput = null;
    public TMP_Text wordTotalUI = null;
    public TMP_Text TimeSpent = null;
    public TMP_Text WordPerminuteText = null;

    public TMP_Text A_count = null;
    public TMP_Text A_error = null;

    public GameObject SummaryUI;
    public TMP_Text [] data = null;

    bool IsGameFinish = false;


    public float accuracy = 0;
    public int wordTotal = 0;
    public int allTypedEntries = 0;
    public int unCorrectedError = 0;
    public int wordPerMinute = 0;
    public float OverallAccuracy = 0;
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

    public TimeSpan SpeedType = new TimeSpan(0, 0, 0);

    public DateTime aDate = DateTime.Now;

    public Animator animator;
    public GameObject Keyboard;
    public bool IsKeyboardActive;

    private int selectedOption = 0;
    public CharacterDataBase CharacterDB;
    public GameObject AtworkSkin;

    public GameObject PlayerPosition;

    public AddCoin addCoin;
    public DataManager dataManager;
    public ChallengeModeDataManger challengeModeDataManager;

    private float CountWordIsTrue = 0;
    private float CountWordIsFalse = 0;
    private double CountAccuracy;

    public TMP_Text SummaryAccuracyText;
    public TMP_Text SummaryTimeText;
    public TMP_Text SummaryCorrect;
    public TMP_Text SummaryInCorrect;
    public TMP_Text SummaryWordPerminuteText = null;

    public DatamanagerOtherMode datamanagerOtherMode;

    [SerializeField] SondFxkeyboardManager KeyboardAudio;

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
        BGanimator.speed  = 0;
        SummaryUI.SetActive(false);
        SetCurrentWord();
        AddEngLetterlist();
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
        SpeedType = SpeedType+TimeSpan.FromSeconds(Time.deltaTime);
        CheckInput();
        
        double TimeSpentTotalSec = delayTimeSpan.TotalSeconds;
        if (TimeSpentTotalSec <= 1)
            TimeSpentTotalSec = 1;
        TimeSpentTotalSec = TimeSpentTotalSec / 60;

        wordPerMinute = (int)Math.Round(((((allTypedEntries / 5) - unCorrectedError)) / TimeSpentTotalSec));
        if (wordPerMinute <= 0)
            wordPerMinute = 0;

        if (!wordList.IsWordLeft() && IsWordComplete() )
        {
            delayTimeSpan = delayTimeSpan;
            if(IsGameFinish == false)
            {
                ShowDataTyper();
                if (dataManager != null)
                {
                    SummaryUI.SetActive(true);
                    dataManager.UploadDataButton();
                    addCoin.AddCoinWhenFinish();
                }
                if (challengeModeDataManager != null)
                    challengeModeDataManager.UploadDataButton();
                if(datamanagerOtherMode != null)
                    datamanagerOtherMode.UploadDataButton();
                canDoAction = false;
                IsGameFinish = true;
            }
        }

        else
            delayTimeSpan += TimeSpan.FromSeconds(Time.deltaTime); 

        TimeSpent.text = delayTimeSpan.ToString(@"hh\:mm\:ss");
        wordTotalUI.text = " WordTotal : " + wordTotal.ToString();
        WordPerminuteText.text = " WPM : " + wordPerMinute.ToString();




    }

    public void ShowDataLetter()
    {
        int i = 0;
        foreach (var item in DataLetterList){
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

            if(keyinput.ToLower() == letter.getName)
            {
                letter.UpdateData();
                letter.UpdateAccuracy();
                UpdateOverallAccuracy();

                float SpeedTypedOneLetter = (float)SpeedType.TotalSeconds;
                letter.UpdateSpeed(SpeedTypedOneLetter);
                Debug.Log("SpeedTypedOneLetter : "+ SpeedTypedOneLetter);
                SpeedType = TimeSpan.Zero;

            }
        }
    }

    private void EnterLetter(string typedLetter)
    {
        if (IsCorrectLetter(typedLetter))
        {
            CountWordIsTrue++;
            KeyboardAudio.PlyerAudioKeyboardIsTrue();
            loopBg_1.IsMove = true;
            loopBg_2.IsMove = true;
            loopBg_3.IsMove = true;
            loopBg_4.IsMove = true;

            CheckLetter(typedLetter);
            RemoveLetter();
            BGanimator.speed = 1; //play background animation//
            animationStateController.animator.SetInteger(animationStateController.AnimationHash, 23);

            

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
        KeyboardAudio.PlyerAudioKeyboardIsFalse();
        BGanimator.speed = 0; //Pause background animation//
        //animationStateController.animator.SetBool(animationStateController.isSittingHash, true);
        animationStateController.animator.SetInteger(animationStateController.AnimationHash, 14);


        foreach (var letter in DataLetterList)
        {
            //string temp = remainWord.ToLower();
            if (remainWord.Substring(0,1) == letter.getName)
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
        return remainWord.IndexOf(letter)==0;
    }
    private void RemoveLetter()
    {
        string newString = remainWord.Remove(0,1);
        SetRemainWord(newString);
    }
    private bool IsWordComplete()
    {
        return remainWord.Length == 0;
    }
    public void UpdateOverallAccuracy()
    {

        float OverallAccuracyTemp = (allTypedEntries - unCorrectedError) ;
        OverallAccuracyTemp = OverallAccuracyTemp / allTypedEntries * 100;
        OverallAccuracy = OverallAccuracyTemp;

    }

    public void ShowDataTyper()
    {
        CalculateAccuracy();
        SummaryAccuracyText.text = "Accuracy : " + CountAccuracy + " %";
        SummaryTimeText.text ="Time : " + delayTimeSpan.ToString(@"hh\:mm\:ss");
        SummaryCorrect.text = "Correct : " + CountWordIsTrue;
        SummaryInCorrect.text = "Incorrect : " + CountWordIsFalse;
        SummaryWordPerminuteText.text = " WPM : " + wordPerMinute.ToString();
    }

    private void CalculateAccuracy()
    {
        float AllCount = CountWordIsTrue + CountWordIsFalse;
        CountAccuracy = Math.Round((CountWordIsTrue * 100) / AllCount);
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
}
