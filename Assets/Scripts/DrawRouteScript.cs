﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRouteScript : MonoBehaviour
{
    public static DrawRouteScript instance;
    bool canDraw = true;
    [HideInInspector]
    public LineRenderer lr;
    public float maxDistance;
    public float yOffset = 2f;
    public Vector3 currentPos;
    public Vector3 previousPos; 
    public LayerMask groundLayer;
    public List<Vector3> pathNodes = new List<Vector3>();
    private ParticleSystem drawParty;
    
    public bool isDrawing = false;

    private void Awake() {
        instance=this;
    }
    private void Start() {
    lr = GetComponent<LineRenderer>();
    drawParty = GetComponentInChildren<ParticleSystem>();
    lr.enabled = false;
    drawParty.gameObject.SetActive(false);
    
    pathNodes.Clear();
    lr.positionCount = pathNodes.Count;
    }
    private void Update() {
       
        if(Input.GetMouseButtonDown(0) && !isDrawing)
        {
           StartDraw();   

        } 
        else if(!canDraw && Input.GetMouseButton(0) && isDrawing){
            
            UpdateLine();
        }
        else if(Input.GetMouseButtonUp(0)){
            canDraw = true;
            GameManager.instance.SwitchState(GameManager.GameState.running);
            
        } 
    }
    void StartDraw(){

        RaycastHit hit;
        Ray ray = CameraScript.instance.mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out hit, Mathf.Infinity,groundLayer)){
            isDrawing = true;
            drawParty.transform.position = hit.point;
            drawParty.gameObject.SetActive(true);
            currentPos = hit.point;
            pathNodes.Add(currentPos);
            lr.positionCount = pathNodes.Count;
            lr.SetPosition(0, currentPos);
            
            previousPos = currentPos;
            currentPos = Vector3.zero;
        }
        
        if(canDraw){
            lr.enabled = true;  
            canDraw = false;
        }  
    }
    void UpdateLine(){
        RaycastHit hit;
        Ray ray = CameraScript.instance.mainCamera.ScreenPointToRay(Input.mousePosition);
        drawParty.gameObject.SetActive(true);
        if(Physics.Raycast(ray,out hit, Mathf.Infinity, groundLayer))
        {
            drawParty.transform.position = hit.point;
            currentPos = hit.point;
            currentPos.y += yOffset;
  
            var distance = Vector3.Distance(previousPos, currentPos);
          
            if(distance > maxDistance){
               {
                   pathNodes.Add(currentPos);
                   lr.positionCount++;
                   
                   
                for (int i = 0; i < pathNodes.Count; i++)
                {
                   lr.SetPosition(i,pathNodes[i]);
                } 
                previousPos = currentPos;
               }
              
                
            } 
        }
    }
    public void RemoveNodes()
    {
        lr.positionCount = 0;
        pathNodes.Clear();
        isDrawing = false;
    }

}
