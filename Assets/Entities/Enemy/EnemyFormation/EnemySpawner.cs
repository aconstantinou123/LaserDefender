using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemyPrefab;
	public float width = 10f;
	public float height = 5f;
	public float speed = 5f;
	private bool movingRight = true;
	public float spawnDelay = 0.5f;
	
	float xmin;
	float xmax;

	// Use this for initialization
	void Start () {
		
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
		xmin = leftMost.x;
		xmax = rightMost.x;
		
		SpawnUntilFull();
	}
	
	public void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
	}
	
	// Update is called once per frame
	void Update () {
		MoveEnemyShips();
		if(AllMembersDead()){
			SpawnUntilFull();
		}
	}
	
	void MoveEnemyShips(){
		
		if(movingRight){
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		else{
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		float rightEdge = transform.position.x + 0.5f*width; 
		float leftEdge = transform.position.x - 0.5f*width; 
		if(rightEdge > xmax){
			movingRight = false;
		}
		else if (leftEdge < xmin){
			movingRight = true;
		}
	}
	
	bool AllMembersDead(){
		foreach(Transform childPositionGameObject in transform){
			if(childPositionGameObject.childCount > 0){
				return false;
			}
		}
		return true;
	}
	
	void SpawnEnemies(){
		foreach(Transform child in transform){
			GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}
	}
	
	void SpawnUntilFull(){
		Transform freePosition = NextFreePosition();
		if(freePosition){
			GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}
		if(NextFreePosition()) {
			Invoke("SpawnUntilFull", spawnDelay);
		}
	}
	
	Transform NextFreePosition(){
		foreach(Transform child in transform){
			if (child.childCount == 0){
				return child;
			}
		}
		return null;
	}
}
