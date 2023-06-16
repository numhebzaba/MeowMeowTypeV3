using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class SelectionCharacterManager : MonoBehaviour
{
    public CharacterDataBase CharacterDB;

    public GameObject AtworkSkin;

    private int selectedOption = 0;

    [SerializeField] GameObject[] Camera;
    void Start()
    {
        UpdateCharacter(selectedOption);
        SetCamera();
    }

    public void NetxtOption()
    {  
        selectedOption++;
        Debug.Log(selectedOption);
        if (selectedOption >= CharacterDB.CharacterCount)
        {
            selectedOption = 0;
        }
        UpdateCharacter(selectedOption);
        UpdateCamera(selectedOption);
        Save();
    }

    public void BackOption()
    {
        selectedOption--;
        if (selectedOption < 0  )
        {
            selectedOption = CharacterDB.CharacterCount - 1;
        }
        UpdateCharacter(selectedOption);
        UpdateCamera(selectedOption);
        Save();

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

    private void Save()
    {
        PlayerPrefs.SetInt("selectedOption", selectedOption);
    }

    public void ChangeScene(int ScenceID)
    {
        SceneManager.LoadScene(ScenceID);
    }
}
