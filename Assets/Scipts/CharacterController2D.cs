using UnityEngine;
using System.Collections.Generic;
using System.Text;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour {
	
//	public float slope = 0.3f;
//	public float stepHeight = 0.1f;
	public float maxGroundSpeed = 6;
	public float maxAirSpeed = 5;
	public float acceleration = 2;
//	public float friction = 10;
//	public float minJumpHeight = 5;
//	public float maxJumpHeight = 7;
	public float jumpSpeed = 10;
//	public bool doubleJump = false;
	public float gravityScale = 1;
	public int overShotTime = 7;
	List<Vector3> btTargets = new List<Vector3> ();
	public LayerMask passThroughPlatforms;


	private bool grounded = false;
//	float horizontal = 0;
	private float speed = 0;
	private Vector2 motionDir = new Vector2 ();
	private bool jump = false;
	private int aquireNewTarget = 0;
//	bool midJump = false;
//	float minTargetJump = 0;
//	float maxTargetJump = 0;
	private CameraController cameraController;

	// Use this for initialization
	void Start ()
	{
		if (GetComponent<Collider2D>() == null || GetComponent<Collider2D>().enabled == false) {
			Collider2D[] childrenColliders = GetComponentsInChildren<Collider2D> (false);
			bool activeCollider = false;
			if (childrenColliders.Length > 0) {
				foreach (Collider2D c in childrenColliders) {
					activeCollider = activeCollider | c.enabled;
				}
			}
			if (!activeCollider) {
				Debug.LogError (this.ToString () + " Error:\n" + "There are no active colliders on this object.");
			}
		}
		GetComponent<Rigidbody2D>().gravityScale = gravityScale;
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		GetComponent<Rigidbody2D>().mass = 1;
	}

	// Update is called once per frame
	// Handle User Input Here
	void Update ()
	{
		if (Input.GetButtonDown ("Horizontal")) {//Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)) {
			speed += Input.GetAxisRaw ("Horizontal") * acceleration;
			speed = Mathf.Clamp(speed, 0, maxGroundSpeed);

		}
//		horizontal = Input.GetAxis("Horizontal");
		jump = Input.GetButton ("Jump");

		if (Input.GetMouseButtonDown (0) && GameManager.bulletTime) {
			GameObject target = Utilities.GetMouseTarget2D (Utilities.TargetLayer);
			if (target != null) {
				
				Debug.Log ("Target Hit: " + target.ToString());
				bool add = true;
				foreach (Vector3 v in btTargets) {
					if (v == target.transform.position) add = false;
				}
				if (add) {
					btTargets .Add (target.transform.position);
					Debug.Log ("Target Hit: " + target.ToString());
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
		if (Input.GetKeyDown(KeyCode.Delete)){
			Application.LoadLevel(0);
		}
	}

	// FixedUpdate is called once per physics frame
	// Hand motion here
	void FixedUpdate ()
	{
		if (!GameManager.bulletTime && btTargets.Count > 0) {
			CameraTracking(false, false);
			if (aquireNewTarget == 0) {
				Vector3 velocity = (btTargets [0] - transform.position).normalized * maxAirSpeed;
				GetComponent<Rigidbody2D>().velocity = velocity;
				if ((velocity * Time.fixedDeltaTime).magnitude > (btTargets [0] - transform.position).magnitude) {
					btTargets.RemoveAt (0);
					aquireNewTarget = overShotTime;
					if (btTargets.Count == 0){
						CameraTracking(true, true);
					}
				}
			} else if (aquireNewTarget > 0) {
				aquireNewTarget--;
			} else
				aquireNewTarget = 0;
		} else {
			Vector2 vel = GetComponent<Rigidbody2D>().velocity;
			if (grounded) {
				CameraTracking(true, false);
				//			if (horizontal == 0 || Mathf.Sign(vel.x) != Mathf.Sign(horizontal)){
				//				float frictionAcc = friction*Time.fixedDeltaTime;
				//				vel.x = Mathf.Abs(vel.x)>frictionAcc ? vel.x-Mathf.Sign(vel.x)*frictionAcc : 0;
				//			}
				if (vel.x < 0) {
					vel.x = 0;
					speed = 0;
				}
				Vector2 direction = vel.sqrMagnitude > 0 ? vel.normalized : motionDir;
				vel = direction * speed;
			
				if (jump) {
					vel.y = jumpSpeed;
					//				minTargetJump = rigidbody2D.position.y + minJumpHeight;
					//				maxTargetJump = rigidbody2D.position.y + maxJumpHeight;
					//				midJump = true;
				}
			}
			
			//		if (midJump) {
			//			if (rigidbody2D.position.y < minTargetJump || (jump && rigidbody2D.position.y < maxTargetJump)) {
			//				vel.y = jumpSpeed;
			//			} else
			//				midJump = false;
			//		}
			
			jump = false;
			GetComponent<Rigidbody2D>().velocity = vel;
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{
//		float lowerbound = GetLowerBound();
//		Debug.Log ("Lowerbound = " + lowerbound);
//		foreach (ContactPoint2D c in col.contacts) {
//			Debug.Log ("Contact Point Y = " + c.point.y);
//			if (c.point.y <= lowerbound){
//				grounded = true;
//			}
//		}
	}

	void OnCollisionStay2D (Collision2D col)
	{
//		float lowerbound = GetLowerBound ();
		foreach (ContactPoint2D c in col.contacts) {
			if (Vector2.Angle(c.normal, Vector2.up) <= 60) {
				grounded = true;
				motionDir = new Vector2(c.normal.y, -c.normal.x);
			}

		}
//		if (col.contacts.Length > 1){
//			Vector2 point1 = col.contacts[0].point;
//			Vector2 point2 = col.contacts[1].point;
//			if (point1.x != point2.x && point1.y == point2.y && point2.y <= lowerbound){
//				grounded = true;
//			}
//
////			Debug.Log(transform.InverseTransformPoint(cp.point));
//		}
//		if (col.contacts.Length > 2){ Debug.LogWarning ("More than two collision points for " + this.ToString());}
	}

	void OnCollisionExit2D (Collision2D col)
	{
//		float lowerbound = GetLowerBound ();
		foreach (ContactPoint2D c in col.contacts) {
			if (Vector2.Angle(c.normal, Vector2.up) <= 60) {
				grounded = false;
				motionDir = new Vector2();
			}
		}
//		if (col.contacts.Length > 1){
//			float lowerbound = GetLowerBound();
//			Vector2 point1 = col.contacts[0].point;
//			Vector2 point2 = col.contacts[1].point;
//			if (point1.x != point2.x && point1.y == point2.y && point2.y < lowerbound){
//				grounded = false;
//			}
//			
//			//			Debug.Log(transform.InverseTransformPoint(cp.point));
//		}
//		if (col.contacts.Length > 2){ Debug.LogWarning ("More than two collision points for " + this.ToString());}
//		foreach (ContactPoint2D cp in col.contacts){
//			Debug.Log("Exit " + Time.frameCount + " " + transform.InverseTransformPoint(cp.point) + "\n" +
//			          col.contacts.Length + " " + transform.InverseTransformPoint(cp.point).x + " " + transform.InverseTransformPoint(cp.point).y);
//		}
	}

	public void SetCamera (CameraController _cc)
	{
		cameraController = _cc;
	}

	public void CameraTracking(bool _x, bool _y){
		cameraController.TrackX = _x;
		cameraController.TrackY = _y;
	}

	float GetLowerBound ()
	{
		float lowerbound = 0;
		if (GetComponent<Collider2D>() != null) {
			lowerbound = GetComponent<Collider2D>().bounds.center.y - GetComponent<Collider2D>().bounds.extents.y * 0.99f;
		}
		foreach (Collider2D c in transform.GetComponentsInChildren<Collider2D>(false)) {
			float testLB = c.bounds.center.y - c.bounds.extents.y * 0.99f;
			lowerbound = Mathf.Min (testLB, lowerbound);
		}
		return lowerbound;
	}


}
