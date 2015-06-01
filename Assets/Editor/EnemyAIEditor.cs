using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(EnemyAI))]
public class EnemyAIEditor : Editor
{
	public override void OnInspectorGUI()
	{
		EnemyAI myEnemy =  (EnemyAI)target;

		DrawDefaultInspector();
		/*
		myEnemy.dmg = EditorGUILayout.IntSlider("Dmg per Bullet",myEnemy.dmg,1,25);
		myEnemy.speed = EditorGUILayout.Slider("start speed",myEnemy.speed,-5f,15f);
		myEnemy.haveTruster = EditorGUILayout.Toggle("Have Truster",myEnemy.haveTruster);
		if(myEnemy.haveTruster)
		{
			GUILayout.Label ("Truster Panel");
			myEnemy.speedGainRate = EditorGUILayout.Slider("Acceleration",myEnemy.speedGainRate,1f,10f);
			myEnemy.speedMax = EditorGUILayout.Slider("Max Speed",myEnemy.speedMax,5f,15f);
		}

		myEnemy.isDrop = EditorGUILayout.Toggle("Is Drop",myEnemy.isDrop);
		if(myEnemy.isDrop)
		{
			GUILayout.Label ("Drop Panel");
			myEnemy.dropSpeedGainRate = EditorGUILayout.Slider("Drop Acceleration",myEnemy.dropSpeedGainRate,1f,10f);
			myEnemy.dropspeedMax = EditorGUILayout.Slider("Max Drop Speed",myEnemy.dropspeedMax,1f,5f);
			myEnemy.noDropWhenReachMaxDropSpeed = EditorGUILayout.Toggle("Max Drop Speed",myEnemy.noDropWhenReachMaxDropSpeed);
		}

		myEnemy.isHolming = EditorGUILayout.Toggle("Is Holming",myEnemy.isHolming);
		if(myEnemy.isHolming)
		{
			GUILayout.Label ("Holming Panel");
			myEnemy._torque = EditorGUILayout.Slider("Rotation Speed",myEnemy._torque,1f,10f);

			myEnemy.isLimitRange = EditorGUILayout.Toggle("Limit Range",myEnemy.isLimitRange);
			if(myEnemy.isLimitRange)
				myEnemy.limitHolmingRange = EditorGUILayout.Slider("Limit Range Value",myEnemy.limitHolmingRange,5f,15f);
		}

		myEnemy.haveSubBullet = EditorGUILayout.Toggle("Sub Bullet is On",myEnemy.haveSubBullet);
		if(myEnemy.haveSubBullet)
		{
			GUILayout.Label ("Sub Bullet Panel");
			//myBullet.subBullet = EditorGUILayout.ObjectField("Subbullet Prefab",myBullet.subBullet.gameObject);
		}

			*/
		
		if (GUI.changed)
			EditorUtility.SetDirty (myEnemy);
	}
}
