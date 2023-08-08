using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    public UserData userData;
    public Typer typer;

    [Header("UserData")]
    public Transform historyContent;
    public Transform leaderboardContent;
    public GameObject scoreElement;



    public GameObject HistoryUI;
    public GameObject LeaderboardUI;
    public bool IsHistoryUIActive = false;
    public bool IsLeaderboardUIActive = false;

    public List<ListLetters> DataLetterList = new List<ListLetters>();

    public ShowKeyboardManager showKeyboardManager;

    public UIChallenge UIChallenge;

    public List<Achievement> AchievementList = new List<Achievement>();
    public AchievementNotificationcontroller _achievementNotificationcontroller;
    bool IsAchievementUIOpen = false;

    public UIGameManager _UIGameManager;


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
    public void UploadDataButton()
    {
        StartCoroutine(UpdateDate(typer.wordPerMinute, typer.delayTimeSpan.ToString(@"hh\:mm\:ss"), typer.OverallAccuracy));

    }
    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(userData.UserEmail, userData.UserPassword);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        User = LoginTask.Result;

        yield return new WaitForSeconds(2);

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

    private IEnumerator UpdateDate(int _Wpm, string _Time, float _OverallAccuracy)
    {

        string Date_StringValue = typer.aDate.ToString("MM:dd:yyyy hh:mm tt");
        //Set the currently logged in user Date
        Debug.Log(Date_StringValue);

        Debug.Log(typer.aDate.ToString("MM/dd/yyyy hh:mm tt"));
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


        foreach (var item in typer.DataLetterList)
        {
            StartCoroutine(UpdateLetterCorrectTypedData(item.getName, item.GetCorrect, Date_StringValue));
            StartCoroutine(UpdateLetterInCorrectTypedData(item.getName, item.GetWrongData, Date_StringValue));
            StartCoroutine(UpdateLetterAccuracyTypedData(item.getName, item.GetAccuracy, Date_StringValue));
            StartCoroutine(UpdateLetterSpeedTypedData(item.getName, item.GetSpeed, Date_StringValue));
        }
        StartCoroutine(LoadTheBestWpm(userData.UserName, _Wpm, Date_StringValue, _Time));
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
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("HistoryPlay").Child(_Date).Child("Wpm").SetValueAsync(_OverallAccuracy);

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
                if (limitHistoryLoop == 10)
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

    private IEnumerator LoadTheBestWpm(string _username, int _Wpm, string _Date, string _Time)
    {

        //Get all the users data ordered by Wpms amount
        var DBTask = DBreference.Child("leaderboard").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            Debug.Log("fail load Best Word Per Minute");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            //foreach (Transform child in historyContent.transform)
            //{
            //    Destroy(child.gameObject);
            //}

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("Username").Value.ToString();
                int ThebestWpm = int.Parse(childSnapshot.Child("Wpm").Value.ToString());

                if (_Wpm > ThebestWpm)
                {
                    UpdateTheBestWpm(_username, _Wpm, _Date, _Time);
                    Debug.Log("1st Best Word Per Minute");
                }
                else if (_Wpm > ThebestWpm)
                {
                    Debug.Log("Best Word Per Minute does not change");
                }
                else
                {
                    UpdateTheBestWpm(_username, _Wpm, _Date, _Time);
                    Debug.Log("New Best Word Per Minute");
                }
            }

        }
    }
    private void UpdateTheBestWpm(string _username, int _Wpm, string _Date, string _Time)
    {
        StartCoroutine(UpdateTheBestWpm_Date(_Date));
        StartCoroutine(UpdateTheBestWpm_Wpm(_Wpm));
        StartCoroutine(UpdateTheBestWpm_username(_username));
        StartCoroutine(UpdateTheBestWpm_Time(_Time));

    }
    private IEnumerator UpdateTheBestWpm_Date(string _Date)
    {
        var DBTask = DBreference.Child("leaderboard").Child(User.UserId).Child("Date").SetValueAsync(_Date);

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
    private IEnumerator UpdateTheBestWpm_Wpm(int _Wpm)
    {
        var DBTask = DBreference.Child("leaderboard").Child(User.UserId).Child("Wpm").SetValueAsync(_Wpm);

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
    private IEnumerator UpdateTheBestWpm_username(string _username)
    {

        var DBTask = DBreference.Child("leaderboard").Child(User.UserId).Child("Username").SetValueAsync(_username);
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
    private IEnumerator UpdateTheBestWpm_Time(string _Time)
    {

        var DBTask = DBreference.Child("leaderboard").Child(User.UserId).Child("Time").SetValueAsync(_Time);
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
    public void HistoryButton()
    {
        IsHistoryUIActive = !IsHistoryUIActive;
        if (IsHistoryUIActive)
        {
            StartCoroutine(LoadHistoryData());
            HistoryUI.SetActive(true);

        } else
            HistoryUI.SetActive(false);

    }
    private IEnumerator LoadHistoryData()
    {
        //Get all the users data ordered by Wpms amount
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("HistoryPlay").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in historyContent.transform)
            {
                Destroy(child.gameObject);
            }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("Username").Value.ToString();
                int Wpm = int.Parse(childSnapshot.Child("Wpm").Value.ToString());
                string Time = childSnapshot.Child("Time").Value.ToString();
                string Date = childSnapshot.Child("Date").Value.ToString();

                //Instantiate new scoreboard elements
                GameObject scoreboardElement = Instantiate(scoreElement, historyContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, Wpm, Time, Date);
            }

        }
    }
    public void LeaderboardButton()
    {
        IsLeaderboardUIActive = !IsLeaderboardUIActive;
        if (IsLeaderboardUIActive)
        {
            StartCoroutine(LoadLeaderboardData());
            LeaderboardUI.SetActive(true);

        }
        else
            LeaderboardUI.SetActive(false);

    }
    private IEnumerator LoadLeaderboardData()
    {
        //Get all the users data ordered by Wpms amount
        //var DBTask = DBreference.Child("users").Child(User.UserId).Child("TheBestWpm").GetValueAsync();
        var DBTask = DBreference.Child("leaderboard").OrderByChild("Wpm").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in leaderboardContent.transform)
            {
                Destroy(child.gameObject);
            }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("Username").Value.ToString();
                int Wpm = int.Parse(childSnapshot.Child("Wpm").Value.ToString());
                string Time = childSnapshot.Child("Time").Value.ToString();
                string Date = childSnapshot.Child("Date").Value.ToString();
                Debug.Log(username + " : " + Wpm);
                //Instantiate new scoreboard elements
                GameObject scoreboardElement = Instantiate(scoreElement, leaderboardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, Wpm, Time, Date);
            }

        }
    }

    public void LoadAccAndSpeedButton()
    {
        StartCoroutine(UpdateAverageAccuracyAndSpeed());
        StartCoroutine(LoadTrackingProgressData());
    }
    private IEnumerator LoadTrackingProgressData()
    {
        //Get all the users data ordered by Wpms amount
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("TrackingProgress").Child("Letters").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in leaderboardContent.transform)
            {
                Destroy(child.gameObject);
            }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                float AverageAccuracy = float.Parse(childSnapshot.Child("AverageAccuracy").Value.ToString());
                float AverageSpeed = float.Parse(childSnapshot.Child("AverageSpeed").Value.ToString());

                foreach (var switches in showKeyboardManager.SwitchesList)
                {
                    if (switches.nameSwitch == childSnapshot.Key)
                    {
                        switches.AverageAccuracy = AverageAccuracy;
                        switches.AverageSpeed = AverageSpeed;
                    }
                }

                Debug.Log(childSnapshot.Key);
                Debug.Log(AverageAccuracy);
                Debug.Log(AverageSpeed);


            }
            showKeyboardManager.ShowKeyboard();
        }

    }
    public void LoadChallengeDataButton()
    {
        StartCoroutine(LoadChallengeData());
    }
    public IEnumerator LoadChallengeData()
    {
        //Get all the users data ordered by Wpms amount
        //var DBTask = DBreference.Child("users").Child(User.UserId).Child("TheBestWpm").GetValueAsync();
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("Challenge").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in leaderboardContent.transform)
            {
                Destroy(child.gameObject);
            }
            int CompleteLevel = 0;
            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                CompleteLevel++;
            }
            UIChallenge.SetChallengeButton(CompleteLevel);
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

    public IEnumerator LoadPlayerAchievementData()
    {
        //Get all the users data ordered by Wpms amount
        //var DBTask = DBreference.Child("users").Child(User.UserId).Child("TheBestWpm").GetValueAsync();

        var DBTask = DBreference.Child("achievement").OrderByChild("Id").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            ////Destroy any existing scoreboard elements
            //foreach (Transform child in leaderboardContent.transform)
            //{
            //    Destroy(child.gameObject);
            //}

            //Loop through every users UID
            AchievementList.Clear();
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {

                string Id = childSnapshot.Child("Id").Value.ToString();
                string Description = childSnapshot.Child("Description").Value.ToString();
                string Title = childSnapshot.Child("Title").Value.ToString();


                var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("achievement").GetValueAsync();

                yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

                if (DBTask2.Result == null)
                {
                    //AchievementList.Add(new Achievement(Id, Description, Title, "locked"));
                }
                else
                {
                    //Data has been retrieved
                    DataSnapshot snapshot2 = DBTask2.Result;

                    //Destroy any existing scoreboard elements
                    foreach (Transform child in historyContent.transform)
                    {
                        Destroy(child.gameObject);
                    }

                    //Loop through every users UID
                    foreach (DataSnapshot childSnapshot2 in snapshot2.Children.Reverse<DataSnapshot>())
                    {
                        //string achievement = childSnapshot.Child("achievement").Value.ToString();
                        string achievement = childSnapshot2.Value.ToString();
                        if (achievement == "unlocked")
                        {
                            if (childSnapshot2.Key == Id)
                            {
                                AchievementList.Add(new Achievement(Id, Description, Title, "unlocked"));
                                Debug.Log(Id + Description + "----------------------");

                            }


                        }
                        else
                        {



                        }

                        //Instantiate new scoreboard elements
                    }

                }
                int temp = 0;
                for (int i = 0; i < AchievementList.Count; i++)
                {
                    if (Id == AchievementList[i].Id)
                    {

                    }
                    else
                    {
                        temp++;

                    }

                }
                if (temp == AchievementList.Count)
                {
                    AchievementList.Add(new Achievement(Id, Description, Title, "locked"));
                    Debug.Log(Id + Description + "+++++++++++++++++++++++");
                }


                //AchievementList.Add(new Achievement(Id,Description,Title));
                AchievementList.Reverse();






            }
            _achievementNotificationcontroller.LoadAchievementTable();

        }
    }
    public void LoadPlayerAchievementButton()
    {
        if (!IsAchievementUIOpen)
        {

            _UIGameManager.UIArray[4].SetActive(true);
            StartCoroutine(LoadPlayerAchievementData());
            IsAchievementUIOpen = !IsAchievementUIOpen;
        }
        else
        {
            _UIGameManager.UIArray[4].SetActive(false);
            IsAchievementUIOpen = !IsAchievementUIOpen;

        }

    }
    public IEnumerator AchievementUnlocked(int AchievemenId)
    {
        //Get all the users data ordered by Wpms amount
        //var DBTask = DBreference.Child("users").Child(User.UserId).Child("TheBestWpm").GetValueAsync();

        var DBTask = DBreference.Child("achievement").OrderByChild("Id").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            ////Destroy any existing scoreboard elements
            //foreach (Transform child in leaderboardContent.transform)
            //{
            //    Destroy(child.gameObject);
            //}

            //Loop through every users UID
            AchievementList.Clear();
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {

                string Id = childSnapshot.Child("Id").Value.ToString();
                string Description = childSnapshot.Child("Description").Value.ToString();
                string Title = childSnapshot.Child("Title").Value.ToString();

                AchievementList.Add(new Achievement(Id,Description,Title,"unlocked"));
                AchievementList.Reverse();





            }
            _achievementNotificationcontroller.ShowNoticationButton(AchievemenId);

        }
    }
    public void showUnlockedAchievementButton(int AchievemenId)
    {
        AchievemenId = 0;
        StartCoroutine(AchievementUnlocked(AchievemenId));

    }

}
