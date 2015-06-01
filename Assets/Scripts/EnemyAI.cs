using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {


	Rigidbody _rigidbody;
	Transform _transform;

	public enum function
	{
		notFight, argressive, protective
	}
	public function AIFunction;

	public enum attackType
	{
		hit, shoot, pass
	}
	public attackType AIAttackType;
	public float protectionRange;
	public float DetectRange;
	public int hp = 15;
	public int hitDmg;

	Vector3 moveDirection;
	public bool isLockPlayer;
	public GameObject bullet;
	public float firerate;
	float moveSpeed;
	public float idelMoveSpeed;
	public float chaseMoveSpeed;
	public float rotateSpeed;

	public bool emitWhenDead;
	public GameObject emitObject;
	public int emitCount;
	     
	public float minDelay = 1.5f;
	public float maxDelay = 3f;
	float delay;
	public float timer;

	public GameObject myExplosionParticle;

	public Vector3 spawnPos;

	public enum stage 
	{
		idle,chase,retreat 
	}
	public stage AIStage;

	public float lineOfSightRange = 3;
	public float fieldOfViewAngle = 110f;    

	public Transform target;
	public Vector3 targetPos;

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody> ();
		_transform = transform;

	}
	
	void Start()
	{
		//x1 = this.gameObject.GetComponentInChildren<Animator>();
		AIStage = stage.idle;
		moveSpeed = idelMoveSpeed;
		//savePosition = transform.position;
		spawnPos = _transform.position;
		GetRandomPos();

	}
	public EnemyAI()
	{
		moveSpeed = 0.5f;
		rotateSpeed = 60f;

		//scorePerKill = 100;

	}
	
	public EnemyAI(float moveSpeed, float rotateSpeed)
	{
		this.moveSpeed = moveSpeed;
		this.rotateSpeed = rotateSpeed; 
	}

	void FixedUpdate()
	{
		switch(AIStage)
		{
		case stage.idle : StageIdle();
			break;
		case stage.chase : StageChase();
			break;
		case stage.retreat : StageRetreat();
			break;			
		default : StageIdle();
			break;
		} 
		transform.SetZ(0);
	}

	void StageIdle()
	{
		// move to anywhere in circle area around spawn point every 1.5-3.0 second
		timer += Time.fixedDeltaTime;

		switch(AIFunction)
		{
		case function.notFight : 
			break;
		case function.argressive: FindingTarget ();
			break;
		case function.protective: FindingTarget ();
			break;
		}

		if(Mathf.Abs(Vector2.Distance(_transform.position,targetPos))> 0.3f)
		{
			Quaternion targetRotation = Quaternion.LookRotation (targetPos - _transform.position);
			//_rigidbody.MoveRotation (Quaternion.RotateTowards (_transform.rotation, targetRotation, 5));
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
			_transform.Translate(Vector3.forward*moveSpeed*Time.deltaTime);
			//_rigidbody.AddForce (transform.forward * moveSpeed * 3);
		}

		if(timer>maxDelay)
		{
			GetRandomPos();
			timer = 0;
		}
	}

	void GetRandomPos()
	{ 
		if(AIFunction == function.protective)
		{
			targetPos = spawnPos;
			
			targetPos.x += Random.Range(-protectionRange,protectionRange);
			targetPos.y += Random.Range(-protectionRange,protectionRange);
			targetPos.z = 0;
		}
		else
		{
			targetPos = Random.insideUnitSphere * protectionRange;
			targetPos.z = 0;
			targetPos += transform.position;
		}
		delay = Random.Range(minDelay,maxDelay);
		
	}

	void StageChase()
	{
		// shoot at player or move toward player

		float distance = Mathf.Abs (Vector2.Distance (transform.position, target.position));
		Quaternion targetRotation = Quaternion.LookRotation (target.position - _transform.position);
		/*
		if(distance>3f)
		{
			target = null;
			Gun gun = GetComponentInChildren<Gun>();
			if(gun!=null)
				gun.enabled = false;
			AIStage = stage.retreat;

		}
		*/
		switch(AIAttackType)
		{
		case attackType.hit: 


			_transform.Translate(Vector3.forward*moveSpeed*Time.deltaTime);
			if(distance>DetectRange*1.5f)
			{
				AIStage = stage.retreat;
			}
			
			if(distance>protectionRange*2f)
			{
				AIStage = stage.retreat;
			}

			if(distance>2f)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
			}
			else
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed*3);
			}

			break;
		case attackType.pass:
			float yDis = target.position.y - _transform.position.y;
			if(target.transform.position.x > _transform.position.x)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), Time.fixedDeltaTime * rotateSpeed*2);
			}
			else
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), Time.fixedDeltaTime * rotateSpeed*2);
			}
			if(yDis > 0.3f)
			{
				_transform.Translate(Vector3.up*moveSpeed/5*Time.deltaTime);
			}
			else if( yDis < -0.3f)
			{
				_transform.Translate(Vector3.down*moveSpeed/5*Time.deltaTime);
			}
			else 
			{
				timer+=Time.deltaTime;
				if(timer>2)
					AIStage = stage.retreat;
			}
			break;
		case attackType.shoot:
			if(isLockPlayer)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
			}
			else
			{
				if(target.transform.position.x > _transform.position.x)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), Time.fixedDeltaTime * rotateSpeed);
				}
				else
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), Time.fixedDeltaTime * rotateSpeed);
				}

				timer += Time.fixedDeltaTime;
				if(timer>maxDelay)
				{
					targetPos = GetRandomPosForShoot();
					timer = 0;
				}
				float moveLeft = Mathf.Abs (Vector2.Distance (transform.position, targetPos));
				if(moveLeft>0.3f)
				{
					transform.Translate(moveDirection*moveSpeed/2*Time.deltaTime);
				}
			}

			if(distance>DetectRange*1.5f)
			{
				AIStage = stage.retreat;
			}
			else if(distance>DetectRange)
			{
				_transform.Translate(Vector3.forward*moveSpeed/5*Time.deltaTime);
			}
			else if(distance <DetectRange *0.5f)
			{
				_transform.Translate(Vector3.back*moveSpeed/5*Time.deltaTime);
			}
			
			if(distance>protectionRange*2f)
			{
				AIStage = stage.retreat;
			}

			break;
		}
	}
	Vector3 GetRandomPosForShoot()
	{
		Vector3 pos = transform.position;
			pos.y = target.position.y;
		if(pos.y > _transform.position.y)
		{
			moveDirection = Vector3.up;
		}
		else
		{
			moveDirection = Vector3.down;
		}
		return pos;
	}

	void StageRetreat()
	{
		float distance;
		Quaternion targetRotation;
		switch(AIAttackType)
		{
		case attackType.hit:
			distance = Mathf.Abs (Vector2.Distance (transform.position, spawnPos));
			targetRotation = Quaternion.LookRotation (spawnPos - _transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed*3);
			_transform.Translate(Vector3.forward*moveSpeed*1.3f*Time.deltaTime);
			
			if(distance<0.3f)
			{
				moveSpeed = idelMoveSpeed;
				AIStage = stage.idle;
			}
			break;
		case attackType.pass:
			_transform.Translate(Vector3.forward*moveSpeed*1.5f*Time.deltaTime);
			break;
		case attackType.shoot:
			distance = Mathf.Abs (Vector2.Distance (transform.position, spawnPos));
			targetRotation = Quaternion.LookRotation (spawnPos - _transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed*3);
			_transform.Translate(Vector3.forward*moveSpeed*1.3f*Time.deltaTime);
			
			if(distance<0.3f)
			{
				Gun[] gun = GetComponentsInChildren<Gun>();
				if(gun!=null)
				{
					foreach(Gun aaa in gun)
					{
						aaa.enabled = false;
					}
				}		
				moveSpeed = idelMoveSpeed;
				AIStage = stage.idle;
			}
			break;
		}

	}

	void FindingTarget()
	{
		bool getTarget = false;
		float distance = Mathf.Abs (Vector2.Distance (_transform.position, PlayerControl.instance.transform.position));


		if(distance<DetectRange)
			SeePlayer();		
		else if (distance > 20)
		{
			//Destruct();
		}
	}

	void Destruct()
	{
		//PoolingManager.instance.enemyPooling.ReturnToPool(this);
		
		if(emitWhenDead)
		{
			//Instantiate(subBullet,
		}
		Destroy(gameObject);
	}

	public void GetHit(int dmg)
	{
		hp -= dmg;
		print(name + " get hit : " + dmg + " HP : " + hp);
		if(hp<=0)
		{
			Instantiate(myExplosionParticle,_transform.position,Quaternion.identity);
			Destruct();
		}

		//Renderer rend = GetComponent<Renderer>();

		//rend.material.shader = Shader.Find("Custom/ToonSketch");
		
		//rend.material.SetColor ("Main Color", Color.blue);
	}
	public void SeePlayer()
	{
		switch(AIAttackType)
		{
		case attackType.hit :				
			break;
		case attackType.shoot :		
			Gun[] gun = GetComponentsInChildren<Gun>();
			if(gun!=null)
			{
				foreach(Gun aaa in gun)
				{
					aaa.enabled = true;
					aaa.isEnemy = true;
				}
			}	
			break;
		case attackType.pass :				
			break;
		}
		print ("chasing ");
		target = PlayerControl.instance.transform;
		moveSpeed = chaseMoveSpeed;
		AIStage = stage.chase;
	}
}
