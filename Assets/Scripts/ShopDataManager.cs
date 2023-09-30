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

public class ShopDataManager : MonoBehaviour
{
    public int UserCurrency = 0;
    public int SkinPrice = 150;
    public GameObject[] skinArray;
    public Button[] BuyButtonArray;
    //public List<string> stateSkinList= new List<string>();
    public SelectionCharacterManager selectionCharacterManager;

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    public UserData userData;
    public TMP_Text Currency_text;

    public GameObject BuyPanel;
    public GameObject CannotBuy_text;




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

        StartCoroutine(UpateDatabaseCurrencyUser());
        


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

    public IEnumerator UpateDatabaseCurrencyUser()
    {
        //Get all the users data ordered by Wpms amount
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("Shop").Child("Currency").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            StartCoroutine(CreateCurrencyUserData());
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result ;


            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {

                string coin = childSnapshot.Value.ToString();
                Debug.Log("Coin : "+coin);

                Currency_text.text = $"{coin.ToString()}";

            }

        }
        StartCoroutine(LoadSkinUserData());
    }
    public IEnumerator CreateCurrencyUserData()
    {
        Debug.Log("CreateCurrencyUserData");
        var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("Shop").Child("Currency").Child("Coin").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

        if (DBTask2.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask2.Exception}");
        }
        else
        {
            Currency_text.text = $"{0}";
        }
        StartCoroutine(CreateSkinUserData());
    }
    public IEnumerator CreateSkinUserData()
    {
        for (int i=0;i < skinArray.Length;i++)
        {
            string i_temp_Name = i.ToString();

            if (i == 0)
            {
                var DBTask = DBreference.Child("users").Child(User.UserId).Child("Shop").Child("Skin").Child(i_temp_Name).SetValueAsync("unlocked");
                yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                if (DBTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                }
                else
                {

                }
            }
            else
            {
                var DBTask = DBreference.Child("users").Child(User.UserId).Child("Shop").Child("Skin").Child(i_temp_Name).SetValueAsync("locked");
                yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                if (DBTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                }
                else
                {

                }
            }
            
        }
        StartCoroutine(LoadSkinUserData());


    }
    public IEnumerator LoadSkinUserData()
    {
        selectionCharacterManager.stateSkinList.Clear();
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("Shop").Child("Skin").GetValueAsync();

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
                string state = childSnapshot.Value.ToString();
                //int i = int.Parse(childSnapshot.Key.ToString());
                Debug.Log("skin : "+childSnapshot.Key +"  " + state);
                //stateSkinList.Add(state);
                selectionCharacterManager.stateSkinList.Add(state);

            }

        }
    }
    public void buySkin()
    {
        StartCoroutine(CheckUserCoinIsEnough());
    }
    public IEnumerator CheckUserCoinIsEnough()
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
            if (UserCurrency >= SkinPrice) 
            {
                int indexSkin = selectionCharacterManager.IndexCharacterSelected;
                int coinLeft = UserCurrency - SkinPrice;
                StartCoroutine(MinusUserCoin(coinLeft));
                StartCoroutine(SendData_buySkin_toFirebase(indexSkin));
                BuyPanel.SetActive(false);
            }
            else
            {
                CannotBuy_text.SetActive(true);
                yield return new WaitForSeconds(1);
                CannotBuy_text.SetActive(false);


            }
        }
    }
    public IEnumerator MinusUserCoin(int coinLeft)
    {
        Debug.Log("CreateCurrencyUserData");
        var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("Shop").Child("Currency").Child("Coin").SetValueAsync(coinLeft);
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

        if (DBTask2.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask2.Exception}");
        }
        else
        {

        }
    }
    public IEnumerator SendData_buySkin_toFirebase(int indexSkin)
    {
        string indexSkinString = indexSkin.ToString();
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("Shop").Child("Skin").Child(indexSkinString).SetValueAsync("unlocked");
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {

        }
        StartCoroutine(UpateDatabaseCurrencyUser());

        //StartCoroutine(LoadSkinUserData());
        yield return new WaitForSeconds(1);
        selectionCharacterManager.IScharacterSelected(indexSkin);
        Debug.Log("Upated Skin already");
    }
}
