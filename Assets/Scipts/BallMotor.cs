using UnityEngine;
using System.Collections.Generic;

public class BallMotor : MonoBehaviour {
	public float maxRotation = 10.0f;
	public float acceleration = 5;
	public float jumpHeight = 5;
	public float maxSpeed = 25;
	public int overShotTime = 7;
	List<Vector3> btTargets = new List<Vector3> ();
	int aquireNewTarget = 0;

	public int Targets {
		get { return btTargets.Count;}
	}

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().maxAngularVelocity = maxRotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && GameManager.bulletTime) {
			GameObject target = Utilities.GetMouseTarget (Utilities.TargetLayer);
			if (target != null) {
				bool add = true;
				foreach (Vector3 v in btTargets) {
					if (v == target.transform.position) add = false;
				}
				if (add) {
					btTargets .Add (target.transform.position);
					Debug.Log ("Target Hit");
				}
			}
		}
	}

	void FixedUpdate () {
		if (!GameManager.bulletTime && btTargets.Count > 0) {
			if (aquireNewTarget == 0) {
				Vector3 velocity = (btTargets[0] - transform.position).normalized * maxSpeed;
				GetComponent<Rigidbody>().velocity = velocity;
				if ((velocity*Time.fixedDeltaTime).magnitude > (btTargets[0] - transform.position).magnitude) {
					btTargets.RemoveAt (0);
					aquireNewTarget = overShotTime;
				}
			} else if (aquireNewTarget > 0) {
				aquireNewTarget--;
			} else aquireNewTarget = 0;
		}
		GetComponent<Rigidbody>().AddTorque (0, 0, Input.GetAxis ("Horizontal")*(-1)*acceleration);
		//rigidbody.AddForce (new Vector3(Input.GetAxis ("Horizontal")*5, 0));
		if (Input.GetKeyDown (KeyCode.Space) && IsGrounded ()) {
			GetComponent<Rigidbody>().AddForce (Vector3.Cross (GetComponent<Rigidbody>().velocity, GetComponent<Rigidbody>().angularVelocity).normalized * jumpHeight, ForceMode.VelocityChange);
		}
	}
	bool IsGrounded ()
	{
		Vector3 velocity = GetComponent<Rigidbody>().velocity;
		Vector3 angularV = GetComponent<Rigidbody>().angularVelocity;
		Vector3 groundNormal = (velocity.magnitude > 0.01) ? Vector3.Cross (angularV, velocity).normalized : new Vector3 (0,-1);
		RaycastHit hit;
		if (Physics.Raycast (this.transform.position, groundNormal, out hit)) {
			Debug.Log (hit.distance);
			SphereCollider c = (SphereCollider)GetComponent<Collider>();
			return hit.distance <= c.radius;
		}
		else {
			Debug.Log ("No hit");
			return false;
		}
	}
}
