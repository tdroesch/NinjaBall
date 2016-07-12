using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	private Vector3 relativePosition;

	private bool trackX = true;
	private bool trackY = true;

	public bool TrackX {
		set {trackX = value;}
	}
	public bool TrackY {
		set {trackY = value;}
	}
	
	// Use this for initialization
	void Start ()
	{
		if (player == null) {
			player = transform.parent.gameObject;
			if (player == null){
				player = GameObject.FindGameObjectWithTag ("Player");
				if (player == null){
					Debug.LogError("Camera Can't find Player");
				}
			} else {
				transform.parent = null;
			}
		}
		relativePosition = transform.position - player.transform.position;
		player.GetComponent<CharacterController2D>().SetCamera(this);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	void LateUpdate ()
	{
		if (player != null) {
			Vector3 newPosition = relativePosition + player.transform.position;
			if (!trackY) {
				newPosition.y = transform.position.y;
			}
			if (!trackX) {
				newPosition.x = transform.position.x;
			}
			transform.position = newPosition;
		}
	}
}
