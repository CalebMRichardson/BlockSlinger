using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public void SetCamPos(Vector2 _camPos) {

        transform.position = new Vector3(_camPos.x, _camPos.y, transform.position.z); 
    }
}
