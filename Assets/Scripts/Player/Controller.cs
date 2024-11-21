using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0.1f;
    [SerializeField] float maxTiltAngle = 6f;
    [SerializeField] float tiltReductionStep = 0.1f;

    private Rigidbody2D player;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput == 0 && player.transform.rotation.z != 0)
        {
            //float newZPos = (player.transform.rotation.z > 0)? 
            //                    player.transform.rotation.z - tiltReductionStep :
            //                    player.transform.rotation.z + tiltReductionStep;
            player.transform.Rotate(0, 0, 0);
        }

        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        player.velocity = movement * moveSpeed;

        if (horizontalInput != 0 && player.transform.rotation.z == 0)
        { 
            player.transform.Rotate(0, 0, (horizontalInput < 0)? -maxTiltAngle : maxTiltAngle);
        }
        //float steerAmount = Input.GetAxis("Horizontal") * steerSpeed;
        //float moveAmount = Input.GetAxis("Vertical") * moveSpeed;

        //transform.Rotate(0, 0, -steerAmount);
        //transform.Translate(0, moveAmount, 0);
    }
}
