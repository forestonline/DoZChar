using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        	this.GetComponent<Animator>().Play("Defend"); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
