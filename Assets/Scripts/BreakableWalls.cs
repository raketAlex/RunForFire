    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWalls : MonoBehaviour
{
    [SerializeField]
    private List<Rigidbody> rigidbodies= new List<Rigidbody>();

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            for (int i = 0; i < rigidbodies.Count; i++)
            {
                rigidbodies[i].isKinematic = false;
                rigidbodies[i].AddForce(Vector3.forward * 3f,ForceMode.Impulse);
                Destroy(rigidbodies[i].gameObject, Random.Range(2f,3f));
            }
        }
    }
}
