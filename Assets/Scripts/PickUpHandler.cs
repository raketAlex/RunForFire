using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHandler : MonoBehaviour
{
    public enum PickupType
    {
        stone, 
        grass,

        finish
    }
    public PickupType pickupType;
    public GameObject[] pickups;
    private ParticleSystem pickupParty;

    private void Start() 
    {
      pickupParty = GetComponentInChildren<ParticleSystem>();
      if(pickupType == PickupType.grass)
      {
        pickups[0].gameObject.SetActive(true); pickups[1].gameObject.SetActive(false);
        pickupParty.GetComponent<ParticleSystemRenderer>().material = pickups[0].GetComponentInChildren<Renderer>().material;
      } 
        
      else if(pickupType == PickupType.stone)
      {
        pickups[1].gameObject.SetActive(true); pickups[0].gameObject.SetActive(false);
        pickupParty.GetComponent<ParticleSystemRenderer>().material = pickups[1].GetComponentInChildren<Renderer>().material;
      } 
    }

    private void Update() 
    {
      switch (pickupType)
      {
        case PickupType.grass:
        transform.Rotate(Vector3.up, 40f * Time.deltaTime);
        break;
        case PickupType.stone:
        transform.Rotate(Vector3.up, 40f * Time.deltaTime);
        break;
      }
    }

    private void OnTriggerEnter(Collider other) 
    {
      if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
      {
        {
        if(pickupType != PickupType.finish)
        {
        GameManager.instance.UpdatePickups(pickupType.ToString());
        StartCoroutine(ShrinkPickup(1.5f));   
        }
        else if(pickupType == PickupType.finish && GameManager.instance.CanFinish())
        {
          pickups[0].gameObject.SetActive(true);
        }
        }
      } 
    }

    IEnumerator ShrinkPickup(float time)
    {
      float elapsed = 0;
      float duration = time;
      Transform pickupObj = (Transform) GetComponentInChildren<Transform>();
      float startSize = pickupObj.localScale.x;
      float size;
      pickupParty.Emit(60);
      while(elapsed < duration)
      {
        size = RSLerp.EaseInOutCirc(startSize, 0.0001f,elapsed, duration);
        this.transform.localScale = Vector3.one * size;
        elapsed = Mathf.Min(duration, elapsed + Time.deltaTime);
        yield return new WaitForEndOfFrame();
      } 
      Destroy(gameObject);
    }
}
