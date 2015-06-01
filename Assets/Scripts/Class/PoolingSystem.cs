using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PoolingSystem<T> where T :Component {

	[NonSerialized]public List<T> active = new List<T>();
	[NonSerialized]public List<T> inactive= new List<T>();

	public IEnumerator IEAddGameObjectsToPool(PoolingPrefab newPoolingPrefab)
	{
		GameObject initGameobject;
		for (uint i = 0; i < newPoolingPrefab.max; i++) {
			yield return initGameobject =UnityEngine.Object.Instantiate (newPoolingPrefab.prefab) as GameObject;
			initGameobject.SetActive(false);
			initGameobject.transform.parent = newPoolingPrefab.parent;
			inactive.Add(initGameobject.GetComponent<T>());
		}
	}
	public T CallPooling(Vector3 newPosition,Quaternion newRotation)
	{
		if(inactive.Count <= 0)
			return null;

		T selected;

		int selectedIndex = inactive.Count-1;
		selected = inactive[selectedIndex];
		inactive.RemoveAt(selectedIndex);
		active.Add(selected);

		selected.gameObject.SetActive(true);
		selected.gameObject.transform.position = newPosition;
		selected.gameObject.transform.rotation = newRotation;
		return selected;
	}
	public void ReturnToPool(T newCompoent)//overload
	{
		active.Remove(newCompoent);
		inactive.Add(newCompoent);
		newCompoent.gameObject.SetActive(false);
		//newGameObject.transform.localPosition = Vector3.zero;
	}
	public void ReturnToPool(int newIndex)//overload
	{
		inactive.Add(active[newIndex]);
		active[newIndex].gameObject.SetActive(false);
		active.RemoveAt(newIndex);
		//newGameObject.transform.localPosition = Vector3.zero;
	}

}
//Input Monster
[Serializable]
public class PoolingPrefab
{
	public GameObject prefab;
	public int max;
	public Transform parent;
};
