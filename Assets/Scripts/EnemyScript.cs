using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public List<Vector3> pathNodes = new List<Vector3>();
    public int posCount;
    public float radius = 1;
    public float moveSpeed = 6f;
    public enum EnemyType
    {
        Dino, Lizard, Mammoth
    }

    EnemyType type;

    private void Awake() {
        type = EnemyType.Dino;
        posCount = 0;
    }

    private void Update() 
    {
        var distBetween = Vector3.Distance(transform.position, pathNodes[posCount]);
        if(distBetween < radius)
        {
            posCount++;
        }
        if(posCount == pathNodes.Count)
        {
            posCount = 0;
        }
            RotateEnemy();
            transform.position = Vector3.MoveTowards(transform.position, pathNodes[posCount], moveSpeed * Time.deltaTime);
    }

    public void StorePath()
    {
        pathNodes = DrawRouteScript.instance.pathNodes;
    }

    void RotateEnemy()
    {
        var target = Quaternion.LookRotation(pathNodes[posCount] - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, moveSpeed * Time.deltaTime);
    }
}
