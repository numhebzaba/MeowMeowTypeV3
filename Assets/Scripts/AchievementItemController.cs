using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AchievementItemController : MonoBehaviour
{
    [SerializeField] Image unlockedIcon;
    [SerializeField] Image lockedIcon;

    [SerializeField] TMP_Text TitleLabel;
    [SerializeField] TMP_Text DescriptionLabel;

    public bool unlocked;
    public Achievement achivement;


    public void RefreshView()
    {
        TitleLabel.text = achivement.Title;
        DescriptionLabel.text = achivement.Description;

        unlockedIcon.enabled = unlocked;
        lockedIcon.enabled = !unlocked;
    }

    private void OnValidate()
    {
        RefreshView();
    }
}
