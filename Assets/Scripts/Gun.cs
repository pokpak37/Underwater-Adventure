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

	public BulletName bulletName;

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
			CallBullet(bulletName);
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
					CallBullet(bulletName);
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

	void CallBullet(BulletName bulletName)
	{
		(PoolingManager.instance.bulletPooling[(int)bulletName].CallPooling(GunPoint.position,submarine.rotation)).isEnemy = isEnemy;

	}

    public void LevelUp()
    {

    }

}
