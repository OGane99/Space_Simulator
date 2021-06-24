using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Globals {
    //Setup all constants required for code
    public const float sunMass = 2000;
    public const float earthMass = 100;
    public const float spacecraftMass = 10;
    public const float G = 0.0001f;
    public const float spaceCraftSize = 1f;
}
public class Main : MonoBehaviour {
    void Start() {
        //-------------------------------------------Create stars----------------------------------------------------------------------------------------
        GameObject sunObject = new GameObject();
        sunObject.name = "The Sun";
        Body.InitializeBody(sunObject, "The Sun", 1, 3000, Globals.sunMass, 0, 0, 0, 0, 0, 0);
        GameObject sunModel = Instantiate(GameObject.Find("The_Sun_Prefab").GetComponent<InstantiateObject>().myPlanet, sunObject.transform.position, Quaternion.Euler(90, 0, 0));
        sunModel.transform.parent = sunObject.transform;
        sunModel.transform.localScale = new Vector3(1000, 1000, 1000);
        SphereCollider thisSphereCollider = sunObject.AddComponent<SphereCollider>();
        thisSphereCollider.center = sunObject.transform.position;
        thisSphereCollider.radius = 1000;

        //-------------------------------------------Create planets------------------------------------------------------------------------------------------------------------
        GameObject EarthObject = new GameObject();
        EarthObject.name = "The Earth";
        Body.InitializeBody(EarthObject, "The Earth", 2, 100, Globals.earthMass, 10000, 0, 0, 0, 0, 0);
        Body.InitializePlanet(EarthObject, 5000, 3);
        GameObject EarthModel = Instantiate(GameObject.Find("The_Earth_Prefab").GetComponent<InstantiateObject>().myPlanet, EarthObject.transform.position, Quaternion.Euler(90, 0, 0));
        EarthModel.transform.parent = EarthObject.transform;
        EarthModel.transform.localScale = new Vector3(100, 100, 100);
        Rigidbody thisRigidBody = EarthModel.AddComponent<Rigidbody>();
        thisRigidBody.transform.parent = EarthObject.transform;
        thisRigidBody.useGravity = false;
        thisRigidBody.mass = 0;
        thisRigidBody.interpolation = RigidbodyInterpolation.Interpolate;
        thisRigidBody.isKinematic = true;
        SphereCollider thisSphereCollider2 = EarthObject.AddComponent<SphereCollider>();
        thisSphereCollider2.center = EarthObject.transform.position;
        thisSphereCollider2.radius = 100;

        //-------------------------------------------Create random aseroids/meteors----------------------------------------------------------------------------------------------
        //uncomment as needed
        //GameObject AsteroidObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //AsteroidObject.name = "Doink";
        //Body.InitializeBody(AsteroidObject, "Doink", 3, 40, Globals.earthMass*30, -4000, 0, 0, 0, 0, -500);

        //------------------------------------------Create Moons for planets------------------------------------------------------------------------------------------------

        GameObject MoonObject = new GameObject();
        MoonObject.name = "The Moon";
        Body.InitializeBody(MoonObject, "The Moon", 5, 1000, Globals.earthMass/2, 0, 0, 0, 0, 0, 0);
        Body.InitializePlanet(MoonObject, 1000, 6);
        GameObject MoonModel = Instantiate(GameObject.Find("The_Moon_Prefab").GetComponent<InstantiateObject>().myPlanet, EarthObject.transform.position, Quaternion.Euler(90, 0, 0));
        MoonModel.transform.parent = MoonObject.transform;
        MoonModel.transform.localScale = new Vector3(200, 200, 200);
        MoonObject.transform.SetParent(EarthObject.transform, true);

        //------------------------------------------Create spacecraft and initialize all relevant things-------------------------------------------------------------------------
        GameObject spacecraftObject = new GameObject();
        spacecraftObject.name = "Player_Spacecraft";
        Body.InitializeBody(spacecraftObject, "Player_Spacecraft", 4, Globals.spaceCraftSize, Globals.spacecraftMass, 3000, 0, 0, 0, 0, 5);
        Spacecraft.InitializeSpacecraft(spacecraftObject);
        SpacecraftCamera.InitializeSpacecraftCamera(spacecraftObject);

        //add model to object
        GameObject prefabToCopy = GameObject.Find("Semyorka_Prefab");
        GameObject spacecraftModel = Instantiate(prefabToCopy.GetComponent<InstantiateObject>().myPrefab, spacecraftObject.transform.position, Quaternion.Euler(90, 0, 0));
        spacecraftModel.transform.parent = spacecraftObject.transform;
        spacecraftModel.transform.localScale = new Vector3(Globals.spaceCraftSize, Globals.spaceCraftSize, Globals.spaceCraftSize);
        spacecraftModel.transform.Rotate(new Vector3(0, 180, 0));

        //add particle system to model
        GameObject plumeToCopy = GameObject.Find("Rocket_Plume");
        GameObject plumeModel = Instantiate(prefabToCopy.GetComponent<InstantiateObject>().myPlume, spacecraftObject.transform.position, Quaternion.Euler(90, 0, 0));
        plumeModel.transform.parent = spacecraftObject.transform;
        plumeModel.transform.Translate(new Vector3(0, 0, .1f), Space.Self);
        plumeModel.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        plumeModel.transform.GetChild(0).GetComponentInChildren<Transform>().localScale = new Vector3(.01f, .01f, .01f);
        plumeModel.transform.GetChild(1).GetComponentInChildren<Transform>().localScale = new Vector3(.01f, .01f, .01f);
        plumeModel.GetComponent<ParticleSystem>().Stop();

        //add sound to spacecraft
        AudioSource myRocketSound = spacecraftObject.AddComponent<AudioSource>();
        myRocketSound.clip = prefabToCopy.GetComponent<InstantiateObject>().myPlumeSound;
        
        //-------------------------------------------Initialize all buttons-------------------------------------------------------------------------------------------------
        GameObject.Find("Prograde Button").GetComponentInChildren<Text>().text = "Prograde";
        GameObject.Find("Retrograde Button").GetComponentInChildren<Text>().text = "Retrograde";
    }

}
