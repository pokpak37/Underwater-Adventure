using UnityEngine;
using System.Collections;

public class RandomAnimation : MonoBehaviour {

	// Use this for initialization
	Animator[] _animators;
	void Start () {
		_animators = GetComponentsInChildren<Animator> ();
		for (int i = 0; i < _animators.Length; i++) {
			_animators[i].speed *= Random.Range (0.5f, 1.5f);
			_animators[i].Play ("Idle", 0, Random.Range (0f, 1f));
		}
		Destroy (this);
	}
}
