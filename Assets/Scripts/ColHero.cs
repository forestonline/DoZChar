using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColHero : PlayerManager
{
    public static bool atHome = false;
    public static bool garrisonPlaced = false;
    public static Vector3 HomeTeleportLocation;
    public static Vector3 HomePlateLocation;

      Collider ColHolder;
      

    // Start is called before the first frame update
    void Start()
    {

            if(PV.IsMine)
		{

         ColHolder = this.GetComponent<CapsuleCollider>();

                HomeTeleportLocation.x = 53.0f;
                HomeTeleportLocation.y = 5.0f;
                HomeTeleportLocation.x = 12.0f;
           
        }

    }

    // Update is called once per framexs
    void Update()
    {
        if(!PV.IsMine)
                return;

            if(Input.GetButtonDown("PlaceGarrison"))
            {
		           if(garrisonPlaced)
                   {

                   } 
                            else
                            {
                                SpawnGarrison(transform.position.x, transform.position.z);
                               
                                    garrisonPlaced = true;
                                    atHome = true;
   
                                    // Set recall Location
                                    

                            }
            }


    }
}
