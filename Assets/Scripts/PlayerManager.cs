using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
	public PhotonView PV;

	GameObject controller;
	GameObject treeController;
	GameObject oakTreeController;
	GameObject claimController;
	GameObject garrisonController;
	GameObject sunController;

	int a = 1;

	void Awake()
	{
		PV = GetComponent<PhotonView>();
	}

	void Start()
	{


		if(PV.IsMine)
		{
			CreateController();
			//SpawnSun();
			
			    while(a < 200)
        		{

           	
					SpawnTree();
					SpawnTreeTwo();


            			a++;
        		}
			
		
		}
	}

	void CreateController()
	{
		Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
		controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
	}

		public void SpawnSun(float x, float z)
	{

			if(PV.IsMine)
			{
		
		Vector3 Pos;

        Pos.x = x + 40;
        Pos.y =  15f;
        Pos.z = z - 40;

       
        print("Spawn Sun: " +   Pos.x   +  "   :    "    + Pos.z);
	
		sunController = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "TimeController"), Pos, Quaternion.identity, 0, new object[] { PV.ViewID });
			}
						else
						{
							Destroy(sunController);
						}
	}

	void SpawnTree()
	{
		 Vector3 randomPos;

        randomPos.x = 1f;
        randomPos.y =  -2f;
        randomPos.z = 1f;

        randomPos.x = Random.Range(-211, 275);
        randomPos.z = Random.Range(-250, 220);
        print("Spawn Tree: " +   randomPos.x   +  "   :    "    + randomPos.z);
	
		treeController = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "FirTreeAlpha"), randomPos, Quaternion.identity, 0, new object[] { PV.ViewID });

	}

		void SpawnTreeTwo()
	{
		 Vector3 randomPos;

        randomPos.x = 1f;
        randomPos.y =  -2f;
        randomPos.z = 1f;

        randomPos.x = Random.Range(-211, 275);
        randomPos.z = Random.Range(-250, 220);
        print("Spawn Oak Tree: " +   randomPos.x   +  "   :    "    + randomPos.z);
	
		oakTreeController = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "OakTreeAlpha"), randomPos, Quaternion.identity, 0, new object[] { PV.ViewID });

	}


	public void SpawnGarrisonClaim()
	{
		Vector3 randomPos;

        randomPos.x = 1f;
        randomPos.y =  -14f;
        randomPos.z = 1f;

        randomPos.x = Random.Range(-140, 140);
        randomPos.z = Random.Range(-150, 38);
        print("Spawn Garrison Claim: " +   randomPos.x   +  "   :    "    + randomPos.z);
	
		claimController = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GarrisonClaim"), randomPos, Quaternion.identity, 0, new object[] { PV.ViewID });
	}

		public void SpawnGarrison(float x, float z)
	{
			if(PV.IsMine)
			{
		
		Vector3 Pos;

        Pos.x = x + 40;
        Pos.y =  15f;
        Pos.z = z - 40;

       
        print("Spawn Garrison: " +   Pos.x   +  "   :    "    + Pos.z);
	
		garrisonController = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GarrisonWalls"), Pos, Quaternion.identity, 0, new object[] { PV.ViewID });
			}
						else
						{
							Destroy(garrisonController);
						}


	}


	public void GOver()
	{
		PhotonNetwork.Destroy(controller);
		CreateController();
	}
}