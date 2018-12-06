using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Canvas inGameCanvas = null;
    public Canvas firstWindowCanvas = null;
    public Canvas newGameCanvas = null;
    public Canvas chooseModeCanvas = null;
    public Canvas oceanCreatorCanvas = null;
    public Canvas mountainCreatorCanvas = null;

    private void Start()
    {
        firstWindowCanvas.gameObject.SetActive(true);
        newGameCanvas.gameObject.SetActive(false); 
        inGameCanvas.gameObject.SetActive(false);
        chooseModeCanvas.gameObject.SetActive(false);  
        oceanCreatorCanvas.gameObject.SetActive(false);
        mountainCreatorCanvas.gameObject.SetActive(false);
    }

    public void ExitButtonClick()
    {
        Application.Quit();
    }

    public void StartGameButtonClick()
    {
        //przepisanie zmiennych
        var playerComponent = TwisterManager.instance.player.GetComponent<PlayerController>();
        playerComponent.moveSpeed = GameSettings.PlayerSpeed;

        TwisterManager.instance.floorLength = GameSettings.FloorLength;
        TwisterManager.instance.maxNumberOfFlyingObjects = GameSettings.MaxNumberOfFlyingObjects;

        foreach(GameObject c in TwisterManager.instance.cubes)
        {
            var flyingObjectComponent = c.GetComponent<FlyingObjectController>();

            flyingObjectComponent.pulse = GameSettings.Pulse;
            if(GameSettings.Pulse == false)
            {
                flyingObjectComponent.pulseSpeed = 0;
                flyingObjectComponent.pulseGrowthBound = 0;
                flyingObjectComponent.pulseShrinkBound = 0;
            }
            else
            {
                flyingObjectComponent.pulseSpeed = GameSettings.PulseSpeed;
                flyingObjectComponent.pulseGrowthBound = GameSettings.PulseGrowthBound;
                flyingObjectComponent.pulseShrinkBound = GameSettings.PulseShrinkBound;
            }

            flyingObjectComponent.rotate = GameSettings.Rotate;
            if(GameSettings.Rotate == false)
            {
                flyingObjectComponent.rotateSpeed = 0;
                flyingObjectComponent.rotateDirection.x = 0;
                flyingObjectComponent.rotateDirection.y = 0;
                flyingObjectComponent.rotateDirection.z = 0;
                flyingObjectComponent.rotateAmount.x = 0;
                flyingObjectComponent.rotateAmount.y = 0;
                flyingObjectComponent.rotateAmount.z = 0;
            }
            else
            {
                flyingObjectComponent.rotateSpeed = GameSettings.RotateSpeed;
                flyingObjectComponent.rotateDirection.x = GameSettings.RotateDirectionX;
                flyingObjectComponent.rotateDirection.y = GameSettings.RotateDirectionY;
                flyingObjectComponent.rotateDirection.z = GameSettings.RotateDirectionZ;
                flyingObjectComponent.rotateAmount.x = GameSettings.RotateAmountX;
                flyingObjectComponent.rotateAmount.y = GameSettings.RotateAmountY;
                flyingObjectComponent.rotateAmount.z = GameSettings.RotateAmountZ;
            }
        }

        //niebo
        var skyScript = TwisterManager.instance.sky.GetComponent<SCS>();
        skyScript.CloudsSpeed = GameSettings.SkySpeed;

        //ocean
        if(TwisterManager.instance.gameMode == Mode.OCEAN)
        {
            Renderer oceanRenderer = TwisterManager.instance.ocean.GetComponentInChildren<Renderer>();
            Shader oceanShader = Shader.Find("Waves");
            oceanRenderer.material.SetVector("_WaveA", new Vector4(GameSettings.WaveADirX, GameSettings.WaveADirY, GameSettings.WaveASteepless, GameSettings.WaveALength));
            oceanRenderer.material.SetVector("_WaveB", new Vector4(GameSettings.WaveBDirX, GameSettings.WaveBDirY, GameSettings.WaveBSteepless, GameSettings.WaveBLength));
            oceanRenderer.material.SetVector("_WaveC", new Vector4(GameSettings.WaveCDirX, GameSettings.WaveCDirY, GameSettings.WaveCSteepless, GameSettings.WaveCLength));
            
            Vector3 oceanWaveScale = new Vector3();
            oceanWaveScale = TwisterManager.instance.ocean.transform.localScale;
            oceanWaveScale.y = GameSettings.WavesHeight;

            TwisterManager.instance.ocean.transform.localScale = oceanWaveScale;
        }
        else if(TwisterManager.instance.gameMode == Mode.MOUNTAIN)
        {



        }

        if (LZWPlib.Core.Instance.isServer){

            //ustawienie canvasow
            inGameCanvas.gameObject.SetActive(true); 
            firstWindowCanvas.gameObject.SetActive(false);
            newGameCanvas.gameObject.SetActive(false); 
            chooseModeCanvas.gameObject.SetActive(false);
            oceanCreatorCanvas.gameObject.SetActive(false);
            mountainCreatorCanvas.gameObject.SetActive(false);

            TwisterManager.instance.StartGame(); 
        }
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
        firstWindowCanvas.gameObject.SetActive(false);
        newGameCanvas.gameObject.SetActive(false); 
        inGameCanvas.gameObject.SetActive(false);  
        chooseModeCanvas.gameObject.SetActive(true);
    }

    public void ChooseOceanMode()
    {
        chooseModeCanvas.gameObject.SetActive(false);
        oceanCreatorCanvas.gameObject.SetActive(true);
        TwisterManager.instance.gameMode = Mode.OCEAN;
    }

    public void ChooseMountainMode()
    {
        chooseModeCanvas.gameObject.SetActive(false);
        mountainCreatorCanvas.gameObject.SetActive(true);
        TwisterManager.instance.gameMode = Mode.MOUNTAIN;       
    }

    public void BackButtonClick()
    {
        if(chooseModeCanvas.gameObject.active == true)
        {
            chooseModeCanvas.gameObject.SetActive(false);
            firstWindowCanvas.gameObject.SetActive(true);
        }
        else if(oceanCreatorCanvas.gameObject.active == true)
        {
            oceanCreatorCanvas.gameObject.SetActive(false);
            chooseModeCanvas.gameObject.SetActive(true);

        }else if(mountainCreatorCanvas.gameObject.active == true)
        {
            mountainCreatorCanvas.gameObject.SetActive(false);
            chooseModeCanvas.gameObject.SetActive(true);
        }
        
       // TwisterManager.instance.firstWindowCanvas.gameObject.SetActive(true);
       // TwisterManager.instance.newGameCanvas.gameObject.SetActive(false); 
      //  TwisterManager.instance.inGameCanvas.gameObject.SetActive(false);
    }
}
