using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject projectilePrefab;
    public float velocityMult = 10f;

    [Header("Dynamic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    private void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }

    void OnMouseEnter()
    {
        //rint("SlingShot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        //rint("Slingshot:OnMouseExit"); 
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        //The player has pressed the mause button over slingshot
        aimingMode = true;
        //Instantiate a projectile
        projectile = Instantiate(projectilePrefab) as GameObject;
        //Start at the launchPoint
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }

    // Start is called before the first frame update
    // void Start()
    // {

    //}

    // Update is called once per frame
    void Update()
    {
        //If Slingshot is not in aimingmode, dont run this code
        if (!aimingMode) return;

        //Get the current mouse position in 2d screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //Find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        //Limit mouseDelta to the radius of the Slingshot SphereCollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //Move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;


        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projRB.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile; //Set the Maincamera POI
            projectile = null;
        }

    }
}

