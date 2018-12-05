﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LZWPlib;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public enum Color
{
    BLACK, //avoid
    BLUE, //right hand
    GREEN, //right foot
    RED, //left foot
    YELLOW //left hand
}

/*
    - odczytanie zapisanej sceny jest ale nie ma synchronizacji z mocapem
    - nie da sie parametryzowac wody
    - ybot jest zepsuty
*/

public class TwisterManager : MonoBehaviour {

    public static TwisterManager instance;

    //prefaby dla Twistera
    public GameObject player;
    public GameObject pieceOfFloor;
    public GameObject gate;
    public GameObject startLine;
    public GameObject fallingPreventor;
    public GameObject[] cubes = new GameObject[5];
    public Text totalScoreText;
    public Canvas inGameCanvas;
    public Canvas beforeGameCanvas;
    public Canvas newGameCanvas;

    //zmienne ktore mozna modyfikowac
    public int floorLength = 8; //dlugosc poziomu
    public int maxNumberOfFlyingObjects = 40;
    
    //inne zmienne
    private float caveHeight = 2.147f;
    private float caveWidth = 2.147f;
    private Vector3 initFloorPosition;
    private Vector3 linePosition;
    private Vector3 gatePosition;
    private Vector3 initFallingPreventorPosition;
    private List<GameObject>floor = new List<GameObject>();
    private List<GameObject>flyingObjects = new List<GameObject>();
    private float pieceOfFloorLength;
    private  int totalScore = 0;
    private GameObject temporaryFallingPreventor;
    private Vector3 ybotPosition;

	// Use this for initialization
	void Start () {
        
        beforeGameCanvas.gameObject.SetActive(true);
        newGameCanvas.gameObject.SetActive(false); 
        inGameCanvas.gameObject.SetActive(false);

        if (LZWPlib.Core.Instance.isServer){
            var ybot = player.transform.GetChild(0);
            ybotPosition = ybot.transform.position;
            temporaryFallingPreventor = (GameObject)Network.Instantiate(fallingPreventor, new Vector3(ybotPosition.x, ybotPosition.y - 0.3f, ybotPosition.z), new Quaternion(), 0);
        }
        
	}
	
	// Update is called once per frame
	void Update () {

        if (!LZWPlib.Core.Instance.isServer){
            GameObject additionalUI = GameObject.FindGameObjectWithTag("UI");
            GameObject.Destroy(additionalUI);
        }
    }

    private void Awake()
    {
        instance = this;
    }
    
    private void InitTwisterScene()
    {
        initFloorPosition = new Vector3(0.02f, ybotPosition.y, 0.5f);
        pieceOfFloorLength = pieceOfFloor.gameObject.transform.localScale.z;
        initFallingPreventorPosition = new Vector3(0.02f, initFloorPosition.y - 0.2f, 0.5f);
   
        GenerateFloor();
        GenerateGate();
        GenerateStartLine();
        GenerateFallingPreventor();
        GenerateFlyingObjects();
        
        SerializeScene();
    }

    private void GenerateFloor()
    {
        Vector3 nextFloorPosition = initFloorPosition;

        for(int i = 0; i < floorLength; i++){
            GameObject nextPiece =  (GameObject)Network.Instantiate(pieceOfFloor, nextFloorPosition, new Quaternion(), 0);
            nextFloorPosition.z += pieceOfFloorLength;
            floor.Add(nextPiece);
        }
    }

    private void GenerateFallingPreventor()
    {
        Vector3 nextFallingPreventorPosition = initFloorPosition;
        float fallingPreventorLength = fallingPreventor.gameObject.transform.localScale.z;

        for(int i = 0; i < floorLength; i++){
            GameObject nextPiece =  (GameObject)Network.Instantiate(fallingPreventor, nextFallingPreventorPosition, new Quaternion(), 0);
            nextFallingPreventorPosition.z += fallingPreventorLength;
            floor.Add(nextPiece);
        }
    }

    private void GenerateGate()
    {
        gatePosition = new Vector3(-1f, initFloorPosition.y + 1.3f, (floorLength - 1) * pieceOfFloorLength + pieceOfFloorLength * 0.5f);
        Network.Instantiate(gate, gatePosition, new Quaternion(), 0); 
    }

    private void GenerateStartLine()
    { 
        Vector3 firstPieceOfFloorPositon = floor[0].transform.position;
        linePosition = new Vector3(0f, initFloorPosition.y + 1.3f, firstPieceOfFloorPositon.z + pieceOfFloorLength * 0.01f);
        Network.Instantiate(startLine, linePosition, new Quaternion(), 0);
    }

    public void GenerateFlyingObjects()
    {
        //wyznaczenie obszaru w ktorym moga zostac wygenerowane
        float minX = floor[0].transform.position.x - floor[0].gameObject.transform.localScale.x * 0.5f;
        float minY = floor[0].transform.position.y + 0.2f;
        float minZ = floor[0].transform.position.z + pieceOfFloorLength / 2;
        float maxX = floor[0].transform.position.x + floor[0].gameObject.transform.localScale.x * 0.5f;
        float maxY = floor[0].transform.position.y + caveHeight;
        float maxZ = gatePosition.z - 0.05f;

        Array colorValues = Enum.GetValues(typeof(Color));
        Vector3 randPosition = new Vector3();
        //wyrownanie zageszczenia poprzez podzial planszy na mniejsze fragmenty podczas generacji
        float depth = (maxZ - minZ) / floorLength;
        int numberOfObjects = (int)(maxNumberOfFlyingObjects / floorLength);
        float startZ = minZ;

        for(int i = 0; i < floorLength; i++)
        {
            //doliczam klocki, ktore gubily sie przez zaokraglenia przy dzieleniu
            if(i == floorLength - 1)
            {
                int lostObjects = maxNumberOfFlyingObjects - numberOfObjects * floorLength;
                if(lostObjects > 0)
                    numberOfObjects += lostObjects;
            }

            for(int j = 0; j < numberOfObjects; j++)
            {
                Color randColor = (Color)colorValues.GetValue(UnityEngine.Random.Range(0, colorValues.Length));
                
                if(randColor == Color.RED || randColor == Color.GREEN) //stopy - ograniczenie maksymalnej wysokosci 
                    randPosition = new Vector3(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY + 0.2f, maxY), UnityEngine.Random.Range(startZ, startZ + depth));
                else 
                    randPosition = new Vector3(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY), UnityEngine.Random.Range(startZ, startZ + depth));

                bool isTooClose = false;

                foreach(GameObject obj in flyingObjects)
                {
                    if(obj.transform.position == randPosition)
                        isTooClose = true;
                }

                if(isTooClose)
                    --j;
                else{
                    GameObject flyingObject = (GameObject)Network.Instantiate(cubes[(int)randColor], randPosition, new Quaternion(), 0);
                    flyingObjects.Add(flyingObject);
                }
            }
            startZ = startZ + depth;
        }

    }

    public void IncreaseTotalScore(int number)
    {
        totalScore += number;
        totalScore = (totalScore < 0) ? 0 : totalScore;
        totalScoreText.text = totalScore.ToString();
    }

    public void SerializeScene()
    {
        Scene scene = new Scene();
        FlyingObjects flyingObjectXML;

        scene.FloorLength = floorLength;
        scene.MaxNumberOfFlyingObjects = maxNumberOfFlyingObjects;
        scene.FlyingObjects = new List<FlyingObjects>();

        foreach(GameObject obj in flyingObjects)
        {
            flyingObjectXML = new FlyingObjects();
            flyingObjectXML.Color = obj.gameObject.tag;
            
            flyingObjectXML.Position = new float[3];
            flyingObjectXML.Position[0] = obj.transform.position.x;
            flyingObjectXML.Position[1] = obj.transform.position.y;
            flyingObjectXML.Position[2] = obj.transform.position.z;

            flyingObjectXML.Pulse = obj.GetComponent<FlyingObjectController>().pulse.ToString();
            flyingObjectXML.PulseCurrentRatio = obj.GetComponent<FlyingObjectController>().pulseCurrentRatio;
            flyingObjectXML.PulseGrowthBound = obj.GetComponent<FlyingObjectController>().pulseGrowthBound;
            flyingObjectXML.PulseShrinkBound = obj.GetComponent<FlyingObjectController>().pulseShrinkBound;
            flyingObjectXML.PulseSpeed = obj.GetComponent<FlyingObjectController>().pulseSpeed;
            flyingObjectXML.Rotate = obj.GetComponent<FlyingObjectController>().rotate.ToString();
            
            flyingObjectXML.RotateAmount = new float[3];
            flyingObjectXML.RotateAmount[0] = obj.GetComponent<FlyingObjectController>().rotateAmount.x;
            flyingObjectXML.RotateAmount[1] = obj.GetComponent<FlyingObjectController>().rotateAmount.y;
            flyingObjectXML.RotateAmount[2] = obj.GetComponent<FlyingObjectController>().rotateAmount.z;

            flyingObjectXML.RotateDirection = new float[3];
            flyingObjectXML.RotateDirection[0] = obj.GetComponent<FlyingObjectController>().rotateDirection.x;
            flyingObjectXML.RotateDirection[1] = obj.GetComponent<FlyingObjectController>().rotateDirection.y;
            flyingObjectXML.RotateDirection[2] = obj.GetComponent<FlyingObjectController>().rotateDirection.z;

            flyingObjectXML.RotateSpeed = obj.GetComponent<FlyingObjectController>().rotateSpeed;

            scene.FlyingObjects.Add(flyingObjectXML);
        }

        scene.Player = new Player();
        scene.Player.Speed = player.GetComponent<PlayerController>().moveSpeed;

        Serialize.ParseToXML(scene);
    }

    public void DeserializeScene(string path)
    {
        Scene scene = Deserialize.ParseFromXML(path);

        var playerComponent = player.GetComponent<PlayerController>();
        playerComponent.moveSpeed = scene.Player.Speed;

        floorLength = scene.FloorLength;
        maxNumberOfFlyingObjects = scene.MaxNumberOfFlyingObjects;

        foreach(FlyingObjects obj in scene.FlyingObjects)
        {
            GameObject flyingObject = new GameObject();
            Vector3 position = new Vector3(obj.Position[0], obj.Position[1], obj.Position[2]);

            int color = 0;
            if(obj.Color == "GreenFlyingObject")
                color = (int)Color.GREEN;
            else if(obj.Color == "YellowFlyingObject")
                color = (int)Color.YELLOW;
            else if(obj.Color == "BlueFlyingObject")
                color = (int)Color.BLUE;
            else if(obj.Color == "RedFlyingObject")
                color = (int)Color.RED;
            else if(obj.Color == "BlackFlyingObject")
                color = (int)Color.BLACK;

            flyingObject = (GameObject)Network.Instantiate(cubes[color], position, new Quaternion(), 0);   
            
            SetFlyingObjectProperties(flyingObject.GetComponent<FlyingObjectController>(), obj);
        }          
    }

    private void SetFlyingObjectProperties(FlyingObjectController component, FlyingObjects obj)
    {
        if(obj.Pulse == "True")
            component.pulse = true;
        else component.pulse = false;
            
        if(obj.Rotate == "True")
            component.rotate = true;
        else component.rotate = false;

        component.pulseCurrentRatio = obj.PulseCurrentRatio;
        component.pulseGrowthBound = obj.PulseGrowthBound;
        component.pulseShrinkBound = obj.PulseShrinkBound;
        component.pulseSpeed = obj.PulseSpeed;
        component.rotateAmount = new Vector3(obj.RotateAmount[0], obj.RotateAmount[1], obj.RotateAmount[2]);
        component.rotateDirection = new Vector3(obj.RotateDirection[0], obj.RotateDirection[1], obj.RotateDirection[2]);
        component.rotateSpeed = obj.RotateSpeed;    
    }

    public void InitSavedTwisterScene(string path)
    {
        DeserializeScene(path);

        initFloorPosition = new Vector3(0.02f, ybotPosition.y, 0.5f);
        pieceOfFloorLength = pieceOfFloor.gameObject.transform.localScale.z;
        initFallingPreventorPosition = new Vector3(0.02f, initFloorPosition.y - 0.2f, 0.5f);

        GenerateFloor();
        GenerateGate();
        GenerateStartLine();
        GenerateFallingPreventor();
    }

    public void NewGameButtonClick()
    {
        beforeGameCanvas.gameObject.SetActive(false);
        newGameCanvas.gameObject.SetActive(true); 
        inGameCanvas.gameObject.SetActive(false);  
    }

    public void LoadGameButtonClick()
    {
        string path = EditorUtility.OpenFilePanel("Twister - Wybierz plik xml", "", "xml");
        if(path.Length != 0)
        {
            if (LZWPlib.Core.Instance.isServer){
                GameObject.Destroy(temporaryFallingPreventor);
                InitSavedTwisterScene(path);
                beforeGameCanvas.gameObject.SetActive(false);
                newGameCanvas.gameObject.SetActive(false); 
                inGameCanvas.gameObject.SetActive(true); 
            }
        }
    }

    public void StartGameButtonClick()
    {
        //przepisanie zmiennych
        var playerComponent = player.GetComponent<PlayerController>();
        playerComponent.moveSpeed = GameSettings.Speed;

        floorLength = GameSettings.FloorLength;
        maxNumberOfFlyingObjects = GameSettings.MaxNumberOfFlyingObjects;

        foreach(GameObject c in cubes)
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
        if (LZWPlib.Core.Instance.isServer){
            GameObject.Destroy(temporaryFallingPreventor);
            InitTwisterScene();
            beforeGameCanvas.gameObject.SetActive(false);
            newGameCanvas.gameObject.SetActive(false); 
            inGameCanvas.gameObject.SetActive(true); 
        }
    }

    public void ExitButtonClick()
    {
        Application.Quit();
    }

    public void BackButtonClick()
    {
        beforeGameCanvas.gameObject.SetActive(true);
        newGameCanvas.gameObject.SetActive(false); 
        inGameCanvas.gameObject.SetActive(false);
    }
}
