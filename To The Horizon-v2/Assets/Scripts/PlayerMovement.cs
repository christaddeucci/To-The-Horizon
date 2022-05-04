using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float forwardSpeed = 5f, strafeSpeed = 10f;
    private float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;
    float bankAngle =30.0f;
    float smooth = 2.0f;
    private float verticalAcc = 2.5f, strafeAcc = 2f;
    public CharacterController controller;
    public float speed = 20f;

    // Start is called before the first frame update
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {
        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, verticalAcc * Time.deltaTime);
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed,  Input.GetAxisRaw("Horizontal") * strafeSpeed, strafeAcc * Time.deltaTime);
        float bankAroundZ = Input.GetAxis("Horizontal") * -bankAngle;
        Quaternion target = Quaternion.Euler(0, 0, bankAroundZ);

        Vector3 forward = new Vector3(0, 0, 5);
        //controller.Move(forward * speed * Time.deltaTime);


        if (activeStrafeSpeed < 0)
        {
            //print("Bank left");
            //transform.transform.Rotate(0.0f, 0.0f, -45.0f, Space.Self);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
        }

        else if (activeStrafeSpeed > 0)
        {
           // print("Bank right");
            //transform.transform.Rotate(0.0f, 0.0f, 45.0f, Space.Self);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
        }
        else
        {
           // print("stable");
            //transform.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
        }

        float x = Mathf.Clamp(transform.position.x, -25.48f, 25.70f);
        float y = Mathf.Clamp(transform.position.y, 0.10f, 15.20f);
        //float z = Mathf.Clamp(transform.position.z, -13, 20);
        transform.position = new Vector3(x, y, transform.position.z);

        //transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeForwardSpeed * Time.deltaTime) + (forward * Time.deltaTime* speed  );

        

        /*if ((Input.GetKey(KeyCode.Space)) && Input.GetKey("left"))
        {
            print("spin");
            //transform.Rotate(Vector3.up * Time.deltaTime, Space.World);
        }*/




    }
}
