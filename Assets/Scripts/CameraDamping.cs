using UnityEngine;
using System.Collections;

public class CameraDamping : MonoBehaviour {

	public Transform target;
	public float smoothTime;
	private Vector3 velocity = Vector3.zero;
	Vector3 offset;
	Vector3 headToOffset = new Vector3(2.5f,0,0);
	PlayerControl targetScript;

	void Start()
	{
		offset = transform.position;
		targetScript = target.gameObject.GetComponent<PlayerControl>();
	}

	void FixedUpdate()
	{
		Vector3 targetPosition;
		if(targetScript.headToLeft == false)
		{
			targetPosition = target.position + offset - headToOffset;

		}
		else
		{
			targetPosition = target.position + offset + headToOffset;
		}
		//transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime*Time.deltaTime); //10
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.5f); //1.0
	}
}
