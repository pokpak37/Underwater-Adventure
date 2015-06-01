using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(BackgroundCollider))]
public class BackgroundColliderEditor  :Editor {
	
	BackgroundCollider backgroundCollider;
	Mesh mesh;

	void OnEnable()
	{
		backgroundCollider = (BackgroundCollider)target;
	}
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		if (backgroundCollider.colliderName =="")
			return;
		if (GUILayout.Button ("Save Collider")) {
			GenerateCollider();
			EditorUtility.DisplayProgressBar("Wait For Rinlom", "Save Assets",1f);
			AssetDatabase.CreateAsset(mesh,"Assets/Models/"+backgroundCollider.colliderName+".asset");
			AssetDatabase.SaveAssets();
			EditorUtility.ClearProgressBar();
		}
	}
	void GenerateCollider()
	{
		MeshCollider meshCollider = backgroundCollider.GetComponent<MeshCollider>();
		Transform[] points = backgroundCollider.points;


		Vector3[] vertices = new Vector3[(points.Length-1)*2];
		int k = 0;
		for (int i = 1; i <  points.Length; i++) {
			for (int l = 0; l < 2; l++) {
				EditorUtility.DisplayProgressBar("Wait For Rinlom", "Generate Vertices ("+ k+"/"+vertices.Length+")",(float)k/vertices.Length);
				vertices[k] = new Vector3( points[i].position.x,points[i].position.y,l==0?-2:2);
				k++;
			}
		}

		int loopLength = 0;
		if (backgroundCollider.closedLoop)
			loopLength = 6;

		int[] triangles = new int[(vertices.Length-2 )* 3 +loopLength];
		
		int j = 2;
		for (int i = 0; i < triangles.Length-loopLength; i++) {
			EditorUtility.DisplayProgressBar("Wait For Rinlom", "Generate Triangles ("+i+"/"+triangles.Length+")",(float)i/triangles.Length);
			if(i%3 ==0)
				j-=2;
			triangles [i] =j;
			j++;
		}
		if (backgroundCollider.closedLoop) {
			triangles [triangles.Length - 1] = 1;
			triangles [triangles.Length - 2] = triangles [triangles.Length - 8];
			triangles [triangles.Length - 3] = triangles [triangles.Length - 7];
			triangles [triangles.Length - 4] = 1;
			triangles [triangles.Length - 5] = 0;
			triangles [triangles.Length - 6] = triangles [triangles.Length - 8];
		}


		mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles =  triangles;
		meshCollider.sharedMesh = mesh;
	}
}
