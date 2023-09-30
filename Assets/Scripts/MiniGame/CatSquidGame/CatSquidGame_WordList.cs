using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class CatSquidGame_WordList : MonoBehaviour
{
    public TMP_InputField WordInput;
    public TMP_Text TextListOutput;
    public GameObject CoundownPanel;
    public GameObject TyperPanel;
    public TMP_Text CountDownTime;
    string StringInput;
    string CurrentListText = null;

    public GameObject WinerPanel;
    public GameObject GameOverPanel;

    private List<string> workingWords = new List<string>();
    private List<string> TutorialwordList = new List<string>()
    {
        //"ffff","ffff","ffff","jjjj","jjjj","jjjj","dddd","dddd","dddd","kkkk","kkkk","kkkk",
        //"ssss", "ssss", "ssss","llll","llll","llll","aaaa","aaaa","aaaa",";;;;",";;;;"
        "Trot","Charge","Young"/*,"Synonymous","Eminent","Stereotyped","Lunch","Daily","Wilderness"*/
        //,"Yard","Dance","Deep","Allow","Lace","Colossal","Boiling","Porter","Like","Fire","Tough"
        //,"Dull","Kick","Stitch","Press","Houses","Look","Overt","Little","Step","Invite"
        //,"Courageous","Murky","Invent","Clover","Arrive","Colossal","Whistle","Guard","Troubled","Easy"
        //,"Momentous","Sneeze","Serious","Abashed","Heavy","General","Cent","Slip","Opposite"
        //,"Loose","Harbor","Rhetorical","Gabby","Stomach","Mountainous","Cool","Elderly","Treatment","Plant","Pickle"
        //,"Broad","Cushion","Grape","Hum","Turkey","Level","Learned","Knowledgeable","Frame"
        //,"Wine","Thin","Simple","Telling","Horse","Ladybug","Abstracted","Hapless","Scarecrow","Tie","Cut"
        //,"Absorbing","Different","Toad","Post","Horse","Understood","Complete","Phobic","Distinct","Worm","Functional"
    };

    public CatSquidGame_Typer CustomTyper;

    void Start()
    {
        GameObject.Find("Typer").GetComponent<CatSquidGame_Typer>().enabled = false;
        GameObject.Find("Typer").GetComponent<CatSquidGameChecker>().enabled = false;

        CoundownPanel.SetActive(true);
        StartCoroutine(CountdownAndStart());
    }

    void Update()
    {
        if(WinerPanel.activeInHierarchy == true || GameOverPanel.activeInHierarchy == true)
        {
            GameObject.Find("Typer").GetComponent<CatSquidGame_Typer>().enabled = false;
            GameObject.Find("Typer").GetComponent<CatSquidGameChecker>().enabled = false;
        }
        //Debug.Log(workingWords.Count);
        //Debug.Log(workingWords.First());
    }

    private IEnumerator CountdownAndStart()
    {
        int countdownTime = 4;
        while (countdownTime > 0)
        {
            if (countdownTime == 4)
            {
                CountDownTime.text = "Ready!";
            }
            yield return new WaitForSeconds(1.0f);
            countdownTime--;
            {
                CountDownTime.text = countdownTime.ToString();
            }
        }
        CoundownPanel.SetActive(false);
        GameObject.Find("Typer").GetComponent<CatSquidGame_Typer>().enabled = true;
        GameObject.Find("Typer").GetComponent<CatSquidGameChecker>().enabled = true;


    }

    public void Addtext()
    {
        StringInput = WordInput.GetComponent<TMP_InputField>().text;
        CurrentListText = null;
        TutorialwordList.Add(StringInput);
        for (int i = 0; i < TutorialwordList.Count; i++)
        {
            CurrentListText += TutorialwordList[i] + " ";
        }
        TextListOutput.text = CurrentListText;
        CustomTyper.SetCurrentWord();
        ShowTextCustomList();

    }

    public void Deletetext()
    {
        StringInput = WordInput.GetComponent<TMP_InputField>().text;
        for (int i = 0; i < TutorialwordList.Count; i++)
        {
            if (TutorialwordList[i] == StringInput)
            {
                TutorialwordList.RemoveAt(i);
                TextListOutput.text = CurrentListText;

            }
        }
        CustomTyper.SetCurrentWord();
        ShowTextCustomList();

    }

    public void IsStartGame()
    {
        GameObject.Find("Typer").GetComponent<CatSquidGame_Typer>().enabled = true;
        TyperPanel.SetActive(true);
    }


    private void PrintWord()
    {
        for (int i = 0; i < TutorialwordList.Count; i++)
        {
            Debug.Log(TutorialwordList[i]);
        }
    }
    public void ShowTextCustomList()
    {
        CurrentListText = null;
        for (int i = 0; i < TutorialwordList.Count; i++)
        {
            CurrentListText += TutorialwordList[i] + " ";
        }
        TextListOutput.text = CurrentListText;

    }


    private void Awake()
    {
        workingWords.AddRange(TutorialwordList);

        //shuffle(workingWords);
        //ConvertToLower(workingWords);

    }

    private void shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int random = Random.Range(i, list.Count);
            string temporary = list[i];

            list[i] = list[random];
            list[random] = temporary;
        }
    }

    public string getWord()
    {
        string newWord = string.Empty;
        string nextWord = string.Empty;


        //if (TutorialwordList.Count != 0)
        //{
        //    newWord = TutorialwordList.First();
        //    TutorialwordList.Remove(newWord);
        //}
        //Debug.Log(newWord);


        for (int i = 0; i < TutorialwordList.Count; i++)
        {
            nextWord = TutorialwordList[i];
            newWord += nextWord + " ";
        }


        return newWord;
    }

    public string getNextWord()
    {
        string newWord = string.Empty;

        if (TutorialwordList.Count >= 1)
        {
            newWord = TutorialwordList[0 + 1];
        }
        return newWord;
    }

    public bool IsWordLeft()
    {
        if (TutorialwordList.Count == 0)
        {
            return false;
        }
        return true;
    }
}
