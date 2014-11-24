using UnityEngine;
using System.Collections;

public class CharacterMover : MonoBehaviour 
{
	public Transform ForwardRig;
	public int Speed;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.transform.Translate(ForwardRig.forward * Speed * Time.deltaTime );
	}
}
