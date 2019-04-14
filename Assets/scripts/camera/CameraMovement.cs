using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    private Vector3 camPos;
    private float maxXMovement;
    private float maxYMovement;

    private const float GYRO_MOVE_SPEED = 1.25f;
    private const float MAX_GYRO_MOVEMENT = 1.0f;
    private const float MIN_GYRO_MOVEMENT = 0.10f;
    private const float MAX_CAM_SPEED = 70.0f;
    private const float CLOSE_ENOUGH = 0.25f;

    private float camSpeed = 0.0f;

    private float camSpeedIncreaseDelay = 0.15f;
    private float timeNow; 

    private enum CameraState { IDLE, MOVING_TO_NEXT_POSITION }
    private CameraState cameraState;

    private CameraAudio cameraAudio;

    private bool playedSwoosh; 

    public void Start() {
        Input.gyro.enabled = true;
        cameraState = CameraState.IDLE;
        cameraAudio = GetComponent<CameraAudio>();
        playedSwoosh = false;
    }

    public void SetCamPos(Vector2 _camPos) {

        transform.position = new Vector3(_camPos.x, _camPos.y, transform.position.z);
        camPos = _camPos;
        camPos.z = -10;
        maxXMovement = transform.position.x + MAX_GYRO_MOVEMENT;
        maxYMovement = transform.position.y + MAX_GYRO_MOVEMENT;
    }

    private void Update() {

        switch (cameraState) {

            case CameraState.IDLE:
                GyroMovement();
                playedSwoosh = false;
                break;
            case CameraState.MOVING_TO_NEXT_POSITION:
                Move();
                break;
        }
    }

    private void GyroMovement() {
        if(Mathf.Abs(Input.gyro.attitude.y) >= MIN_GYRO_MOVEMENT) {

            transform.position = Vector3.Lerp(transform.position,
                new Vector3(Mathf.Clamp(camPos.x + (2 * Mathf.Sign(Input.gyro.attitude.y)), camPos.x - MAX_GYRO_MOVEMENT, camPos.x + MAX_GYRO_MOVEMENT),
                transform.position.y, -10), GYRO_MOVE_SPEED * Time.deltaTime);

        } else {
            transform.position = Vector3.Lerp(transform.position, new Vector3(camPos.x, transform.position.y, transform.position.z), GYRO_MOVE_SPEED * Time.deltaTime);
        }
    }

    public void MoveToNextLevel(Vector2 _camPos) {

        cameraState = CameraState.MOVING_TO_NEXT_POSITION;

        camPos = _camPos;
        camPos.z = -10;

        StartCoroutine("IncreaseCameraSpeed");
    }

    private IEnumerator IncreaseCameraSpeed() {

        yield return new WaitForSeconds(1.0f);

        if(!playedSwoosh) {
            AudioManager.PlaySingleAtVolume(cameraAudio.GetSwooshSFX(), .7f);
            playedSwoosh = true;
        }

        camSpeed = MAX_CAM_SPEED;

    }
    private void Move() {

        transform.position = Vector3.MoveTowards(transform.position, camPos, camSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, camPos) <= CLOSE_ENOUGH) {

            //transform.position = camPos;
            cameraState = CameraState.IDLE;
            camSpeed = 0;

        }
    }

    public void SetCameraPos(Vector2 _camPos) {
        camPos = _camPos;
        camPos.z = -10;

        transform.position = camPos;
    }
}