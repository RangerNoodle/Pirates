using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ship_control : MonoBehaviour
{
    public bool at_wheel, controlling_ship;
    public float vertical, horizontal, speed, rot_speed;
    Vector3 inp_move;
    public Vector3 my_offset;
    PlayerController pc;
    public GameObject controlled_ship;
    Rigidbody ship_rbody, my_rbody;
    Transform ship_transform;
    public Transform cam;
    public GameObject ship_cam_prefab, ship_cam;
    FixedJoint joint;
    // Start is called before the first frame update
    void Start()
    {
        at_wheel = false;
        controlling_ship = false;
        pc = GetComponent<PlayerController>();
        my_rbody = GetComponent<Rigidbody>();
        speed = 5f;
        rot_speed = 30f;
        joint = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (at_wheel && Input.GetKeyDown(KeyCode.E))
        {
            ShipMountToggle();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ship"))
        {
            at_wheel = true;
            controlled_ship = other.gameObject;
            //display a prompt letting the player know they can press E to control the ship
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ship"))
        {
            at_wheel = false;
            controlled_ship = null;
            //remove the prompt letting the player know they can press E to control the ship
        }
    }
    private void FixedUpdate()
    {
        if (controlling_ship)
        {
            //the ship should go "straight" forward based on its current orientation
            //the ship should rotate with horizontal input
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            inp_move = new Vector3(0f, 0f, vertical).normalized;
            if (inp_move.magnitude > 0.1f)
            {
                Quaternion turn_value = Quaternion.Euler(0f, horizontal * rot_speed * Time.fixedDeltaTime, 0f);
                ship_rbody.MoveRotation(ship_rbody.rotation * turn_value);
                Vector3 move_dir = ship_transform.forward * vertical * speed * Time.fixedDeltaTime;
                ship_rbody.MovePosition(ship_rbody.position + move_dir);
            }
        }
    }
    void ShipMountToggle()
    {
        if (controlling_ship) //if currently controlling ship, then dismount the ship
        {
            Dismount();
            return;
        }
        else //if not controlling the ship, then mount the ship
        {
            Mount();
            return;
        }
    }
    private void Mount() //do stuff to disable player movement, center them on ship, change camera?
    {
        controlling_ship = true;
        pc.DisablePlayerMovement();
        ship_rbody = controlled_ship.GetComponent<Rigidbody>();
        ship_transform = controlled_ship.transform;
        my_offset = controlled_ship.transform.position - gameObject.transform.position;
        //if ship cam does not already exist, spawn ship cam
        if (!GameObject.Find("Ship Cam(Clone)"))
        {
            ship_cam = Instantiate(ship_cam_prefab);
            ship_cam.GetComponent<CinemachineFreeLook>().Follow = controlled_ship.transform;
            ship_cam.GetComponent<CinemachineFreeLook>().LookAt = controlled_ship.transform;
        }
        ship_cam.GetComponent<CinemachineFreeLook>().Priority = 20;
        if (joint == null)
        {
            joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = ship_rbody;
        }
    }
    private void Dismount() //undo above stuff
    {
        controlling_ship = false;
        pc.EnablePlayerMovement();
        //delete ship cam if it exists
        if (ship_cam != null)
        {
            Destroy(ship_cam);
        }
        ship_rbody = null;
        ship_cam.GetComponent<CinemachineFreeLook>().Priority = 0;
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }
    }
}
