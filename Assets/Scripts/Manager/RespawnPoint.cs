using UnityEngine;
using System.Collections;

public class RespawnPoint : MonoBehaviour {


    public Transform[] respawnPointTransforms; 

	public static RespawnPoint instance;

	void Awake()
	{
		respawnPointTransforms = GetComponentsInChildren<Transform>();
		PlayerControl.instance.transform.position = respawnPointTransforms[Random.Range(1,1)].position;
	}

}
