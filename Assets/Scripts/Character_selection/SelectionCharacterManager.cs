using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class SelectionCharacterManager : MonoBehaviour
{
    public CharacterDataBase CharacterDB;

    public GameObject AtworkSkin;

    private int selectedOption;
    private int IndexCharacterSelected;

    public GameObject SelectButton;
    public GameObject IsSelectedButton;

    [SerializeField] GameObject[] Camera;
    void Start()
    {
        Load();
        UpdateCharacter(selectedOption);
        UpdateCamera(selectedOption);
        IndexCharacterSelected = selectedOption;
        IScharacterSelected(IndexCharacterSelected);


    }

    public void NetxtOption()
    {
        IndexCharacterSelected++;
        selectedOption++;
        Debug.Log(selectedOption);
        if (selectedOption >= CharacterDB.CharacterCount)
        {
            selectedOption = 0;
        }
        UpdateCharacter(selectedOption);
        UpdateCamera(selectedOption);

        IScharacterSelected(IndexCharacterSelected);
    }

    public void BackOption()
    {
        IndexCharacterSelected--;
        selectedOption--;
        if (selectedOption < 0  )
        {
            selectedOption = CharacterDB.CharacterCount - 1;
        }
        UpdateCharacter(selectedOption);
        UpdateCamera(selectedOption);
        IScharacterSelected(IndexCharacterSelected);

    }

    private void UpdateCharacter(int CharacterIndex)
    {   
        Character character = CharacterDB.GetCharacter(CharacterIndex);
        AtworkSkin = character.CharacterPrefab;
        Debug.Log(AtworkSkin.name);
    }
    
    private void UpdateCamera(int CharacterIndex)
    {
        for (int i = 0; i <= Camera.Length-1; i++)
        {
            Camera[i].SetActive(false);
        }
        Camera[CharacterIndex].SetActive(true);
    }

    private void SetCamera()
    {
        for (int i =1; i <= Camera.Length - 1; i++)
        {
            Camera[i].SetActive(false);
        }
        Camera[0].SetActive(true);
    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }

    public void Save()
    {
        PlayerPrefs.SetInt("selectedOption", selectedOption);
        IScharacterSelected(IndexCharacterSelected);

    }

    public void ChangeScene(int ScenceID)
    {
        SceneManager.LoadScene(ScenceID);
    }

    private void IScharacterSelected(int IndexCharacterSelected)
    {
        if (PlayerPrefs.GetInt("selectedOption") == IndexCharacterSelected)
        {
            SelectButton.SetActive(false);
            IsSelectedButton.SetActive(true);
            return;
        }
        SelectButton.SetActive(true);
        IsSelectedButton.SetActive(false);
    }
}
