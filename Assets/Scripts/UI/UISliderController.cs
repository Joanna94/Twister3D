using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISliderController : MonoBehaviour {

    public Slider skySlider;
    public Text skySpeedValue;

    private void Start()
    {
        int value = 0;
        skySpeedValue.text = value.ToString();
        skySpeedValue.text = skySlider.value.ToString();
    }

    public void OnChange()
    {
        if(skySlider != null){
            GameSettings.SkySpeed = skySlider.value;
            skySpeedValue.text = skySlider.value.ToString();
        }
    }
}
