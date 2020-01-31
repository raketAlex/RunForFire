using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public static CameraScript instance;
    public Transform target;
    public float offset = 9f;
    private Vector3 camPos;
    public bool isFollowing = true;
    public Camera mainCamera;
    
    private void Awake() {
        instance = this;
    }
    void Start()
    {
        mainCamera = gameObject.GetComponent<Camera>();
        camPos = transform.position;
    }

    void Update(){
        if(isFollowing){
//        camPos = new Vector3(transform.position.x, transform.position.y, target.position.z + offset);
        } 
    }
    void FixedUpdate()
    {
        if(isFollowing){
        transform.position = Vector3.Lerp(transform.position, camPos, 0.4f);
        }
    }
}
