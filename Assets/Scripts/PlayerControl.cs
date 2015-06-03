using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public float hp;
	public bool TouchActived;
	
	public float powerX;
	public float powerY;
	
	public float currentSpeedX;
	public float deltaSpeedX;
	
	public float currentSpeedY;
	public float deltaSpeedY;
	
	Vector3 addedVelocity;
	
	float maxSpeed = 2f;
	//float acceleration = 5f;
	//float breakPower = 30f;
	float angle;
	//float rotateSpeed = 180f;
	
	public bool headToLeft = true; 
	
	
	
	
	public static PlayerControl instance;
	Transform myTranform;
	Rigidbody myRigidbody;
	Renderer myRend;

	Gun[] allgun;
	
	void Awake()
	{
		instance = this;
		hp = 30;
		myTranform = transform;
		myRigidbody = GetComponent<Rigidbody>();
		myRend = GetComponent<Renderer>();
		//myRend.material.shader = Shader.Find("");
	}
	
	public void StartMovementControl()
	{
		StartCoroutine (MovementControl ());
		
		#if UNITY_EDITOR || UNITY_STANDALONE
		TouchActived = false;
		#else
		TouchActived = true;
		#endif
	}
	public void ActivedGun()
	{
		allgun = GetComponentsInChildren<Gun>();
		foreach( Gun gun in allgun )
		{
			gun.enabled = true;
			gun.submarine = myTranform;
			gun.isEnemy = false;
		}
	}
	
	IEnumerator MovementControl()
	{ 
		for(;;)
		{		
			// Input
			while(isMove)
			{
				if(!TouchActived)
				{				
					powerX = Input.GetAxis("Horizontal");
					powerY = Input.GetAxis("Vertical");
				}
				// Calculate Amount to move
				
				currentSpeedX = powerX * maxSpeed;
				currentSpeedY = powerY * maxSpeed;
				
				// Rigidbody
				
				addedVelocity.x = currentSpeedX;
				addedVelocity.y = currentSpeedY;
				myRigidbody.velocity = addedVelocity;
				yield return null;
			}

			foreach( Gun gun in allgun )			
				gun.enabled = false;

			Vector3 rotation = Vector3.zero;
			myRigidbody.velocity = Vector3.zero;;
			Vector3 startAngle = myTranform.eulerAngles;
			Vector3 endAngle = startAngle - (Vector3.up*-180);
			for (float t = 0; t < 1f; t+=Time.deltaTime) 
			{
				rotation.y = Mathf.Lerp(startAngle.y,endAngle.y,t);
				myTranform.eulerAngles = rotation;

				myRigidbody.velocity = new Vector3((0.5f-t)*15*transform.forward.x,0,(0.5f-t)*10*transform.forward.z);

				yield return null;
			}
			myTranform.eulerAngles = endAngle;
			headToLeft = !headToLeft;

			foreach( Gun gun in allgun )			
				gun.enabled = true;
			isMove = true;
			
		}
	}
	bool isMove = true;
	
	public void Flip()
	{
		isMove = false;
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Item")
		{
			
		}
		if(other.tag == "Enemy")
		{
			EnemyAI enemyScript = other.GetComponent<EnemyAI>();
			GetHit(enemyScript.hitDmg);
            enemyScript.GetHit(5f);
		}
	}
	
	public void Respawn()
	{
		int x = Random.Range(1,RespawnPoint.instance.respawnPointTransforms.Length);
		
		myTranform.position = RespawnPoint.instance.respawnPointTransforms[x].position;
	}

	public void GetHit(float dmg)
	{
		hp-= dmg;
		print("Player hp" + hp);
		if(hp<=0)
			Dead();
	}
	
	public void Dead()
	{
		
	}

	public void Hit()
	{
		float value = 0.1f;
		myRend.material.SetFloat("_Intensity", value);
	}
}
