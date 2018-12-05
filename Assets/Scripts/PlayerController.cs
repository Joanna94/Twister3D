using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 0.1f;
    public bool isMoving = false;

    // Use this for initialization
    void Start()
    {
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
            MoveAhead();

        if (Input.GetKey(KeyCode.W))
        {
            MoveAhead();

        }
        if (Input.GetKey(KeyCode.A))
        {
            MoveLeft();

        }
        if (Input.GetKey(KeyCode.S))
        {
            MoveBack();

        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveRight();
        }
    }

    void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void MoveLeft()
    {
        transform.Translate((-moveSpeed) * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void MoveAhead()
    {
        transform.Translate(0.0f, 0.0f, moveSpeed * Time.deltaTime, Space.World);
    }

    void MoveBack()
    {
        transform.Translate(0.0f, 0.0f, (-moveSpeed) * Time.deltaTime, Space.World);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            isMoving = false;
            //GameManager.instance.LevelCompleted();
        }
        else if (other.CompareTag("StartLine"))
        {
            isMoving = true;
            other.gameObject.SetActive(false);
        }
        
    }
}
