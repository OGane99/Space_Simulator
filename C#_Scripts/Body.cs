using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour {
    private float forceGravity;
    private float xForceGravity;
    private float yForceGravity;
    private float zForceGravity;
    private float theta_XY;    //angle of gravity in xy plane
    private float theta_XZ;
    private float bodyType;     //1 = star, 2 = planet, 3 = meteor/asteroid, 4 = spacecraft (3 and 4 move due to gravity of other objects), 5 = moon

    //for planets only
    private float orbitRadius;
    private float orbitVelocity;
    public GameObject sphere;

    //All Body's must be initialized first using this function
    public static Body InitializeBody(GameObject iGivenObject, string iName, float iBodyType, float iRadius, float iMass, float iX0Pos, float iY0Pos, float iZ0Pos, float iX0Vel, float iY0Vel, float iZ0Vel) {
        //Adds the body class to a given GameObject
        Body thisBody = iGivenObject.AddComponent<Body>();
        thisBody.name = iName;
        thisBody.bodyType = iBodyType;

        //Sets positions and scales given body
        if (thisBody.bodyType == 4) {
            iGivenObject.transform.position = new Vector3(iX0Pos, iY0Pos, iZ0Pos);
        }

        //Add rigid body and initial velocity
        Rigidbody thisRigidBody = iGivenObject.AddComponent<Rigidbody>();
        thisRigidBody.velocity = new Vector3(iX0Vel, iY0Vel, iZ0Vel);
        thisRigidBody.useGravity = false;
        thisRigidBody.mass = iMass;
        if (thisBody.bodyType == 1) {
            thisRigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }
        thisRigidBody.interpolation = RigidbodyInterpolation.Interpolate;

        return thisBody;
    }

    // Body's which are planets are also initialized using this function
    public static Body InitializePlanet(GameObject iGivenObject, float iOrbitRadius, float iOrbitalVelocity) {
        Body thisBody = iGivenObject.GetComponent<Body>();
        thisBody.orbitRadius = iOrbitRadius;
        thisBody.orbitVelocity = iOrbitalVelocity;
        return thisBody;
    }

    void Start() {
        if (bodyType == 2) {
            Rigidbody thisRigidBody = GetComponent<Rigidbody>();
        }
    }

    void FixedUpdate() {
        //gets current body position
        Rigidbody thisRigidBody = GetComponent<Rigidbody>();
        Vector3 currentPos = thisRigidBody.position;

        //------------------------------------------handles gravity on body types 3(asteroids/metors) and 4(spacecraft)-----------------------------------------------------------------
        if (bodyType == 3 || bodyType == 4) {
            //set to 0 at beginning of loop
            xForceGravity = 0;
            yForceGravity = 0;
            zForceGravity = 0;

            //finds all the hitColliders within a physics overlap sphere of given radius
            Collider[] hitColliders = Physics.OverlapSphere(currentPos, 100000);
            //Loop through each hit collider
            foreach (var hitCollider in hitColliders) {
                // calculate the gravity on this body due to each hitCollider and sum them up
                if (hitCollider.name != name ) {
                    forceGravity = (Globals.G * hitCollider.attachedRigidbody.mass * thisRigidBody.mass) / Mathf.Sqrt((currentPos.x - hitCollider.attachedRigidbody.position.x) * (currentPos.x - hitCollider.attachedRigidbody.position.x) + (currentPos.y - hitCollider.attachedRigidbody.position.y) * (currentPos.y - hitCollider.attachedRigidbody.position.y) + (currentPos.z - hitCollider.attachedRigidbody.position.z) * (currentPos.z - hitCollider.attachedRigidbody.position.z));
                    theta_XY = Mathf.Atan2(currentPos.y - hitCollider.attachedRigidbody.position.y, currentPos.x - hitCollider.attachedRigidbody.position.x);
                    theta_XZ = Mathf.Atan2(currentPos.z - hitCollider.attachedRigidbody.position.z, currentPos.x - hitCollider.attachedRigidbody.position.x);

                    xForceGravity += forceGravity * Mathf.Cos(theta_XY);
                    yForceGravity += forceGravity * Mathf.Sin(theta_XY);
                    zForceGravity += forceGravity * Mathf.Sin(theta_XZ);
                }
            }
            thisRigidBody.AddForce(new Vector3(-xForceGravity, -yForceGravity, -zForceGravity), ForceMode.Impulse);
        }
       
        //------------------------------------------Handles planet motion and gives them orbit-----------------------------------------------------------------
        if (bodyType == 2) {
            float planetXPos = orbitRadius * Mathf.Cos((float)(Time.time * orbitVelocity * 0.01));
            float planetZPos = orbitRadius * Mathf.Sin((float)(Time.time * orbitVelocity * 0.01));
            thisRigidBody.MovePosition(new Vector3(planetXPos, 0, planetZPos));
        }

        if (bodyType == 5) {
            float planetXPos1 = orbitRadius * Mathf.Cos((float)(Time.time * orbitVelocity * 0.01));
            float planetZPos1 = orbitRadius * Mathf.Sin((float)(Time.time * orbitVelocity * 0.01));
            thisRigidBody.MovePosition(new Vector3(GameObject.Find("The Earth").GetComponent<Transform>().position.x + planetXPos1, 0, GameObject.Find("The Earth").GetComponent<Transform>().position.z + planetZPos1));
        }
    }
}
