using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMoveController : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private string movementType;
    private PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerController.IsDead || GameManager.SharedInstance.IsGamePaused)
        {
            return;
        }
        switch (movementType)
        {
            case "Zigzag" :
                MoveZigzag();
                break;
            case "Diagonal":
                //MoveDiagonal();
                break;
            default :
            case "Straight" :
                MoveStraight();
                break;
        }
    }

    private void MoveStraight()
    {
        transform.position += Vector3.back * Time.deltaTime * speed;
        transform.Rotate(Vector3.back * Time.deltaTime * 75);
    }

    private void MoveZigzag()
    {
        var variationOfDirection = new Vector3(2.5f, 0, 0);
        var direction = Random.Range(0, 3);
        switch (direction)
        {
            case 0:
                transform.position += variationOfDirection * Time.deltaTime;
                break;
            case 1:
                transform.position -= variationOfDirection * Time.deltaTime; 
                break;
            default:break;
        }
        transform.position += Vector3.back * Time.deltaTime * speed;
        transform.Rotate(Vector3.back * Time.deltaTime * 75);
    }

    private void MoveDiagonal()
    {
        //transform.position += Vector3.back * Time.deltaTime * speed;
        //transform.Rotate(Vector3.back * Time.deltaTime * 75);
    }
}
