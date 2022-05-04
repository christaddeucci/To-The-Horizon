using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = new Vector3(0, 0, 5).normalized;
        controller.Move(forward * speed * Time.deltaTime);
    }
}
