using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {
	public GameObject[] obstacle; //prefab being spawned
	//public float spawntime; //time it takes to spawn
	public float spawnMin = 1f; //how fast to spawn. we wil randomly spawn between 1 and 2 seconds
	public float spawnMax = 2f;

	// Use this for initialization
	void Start () 
	{
		Spawn ();
	}
	
	// Update is called once per frame
	void Spawn () //function called spawn
	{
		Instantiate(obstacle[Random.Range (0, obstacle.Length)], transform.position, Quaternion.identity); //randomly picks between obstacles we provide to spawn. with no rotation.
		Invoke ("Spawn", Random.Range (spawnMin, spawnMax)); //after some random time between spawn min and spawn max it will spawn again
	}
}


