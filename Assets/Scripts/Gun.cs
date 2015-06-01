using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public Transform submarine;

	public bool isEnemy;

	public float fireRate;
	
	public bool haveRound;
	public int shotPerRound;
	public float roundCooldown;

	public Transform GunPoint;

	public GameObject bulletPrefab;
	BulletName bulletName;

	float timer;
	float timerRound;
	int bulletLeftInRound;

//	Vector3 rotationOfBullet = new Vector3(0,90,0);

	void Start()
	{

	}

	void Update()
	{
		if (haveRound)		
			FireRound();
		else
			FireNormal();
	}

	void FireNormal()
	{
		timer += Time.deltaTime;
		if(timer>=fireRate)
		{
			CallBullet(bulletPrefab);
			timer = 0;
		}
	}


	void FireRound()
	{
		if(timerRound<roundCooldown)
			timerRound += Time.deltaTime;
		else if(timerRound>= roundCooldown)
		{
			if(bulletLeftInRound>0)
			{
				timer+=Time.deltaTime;
				if(timer>=fireRate)
				{
					CallBullet(bulletPrefab);
					bulletLeftInRound --;
					timer = 0;
				}
			}
			else
			{
				bulletLeftInRound = shotPerRound;
				timerRound = 0;
			}
		}
	}

	void CallBullet(GameObject bulletprefab)
	{
		switch(bulletprefab.name.Substring(1,1))
		{
		case "0": bulletName = BulletName.MachineGun;
			break;
		case "1": bulletName = BulletName.Cannon;
			break;
		case "2": bulletName = BulletName.Purse_Laser;
			break;
		case "3": bulletName = BulletName.Wave;
			break;
		case "4": bulletName = BulletName.Laser_Beam;
			break;
		case "5": bulletName = BulletName.Missile;
			break;
		case "6": bulletName = BulletName.Holming_Missile;
			break;
		case "7": bulletName = BulletName.Bomb;
			break;
		case "8": bulletName = BulletName.Electric;
			break;		
		}
		(PoolingManager.instance.bulletPooling[(int)bulletName].CallPooling(GunPoint.position,submarine.rotation)).isEnemy = isEnemy;
		//bullet.myName = bulletName;
		
		//print(bullet.name + (int)bulletName);
		/*
		Bullet bullet =  PoolingManager.instance.bulletPooling[(int)BulletName.Holming_Missile].CallPooling(GunPoint.position,submarine.rotation);
		bullet.myName = BulletName.Holming_Missile;
		
		print(bullet.name + (int)BulletName.Holming_Missile);
		*/

	}

}
