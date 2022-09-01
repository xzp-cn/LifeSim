using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;

    private Vector3 originPos;

    private TransfromStruct transfromStruct;
    private void Start()
    {
        transfromStruct = new TransfromStruct()
        {
            pos = transform.localPosition,
            rotation = transform.localRotation,
            scale = transform.localScale
        };
    }

    // Update is called once per frame
    void Update()
    { 
        OnMove();
        if ((Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift))&&Input.GetKey(KeyCode.F1))
        {
            ResetPose();
        }
    }
    void OnMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Vector3 move = new Vector3(x, 0f, z);// × global movement, we dont want
        Vector3 move = transform.right * x + transform.forward * z;// move along the local coordinates right and forward

        controller.Move(move * speed * Time.deltaTime);
    }
    public void ResetPose()
    {
        transform.localPosition = transfromStruct.pos;
        transform.localRotation = transfromStruct.rotation;
        transform.localScale = transfromStruct.scale;

    }
}