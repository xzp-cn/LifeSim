using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;

    public Transform playerBody;

    float xRotation = 0f;

    private TransfromStruct transfromStruct;
    // Start is called before the first frame update
    void Start()
    {
       transfromStruct= new TransfromStruct(transform.localPosition, transform.localScale, transform.localRotation);
    } 
    // Update is called once per frame
    void Update()
    {
        OnMove();

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.F1))
        {
            ResetPose();
        }
    }

    void OnMove()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Cursor.visible = false;
        }
        else if (Input.GetKey(KeyCode.LeftControl)||Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
                
            //Debug.Log("mouseY: "+mouseY);
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);// limit the angle

            // rotate the camera within Y axis
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            // rotate the player within X axis
            playerBody.Rotate(Vector3.up * mouseX);
            Vector3 eular = playerBody.localEulerAngles;    
            eular.x = 0;    
            eular.z = 0;
            playerBody.localRotation = Quaternion.Euler(eular);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Cursor.visible = true;
        }
    }

    public void ResetPose()
    {
        transform.localPosition = transfromStruct.pos;
        transform.localRotation = transfromStruct.rotation;
        transform.localScale = transfromStruct.scale;
       
    }

}
public struct TransfromStruct
{
    public Vector3 pos;
    public Vector3 scale;
    public Quaternion rotation;
    public TransfromStruct(Vector3 _pos, Vector3 _scale, Quaternion _rotation)
    {
        pos = _pos;
        scale = _scale;
        rotation = _rotation;
    }

    public TransfromStruct(Vector3 _pos, Vector3 _scale, Vector3 _eular)
    {
        pos = _pos;
        scale = _scale;
        rotation = Quaternion.Euler(_eular);
    }

    public static implicit operator Transform(TransfromStruct ts)
    {
        Transform tf = new TransformExtension();
        tf.localPosition = ts.pos;
        tf.localRotation = ts.rotation;
        tf.localScale = ts.scale;
        return tf;
    }
    public static implicit operator TransfromStruct(Transform ts)
    {
        return new TransfromStruct(ts.localPosition, ts.localScale, ts.localRotation);
    }
}
public class TransformExtension : Transform
{
    public TransformExtension()
    {
    }
}
