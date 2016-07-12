using UnityEngine;
using UnityEditor;
using System.Collections;

//[CustomEditor(typeof(CharacterController2D))]
public class CharacterController2DEditor : Editor {

	Rigidbody2D rigidbody;

	void OnEnable ()
	{
		CharacterController2D castedTarget = target as CharacterController2D;
		rigidbody = castedTarget.GetComponent<Rigidbody2D>();
		rigidbody.hideFlags = HideFlags.HideInInspector;
//		Debug.Log("Enabled");
	}

	void OnDestroy ()
	{
		if ((CharacterController2D)target == null) {
			rigidbody.hideFlags = HideFlags.None;
		}
//		Debug.Log("Destroy");
	}

	public override void OnInspectorGUI ()
	{
		CharacterController2D castedTarget = target as CharacterController2D;
		DrawDefaultInspector ();
		if (castedTarget.GetComponent<Collider2D>() == null || castedTarget.GetComponent<Collider2D>().enabled == false) {
			Collider2D[] childrenColliders = castedTarget.GetComponentsInChildren<Collider2D> (false);
			bool activeCollider = false;
			if (childrenColliders.Length > 0) {
				foreach (Collider2D c in childrenColliders) {
					activeCollider = activeCollider | c.enabled;
				}
			}
			if (!activeCollider) {
				EditorGUILayout.HelpBox ("There are no active colliders on this object.", MessageType.Error);
			}
		}
	}
}
