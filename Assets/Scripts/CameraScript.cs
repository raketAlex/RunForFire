using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public static CameraScript instance;
    public Transform target;
    public float offset = 9f;
    private Vector3 camPos;
    public bool isFollowing = false;
    public Camera mainCamera;
     Vector3 startPos;
    public Vector3 endPos;
    Vector3 startRot;
    public Vector3 endRot;
    
    private void Awake() {
        instance = this;
    }
    void Start()
    {
        Debug.Log(mainCamera.transform.eulerAngles);
       startPos = mainCamera.transform.localPosition;
       startRot = mainCamera.transform.eulerAngles;
       camPos = mainCamera.transform.position;
       //StartCoroutine(CameraZoomIn());
    }

    void Update(){
        if(isFollowing){
  
       
        } 
    }
    void LateUpdate()
    {
        if(isFollowing){
         Debug.Log(camPos);
         camPos.z = offset;
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, target.position.z - offset), 2f * Time.fixedDeltaTime);
        mainCamera.transform.LookAt(target.position);
        }
    }

    public void CameraZooming(bool isZooming)
    {
        if(isZooming)
        {
            StartCoroutine(CameraZoomIn());
        }
    }
    IEnumerator CameraZoomIn()
    {
        GameManager.instance.gameState = GameManager.GameState.camZoom;
        float elapsed = 0;
        float duration = 1f;
        Vector3 movingPos;
        Vector3 currentAngle;
        while(elapsed < duration)
        {
            currentAngle = RSLerp.EaseInCubic(startRot,endRot,elapsed,duration);
            movingPos = RSLerp.EaseInCubic(startPos, endPos,elapsed, duration);
            mainCamera.transform.eulerAngles = currentAngle;
            mainCamera.transform.localPosition = movingPos;
            elapsed = Mathf.Min(duration, elapsed+Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        offset = Vector3.Distance(mainCamera.transform.position, CaveManController.instance.myRb.position);
        Debug.Log(offset);
        isFollowing = true;
        GameManager.instance.gameState = GameManager.GameState.running;
    }
}
