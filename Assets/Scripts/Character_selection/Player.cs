using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterDataBase CharacterDB;

    public GameObject AtworkSkin;

    private int selectedOption = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }
        else
        {
            Load();
        }

        UpdateCharacter(selectedOption);
    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }

    public void UpdateCharacter(int CharacterIndex)
    {
        Character character = CharacterDB.GetCharacter(CharacterIndex);
        AtworkSkin = character.CharacterPrefab;
        Instantiate(AtworkSkin, new Vector3(1, 0, 0), Quaternion.identity);
        Debug.Log(AtworkSkin.name);
    }
}