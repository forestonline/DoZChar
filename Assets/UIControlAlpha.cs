using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UIControlAlpha : MonoBehaviour
{
           PhotonView PV;


    	void Awake()
	{
	    	PV = GetComponent<PhotonView>();
	}


    // Start is called before the first frame update
    void Start()
    {
        
    }


       public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Until next time..."); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
