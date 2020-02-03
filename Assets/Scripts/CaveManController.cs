using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveManController : MonoBehaviour
{
  public Transform direction;
  //public float moveSpeed = 5f;
  private float rotateSpeed = 5;
  public float followRadius = 1f;
  public int currentPos;
  public Rigidbody myRb;
Vector3 movePos;
  Input runButton;

private void Start() {
    currentPos = 0;
    //myRb = GetComponent<Rigidbody>();
}

 public float moveSpeed()
   {
     if(Input.GetKey(KeyCode.Space))
     {
       var runSpeed = RSLerp.EaseInBack(5, 10f, 1f);
       rotateSpeed = 8;
       return runSpeed;
     }
     else 
     {
       rotateSpeed = 5f;
       var walkSpeed = 5f;
       return walkSpeed;
     }
   }

  private void Update() 
  { 
  if(DrawRouteScript.instance.pathNodes.Count != 0 && GameManager.instance.gameState == GameManager.GameState.running)
    {
      FollowPath();
    }    
  }
  void FollowPath()
  {
    
    if(currentPos == DrawRouteScript.instance.pathNodes.Count)
        {
            currentPos = 0;
            DrawRouteScript.instance.RemoveNodes();
            GameManager.instance.SwitchState(GameManager.GameState.drawing);
        }
    
    if(DrawRouteScript.instance.pathNodes.Count != 0)
    {
      float distBetween = Vector3.Distance(transform.position,DrawRouteScript.instance.pathNodes[currentPos]);
      if(followRadius > distBetween)
      {
        currentPos++;  
      }
      PlayerRotation(DrawRouteScript.instance.pathNodes[currentPos]);
      transform.position = Vector3.MoveTowards(transform.position,DrawRouteScript.instance.pathNodes[currentPos], moveSpeed() * Time.deltaTime);
    }
  }

  private void PlayerRotation(Vector3 lookPos)
  {
    var targetRot = Quaternion.LookRotation(lookPos- transform.position);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
  }

  void OnTriggerEnter(Collider other) {
    Debug.Log("works");
    if(other.gameObject.layer == LayerMask.NameToLayer("Dino"))
    {
      Debug.Log("GIT");
      GameManager.instance.SwitchState(GameManager.GameState.drawing);
    }
  }

  
}