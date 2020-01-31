using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveManController : MonoBehaviour
{
  Transform player;
  public float moveSpeed = 5f;
  public float followRadius = 1f;
  int currentPos;

private void Start() {
    currentPos = 0;
}
  private void Update() 
  {
  if(DrawRouteScript.instance.pathNodes.Count != 0 || GameManager.instance.gameState == GameManager.GameState.running)
  {
      FollowPath();
  }    
  }
  void FollowPath()
  {
    if(currentPos >= DrawRouteScript.instance.pathNodes.Count)
        {
            DrawRouteScript.instance.RemoveNodes();
            GameManager.instance.gameState = GameManager.GameState.drawing;
        }

    transform.LookAt(DrawRouteScript.instance.pathNodes[currentPos],Vector3.up);
    float distBetween = Vector3.Distance(transform.position,DrawRouteScript.instance.pathNodes[currentPos]);
    if(followRadius > distBetween)
    {
        currentPos++;
        
    }
    
    transform.position = Vector3.MoveTowards(transform.position,DrawRouteScript.instance.pathNodes[currentPos], moveSpeed * Time.deltaTime);
    
  }
}
