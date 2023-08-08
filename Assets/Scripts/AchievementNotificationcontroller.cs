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

    public GameObject achievementItemPrefab;
    public Transform content;

    [SerializeField][HideInInspector]
    public List<GameObject> achievementItems;
    private void Awake()
    {
        m_animator = GetComponent<Animator>();

    }
    private void Start()
    {
        
    }
    public void ShowNoticationButton(int AchievemenId)
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
    [ContextMenu("LoadAchievementsTable")]
    public void LoadAchievementTable()
    {
        foreach (GameObject controller in achievementItems)
        {
            Debug.Log(controller);
            controller.gameObject.SetActive(false);
            DestroyImmediate(controller.gameObject);
        }
        achievementItems.Clear();
        for(int i =0;i< _DataManager.AchievementList.Count; i++)
        {
            
            GameObject obj = Instantiate(achievementItemPrefab, content);
            AchievementItemController controller = obj.GetComponent<AchievementItemController>();
            if (_DataManager.AchievementList[i].State == "unlocked")
            {
                controller.unlocked = true;

            }
            else
            {
                controller.unlocked = false;

            }
            controller.achivement = _DataManager.AchievementList[i];
            controller.RefreshView();
            achievementItems.Add(obj);
        }
        Debug.Log(achievementItems.Count);

    }
     
}
