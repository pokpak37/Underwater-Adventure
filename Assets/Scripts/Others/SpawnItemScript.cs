using UnityEngine;
using System.Collections;

public class SpawnItemScript : MonoBehaviour {
	public float size;
	public GameObject[] items;

	IEnumerator Start()
	{
		while (true) {
			yield return new WaitForSeconds (Random.Range (0, 15f));
			for (int i = 0; i < size/3; i++) {
				yield return new WaitForSeconds (Random.Range (0, 5f));
				GameObject game = Instantiate (items [Random.Range (0, items.Length)], new Vector3 (transform.position.x + Random.Range (-size / 2f, size / 2), transform.position.y + Random.Range (-size / 2f, size / 2), 0), Random.rotation) as GameObject;
				Destroy(game,30f);
			}
		}
	}
	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube (transform.position, new Vector3(size,size,0));
	}
}
