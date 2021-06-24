using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spacecraft : MonoBehaviour {
    private Vector3 lastPosition;
    private Vector3 currentPosition;
    private Quaternion lookRotation;
    private GameObject myPrefab;

    public static Spacecraft InitializeSpacecraft(GameObject iGivenObject){
        Spacecraft thisSpacecraft = iGivenObject.AddComponent<Spacecraft>();
        return thisSpacecraft;
    }

    public static Vector3 returnSpacecraftPosition(string iGivenObjectName) {
        var givenObject = GameObject.Find(iGivenObjectName);
        Spacecraft thisSpacecraft = givenObject.GetComponent<Spacecraft>();
        Vector3 curPos = thisSpacecraft.gameObject.transform.position;
        return curPos;
    }

    public static GameObject getPrefab(GameObject iGivenObject) {
        Spacecraft thisSpacecraft = iGivenObject.GetComponent<Spacecraft>();
        GameObject spacecraftPrefab = thisSpacecraft.myPrefab;
        return spacecraftPrefab;
    }

    void Update() {
        Rigidbody thisRigidBody = GetComponent<Rigidbody>();
        Spacecraft thisSpacecraft = GetComponent<Spacecraft>();

        //on first frame log a position for prograde and retrograde
        if (Time.frameCount == 0) {
            lastPosition = transform.position;
        }

        //------------------------------------------Control craft movement and thrust, (W,A,S,D,T)-------------------------------------------------------
        if (Input.GetKey(KeyCode.Z)) {
            thisRigidBody.AddRelativeForce(new Vector3(0, Globals.spaceCraftSize/25f, 0), ForceMode.Impulse);
        }


        if (Input.GetKeyDown(KeyCode.Z)) {
            GameObject.Find("Player_Spacecraft").transform.GetChild(1).GetComponentInChildren<ParticleSystem>().Play();
            thisSpacecraft.GetComponent<AudioSource>().Play();
        }
        if (Input.GetKeyUp(KeyCode.Z)) {
            GameObject.Find("Player_Spacecraft").transform.GetChild(1).GetComponentInChildren<ParticleSystem>().Stop();
            thisSpacecraft.GetComponent<AudioSource>().Stop();
        }
        if (Input.GetKey(KeyCode.W)) {
            thisRigidBody.AddTorque(new Vector3(Globals.spaceCraftSize / 3f, 0, 0), ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.A)) {
            thisRigidBody.AddTorque(new Vector3(0, 0, Globals.spaceCraftSize / 3f), ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.S)) {
            thisRigidBody.AddTorque(new Vector3(Globals.spaceCraftSize / -3f, 0, 0), ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.D)) {
            thisRigidBody.AddTorque(new Vector3(0, 0, Globals.spaceCraftSize / -3f), ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.T)) {
            thisRigidBody.angularDrag = 10f;
        }


        //--------------------------------------------------Point retrograde when holding 'R'--------------------------------------------------------------------------------------
        if (Input.GetKey(KeyCode.R)) {
            currentPosition = transform.position;
            if (currentPosition != lastPosition) {
                Vector3 targetDir = (lastPosition - currentPosition);
                lookRotation = Quaternion.LookRotation(targetDir, new Vector3(0, 1, 0));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation * Quaternion.Euler(90, 0, 0), Time.deltaTime * 25);
                
            }
        }

        //-------------------------------------------------point prograde when holding 'F'----------------------------------------------------------
        if (Input.GetKey(KeyCode.F)) {
            thisSpacecraft.currentPosition = thisRigidBody.transform.position;
            if (currentPosition != lastPosition) {
                Vector3 targetDir = (lastPosition - currentPosition);
                lookRotation = Quaternion.LookRotation(targetDir, new Vector3(0, 1, 0));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation * Quaternion.Euler(-90, 0, 0), Time.deltaTime * 25);
            }
        }

        // log last position for pointing retrograde
        lastPosition = thisRigidBody.transform.position;

        //update velocity tracker on game ui
        GameObject.Find("Velocity Odometer").GetComponentInChildren<Text>().text = thisRigidBody.velocity.sqrMagnitude.ToString();

        //constant angular drag when not rotating
        thisRigidBody.angularDrag = 3f;
        thisRigidBody.inertiaTensor = new Vector3(Globals.spacecraftMass*30, Globals.spacecraftMass*30, Globals.spacecraftMass*30);
    }



    
}
