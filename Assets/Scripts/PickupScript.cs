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
}
