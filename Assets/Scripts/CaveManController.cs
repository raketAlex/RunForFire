using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveManController : MonoBehaviour
{
  public Transform direction;
  public static CaveManController instance;
  public float rotateSpeed = 5;
  public float followRadius = 1f;
  public int currentPos;
  public Rigidbody myRb;
  Input runButton;
  Quaternion startRotation;
  Vector3 startPos;
  public bool resetPlayer = false;
  GameObject startRadius;
  public bool canDraw = false;
  public Material startMaterial;
  Vector3 radiusScale;
  float radiusSize = 4f;
  public ParticleSystem crashParticle;
  public LayerMask groundLayer;
  

  private void Awake() 
  {
    instance = this;  
  }
  private void Start() 
  {
    currentPos = 0;
    
    startRotation = myRb.transform.localRotation;
    startPos = myRb.transform.position;

  }

  private void OnEnable() 
  {
    UpdateDrawRadius(false);
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
    Debug.Log(IsGrounded());
    switch(GameManager.instance.gameState)
    {
      case GameManager.GameState.running:
      if(!IsGrounded())
      {
        StartCoroutine(Falling());
      }
      FollowPath();
      break;

      case GameManager.GameState.falling:

      break;

      case GameManager.GameState.drawing:
      if(canDraw)
      {
        
        UpdateDrawRadius(false);
      }
      break;

      case GameManager.GameState.gameOver:
      break;
    }
  }
  void FollowPath()
  {
    
    if(currentPos == DrawRouteScript.instance.pathNodes.Count)
        {
            currentPos = 0;
            DrawRouteScript.instance.RemoveNodes();
            StartCoroutine(ShowDrawRadius());
            myRb.velocity = Vector3.zero;
            myRb.angularVelocity = Vector3.zero;
            GameManager.instance.SwitchState(GameManager.GameState.drawing);
        }
    
    if(DrawRouteScript.instance.pathNodes.Count != 0)
    {
      float distBetween = Vector3.Distance(myRb.transform.position,DrawRouteScript.instance.pathNodes[currentPos]);
      if(followRadius > distBetween)
      {
        currentPos++;  
      }
      myRb.angularVelocity = Vector3.zero;
      PlayerRotation(DrawRouteScript.instance.pathNodes[currentPos]);
      
      myRb.velocity = myRb.transform.forward * moveSpeed();
      //myRb.transform.position = Vector3.MoveTowards(myRb.transform.position,DrawRouteScript.instance.pathNodes[currentPos], moveSpeed() * Time.deltaTime);
    }
  }

  private void PlayerRotation(Vector3 lookPos)
  {
    var targetRot = Quaternion.LookRotation(lookPos- myRb.transform.position);
    targetRot.x = 0;
    targetRot.z = 0;
    myRb.transform.rotation = Quaternion.Slerp(myRb.transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
  }

  void OnTriggerEnter(Collider other) {
    
    if(other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
    {
      RaycastHit hit;
      if(Physics.Raycast(myRb.transform.position, myRb.transform.forward, out hit))
      {
      var contact = hit.point;
      var direction = myRb.transform.position - contact;
      var party = Instantiate(crashParticle, contact,Quaternion.identity );
      party.Emit(80);
      DrawRouteScript.instance.RemoveNodes();
      
      DeathCollision(direction);
      GameManager.instance.SwitchState(GameManager.GameState.drawing);
      }
    }
  }
  public void DeathCollision(Vector3 direction)
  {
    GameManager.instance.SwitchState(GameManager.GameState.gameOver);
    myRb.isKinematic = false;
    myRb.useGravity = true;
    myRb.constraints = RigidbodyConstraints.None;
    myRb.AddForce(direction.normalized * moveSpeed(),ForceMode.Impulse);
    myRb.AddForce(Vector3.up * 2f, ForceMode.Impulse);
    myRb.AddTorque(Vector3.right * -10f);
  }

  public void ResetCharacter()
  {
    myRb.velocity = Vector3.zero;
    myRb.isKinematic = true;
    myRb.useGravity = false;
    myRb.constraints = RigidbodyConstraints.FreezePositionX;
    myRb.constraints = RigidbodyConstraints.FreezePositionZ;
    myRb.transform.localRotation = startRotation;
    myRb.transform.localPosition = startPos;
  }
  public void UpdateDrawRadius(bool update)
    {
      if(!update)
      {
        startRadius = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        myRb.transform.position = myRb.position;
        startRadius.GetComponent<Renderer>().material = startMaterial;        
        startRadius.GetComponent<Collider>().isTrigger = true;
        radiusScale = new Vector3(radiusSize,0.01f,radiusSize);
        startRadius.transform.localScale = radiusScale;
        startRadius.layer = LayerMask.NameToLayer("DrawRadius");
      }
        

        RaycastHit hit;
        if(Physics.Raycast(myRb.transform.position, Vector3.down, out hit,Mathf.Infinity))
        {
          startRadius.transform.position = new Vector3(hit.point.x, hit.point.y , hit.point.z);
        }
    }

    public IEnumerator HideDrawRadius()
    {
      float elapsed = 0;
      float duration = .9f;
      startRadius.GetComponent<Collider>().enabled = false;
      
      while(elapsed < duration)
      {

      float size = RSLerp.EaseInBack(radiusSize, 0.00001f, elapsed, duration);
      elapsed = Mathf.Min(duration, elapsed + Time.deltaTime);
      startRadius.transform.localScale = new Vector3(size * 1, 0.0001f, size * 1);// size * Vector3.one;
      yield return new WaitForEndOfFrame();
      }
      startRadius.SetActive(false);
    }
    public IEnumerator ShowDrawRadius()
    {
      float elapsed = 0;
      float duration = .5f;
      UpdateDrawRadius(true);
      startRadius.GetComponent<Collider>().enabled = true;
      startRadius.SetActive(true);
      while(elapsed < duration)
      {
        float size = RSLerp.EaseInCubic(0.00001f, radiusSize, elapsed, duration);
        elapsed = Mathf.Min(duration, elapsed + Time.deltaTime);
        startRadius.transform.localScale = new Vector3(1 * size, radiusScale.y, 1 * size);
        yield return new WaitForEndOfFrame();

      }
    }
    public bool IsGrounded()
    {
        if(Physics.Raycast(myRb.transform.position, -myRb.transform.up,2f,groundLayer))
        {
          return true;
        }    else
        {
          return false;
        }
    }
    IEnumerator Falling()
    {
      float elapsed = 0;
      float duration = 0.2f;
      while(elapsed < duration)
      {
        elapsed = Mathf.Min(duration, elapsed + Time.deltaTime);
        yield return new WaitForEndOfFrame();
      }
      GameManager.instance.gameState = GameManager.GameState.falling;
    }

}