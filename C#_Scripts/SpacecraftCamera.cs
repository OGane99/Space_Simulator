using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacecraftCamera : MonoBehaviour
{
    private float xCirclePos;
    private float yCirclePos;
    private float zCirclePos;
    private float relPositionX;
    private float relPositionY;
    private float relPositionZ;
    private float viewRadius;
    private float scrollPosition; //tracks scrollPosition in exponential scroll function

    public static SpacecraftCamera InitializeSpacecraftCamera(GameObject iGivenObject) {
        SpacecraftCamera thisSpacecraftCamera = iGivenObject.AddComponent<SpacecraftCamera>();
        return thisSpacecraftCamera;
    }
  
    void Start()
    {
        relPositionX = 0;
        relPositionY = 0;
        relPositionZ = 0;
        scrollPosition = 40f; //gives a starting radius ~= 150
        viewRadius = 0.005f*Mathf.Exp(0.15f*scrollPosition);
    }

    void Update()
    {
        var currentPosition = Spacecraft.returnSpacecraftPosition("Player_Spacecraft");

        //---------------------------------------scrolls the camera in and out depending on the scrollwheel input-------------------------------------------------------------
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
            scrollPosition -= 1;
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
            scrollPosition += 1;
        }
        viewRadius = 0.1f * Mathf.Exp(0.2f * scrollPosition);

        //---------------------------------------Rotates camera around craft for the up, down, left and right arrow keys-----------------------------------------------------------------
        if (Input.GetKey(KeyCode.RightArrow)) {
            relPositionX += 0.04f;
            relPositionZ += 0.04f;
            xCirclePos = viewRadius * Mathf.Cos(0.1f * relPositionX);
            zCirclePos = viewRadius * Mathf.Sin(0.1f * relPositionZ);
            Camera.main.transform.position = new Vector3(currentPosition.x+xCirclePos, currentPosition.y + yCirclePos, currentPosition.z + zCirclePos);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            relPositionX -= 0.04f;
            relPositionZ -= 0.04f;
            xCirclePos = viewRadius * Mathf.Cos(0.1f * relPositionX);
            zCirclePos = viewRadius * Mathf.Sin(0.1f * relPositionZ);
            Camera.main.transform.position = new Vector3(currentPosition.x + xCirclePos, currentPosition.y + yCirclePos, currentPosition.z + zCirclePos);
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            relPositionY += 0.04f;
            yCirclePos = viewRadius * Mathf.Sin(0.1f * relPositionY);
            Camera.main.transform.position = new Vector3(currentPosition.x + xCirclePos, currentPosition.y + yCirclePos, currentPosition.z + zCirclePos);
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            relPositionY -= 0.04f;
            yCirclePos = viewRadius * Mathf.Sin(0.1f * relPositionY);
            Camera.main.transform.position = new Vector3(currentPosition.x + xCirclePos, currentPosition.y + yCirclePos, currentPosition.z + zCirclePos);
        }

        //------------------------------------------------If no input from arrow keys keep camera pointed at craft and following------------------------------------------------------
        xCirclePos = viewRadius * Mathf.Cos(0.1f * relPositionX);
        yCirclePos = viewRadius * Mathf.Sin(0.1f * relPositionY);
        zCirclePos = viewRadius * Mathf.Sin(0.1f * relPositionZ);
        Camera.main.transform.position = new Vector3(currentPosition.x + xCirclePos, currentPosition.y + yCirclePos, currentPosition.z + zCirclePos);
        Camera.main.transform.LookAt(currentPosition);
    }
}
