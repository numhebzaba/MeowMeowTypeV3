using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CatSurvival_CheckCatHp : MonoBehaviour
{
    public TMP_Text HP;

    private void Update()
    {
        if (HP.text == "0")
        {
            Debug.Log("Game over");
            GameObject.Find("Typer").GetComponent<CatSurvival_Typer>().enabled = false;
        }
    }
}
