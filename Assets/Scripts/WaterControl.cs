using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterControl : ColHero
{

    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;
    public float floatingPower = 250f;

    public float waterLevel = -2.001f;

    Rigidbody m_Rigidbody;

    bool underwater;





    // Start is called before the first frame update
    void Start()
    {
             if(PV.IsMine)
		{
        m_Rigidbody = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
               if(!PV.IsMine)
                    return;
		
            float difference = transform.position.y - waterLevel;

    if(difference < 0)
     {

    m_Rigidbody.AddForceAtPosition (Vector3.up * floatingPower * Mathf.Abs(difference), transform.position, ForceMode.Force );
           
            if(!underwater)
            {
                underwater = true;
                SwitchState(true);
            }


     }
        else if(underwater)
        {
            underwater = false;
            SwitchState(false);
        }


        void SwitchState(bool isUnderwater)
        {

            if(isUnderwater)
            {
                    m_Rigidbody.drag = underWaterDrag;
                    m_Rigidbody.angularDrag = underWaterAngularDrag;
            }
                else
                {
                    m_Rigidbody.drag = airDrag;
                    m_Rigidbody.angularDrag = airAngularDrag;
                }

        }



    }
}
