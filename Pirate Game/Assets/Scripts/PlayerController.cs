using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
	[SerializeField] GameObject cameraHolder;
	[SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
	[SerializeField] Item[] items;
	
	int itemIndex;
	int previousItemIndex = -1;
	
	float verticalLookRotation;
    bool grounded;
    public bool movement_enabled;
	Vector3 smoothMoveVelocity;
	Vector3 moveAmount;
	
    Rigidbody rb;
	
	PhotonView PV;
	
	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		PV = GetComponent<PhotonView>();
	}
	
	void Start()
	{
		if(PV.IsMine)
		{
			EquipItem(0);
		}
		else
		{
			Destroy(GetComponentInChildren<Camera>().gameObject);
			Destroy(rb);
		}
        movement_enabled = true;
	}
	
	void Update()
	{
		if(!PV.IsMine)
			return;
		Look();
        if (movement_enabled)
        {
            Move();
            Jump();
        }
		for(int i = 0; i < items.Length; i++)
		{
			if(Input.GetKeyDown((i + 1).ToString()))
			{
				EquipItem(i);
				break;
			}
		}
	}
	
	void Move()
	{
		Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
		
		//basically this has a compacted if statement that checks if the shift key is enabled and uses either sprint or walk speed.
		moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
	}
	
	void Jump()
	{
		if(Input.GetKeyDown(KeyCode.Space) && grounded)
		{
			rb.AddForce(transform.up * jumpForce);
		}
	}
	
	void EquipItem(int _index)
	{
		if(_index == previousItemIndex)
			return;
		itemIndex = _index;
		
		items[itemIndex].itemGameObject.SetActive(true);
		
		if(previousItemIndex != -1)
		{
			items[previousItemIndex].itemGameObject.SetActive(false);
		}
		previousItemIndex = itemIndex;
	}

	void Look()
	{
		transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
		
		verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
		
		cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
	}
	
	public void SetGroundedState(bool _grounded)
	{
		grounded = _grounded;
	}
	
	void FixedUpdate()
	{
		if(!PV.IsMine)
			return;
		rb.MovePosition(rb.position + transform.TransformDirection(moveAmount)*Time.fixedDeltaTime);
	}
    public void DisablePlayerMovement()
    {
        rb.velocity = new Vector3(0, 0, 0);
        moveAmount = new Vector3(0, 0, 0);
        movement_enabled = false;
    }
    public void EnablePlayerMovement()
    {
        movement_enabled = true;
    }
}
