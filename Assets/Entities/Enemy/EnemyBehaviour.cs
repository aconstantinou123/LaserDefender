using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

	public float health = 200f;
	public GameObject laserPrefab;
	public float laserSpeed = 10f;
	public float firingRate = 10f;
	public float shotsPerSecond = 0.5f;
	public int points = 150;
	private ScoreKeeper scoreKeeper;
	public AudioClip enemyLaserSound;
	public AudioClip enemyDiesSound;
	
	void Start(){
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
	}
	
	void Update(){
		FireLasersRandom();
	}
	
	void EnemyFireLaser(){
		Vector3 startPosition = transform.position + new Vector3(0f, -1f, 0f);
		GameObject laser = Instantiate(laserPrefab, startPosition, Quaternion.identity) as GameObject;
		laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -laserSpeed);
		AudioSource.PlayClipAtPoint(enemyLaserSound, transform.position);
	}
	
	void FireLasersRandom(){
		float probablity = Time.deltaTime * shotsPerSecond;
		if(Random.value < probablity){
			EnemyFireLaser();
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		Projectile missile = col.gameObject.GetComponent<Projectile>();
		if(missile){
			health -= missile.GetDamage();
			missile.Hit();
			if (health <= 0){
				Destroy(gameObject);
				scoreKeeper.Score(points);
				AudioSource.PlayClipAtPoint(enemyDiesSound, transform.position);
				
			}
		}
	}
}
