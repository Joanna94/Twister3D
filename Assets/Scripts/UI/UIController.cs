﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

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

    public void ExitButtonClick()
    {
        Application.Quit();
    }

    public void BackButtonClick()
    {
        TwisterManager.instance.beforeGameCanvas.gameObject.SetActive(true);
        TwisterManager.instance.newGameCanvas.gameObject.SetActive(false); 
        TwisterManager.instance.inGameCanvas.gameObject.SetActive(false);
    }

    public void StartGameButtonClick()
    {
        //przepisanie zmiennych
        var playerComponent = TwisterManager.instance.player.GetComponent<PlayerController>();
        playerComponent.moveSpeed = GameSettings.Speed;

        TwisterManager.instance.floorLength = GameSettings.FloorLength;
        TwisterManager.instance.maxNumberOfFlyingObjects = GameSettings.MaxNumberOfFlyingObjects;

        foreach(GameObject c in TwisterManager.instance.cubes)
        {
            var flyingObjectComponent = c.GetComponent<FlyingObjectController>();
            flyingObjectComponent.pulse = GameSettings.Pulse;
            flyingObjectComponent.pulseSpeed = GameSettings.PulseSpeed;
            flyingObjectComponent.pulseGrowthBound = GameSettings.PulseGrowthBound;
            flyingObjectComponent.pulseShrinkBound = GameSettings.PulseShrinkBound;
            flyingObjectComponent.rotate = GameSettings.Rotate;
            flyingObjectComponent.rotateSpeed = GameSettings.RotateSpeed;
            flyingObjectComponent.rotateDirection.x = GameSettings.RotateDirectionX;
            flyingObjectComponent.rotateDirection.y = GameSettings.RotateDirectionY;
            flyingObjectComponent.rotateDirection.z = GameSettings.RotateDirectionZ;
            flyingObjectComponent.rotateAmount.x = GameSettings.RotateAmountX;
            flyingObjectComponent.rotateAmount.y = GameSettings.RotateAmountY;
            flyingObjectComponent.rotateAmount.z = GameSettings.RotateAmountZ;
        }
        if (LZWPlib.Core.Instance.isServer)
            TwisterManager.instance.StartGame();        
    }

    public void LoadGameButtonClick()
    {/*
        string path = EditorUtility.OpenFilePanel("Twister - Wybierz plik xml", "", "xml");
        if(path.Length != 0)
        {
            if (LZWPlib.Core.Instance.isServer){
                InitSavedTwisterScene(path);
            }
        }*/
    }

    public void NewGameButtonClick()
    {
        TwisterManager.instance.beforeGameCanvas.gameObject.SetActive(false);
        TwisterManager.instance.newGameCanvas.gameObject.SetActive(true); 
        TwisterManager.instance.inGameCanvas.gameObject.SetActive(false);  
    }

}