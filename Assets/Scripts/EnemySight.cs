using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {

	private EnemyAI hostAi;
	private SphereCollider col;                   
	public GameObject player;                     
	private int playerHealth;


	void Awake () {

		hostAi = GetComponentInParent<EnemyAI>();
		col = GetComponent<SphereCollider>();
		player = PlayerControl.instance.gameObject;
		playerHealth = PlayerControl.instance.hp;
		col.radius = hostAi.lineOfSightRange;	
	}
	


	void OnTriggerStay (Collider other)
	{
		// casting Line of Sight
		if(hostAi.AIStage == EnemyAI.stage.idle)
		{
			if(other.gameObject == player)
			{
				Vector3 direction = other.transform.position - transform.position;
				float angle = Vector3.Angle(direction, transform.forward);
				
				if(angle < hostAi.fieldOfViewAngle * 0.5f)
				{
					RaycastHit hit;

					Debug.DrawRay(transform.position, direction.normalized,Color.green);
					if(Physics.Raycast(transform.position, direction.normalized, out hit, col.radius))
					{
						if(hit.collider.gameObject == player)
						{
							hostAi.SeePlayer();
						}
					}
				}
			}
		}
	}
}
