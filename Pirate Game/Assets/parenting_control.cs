using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parenting_control : MonoBehaviour
{
    public Transform parent;
    public GameObject cam;
    GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "PlayerGroundCheck")
        {
            player = other.gameObject.transform.parent.gameObject;
            player.transform.parent = parent;
            player.GetComponent<ship_control>().controlled_ship = gameObject;
            //collision.gameObject.GetComponent<ship_control>().ship_cam = cam;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "PlayerGroundCheck")
        {
            player = other.gameObject.transform.parent.gameObject;
            player.transform.parent = null;
            player.GetComponent<ship_control>().controlled_ship = null;
        }
    }
}
