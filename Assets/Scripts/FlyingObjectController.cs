using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObjectController : MonoBehaviour {

    public bool pulse = true;
    public bool rotate = true;
    
    public Vector3 rotateDirection = new Vector3(1f, 1f, 1f);
    public float rotateSpeed = 50f;

    public float pulseSpeed = 0.3f;
    public float pulseGrowthBound = 0.2f;
    public float pulseShrinkBound = 0.15f;
    public float pulseCurrentRatio = 0.15f;

    public Vector3 rotateAmount = new Vector3(0.05f, 0.05f, 0f);
    private bool grow = true;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
       
        if(rotate)
            Rotate();
        
        if(pulse)
            Pulse();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (this.CompareTag("BlackFlyingObject"))
        {
            if(other.CompareTag("Body") || other.CompareTag("LeftHand") || other.CompareTag("RightHand") || other.CompareTag("LeftFoot") || other.CompareTag("RightFoot"))
            {
                TwisterManager.instance.IncreaseTotalScore(-100);
                this.gameObject.SetActive(false);
                Debug.Log("Kolizja z czarnym klockiem");
            }
        }
        else if (this.CompareTag("RedFlyingObject"))
        {
            if (other.CompareTag("LeftFoot"))
            {
                TwisterManager.instance.IncreaseTotalScore(200);
                this.gameObject.SetActive(false);
                Debug.Log("Kolizja czerwony klocek - lewa stopa (prawidlowo)");
            }else if(other.CompareTag("Body") || other.CompareTag("LeftHand") || other.CompareTag("RightHand") || other.CompareTag("RightFoot"))
            {
                TwisterManager.instance.IncreaseTotalScore(10);
                this.gameObject.SetActive(false);
                Debug.Log("Kolizja czerwony klocek - dowolna czesc ciala (nieprawidlowo)");
            }
        }
        else if (this.CompareTag("YellowFlyingObject"))
        {
            if (other.CompareTag("LeftHand"))
            {
                TwisterManager.instance.IncreaseTotalScore(200);
                this.gameObject.SetActive(false);
                Debug.Log("Kolizja zolty klocek - lewa dlon (prawidlowo)");
            }else if (other.CompareTag("Body") || other.CompareTag("LeftFoot") || other.CompareTag("RightHand") || other.CompareTag("RightFoot"))
            {
                TwisterManager.instance.IncreaseTotalScore(10);
                this.gameObject.SetActive(false);
                Debug.Log("Kolizja zolty klocek - dowolna czesc ciala (nieprawidlowo)");
            }
        }
        else if (this.CompareTag("BlueFlyingObject"))
        {
            if (other.CompareTag("RightHand"))
            {
                TwisterManager.instance.IncreaseTotalScore(200);
                this.gameObject.SetActive(false);
                Debug.Log("Kolizja niebieski klocek - prawa dlon (prawidlowo)");
            }else if (other.CompareTag("Body") || other.CompareTag("LeftFoot") || other.CompareTag("LeftHand") || other.CompareTag("RightFoot"))
            {
                TwisterManager.instance.IncreaseTotalScore(10);
                this.gameObject.SetActive(false);
                Debug.Log("Kolizja niebieski klocek - dowolna czesc ciala (nieprawidlowo)");
            }
        }
        else if (this.CompareTag("GreenFlyingObject"))
        {
            if (other.CompareTag("RightFoot"))
            {
                TwisterManager.instance.IncreaseTotalScore(200);
                this.gameObject.SetActive(false);
                Debug.Log("Kolizja zielony klocek - prawa stopa (prawidlowo)");
            }else if (other.CompareTag("Body") || other.CompareTag("LeftFoot") || other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
            {
                TwisterManager.instance.IncreaseTotalScore(10);
                this.gameObject.SetActive(false);
                Debug.Log("Kolizja zielony klocek - dowolna czesc ciala (nieprawidlowo)");
            }
        }
        else
            Debug.Log("Niezdefiniowana kolizja");
    }

    void Rotate()
    {
        transform.Rotate(rotateAmount, rotateSpeed * Time.deltaTime);
    }

    void Pulse()
    {
        if (grow)
        {
            pulseCurrentRatio = Mathf.MoveTowards(pulseCurrentRatio, pulseGrowthBound, pulseSpeed * Time.deltaTime);
            this.gameObject.transform.localScale = Vector3.one * pulseCurrentRatio;

            if(pulseCurrentRatio >= pulseGrowthBound)
                grow = false;
        }
        else
        {
            pulseCurrentRatio = Mathf.MoveTowards(pulseCurrentRatio, pulseShrinkBound, pulseSpeed * Time.deltaTime);
            this.gameObject.transform.localScale = Vector3.one * pulseCurrentRatio;

            if(pulseCurrentRatio <= pulseShrinkBound)
                grow = true;
        }
    }

}
