using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescaleableCamera : MonoBehaviour
{

    #region Pola
    private int ScreenSizeX = 0;
    private int ScreenSizeY = 0;
    #endregion

    #region metody

    #region rescale camera
    private void RescaleCamera() {

        if(Screen.width == ScreenSizeX && Screen.height == ScreenSizeY) return;

        float targetaspect = 16.0f / 9.0f;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;
        Camera camera = GetComponent<Camera>();

        if(scaleheight < 1.0f) {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        } else // add pillarbox
          {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }

        ScreenSizeX = Screen.width;
        ScreenSizeY = Screen.height;
    }
    #endregion

    #endregion

    #region metody unity

    void OnPreCull() {
        if(Application.isEditor) return;
        Rect wp = Camera.main.rect;
        Rect nr = new Rect(0, 0, 1, 1);

        Camera.main.rect = nr;
        GL.Clear(true, true, Color.black);

        Camera.main.rect = wp;

    }

    // Use this for initialization
    void Start() {
        RepositionCamera();
        RescaleCamera();
    }

    void RepositionCamera() {

        GameObject blank = Instantiate(Resources.Load(LevelObjectLookup.BLANK_TILE_PATH)) as GameObject;

        SpriteRenderer sr = blank.GetComponent<SpriteRenderer>();

        float middleX = (sr.size.x * 14) / 2 - sr.size.x / 2;
        float middleY = (sr.size.y * 10) / 2 - sr.size.y / 2;
        float z = transform.position.z;

        transform.position = new Vector3(middleX, middleY, z);

    }

    // Update is called once per frame
    void Update() {
        RescaleCamera();
    }
    #endregion
}
