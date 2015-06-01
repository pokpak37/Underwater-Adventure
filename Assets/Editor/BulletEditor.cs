using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Bullet))]
public class BulletEditor : Editor
{
	public override void OnInspectorGUI()
	{
		Bullet myBullet =  (Bullet)target;

		//DrawDefaultInspector();
		myBullet.dmg = EditorGUILayout.IntSlider("Dmg per Bullet",myBullet.dmg,1,25);
		myBullet.speed = EditorGUILayout.Slider("start speed",myBullet.speed,-5f,15f);
		myBullet.haveTruster = EditorGUILayout.Toggle("Have Truster",myBullet.haveTruster);
		if(myBullet.haveTruster)
		{
			GUILayout.Label ("Truster Panel");
			myBullet.speedGainRate = EditorGUILayout.Slider("Acceleration",myBullet.speedGainRate,1f,10f);
			myBullet.speedMax = EditorGUILayout.Slider("Max Speed",myBullet.speedMax,5f,15f);
		}

		myBullet.isDrop = EditorGUILayout.Toggle("Is Drop",myBullet.isDrop);
		if(myBullet.isDrop)
		{
			GUILayout.Label ("Drop Panel");
			myBullet.dropSpeedGainRate = EditorGUILayout.Slider("Drop Acceleration",myBullet.dropSpeedGainRate,1f,10f);
			myBullet.dropspeedMax = EditorGUILayout.Slider("Max Drop Speed",myBullet.dropspeedMax,1f,5f);
			myBullet.noDropWhenReachMaxDropSpeed = EditorGUILayout.Toggle("Max Drop Speed",myBullet.noDropWhenReachMaxDropSpeed);
		}

		myBullet.isHolming = EditorGUILayout.Toggle("Is Holming",myBullet.isHolming);
		if(myBullet.isHolming)
		{
			GUILayout.Label ("Holming Panel");
			myBullet._torque = EditorGUILayout.Slider("Rotation Speed",myBullet._torque,1f,10f);

			myBullet.isLimitRange = EditorGUILayout.Toggle("Limit Range",myBullet.isLimitRange);
			if(myBullet.isLimitRange)
				myBullet.limitHolmingRange = EditorGUILayout.Slider("Limit Range Value",myBullet.limitHolmingRange,5f,15f);
		}

		myBullet.haveSubBullet = EditorGUILayout.Toggle("Sub Bullet is On",myBullet.haveSubBullet);
		if(myBullet.haveSubBullet)
		{
			GUILayout.Label ("Sub Bullet Panel");
			//myBullet.subBullet = EditorGUILayout.ObjectField("Subbullet Prefab",myBullet.subBullet.gameObject);
		}

			
		
		if (GUI.changed)
			EditorUtility.SetDirty (myBullet);
	}
}
