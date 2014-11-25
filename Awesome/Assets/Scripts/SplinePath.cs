using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplinePath : MonoBehaviour 
{
	public SplinePointGenerator ControlPoints;

	public int i_StepSize = 1000;
	public float f_Velocity = 0.5f;

	public List<Vector3> l_SplinePoints;
	private Matrix4x4 m_SplineBasis;
	private int i_IndexSolved;

	// Use this for initialization
	void Start () 
	{
		i_IndexSolved = 3;
		l_SplinePoints = new List<Vector3>();

		//Set up basis matrix
		Vector4 v4_Row0 = new Vector4(-1 * f_Velocity, 2 - f_Velocity,
		                              f_Velocity - 2, f_Velocity);
		Vector4 v4_Row1 = new Vector4(2 * f_Velocity, f_Velocity - 3,
		                              3 - (2 * f_Velocity), -1 * f_Velocity);
		Vector4 v4_Row2 = new Vector4(-1 * f_Velocity, 0, f_Velocity, 0);
		Vector4 v4_Row3 = new Vector4(0, 1, 0, 0);
		m_SplineBasis.SetRow(0, v4_Row0);
		m_SplineBasis.SetRow(1, v4_Row1);
		m_SplineBasis.SetRow(2, v4_Row2);
		m_SplineBasis.SetRow(3, v4_Row3);

		//Transposing Matrix because of Column Major
		m_SplineBasis = m_SplineBasis.transpose;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Won't Be rearranging splien points to recalculate
		if (ControlPoints.SplinePoints.Count - 1 > i_IndexSolved) 
		{
			CalculateSplinePoints();
		}

		for(int i = 0; i < l_SplinePoints.Count - 1; i++)
		{
			Debug.DrawLine(l_SplinePoints[i], l_SplinePoints[i + 1], Color.yellow);
		}

		if(Input.GetKeyDown(KeyCode.A))
		{
			CalculateSplinePoints();
		}
	}

	public void CalculateSplinePoints()
	{
		//Gonna assume there are enough CPs
		for(int i_CurCPIndex = i_IndexSolved + 1; i_CurCPIndex < ControlPoints.SplinePoints.Count; i_CurCPIndex++)
		{
			
			Vector4 v4_XCPs = new Vector4(ControlPoints.SplinePoints[i_CurCPIndex - 3].x,
			                              ControlPoints.SplinePoints[i_CurCPIndex - 2].x,
			                              ControlPoints.SplinePoints[i_CurCPIndex - 1].x,
			                              ControlPoints.SplinePoints[i_CurCPIndex].x);
			
			Vector4 v4_YCPs = new Vector4(ControlPoints.SplinePoints[i_CurCPIndex - 3].y,
			                              ControlPoints.SplinePoints[i_CurCPIndex - 2].y,
			                              ControlPoints.SplinePoints[i_CurCPIndex - 1].y,
			                              ControlPoints.SplinePoints[i_CurCPIndex].y);
			
			Vector4 v4_ZCPs = new Vector4(ControlPoints.SplinePoints[i_CurCPIndex - 3].z,
			                              ControlPoints.SplinePoints[i_CurCPIndex - 2].z,
			                              ControlPoints.SplinePoints[i_CurCPIndex - 1].z,
			                              ControlPoints.SplinePoints[i_CurCPIndex].z);
			
			for(int i = 0; i < i_StepSize; i++)
			{
				float u = ((float)i)/i_StepSize;
				Vector4 v4_UVec = new Vector4(Mathf.Pow(u, 3), Mathf.Pow(u, 2), u, 1);
				
				Vector4 v4_UBasis = m_SplineBasis * v4_UVec;
				
				Vector3 v3_CurPos = new Vector3(Vector4.Dot(v4_UBasis, v4_XCPs), Vector4.Dot(v4_UBasis, v4_YCPs),
				                                Vector4.Dot(v4_UBasis, v4_ZCPs));
				
				l_SplinePoints.Add(v3_CurPos);
			}
		}
		i_IndexSolved = ControlPoints.SplinePoints.Count - 1;
		
	}
}
