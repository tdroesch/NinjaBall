using UnityEngine;
using System.Collections;

public class UnitKill : MonoBehaviour {
	public float explosiveForce;

	void OnTriggerExit2D(Collider2D collider)
	{
		/*if (collision.gameObject.tag == "NinjaBall")
		{

		}*/
		Destroy (gameObject);
	}
	void OnDestroy() 
	{
		Transform[] children = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			children[i] = transform.GetChild (i);
		}
		transform.DetachChildren();
		foreach (Transform child in children) {
			child.GetComponent<Collider2D>().enabled = true;
			child.GetComponent<Renderer>().enabled = true;
			child.GetComponent<Rigidbody2D>().isKinematic = false;
			child.GetComponent<Rigidbody2D>().gravityScale = 1;
			Vector3 forceVector = (child.position - transform.position)*explosiveForce;
			child.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceVector.x, forceVector.y));
			child.GetComponent<Rigidbody2D>().AddTorque(Random.Range (-5,5));
		}
	}
}
