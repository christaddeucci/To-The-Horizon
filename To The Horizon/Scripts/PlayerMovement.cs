using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float forwardSpeed = 7f, strafeSpeed = 10f;
    private float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;
    float bankAngle =30.0f;
    float smooth = 2.0f;
    private float verticalAcc = 2.5f, strafeAcc = 2f;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, verticalAcc * Time.deltaTime);
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed,  Input.GetAxisRaw("Horizontal") * strafeSpeed, strafeAcc * Time.deltaTime);
        float bankAroundZ = Input.GetAxis("Horizontal") * -bankAngle;
        Quaternion target = Quaternion.Euler(0, 0, bankAroundZ);



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

        float x = Mathf.Clamp(transform.position.x, -448, 570);
        float y = Mathf.Clamp(transform.position.y, -310, 520);
        //float z = mathf.clamp(transform.position.z, -13, 20);
        transform.position = new Vector3(x, y, transform.position.z);

        //transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeForwardSpeed * Time.deltaTime);

        

        /*if ((Input.GetKey(KeyCode.Space)) && Input.GetKey("left"))
        {
            print("spin");
            //transform.Rotate(Vector3.up * Time.deltaTime, Space.World);
        }*/




    }
}
