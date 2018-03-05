using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed = 15.0f;
	public float padding = 1f;
	public GameObject laserPrefab;
	public float laserSpeed;
	public float firingRate = 0.2f;
	public float health = 1000f;
	public AudioClip playerLaserSound;
	public AudioClip playerDiesSound;
	
	float xmin;
	float xmax;
	
	void Start(){
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
		xmin = leftMost.x + padding;
		xmax = rightMost.x - padding;
	}
	
	void Update () {
		MoveShip();
		FireLaser();
	}
	
	void MoveShip(){
	if(Input.GetKey(KeyCode.LeftArrow)){
		transform.position += Vector3.left * speed * Time.deltaTime;
		}
	else if (Input.GetKey(KeyCode.RightArrow)){
		transform.position += Vector3.right * speed * Time.deltaTime;
		}
		float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);	
	}
	
	void FireLaser(){
	if(Input.GetKeyDown(KeyCode.Space)){
		  InvokeRepeating("Fire", 0.000001f, firingRate);
		}
	if(Input.GetKeyUp(KeyCode.Space)){
		CancelInvoke("Fire");
		}
	}
	
	void Fire(){
	Vector3 offset = new Vector3(0, 1, 0);
		GameObject laser = Instantiate(laserPrefab, transform.position + offset, Quaternion.identity) as GameObject;
		laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, laserSpeed);
		AudioSource.PlayClipAtPoint(playerLaserSound, transform.position);
	}
	
	void OnTriggerEnter2D(Collider2D col){
		Projectile missile = col.gameObject.GetComponent<Projectile>();
		if(missile){
			health -= missile.GetDamage();
			missile.Hit();
			if (health <= 0){
				Die ();
			}
		}
	}
	
	void Die(){
		AudioSource.PlayClipAtPoint(playerDiesSound, transform.position);
		LevelManager man = GameObject.Find ("LevelManager").GetComponent<LevelManager>();
		man.LoadLevel("Win Screen");
		Destroy(gameObject);
	}
	
	
	
}
