using UnityEngine;
using System.Collections;

public class TestCoroutine : MonoBehaviour {

	public void StartIdle()
	{
		StartCoroutine(Idle());
		transform.SetX(0);
	}
	IEnumerator Start()
	{
		StartCoroutine(State());
		yield return null;
	}

	IEnumerator State()
	{
		//Start

		bool condition =true;
		if(1==1)
			condition = 0==1;

		while(condition)
		{
			yield return null;
		}
		//Action
		yield return StartCoroutine(Action());
		//End
		while(true)
		{
			yield return null;
		}
		gameObject.SetActive(false);
	}


	IEnumerator Action(){
		IEnumerator _move = Move();
		IEnumerator _atk = Atk();

		for (float t = 0; t < 2; t+=Time.deltaTime) 
		{
			yield return _move.MoveNext();
			yield return _atk.MoveNext();
		}
	}
	IEnumerator Move()
	{
		while(true)
		{
			yield return null;
		}
	}
	IEnumerator Atk()
	{
		while(true)
		{

			yield return null;
		}
	}
	IEnumerator Count()
	{
			for (float t = 0; t < 2; t+=Time.deltaTime) {
				yield return null;
			}
			print("ta");
	}
	
	public IEnumerator Idle()
	{
			while(transform.position.x < 2)
			{
				transform.Translate(Vector3.right *Time.deltaTime);
				yield return null;
			}
			yield return StartCoroutine(Atk());
	}













}
