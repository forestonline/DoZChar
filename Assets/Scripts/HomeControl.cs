using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;



public class HomeControl : MonoBehaviourPunCallbacks
{
        PhotonView PV;


    [SerializeField] GameObject HomeBase;
    [SerializeField] GameObject HomePlate;
   

    	void Awake()
	{
	          	PV = GetComponent<PhotonView>();
	}
 
    // Start is called before the first frame update
    void Start()
    {
        // ColHero.HomeTeleportLocation = HomePlate.transform.position;
        ColHero.HomeTeleportLocation = HomeBase.transform.position;
        ColHero.HomePlateLocation = HomePlate.transform.position;
          ColHero.HomePlateLocation.y = 5.0f;

    }

    // Update is called once per frame
    void Update()
    {
         if(!PV.IsMine)
                return;

           //     if(Input.GetButtonDown("PlaceGarrison"))
         //   {
		      //   if(ColHero.atHome)
                 ///       {
                 //            HomeCanvas.SetActive(true);
                   //     }
         //   }
    }


    public void MakePublic()
    {
            if(!PV.IsMine)
                return;
        // Destroy Walls
    }


    //[PunRPC]
    public void DestroyHome()
    {
      if(!PV.IsMine)
         return;
           
           ColHero.atHome = false;  
           ColHero.garrisonPlaced = false;    
        PhotonNetwork.Destroy(HomeBase);

         
    }



}
