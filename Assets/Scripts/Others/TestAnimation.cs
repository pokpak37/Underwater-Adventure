using UnityEngine;
using System.Collections;

public class TestAnimation : MonoBehaviour {

	// Update is called once per frame
	IEnumerator Start(){
		Animator ani = GetComponent<Animator> ();
		while (true) {
			ani.CrossFade ("Idle",0.1f);
			while (!Input.GetKeyDown (KeyCode.Space)) {
				yield return null;
			}
			ani.Play ("Hit",0,Random.Range(0,1f));
			yield return new WaitForSeconds(0.5f);
		}
	}
}
