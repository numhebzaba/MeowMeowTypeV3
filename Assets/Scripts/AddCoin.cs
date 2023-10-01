using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class AddCoin : MonoBehaviour
{
    public int UserCurrency = 0;
    public int CoinToAdd = 0;

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    public UserData userData;
    //public TMP_Text Currency_text;


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
        //selectionCharacterManager.GetComponent<SelectionCharacterManager>();

    }
    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(userData.UserEmail, userData.UserPassword);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        User = LoginTask.Result;


        StartCoroutine(UpdateUsernameAuth(userData.UserName));
        //yield return new WaitForSeconds(1);


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
    public void AddCoinWhenFinish()
    {
        StartCoroutine(GetUserCoin());
    }
    public IEnumerator GetUserCoin()
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("Shop").Child("Currency").GetValueAsync();
        //var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("Shop").Child("Currency").Child("Coin").SetValueAsync(0);


        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Loop through every users UID

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>().Reverse())
            {

                UserCurrency = int.Parse(childSnapshot.Value.ToString());
                
                Debug.Log(UserCurrency);

            }
            UserCurrency = UserCurrency + CoinToAdd;
            StartCoroutine(AddCoinToDatabase(UserCurrency));

        }
    }
    public IEnumerator AddCoinToDatabase(int UserCurrency)
    {
        Debug.Log("CreateCurrencyUserData");
        var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("Shop").Child("Currency").Child("Coin").SetValueAsync(UserCurrency);
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

        if (DBTask2.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask2.Exception}");
        }
        else
        {

        }
    }
}
