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
    public int LimitWord = 30;

    private List<string> workingWords = new List<string>();
    private List<string> TutorialwordList = new List<string>()
    {
        "Hello", "Good", "Morning", "Friend", "Family", "Love", "Home", "Work", "School", "Study", "Food", "Eat",
        "Drink", "Water", "Coffee", "Tea", "Sleep", "Dream", "Wake", "Up", "Sunrise", "Sunset", "Sky", "Cloud",
        "Rain", "Snow", "Weather", "Cold", "Hot", "Day", "Night", "Sun", "Moon", "Stars", "Nature", "Park", "Walk",
        "Run", "Exercise", "Health", "Happy", "Sad", "Laugh", "Cry", "Smile", "Frown", "Music", "Dance", "Sing",
        "Song", "Art", "Paint", "Read", "Book", "Write", "Pen", "Paper", "Learn", "Know", "Think", "Imagine", "Create",
        "Build", "Design", "Plan", "Goal", "Achieve", "Success", "Failure", "Mistake", "Fix", "Improve", "Change",
        "Grow", "Friendship", "Trust", "Help", "Kind", "Give", "Receive", "Thanks", "Sorry", "Please", "Excuse",
        "Time", "Now", "Past", "Present", "Future", "Today", "Tomorrow", "Yesterday", "Month", "Year", "Week", "Day",
        "Hour", "Minute", "Second", "Clock", "Watch", "Calendar", "Date", "Holiday", "Celebrate", "Party", "Gift",
        "Shop", "Buy", "Sell", "Money", "Cost", "Save", "Budget", "Job", "Career", "Business", "Office", "Meeting",
        "Team", "Colleague", "Boss", "Employee", "Customer", "Service", "Product", "Quality", "Price", "Sale", "Discount",
        "Market", "Buyer", "Seller", "Brand", "Company", "Project", "Task", "Deadline", "Challenge", "Problem", "Solution",
        "Technology", "Computer", "Internet", "Website", "Social", "Media", "Post", "Share", "Like", "Comment", "Photo",
        "Video", "Call", "Message", "Email", "Chat", "Conversation", "Talk", "Listen", "Understand", "Question", "Answer",
        "Knowledge", "Learn", "Teach", "Teacher", "Student", "Class", "Subject", "Test", "Exam", "Score", "Pass", "Fail",
        "Degree", "Diploma", "Certificate", "Education", "University", "College", "Graduate", "Undergraduate", "Schooling",
        "Lesson", "Study", "Homework", "Quiz", "Essay", "Project", "Research", "Discovery", "Experiment", "Laboratory",
        "Scientist", "Researcher", "Analysis", "Data", "Information", "Wisdom", "Understanding", "Intelligence", "Smart",
        "Clever", "Wise", "Brilliant", "Genius", "Stupid", "Dumb", "Foolish", "Silly", "Ignorant", "Unaware", "Oblivious",
        "Conscious", "Aware", "Mindful", "Thoughtful", "Considerate", "Caring", "Compassionate", "Empathetic", "Sympathetic",
        "Listening", "Hearing", "Seeing", "Observing", "Perceiving", "Sensing", "Feeling", "Touching", "Tasting", "Smelling",
        "Sight", "Sound", "Taste", "Touch", "Smell", "Vision", "Hearing", "Sense", "Perception", "Intuition", "Instinct",
        "Emotion", "Mood", "Temperament", "Attitude", "Behavior", "Action", "Reaction", "Response", "Consequence", "Outcome",
        "Result", "Effect", "Affect", "Impact", "Influence", "Power", "Strength", "Weakness", "Vulnerability", "Courage",
        "Bravery", "Fear", "Afraid", "Anxiety", "Worry", "Stress", "Pressure", "Tension", "Relax", "Relief", "Calm",
        "Tranquil", "Peace", "War", "Conflict", "Struggle", "Obstacle", "Barrier", "Hurdle", "Difficulty", "Solution",
        "Resolve", "Fix", "Mend", "Repair", "Heal", "Cure", "Treatment", "Therapy", "Medicine", "Doctor", "Nurse", "Hospital",
        "Clinic", "Emergency", "Urgency", "Priority", "Importance", "Significance", "Value", "Worth", "Price", "Cost",
        "Expense", "Budget", "Invest", "Profit", "Loss", "Growth", "Development", "Progress", "Change", "Transformation",
        "Evolution", "Revolution", "Innovation", "Creativity", "Invention", "Exploration", "Adventure", "Journey", "Travel",
        "Explore", "Destination", "Place", "Location", "Site", "Scene", "View", "Observation", "Witness", "See", "Look",
        "Observe", "Notice", "Realize", "Recognize", "Comprehend", "Grasp", "Educate", "Instruct", "Guide", "Lead", "Follow",
        "Believe", "Doubt", "Wonder", "Curiosity", "Interest", "Fascination", "Passion", "Care", "Concern", "Affection",
        "Embrace", "Hug", "Kiss", "Hold", "Release", "Let", "Go", "Free", "Liberty", "Freedom", "Choice", "Decision",
        "Choose", "Decide", "Option", "Alternative", "Preference", "Prefer", "Dislike", "Hate", "Anger", "Mad", "Upset",
        "Irritated", "Frustrated", "Annoyed", "Pleased", "Delighted", "Content", "Satisfied", "Fulfilled", "Joyful", "Joy",
        "Happiness", "Sadness", "Sorrow", "Grief", "Regret", "Remorse", "Lost", "Find", "Search", "Seek", "Greet", "Welcome",
        "Companion", "Kin", "Dwelling", "Occupation", "Institute", "Nourishment", "Consume", "Beverage", "Hydration", "Brew",
        "Infusion", "Slumber", "Fantasy", "Awaken", "Rise", "Ascent", "Heavens", "Atmosphere", "Precipitation", "Flakes",
        "Conditions", "Chilly", "Scorching", "Daytime", "Nocturnal", "Star", "Lunar", "Celestial", "Scenery", "Cumulus",
        "Downpour", "Frozen", "Meteorological", "Temperature", "Heat", "Daybreak", "Darkness", "Sol", "Luna", "Constellations",
        "Natural", "Garden", "Stroll", "Jog", "Physical", "Well-being", "Joyous", "Unhappy", "Chuckle", "Weep", "Grin", "Pout",
        "Melody", "Movement", "Vocalize", "Melodic", "Composition", "Artistic", "Sketch", "Literature", "Journal", "Quill",
        "Document", "Stationery", "Familiarize", "Ponder", "Envision", "Generate", "Construct", "Scheme", "Objective",
        "Attainment", "Triumph", "Fiasco", "Blunder", "Rectify", "Boost", "Alteration", "Develop", "Companionship", "Reliance",
        "Assistance", "Generous", "Provide", "Accept", "Appreciation", "Apology", "Kindly", "Grant", "Gratitude", "Kindness",
        "Utmost", "Now", "Earlier", "Contemporary", "Presently", "Subsequently", "Yesterday", "Calendar", "Appointment", "Date",
        "Festivity", "Celebrate", "Gala", "Present", "Retail", "Transaction", "Currency", "Expenditure", "Thrifty", "Economize",
        "Financial", "Occupation", "Vocation", "Commercial", "Reunion", "Fellow", "Administrator", "Supervisor", "Worker",
        "Consumer", "Merchandise", "Caliber", "Value", "Amount", "Preserve", "Economize", "Funds", "Allocate", "Duty",
        "Profession", "Workplace", "Assembly", "Associate", "Overseer", "Boss", "Employee", "Client", "Service", "Merchandise",
        "Superiority", "Cost", "Discount", "Bazaar", "Purchaser", "Vendor", "Mark", "Corporation", "Clientele", "Undertaking",
        "Mission", "Due", "Constraint", "Crisis", "Problem", "Resolution", "Technology", "Processor", "Web", "Page",
        "Socialize", "Mass", "Publication", "Post", "Participate", "Appreciate", "Comment", "Photograph", "Video", "Ring",
        "Text", "Electronic", "Converse", "Conversation", "Communicate", "Speak", "Hear", "Understand", "Inquire",
        "Response", "Facts", "Data", "Information", "Wisdom", "Realization", "Awareness", "Intellect", "Clever",
        "Adequate", "Wise", "Bright", "Genius", "Foolish", "Dull", "Simple", "Silly", "Uninformed", "Unmindful",
        "Aware", "Attentive", "Mindful", "Considerate", "Kind", "Empathy", "Pity", "Perceive", "Apprehend", "See",
        "Observe", "Detect", "Feel", "Grasp", "Identify", "Realize", "Acknowledge", "Comprehend", "Learn", "Educate",
        "Instruct", "Guide", "Direct", "Pursue", "Adhere", "Trust", "Confide", "Distrust", "Challenge", "Question",
        "Wonder", "Interest", "Curiosity", "Fascination", "Passion", "Adoration", "Admire", "Awe", "Like", "Dislike",
        "Despise", "Anger", "Enrage", "Infuriate", "Madness", "Irritate", "Frustrate", "Pleasure", "Delight", "Elated",
        "Happy", "Content", "Satisfied", "Fulfillment", "Jovial", "Joy", "Misery", "Sorrow", "Grieving", "Lament",
        "Regretful", "Remorseful", "Deficiency", "Misplace", "Retrieve", "Search", "Seek", "Inspect",
        "Zero", "Zone", "Zoo", "Zest", "Zipper", "Zigzag", "Zephyr", "Zoom", "Zodiac", "Zenith","Xerox", "Xylophone", "X-ray",
        "Xenon", "Xanadu", "Xmas","Yellow", "Yoga", "Yogurt", "Yawn", "Yard", "Yummy", "Youth"
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
        shuffle(workingWords);
        cutoffWord(workingWords);
        //shuffle(workingWords);
        //ConvertToLower(workingWords);

    }
    private void cutoffWord(List<string> list)
    {
        if (list.Count > LimitWord)
            list.RemoveRange(LimitWord, list.Count - LimitWord);
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


        for (int i = 0; i < workingWords.Count; i++)
        {
            nextWord = workingWords[i];
            newWord += nextWord + " ";
        }


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
