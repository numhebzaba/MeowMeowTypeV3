using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementNotificationcontroller : MonoBehaviour
{
    [SerializeField] Text AchievementTitleLabel;
    public void ShowNotification(Achievement achievement)
    {
        AchievementTitleLabel.text = achievement.title;
    }
         
}
