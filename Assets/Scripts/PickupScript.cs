using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public enum PickupType
    {
        stone, 
        grass
    }
    public PickupType pickupType;

    private void Update() 
    {
      transform.Rotate(Vector3.up, 40f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) 
    {
      other.GetComponent<Collider>().enabled = false;
      if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
      {
        {
        GameManager.instance.UpdatePickups(pickupType.ToString());
        StartCoroutine(ShrinkPickup(.5f));   
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
      while(elapsed < duration)
      {
        size = RSLerp.EaseInOutCirc(startSize, 0.0001f,elapsed, duration);
        pickupObj.localScale = size * Vector3.one;
        elapsed = Mathf.Min(duration, elapsed + Time.deltaTime);
        yield return new WaitForEndOfFrame();
      } 
      Destroy(gameObject);
    }
}
