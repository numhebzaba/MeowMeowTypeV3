using Firebase.Auth;
using Firebase.Database;
using Firebase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DatamanagerOtherMode : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    public List<ListLetters> DataLetterList = new List<ListLetters>();


    public UserData userData;
    public Typer typer;
    public TutorialTyperPart4 TutorialTyperPart4;
    public CatDeadZone_Typer catDeadZone_Typer;
    public CatSquidGame_Typer catSquidGame_Typer;
    public CatSurvival_Typer catSurvival_Typer;

    public int limitTrackingLoopCalculate = 20;

    public string GameMode = "";


    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    private void Start()
    {
        Debug.Log(userData.UserEmail);
        Debug.Log(userData.UserPassword);
        StartCoroutine(Login(userData.UserEmail, userData.UserPassword));


    }
    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(userData.UserEmail, userData.UserPassword);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        User = LoginTask.Result;

        yield return new WaitForSeconds(1);

        StartCoroutine(UpdateUsernameAuth(userData.UserName));

        //StartCoroutine(UpdateUsernameDatabase(userData.UserName));

    }
    private IEnumerator UpdateUsernameAuth(string _username)
    {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        //Call the Firebase auth update user profile function passing the profile with the username
        var ProfileTask = User.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //Auth username is now updated
        }
    }
    public void UploadDataButton()
    {
        if(GameMode == "Tutorial")
        {
            StartCoroutine(UpdateDate(TutorialTyperPart4.wordPerMinute, TutorialTyperPart4.delayTimeSpan.ToString(@"hh\:mm\:ss"), TutorialTyperPart4.OverallAccuracy));
        }else if (GameMode == "MiniGame")
        {
            if (catDeadZone_Typer != null)
            {
                StartCoroutine(UpdateDate(catDeadZone_Typer.wordPerMinute, catDeadZone_Typer.delayTimeSpan.ToString(@"hh\:mm\:ss"), catDeadZone_Typer.OverallAccuracy));
            }
            else if (catSurvival_Typer != null)
            {
                StartCoroutine(UpdateDate(catSurvival_Typer.wordPerMinute, catSurvival_Typer.delayTimeSpan.ToString(@"hh\:mm\:ss"), catSurvival_Typer.OverallAccuracy));
            }
            else if (catSquidGame_Typer != null)
            {
                StartCoroutine(UpdateDate(catSquidGame_Typer.wordPerMinute, catSquidGame_Typer.delayTimeSpan.ToString(@"hh\:mm\:ss"), catSquidGame_Typer.OverallAccuracy));
            }
            

        }
        else if (GameMode == "Challenge")
        {
            StartCoroutine(UpdateDate(typer.wordPerMinute, typer.delayTimeSpan.ToString(@"hh\:mm\:ss"), typer.OverallAccuracy));
        }

    }
    private IEnumerator UpdateDate(int _Wpm, string _Time, float _OverallAccuracy)
    {
        string Date_StringValue = "";
        if (GameMode == "Tutorial")
        {
            Date_StringValue = TutorialTyperPart4.aDate.ToString("MM:dd:yyyy hh:mm tt");
        }
        else if (GameMode == "MiniGame")
        {
            if (catDeadZone_Typer != null)
            {
                Date_StringValue = catDeadZone_Typer.aDate.ToString("MM:dd:yyyy hh:mm tt");

            }
            else if (catSurvival_Typer != null)
            {
                Date_StringValue = catSurvival_Typer.aDate.ToString("MM:dd:yyyy hh:mm tt");

            }
            else if (catSquidGame_Typer != null)
            {
                Date_StringValue = catSquidGame_Typer.aDate.ToString("MM:dd:yyyy hh:mm tt");

            }

        }
        else if (GameMode == "Challenge")
        {
            Date_StringValue = typer.aDate.ToString("MM:dd:yyyy hh:mm tt");
        }
        //Set the currently logged in user Date
        Debug.Log(Date_StringValue);

        var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("HistoryPlay").Child(Date_StringValue).Child("Date").SetValueAsync(Date_StringValue);


        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

        if (DBTask2.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask2.Exception}");
        }
        else
        {
            //Date is now updated
        }

        StartCoroutine(UpdateUsernameDatabase(userData.UserName, Date_StringValue));

        StartCoroutine(UpdateWpm(_Wpm, Date_StringValue));
        StartCoroutine(UpdateOverallAccuracy(_OverallAccuracy, Date_StringValue));
        StartCoroutine(UpdateTime(_Time, Date_StringValue));
        StartCoroutine(UpdateModeRank(Date_StringValue));

        if (GameMode == "Tutorial")
        {
            foreach (var item in TutorialTyperPart4.DataLetterList)
            {
                StartCoroutine(UpdateLetterCorrectTypedData(item.getName, item.GetCorrect, Date_StringValue));
                StartCoroutine(UpdateLetterInCorrectTypedData(item.getName, item.GetWrongData, Date_StringValue));
                StartCoroutine(UpdateLetterAccuracyTypedData(item.getName, item.GetAccuracy, Date_StringValue));
                StartCoroutine(UpdateLetterSpeedTypedData(item.getName, item.GetSpeed, Date_StringValue));
            }
        }
        else if (GameMode == "MiniGame")
        {
            if (catDeadZone_Typer != null)
            {
                foreach (var item in catDeadZone_Typer.DataLetterList)
                {
                    StartCoroutine(UpdateLetterCorrectTypedData(item.getName, item.GetCorrect, Date_StringValue));
                    StartCoroutine(UpdateLetterInCorrectTypedData(item.getName, item.GetWrongData, Date_StringValue));
                    StartCoroutine(UpdateLetterAccuracyTypedData(item.getName, item.GetAccuracy, Date_StringValue));
                    StartCoroutine(UpdateLetterSpeedTypedData(item.getName, item.GetSpeed, Date_StringValue));
                }
            }
            else if (catSurvival_Typer != null)
            {
                foreach (var item in catSurvival_Typer.DataLetterList)
                {
                    StartCoroutine(UpdateLetterCorrectTypedData(item.getName, item.GetCorrect, Date_StringValue));
                    StartCoroutine(UpdateLetterInCorrectTypedData(item.getName, item.GetWrongData, Date_StringValue));
                    StartCoroutine(UpdateLetterAccuracyTypedData(item.getName, item.GetAccuracy, Date_StringValue));
                    StartCoroutine(UpdateLetterSpeedTypedData(item.getName, item.GetSpeed, Date_StringValue));
                }
            }
            else if (catSquidGame_Typer != null)
            {
                foreach (var item in catSquidGame_Typer.DataLetterList)
                {
                    StartCoroutine(UpdateLetterCorrectTypedData(item.getName, item.GetCorrect, Date_StringValue));
                    StartCoroutine(UpdateLetterInCorrectTypedData(item.getName, item.GetWrongData, Date_StringValue));
                    StartCoroutine(UpdateLetterAccuracyTypedData(item.getName, item.GetAccuracy, Date_StringValue));
                    StartCoroutine(UpdateLetterSpeedTypedData(item.getName, item.GetSpeed, Date_StringValue));
                }
            }
        }
        else if (GameMode == "Challenge")
        {
            foreach (var item in typer.DataLetterList)
            {
                StartCoroutine(UpdateLetterCorrectTypedData(item.getName, item.GetCorrect, Date_StringValue));
                StartCoroutine(UpdateLetterInCorrectTypedData(item.getName, item.GetWrongData, Date_StringValue));
                StartCoroutine(UpdateLetterAccuracyTypedData(item.getName, item.GetAccuracy, Date_StringValue));
                StartCoroutine(UpdateLetterSpeedTypedData(item.getName, item.GetSpeed, Date_StringValue));
            }
        }

        StartCoroutine(UpdateAverageAccuracyAndSpeed());

    }
    private IEnumerator UpdateUsernameDatabase(string _username, string _Date)
    {
        //Set the currently logged in user username in the database
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("HistoryPlay").Child(_Date).Child("Username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
        }
    }
    private IEnumerator UpdateWpm(int _Wpm, string _Date)
    {

        //Set the currently logged in user Wpm
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("HistoryPlay").Child(_Date).Child("Wpm").SetValueAsync(_Wpm);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Wpms are now updated
        }
    }
    private IEnumerator UpdateOverallAccuracy(float _OverallAccuracy, string _Date)
    {
        //Set the currently logged in user Wpm
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("HistoryPlay").Child(_Date).Child("Accuracy").SetValueAsync(_OverallAccuracy);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Wpms are now updated
        }
    }
    private IEnumerator UpdateTime(string _Time, string _Date)
    {
        //Set the currently logged in user Time
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("HistoryPlay").Child(_Date).Child("Time").SetValueAsync(_Time);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Time is now updated
        }
    }
    private IEnumerator UpdateModeRank(string _Date)
    {
        //Set the currently logged in user Time
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("HistoryPlay").Child(_Date).Child("Mode").SetValueAsync(GameMode);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Time is now updated
        }
    }
    private IEnumerator UpdateLetterCorrectTypedData(string letter, int Correct, string _Date)
    {
        //Set the currently logged in user Time
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("HistoryPlay").Child(_Date).Child("Letters").Child(letter).Child("Correct").SetValueAsync(Correct);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Time is now updated
        }
    }
    private IEnumerator UpdateLetterInCorrectTypedData(string letter, int Incorrect, string _Date)
    {
        //Set the currently logged in user Time
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("HistoryPlay").Child(_Date).Child("Letters").Child(letter).Child("Incorrect").SetValueAsync(Incorrect);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Time is now updated
        }
    }
    private IEnumerator UpdateLetterAccuracyTypedData(string letter, float accuracy, string _Date)
    {
        //Set the currently logged in user Time
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("HistoryPlay").Child(_Date).Child("Letters").Child(letter).Child("Accuracy").SetValueAsync(accuracy);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Time is now updated
        }
    }
    private IEnumerator UpdateLetterSpeedTypedData(string letter, float Speed, string _Date)
    {
        //Set the currently logged in user Time
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("HistoryPlay").Child(_Date).Child("Letters").Child(letter).Child("Speed").SetValueAsync(Speed);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Time is now updated
        }
    }
    private IEnumerator UpdateAverageAccuracyAndSpeed()
    {
        AddEngLetterlist();
        //Get all the users data ordered by Wpms amount
        var DBTask1 = DBreference.Child("users").Child(User.UserId).Child("HistoryPlay").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

        if (DBTask1.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask1.Exception}");
        }
        else
        {
            DataSnapshot snapshot1 = DBTask1.Result;
            int limitHistoryLoop = 0;
            foreach (DataSnapshot childSnapshot in snapshot1.Children.Reverse<DataSnapshot>())
            {
                if (limitHistoryLoop == limitTrackingLoopCalculate)
                    break;
                limitHistoryLoop++;

                string Date = childSnapshot.Child("Date").Value.ToString();
                Debug.Log(Date);

                var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("HistoryPlay").Child(Date).Child("Letters").GetValueAsync();
                yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);


                if (DBTask2.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {DBTask2.Exception}");
                }
                else
                {
                    //Data has been retrieved
                    DataSnapshot snapshot2 = DBTask2.Result;


                    //Loop through every users UID

                    foreach (DataSnapshot childSnapshot2 in snapshot2.Children.Reverse<DataSnapshot>())
                    {
                        float accuracy = float.Parse(childSnapshot2.Child("Accuracy").Value.ToString());
                        float speed = float.Parse(childSnapshot2.Child("Speed").Value.ToString());
                        Debug.Log("Date " + Date + " " + childSnapshot2.Key + " acc : " + accuracy);
                        Debug.Log("Date " + Date + " " + childSnapshot2.Key + " speed : " + speed);

                        foreach (var item in DataLetterList)
                        {
                            if (childSnapshot2.Key == item.getName)
                            {
                                item.AverageAccuracy += accuracy;
                                item.AverageSpeed += speed;
                                item.UpdateData();
                            }
                        }
                    }
                }
            }
            foreach (var item in DataLetterList)
            {
                item.GetAverageAccuracyAndSpeed();
                StartCoroutine(UpdateAverageAccuracy(item.getName, item.AverageAccuracy));
                StartCoroutine(UpdateAverageSpeed(item.getName, item.AverageSpeed));

                Debug.Log(item.getName + " Accuracy : " + item.AverageAccuracy);
                Debug.Log(item.getName + " Speed : " + item.AverageSpeed);
            }
        }
    }
    private IEnumerator UpdateAverageAccuracy(string letter, float _AverageAccuracy)
    {
        //Set the currently logged in user Wpm
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("TrackingProgress").Child("Letters").Child(letter).Child("AverageAccuracy").SetValueAsync(_AverageAccuracy);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Wpms are now updated
        }
    }
    private IEnumerator UpdateAverageSpeed(string letter, float _AverageSpeed)
    {
        //Set the currently logged in user Wpm
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("TrackingProgress").Child("Letters").Child(letter).Child("AverageSpeed").SetValueAsync(_AverageSpeed);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Wpms are now updated
        }
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
