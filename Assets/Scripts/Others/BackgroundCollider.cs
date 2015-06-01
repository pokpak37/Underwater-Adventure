using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundCollider: MonoBehaviour {

	[HideInInspector]public Transform[] points;
	public string colliderName;
	public bool closedLoop;
	void  OnDrawGizmos()
	{
		points = GetComponentsInChildren<Transform> ();
		List<Transform> pointList = new List<Transform> (points);
		pointList.RemoveAt (0);

		float minDist = 0;
		int minIndex = 1;

		Vector3 startPos = pointList [0].position;
		pointList.RemoveAt (0);

		for (int i = 1; i < points.Length-1; i++)
		{
			for (int j = 0; j < pointList.Count;j++) {
				float dist = Vector3.Distance(startPos,pointList[j].position);
				if(j ==0)
				{
					minDist = dist;
					minIndex = j;
				}else if(dist < minDist )
				{
					minDist = dist;
					minIndex = j;
				}
			}
			points[i+1] = pointList[minIndex];
			startPos = pointList[minIndex].position;
			pointList.RemoveAt(minIndex);
		}




		for (int i = 1; i < points.Length; i++) {
			points[i].name = "Point"+i.ToString("00");
			points[i].SetZ(0);
			if(i < points.Length-1)
				Debug.DrawLine(points[i].position,points[i+1].position,Color.red); 
		}
	}
}
