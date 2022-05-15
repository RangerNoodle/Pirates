using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f, nextTimeRGShot, RGreloadTime = 120f, distance;

    Transform target;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        target = null;
        agent = GetComponent<NavMeshAgent>();
        nextTimeRGShot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            float distance = Vector3.Distance(target.position, transform.position);
        }

        if(target == null || distance > lookRadius)
        {
            FindTarget();
        }

        if(target != null)
        {
            agent.SetDestination(target.position);
            //float distance = Vector3.Distance(target.position, transform.position);
            if(Time.time >= nextTimeRGShot)
            {
                if(RGTargetInSights() == true)
                {
                    // shoot the gun
                }
                else 
                {
                    //aim the gun instead
                }
                  
            }
        }
    }
    void OnDrawGizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    void FindTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform bestTarget = null;
        float closestDistanceSQR = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        for(int i = 0; i < players.Length; i++)
        {
            Vector3 directionToTarget = players[i].transform.position - currentPosition;
            float dSQRTarget = directionToTarget.sqrMagnitude;
            if(dSQRTarget < closestDistanceSQR && dSQRTarget < lookRadius)
            {
                closestDistanceSQR = dSQRTarget;
                bestTarget = players[i].transform;
            }
        }
        target = bestTarget;
    }
    bool RGTargetInSights()
    {
        if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hitInfo) && hitInfo.transform == target)
        {
            return true;
        }
        else {return false;}
    }


}
