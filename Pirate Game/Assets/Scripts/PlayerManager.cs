using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
	
	PhotonView PV;
	
	void Awake()
	{
		PV = GetComponent<PhotonView>();
	}
	
	void CreateController()
	{
		//Spawn the player controller at Vector3.zero and Quaternion.identity change later to spawn on ship
		PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);	
	}
    // Start is called before the first frame update
    void Start()
    {
        if(PV.IsMine)
		{
			CreateController();
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
