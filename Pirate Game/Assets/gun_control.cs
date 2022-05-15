using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun_control : MonoBehaviour
{
    public bool at_gun, controlling_gun;
    public GameObject triggered_gun, t_platform;
    public PlayerController pc;
    Transform t_seat, t_pivot;
    float mouseSensitivity, smoothTime, verticalLookRotation;
    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
        mouseSensitivity = pc.mouseSensitivity;
        smoothTime = pc.smoothTime;
        verticalLookRotation = 90f;
    }

    // Update is called once per frame
    void Update()
    {
        if (at_gun && Input.GetKeyDown(KeyCode.E))
        {
            GunMountToggle();
        }
        if (controlling_gun)
        {
            //rotate y axis via parent, rotate z axis via gun seat
            t_platform.transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X"));

            verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, 45f, 120f);

            t_pivot.localEulerAngles = Vector3.left* verticalLookRotation + new Vector3(0,0,-180);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ShipTurret"))
        {
            at_gun = true;
            triggered_gun = other.gameObject;
            //display a prompt letting the player know they can press E to control the ship turret
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ShipTurret"))
        {
            at_gun = false;
            triggered_gun = null;
        }
    }
    void GunMountToggle()
    {
        if (controlling_gun) //if currently controlling gun, then dismount the gun
        {
            Dismount();
            return;
        }
        else //if not controlling the gun, then mount the gun
        {
            Mount();
            return;
        }
    }
    private void Mount() //do stuff to disable player movement, center them on gun turret, change camera?, show turret UI (reticle, ammo remaining, reload time)
    {
        controlling_gun = true;
        pc.DisablePlayerMovement();
        t_platform = triggered_gun;
        t_pivot = t_platform.transform.Find("Turret Base").Find("Pivot Point");
        t_seat = t_pivot.Find("Turret Seat");
        transform.parent = t_seat;
    }
    private void Dismount() //undo above stuff
    {
        controlling_gun = false;
        pc.EnablePlayerMovement();
        t_platform = null;
    }
}
