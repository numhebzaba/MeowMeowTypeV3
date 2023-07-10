using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class ChallengeModeDataManger : MonoBehaviour
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
        StartCoroutine(Login(userData.UserEmail, userData.UserPassword));

    }
    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
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
    public void UploadDataButton()
    {
        StartCoroutine(UpdateChallengeLevel(1));

    }
    private IEnumerator UpdateChallengeLevel(int level)
    {

        switch (level)
        {
            case 1:
                {
                    var DBTask = DBreference.Child("users").Child(User.UserId).Child("Challenge").Child("Level_1").SetValueAsync("complete");

                    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                    if (DBTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                    }
                    else
                    {
                        //Wpms are now updated
                    }
                    break;
                }
            case 2:
                {
                    var DBTask = DBreference.Child("users").Child(User.UserId).Child("Challenge").Child("Level_2").SetValueAsync("complete");

                    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                    if (DBTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                    }
                    else
                    {
                        //Wpms are now updated
                    }
                    break;
                }
            case 3:
                {
                    var DBTask = DBreference.Child("users").Child(User.UserId).Child("Challenge").Child("Level_3").SetValueAsync("complete");

                    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                    if (DBTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                    }
                    else
                    {
                        //Wpms are now updated
                    }
                    break;
                }
            case 4:
                {
                    var DBTask = DBreference.Child("users").Child(User.UserId).Child("Challenge").Child("Level_4").SetValueAsync("complete");

                    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                    if (DBTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                    }
                    else
                    {
                        //Wpms are now updated
                    }
                    break;
                }
            case 5:
                {
                    var DBTask = DBreference.Child("users").Child(User.UserId).Child("Challenge").Child("Level_5").SetValueAsync("complete");

                    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                    if (DBTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                    }
                    else
                    {
                        //Wpms are now updated
                    }
                    break;
                }
            case 6:
                {
                    var DBTask = DBreference.Child("users").Child(User.UserId).Child("Challenge").Child("Level_6").SetValueAsync("complete");

                    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                    if (DBTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                    }
                    else
                    {
                        //Wpms are now updated
                    }
                    break;
                }
            case 7:
                {
                    var DBTask = DBreference.Child("users").Child(User.UserId).Child("Challenge").Child("Level_7").SetValueAsync("complete");

                    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                    if (DBTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                    }
                    else
                    {
                        //Wpms are now updated
                    }
                    break;
                }
            case 8:
                {
                    var DBTask = DBreference.Child("users").Child(User.UserId).Child("Challenge").Child("Level_8").SetValueAsync("complete");

                    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                    if (DBTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                    }
                    else
                    {
                        //Wpms are now updated
                    }
                    break;
                }
            case 9:
                {
                    var DBTask = DBreference.Child("users").Child(User.UserId).Child("Challenge").Child("Level_9").SetValueAsync("complete");

                    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                    if (DBTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                    }
                    else
                    {
                        //Wpms are now updated
                    }
                    break;
                }
            case 10:
                {
                    var DBTask = DBreference.Child("users").Child(User.UserId).Child("Challenge").Child("Level_10").SetValueAsync("complete");

                    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                    if (DBTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                    }
                    else
                    {
                        //Wpms are now updated
                    }
                    break;
                }

        }
        //Set the currently logged in user Wpm
        
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
