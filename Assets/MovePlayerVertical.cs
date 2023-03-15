using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class MovePlayerVertical : MonoBehaviour
{
    public Rigidbody player;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("left joystick triggered");
        var joystickAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch);

        float fixedX = player.position.x;
        float fixedZ = player.position.z;

        player.position += (transform.up * joystickAxis.y) * Time.deltaTime * speed;
        // Debug.Log(player.position);
        player.position = new Vector3(fixedX, player.position.y, fixedZ);
    }
}
