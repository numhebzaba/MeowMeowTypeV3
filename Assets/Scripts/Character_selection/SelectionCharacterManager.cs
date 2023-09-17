using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class SelectionCharacterManager : MonoBehaviour
{
    public CharacterDataBase CharacterDB;

    public GameObject AtworkSkin;

    private int selectedOption;
    public int IndexCharacterSelected;

    public GameObject SelectButton;
    public GameObject IsSelectedButton;
    public GameObject LockedButton;

    public GameObject BuyPanel;
    public GameObject CloseBuyPanelButton;

    [SerializeField] GameObject[] Camera;

    public ShopDataManager ShopDataManager;

    public List<string> stateSkinList = new List<string>();

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
            IndexCharacterSelected = 0;

        }
        UpdateCharacter(selectedOption);
        UpdateCamera(selectedOption);

        IScharacterSelected(IndexCharacterSelected);
        closeBuyPanel();
    }

    public void BackOption()
    {
        IndexCharacterSelected--;
        selectedOption--;
        if (selectedOption < 0  )
        {
            selectedOption = CharacterDB.CharacterCount - 1;
            IndexCharacterSelected = CharacterDB.CharacterCount - 1;
        }
        UpdateCharacter(selectedOption);
        UpdateCamera(selectedOption);
        IScharacterSelected(IndexCharacterSelected);
        closeBuyPanel();

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

    public void IScharacterSelected(int IndexCharacterSelected)
    {
        //for(int i =0;i<= ShopDataManager.stateSkinArray.Length; i++)
        //{

        //}
        if (PlayerPrefs.GetInt("selectedOption") == IndexCharacterSelected )
        {
            SelectButton.SetActive(false);
            IsSelectedButton.SetActive(true);
            LockedButton.SetActive(false);

            return;
        }else if (stateSkinList[IndexCharacterSelected] == "locked")
        {
            SelectButton.SetActive(false);
            IsSelectedButton.SetActive(false);
            LockedButton.SetActive(true);
            return;

        }
        SelectButton.SetActive(true);
        IsSelectedButton.SetActive(false);
        LockedButton.SetActive(false);

    }
    public void showBuyPanel()
    {
        if(!BuyPanel.activeSelf)
            BuyPanel.SetActive(true);
        else 
            BuyPanel.SetActive(false);
    }
    public void closeBuyPanel()
    {
        BuyPanel.SetActive(false);
    }
}
