using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour {

	public ParticleData[] particleData;

	[System.Serializable]
	public class ParticleData
	{
		public string name;
		public GameObject particle;
		public float duration;
	}
	public static ParticleManager instace;
	void Awake()
	{
		instace = this;
	}
	GameObject _GameObject;
	public void Play(string newName,Vector3 newPosition)
	{
		for (int i = 0; i < particleData.Length; i++) {
			if(particleData[i]!=null && newName == particleData[i].name)
			{
				_GameObject = UnityEngine.Object.Instantiate(particleData[i].particle,newPosition,Quaternion.identity) as GameObject;
				print(_GameObject.name);
				Destroy(_GameObject,particleData[i].duration);
				return;
			}
		}
		print ("Particle Not Found: "+newName);
	}
}
