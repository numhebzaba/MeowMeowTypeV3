using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(Animator))]
public class AchievementNotificationcontroller : MonoBehaviour
{
    [SerializeField] TMP_Text AchievementTitleLabel;
    public DataManager _DataManager;
    public int AchievemenId = 0;

    private Animator m_animator;
    private void Awake()
    {
        m_animator = GetComponent<Animator>();

    }
    public void ShowNoticationButton()
    {

        Debug.Log(_DataManager.AchievementList[AchievemenId]);
        Achievement achievement = _DataManager.AchievementList[AchievemenId];
        ShowNotification(achievement);
    }
    public void ShowNotification(Achievement achievement)
    {
        AchievementTitleLabel.text = achievement.Title;
        m_animator.SetTrigger("Appear");
    }
         
}
