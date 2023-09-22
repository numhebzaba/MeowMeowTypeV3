using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionSpawner : MonoBehaviour
{
    public CharacterDataBase CharacterDB;
    public GameObject AtworkSkin;
    private int selectedOption = 0;
    public Transform CharacterObject;
    public GameObject CharacterPoint;
    private GameObject CharacterInstance;

    public AnimatorControllerState animationStateController;


    private void Start()
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
        SetUpCharacterInstance();
        Debug.Log(AtworkSkin.name);
    }

    private void SetUpCharacterInstance()
    {
        CharacterInstance = Instantiate(AtworkSkin, new Vector3(CharacterObject.position.x, CharacterObject.position.y, CharacterObject.position.z), Quaternion.Euler(0f, 180f, 0f));
        CharacterInstance.transform.SetParent(CharacterPoint.transform);
        CharacterInstance.layer = LayerMask.NameToLayer("UI");
        CharacterInstance.transform.localScale = new Vector3(1f, 1f, 1f);
        SetAnimation();

    }

    private void SetAnimation()
    {
        animationStateController = CharacterInstance.GetComponent<AnimatorControllerState>();
        animationStateController.animator.SetInteger(animationStateController.AnimationHash, 6);
    }
}
