using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UInteractions : MonoBehaviour
{

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Until next time..."); 
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

            //********************************************************************************
        //                               EXIT GAME
//********************************************************************************
           if(Input.GetButtonDown("Cancel"))
            {
                            Application.Quit();
                             Debug.Log("Until next time..."); 
            }
//********************************************************************************
//********************************************************************************



        
    }
}
