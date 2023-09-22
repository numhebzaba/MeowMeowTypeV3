using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccuracyAndSpeedToggleButton : MonoBehaviour
{
    [SerializeField] Toggle button;
    public ShowKeyboardManager showKeyboardManager_;
    private Image imageComponent;

    private void Start()
    {
        //button = GetComponent<Toggle>();
        imageComponent = button.GetComponent<Image>();
    }

    public void IsToggle()
    {
        if (button.isOn == false)
        {
            imageComponent.sprite.name = "AccuracyButton";
            showKeyboardManager_.ShowAverageAccuracyButton();
        }
        if(button.isOn == true)
        {
            imageComponent.sprite.name = "SpeedButton";
            showKeyboardManager_.ShowAverageSpeedButton();

        }
    }
}
