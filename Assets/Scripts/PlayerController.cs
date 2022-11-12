using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
	[SerializeField] Image healthbarImage;
	[SerializeField] GameObject ui;

	[SerializeField] GameObject cameraHolder;

	[SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, smoothTime;

	[SerializeField] float jumpForce = 10.0f;

	[SerializeField] Item[] items;

	int itemIndex;
	int previousItemIndex = -1;

	float verticalLookRotation;
	bool grounded;
	bool autoRun = false;
	Vector3 smoothMoveVelocity;
	Vector3 moveAmount;

	Rigidbody rb;

	PhotonView PV;

	const float maxHealth = 100f;
	float currentHealth = maxHealth;

	PlayerManager playerManager;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		PV = GetComponent<PhotonView>();

		playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
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
			Destroy(ui);
		}
	}





      public void LeaveHome()
    {
            if(!PV.IsMine)
                return;
	    
		transform.position = ColHero.HomePlateLocation;
             

    }


	void Update()
	{
		if(!PV.IsMine)
			return;

		Look();
		Move();
		Jump();

//********************************************************************************
        // AUTORUN TOGGLE
//************************************************a********************************
           if(Input.GetButtonDown("AutoRun"))
            {

                if(autoRun)
                    autoRun = false;
                        else
                        {
                                autoRun = true;
                        }
            }

               if(autoRun)
        {


            rb.MovePosition(rb.position + transform.forward * walkSpeed * Time.fixedDeltaTime);

        }

//********************************************************************************
//********************************************************************************

//********************************************************************************
//						TELEPORT
//********************************************************************************

					          if(Input.GetButtonDown("Teleport"))
            {
						if(ColHero.atHome)
						{
								transform.position = ColHero.HomePlateLocation;
								ColHero.atHome = false;
						}
							else
							{
						transform.position = ColHero.HomeTeleportLocation;
						ColHero.atHome = true;
							}

            }

	//	for(int i = 0; i < items.Length; i++)
	//	{
	//		if(Input.GetKeyDown((i + 1).ToString()))
	//		{
	//			EquipItem(i);
	//			break;
	//		}
	//	}

		if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
		{
			if(itemIndex >= items.Length - 1)
			{
				EquipItem(0);
			}
			else
			{
				EquipItem(itemIndex + 1);
			}
		}
		else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
		{
			if(itemIndex <= 0)
			{
				EquipItem(items.Length - 1);
			}
			else
			{
				EquipItem(itemIndex - 1);
			}
		}

		if(Input.GetMouseButtonDown(0))
		{
			print("ATTACKING");
			items[itemIndex].Use();

		}

		if(transform.position.y < -30f)
		{
			GOver();
		}
	}

	void Look()
	{
		    if (!Input.GetMouseButton(1))
        {
            return;
        }
		
		transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

		verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

		cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
	}

	void Move()
	{
		Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

		moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);

	}

	void Jump()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			 print("JUMPING!!!!");
		
			rb.velocity = Vector3.up * jumpForce;
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

		if(PV.IsMine)
		{
			Hashtable hash = new Hashtable();
			hash.Add("itemIndex", itemIndex);
			PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		if(!PV.IsMine && targetPlayer == PV.Owner)
		{
			EquipItem((int)changedProps["itemIndex"]);
		}
	}

	public void SetGroundedState(bool _grounded)
	{
		grounded = _grounded;
	}

	void FixedUpdate()
	{

		if(!PV.IsMine)
			return;

		if(autoRun)
		{

		}
			else
			{
					rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
			}

		

	}

	public void TakeDamage(float damage)
	{
		PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
	}

	[PunRPC]
	void RPC_TakeDamage(float damage)
	{
		if(!PV.IsMine)
			return;

		currentHealth -= damage;

		healthbarImage.fillAmount = currentHealth / maxHealth;

		if(currentHealth <= 0)
		{
			GOver();
		}
	}

	void GOver()
	{
		playerManager.GOver();
	}
}