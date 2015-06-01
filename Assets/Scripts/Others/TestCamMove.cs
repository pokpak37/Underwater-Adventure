using UnityEngine;
using System.Collections;

public class TestCamMove : MonoBehaviour {

	public float speed;

	Vector3 dir;
		void Update() {
		dir= new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0 );
		//dir= transform.TransformDirection(dir);

		transform.Translate (dir*speed);

		}
}
