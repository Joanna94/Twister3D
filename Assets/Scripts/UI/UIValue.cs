using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIValue : MonoBehaviour {

	public string param;

    public void OnChange()
    {
        if(param == "floorLength")
            GameSettings.FloorLength = ParseToInt();
        else if(param == "maxNumberOfFlyingObjets")
            GameSettings.MaxNumberOfFlyingObjects = ParseToInt();
        else if(param == "speed")
            GameSettings.Speed = ParseToFloat();
        else if(param == "pulseOn")
            GameSettings.Pulse = true;
        else if(param == "pulseOff")
            GameSettings.Pulse = false;
        else if(param == "pulseSpeed")
            GameSettings.PulseSpeed = ParseToFloat();
        else if(param == "growth")
            GameSettings.PulseGrowthBound = ParseToFloat();
        else if(param == "shrink")
            GameSettings.PulseShrinkBound = ParseToFloat();
        else if(param == "rotateOn")
            GameSettings.Rotate = true;
        else if(param == "rotateOff")
            GameSettings.Rotate = false;
        else if(param == "rotateSpeed")
            GameSettings.RotateSpeed = ParseToFloat();
        else if(param == "rotatedx")
            GameSettings.RotateDirectionX = ParseToFloat();
        else if(param == "rotatedy")
            GameSettings.RotateDirectionY = ParseToFloat();
        else if(param == "rotatedz")
            GameSettings.RotateDirectionZ = ParseToFloat();
        else if(param == "rotateax")
            GameSettings.RotateAmountX = ParseToFloat();
        else if(param == "rotateay")
            GameSettings.RotateAmountY = ParseToFloat();
        else if(param == "rotateaz")
            GameSettings.RotateAmountZ = ParseToFloat();
    }

    private float ParseToFloat()
    {
        float p;
        float.TryParse(this.GetComponent<InputField>().text, out p);
        return p;
    }

    private int ParseToInt()
    {
        int i;
        int.TryParse(this.GetComponent<InputField>().text, out i);
        return i;
    }

}
