using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplinePointGenerator : MonoBehaviour 
{
	public List<Vector3> SplinePoints;
	public Transform FirstPoint;
	public Transform ForwardObject;
	private Vector3 ForwardDirection;
	public float AngularSpread = 85.0f;
	public float MinDistance = 10.0f;
	public float MaxDistance = 1000.0f;

	public GameObject test1;
	public GameObject test2;
	public GameObject test3;
	// Use this for initialization
	void Start () 
	{
		ForwardDirection = ForwardObject.forward;
		SplinePoints = new List<Vector3> ();
		SplinePoints.Add (FirstPoint.position);
		ForwardDirection.Normalize ();
		GenerateNextPoint ();
		GenerateNextPoint ();
		GenerateNextPoint ();

		test1.transform.position = SplinePoints [1];
		test2.transform.position = SplinePoints [2];
		test3.transform.position = SplinePoints [3];
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void GenerateNextPoint()
	{
		Vector3 last_point = SplinePoints [SplinePoints.Count - 1];

		float xz_angle = Random.Range (-1 * AngularSpread, AngularSpread);
		float yz_angle = Random.Range (-1 * AngularSpread, AngularSpread);
		float dist = Random.Range (MinDistance, MaxDistance);

		float fd_dist_proj = Mathf.Cos (Mathf.Deg2Rad * Mathf.Abs(xz_angle)) * dist;
		float over_dist_proj = Mathf.Sin (Mathf.Deg2Rad * Mathf.Abs(xz_angle)) * dist;

		Vector3 xz_pos = last_point + (ForwardDirection * fd_dist_proj);

		if(xz_angle > 0.0f)
		{
			Vector3 over_vec = Vector3.Cross(Vector3.up, ForwardDirection);
			over_vec.Normalize();
			xz_pos = xz_pos + (over_vec * over_dist_proj);
		}
		else if(xz_angle < 0.0f)
		{
			Vector3 over_vec = Vector3.Cross(Vector3.down, ForwardDirection);
			over_vec.Normalize();
			xz_pos = xz_pos + (over_vec * over_dist_proj);
		}

		float yz_arc_chord_len = 2 * Mathf.Sin (Mathf.Deg2Rad * Mathf.Abs(yz_angle)) * dist;
		float yz_unit_angle = 90.0f - ((180.0f - Mathf.Abs (yz_angle)) / 2);

		float z_len = Mathf.Sin (yz_unit_angle) * yz_arc_chord_len;
		float y_len = Mathf.Cos (yz_unit_angle) * yz_arc_chord_len;

		Vector3 yz_dir = xz_pos - last_point;
		yz_dir.Normalize ();

		Vector3 xyz_pos = xz_pos - (yz_dir * z_len);

		if(yz_angle > 0.0f)
		{
			Vector3 yover_vec = Vector3.Cross(yz_dir, Vector3.right);
			yover_vec.Normalize();
			xyz_pos = xyz_pos + (yover_vec * y_len);
		}
		else if(yz_angle < 0.0f)
		{
			Vector3 yover_vec = Vector3.Cross(yz_dir, Vector3.left);
			yover_vec.Normalize();
			xyz_pos = xyz_pos + (yover_vec * y_len);
		}

		Debug.Log (xyz_pos); 
		SplinePoints.Add(xz_pos);
	}
}
