using UnityEngine;
using System.Collections;

public class PoolingManager : MonoBehaviour {
	public PoolingPrefab[] bulletPrefabs;
	public PoolingPrefab[] enemyPrefabs;	
	public PoolingSystem<Bullet>[] bulletPooling;
	public PoolingSystem<EnemyAI> enemyPooling = new PoolingSystem<EnemyAI>();
	
	public static PoolingManager instance;
	
	void Awake()
	{
		instance = this;
	}

	public IEnumerator Initialize()
	{
		bulletPooling = new PoolingSystem<Bullet>[bulletPrefabs.Length];
		
		for(int i = 0; i<bulletPrefabs.Length;i++)
		{
			bulletPooling[i] = new PoolingSystem<Bullet>();
			yield return StartCoroutine(bulletPooling[i].IEAddGameObjectsToPool(bulletPrefabs[i]));
		}
		
		for(int j = 0; j<enemyPrefabs.Length;j++)
		{
			yield return StartCoroutine(enemyPooling.IEAddGameObjectsToPool(enemyPrefabs[j]));
		}
	}
}
public enum BulletName
{
	MachineGun = 0,
	Cannon = 1,
	Purse_Laser = 2,
	Wave = 3,
	Laser_Beam = 4,
	Missile = 5,
	Holming_Missile = 6,
	Bomb = 7,
	Electric = 8,

}
