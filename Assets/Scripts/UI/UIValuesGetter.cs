using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIValuesGetter : MonoBehaviour {

    public string param;

    public void OnChange()
    {
        if(param == "floorLength")
            GameSettings.FloorLength = ParseToInt();
        else if(param == "maxNumberOfFlyingObjects")
            GameSettings.MaxNumberOfFlyingObjects = ParseToInt();
        else if(param == "speed")
            GameSettings.PlayerSpeed = ParseToFloat();
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
        else if(param == "skySpeed")
            GameSettings.SkySpeed = ParseToFloat();
        else if(param == "axdir")
            GameSettings.WaveADirX = ParseToFloat();
        else if(param == "aydir")
            GameSettings.WaveADirY = ParseToFloat();
        else if(param == "asteepless")
            GameSettings.WaveASteepless = ParseToFloat();
        else if(param == "alength")
            GameSettings.WaveALength = ParseToFloat();
        else if(param == "bxdir")
            GameSettings.WaveBDirX = ParseToFloat();
        else if(param == "bydir")
            GameSettings.WaveBDirY = ParseToFloat();
        else if(param == "bsteepless")
            GameSettings.WaveBSteepless = ParseToFloat();
        else if(param =="blength")
            GameSettings.WaveBLength = ParseToFloat();
        else if(param == "cxdir")
            GameSettings.WaveCDirX = ParseToFloat();
        else if(param == "cydir")
            GameSettings.WaveCDirY = ParseToFloat();
        else if(param == "csteepless")
            GameSettings.WaveCSteepless = ParseToFloat();
        else if(param == "clength")
            GameSettings.WaveCLength = ParseToFloat();
        else if(param == "wavesHeight")
            GameSettings.WavesHeight = ParseToFloat();

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
