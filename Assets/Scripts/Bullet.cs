using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	Rigidbody _rigidbody;
	Transform _tranform;

	public bool isEnemy;

	public BulletName myName;
	public float speed;
	float startSpeed;
	public int dmg;

	public bool haveTruster;
	public float speedGainRate;
	public float speedMax;

	public bool isDrop;
	public float dropSpeed;
	float startDropSpeed;
	public float dropSpeedGainRate;
	public float dropspeedMax;
	public bool noDropWhenReachMaxDropSpeed;

	public bool isHolming;
	public Transform target;
	public float _torque = 5f;
	public bool isLimitRange;
	public float limitHolmingRange;

	public bool isBeam;

	public bool isNoCollision;
	
	public bool haveSubBullet;
	public GameObject subBullet;

	public enum EmitWhen{ Hit, CountDown}	
	public EmitWhen emitWhen;
	public float counDownTime;

	public GameObject myParticle;
	public GameObject rockParticle;

	float timer;
	float timeLimit = 6f;


	public Bullet()
	{
		speed = 10f;
		dmg = 1;
	}


	public Bullet(float Speed, int Dmg)
	{
		speed = Speed;
		dmg = Dmg;
	}

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody> ();
		startSpeed = speed;
	}

	public void Move()
	{
		transform.Translate(Vector3.forward*speed*Time.deltaTime);
		//if (isHolming)
		//	Holming ();
		if(isDrop)
			Drop();
	}

	public void MoveTruster()
	{
		if(speed<speedMax)
			speed += speedGainRate * Time.deltaTime;
		transform.Translate(Vector3.forward*speed*Time.deltaTime);
		//if (isHolming)
		//	Holming ();
		if(isDrop)
			Drop();

	}

	public void Holming()
	{			
		float distance = 20f;
			
		foreach (GameObject xxx in GameObject.FindGameObjectsWithTag("Enemy")) 
		{ 				
			float diff = (xxx.transform.position - transform.position).sqrMagnitude;   
			if(isLimitRange)
			{
				if(diff<limitHolmingRange)
				{
					if (diff < distance) 
					{					
						distance = diff;
						target = xxx.transform;
					}
				}/*
				if(target!=null)
				{
					float difffff = (target.transform.position - transform.position).sqrMagnitude;   
					if(difffff>limitHolmingRange||difffff<limitHolmingRange/3)
					{
						target = null;
					}
				}*/
			}
			else
			{
				if (diff < distance) 
				{					
					distance = diff;
					target = xxx.transform;
				}
			}
		}		

		if (target == null)
			return;		
		/*	
		Quaternion targetRotation = Quaternion.LookRotation (target.position - transform.position);
		//_rigidbody.MoveRotation (Quaternion.RotateTowards (transform.rotation, targetRotation, _torque));
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * _torque);
		*/

		Vector3 targetDir = target.position - transform.position;
		float step = _torque * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
		newDir.z = 0;
		transform.rotation = Quaternion.LookRotation(newDir);
		/*
		Vector3 qqq = targetRotation.eulerAngles;

		//qqq.y = 90;

		Vector3 aaa = transform.rotation.eulerAngles;

		transform.Rotate (Vector3.right,_torque);
		transform.eulerAngles = Vector3.RotateTowards(aaa,qqq,_torque,1);

		//transform.eulerAngles = Vector3.Lerp(aaa,qqq,1);
			*/
	}

	public void Drop()
	{
		if(dropSpeed<dropspeedMax)
			dropSpeed += dropSpeedGainRate * Time.deltaTime;
		else 
		{
			if (isHolming&&timer<3f)
				Holming ();
			if(noDropWhenReachMaxDropSpeed)
				return;
		}

		transform.Translate(Vector3.down*dropSpeed*Time.deltaTime);
	}



	void OnTriggerEnter(Collider other)
	{
		switch(other.tag)
		{
		case "Player": 
			if(isEnemy)
			{
				PlayerControl.instance.GetComponentInChildren<Animator>().Play("Hit");
				PlayerControl.instance.GetHit(dmg);
				Destruct();
			}

			break;
		case "Bullet":
			break;
		case "Enemy":
			if(!isEnemy)
			{
				EnemyAI enemy = other.GetComponent<EnemyAI>();
				enemy.GetHit(dmg);
				Destruct();
			}
			//if(!isNoCollision)
			//{
				
			//}
			break;
		case "Ground": Destruct();
			break;
		default : 
	

				//if(!isNoCollision)				
					
			break;
		}
	}


	void Destruct()
	{
		timer = 0;
		speed = startSpeed;
		dropSpeed = startDropSpeed;
		target = null;
		if(haveSubBullet&&emitWhen == EmitWhen.Hit)
		{
			//Instantiate(subBullet,
		}
		PoolingManager.instance.bulletPooling[(int)myName].ReturnToPool(this);
		

	}

	void Update()
	{
		timer+= Time.fixedDeltaTime;
		if(timer>timeLimit)
			Destruct();

		if(haveTruster)
			MoveTruster();
		else
			Move ();
	}

}
