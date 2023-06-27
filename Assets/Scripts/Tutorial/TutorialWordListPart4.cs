using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class TutorialWordListPart4 : MonoBehaviour
{


    private List<string> workingWords = new List<string>();

    private List<string> TutorialwordList = new List<string>()
    {
        //"bbbb","bbbb","vvvv","vvvv","cccc","cccc","xxxx","xxxx","zzzz","zzzz","nnnn","nnnn","mmmm","mmmm"
        //"Trot","Charge","Young","Synonymous","Eminent","Stereotyped","Lunch","Daily","Wilderness"
        //,"Yard","Dance","Deep","Allow","Lace","Colossal","Boiling","Porter","Like","Fire","Tough"
        //,"Dull","Kick","Stitch","Press","Houses","Look","Overt","Little","Step","Invite"
        //,"Courageous","Murky","Invent","Clover","Arrive","Colossal","Whistle","Guard","Troubled","Easy"
        //,"Momentous","Sneeze","Serious","Abashed","Heavy","General","Cent","Slip","Opposite"
        //,"Loose","Harbor","Rhetorical","Gabby","Stomach","Mountainous","Cool","Elderly","Treatment","Plant","Pickle"
        "broad","cushion","grape","hum","turkey","level","learned","knowledgeable","frame",
        "wine","thin","simple","telling","horse","ladybug","abstracted","hapless","scarecrow","tie","cut"
        //,"Absorbing","Different","Toad","Post","Horse","Understood","Complete","Phobic","Distinct","Worm","Functional"
    };

    void Update()
    {
        //Debug.Log(workingWords.Count);
        //Debug.Log(workingWords.First());
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

        if (workingWords.Count != 0)
        {
            newWord = workingWords.First();
            workingWords.Remove(newWord);
        }
        //Debug.Log(newWord);

        

        return newWord;
    }

    public string getNextWord()
    {
        string newWord = string.Empty;

        if (workingWords.Count >= 1)
        {
            newWord = workingWords[0 + 1];
        }
        return newWord;
    }

    public bool IsWordLeft()
    {
        if (workingWords.Count == 0)
        {
            return false;
        }
        return true;
    }



 
}
