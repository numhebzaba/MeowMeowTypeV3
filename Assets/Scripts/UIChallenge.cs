using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChallenge : MonoBehaviour
{
    public bool IsShowKeyboard = false;
    public UIGameManager _UIGameManager;
    // Start is called before the first frame update
    public void ShowUIChallenge()
    {
        IsShowKeyboard = !IsShowKeyboard;
        if (IsShowKeyboard)
        {
            _UIGameManager.ClearScreen();
            _UIGameManager.showUI(3);// show challenge screen
        }
        else
        {
            _UIGameManager.ClearScreen();
            _UIGameManager.showUI(0);// show Main screen
            return;
        }
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
