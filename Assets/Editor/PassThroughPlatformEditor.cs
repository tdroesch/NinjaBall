using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PassThroughPlatform))]
public class PassThroughPlatformEditor : Editor {

	BoxCollider2D collider;
	BoxCollider2D trigger;
	
	void OnEnable ()
	{
		PassThroughPlatform castedTarget = target as PassThroughPlatform;
		collider = castedTarget.platformCollider;
		trigger = castedTarget.trigger;
		if (collider == null) {
			collider = (BoxCollider2D)castedTarget.GetComponent<Collider2D>();
			castedTarget.platformCollider = collider;
		}
		if (trigger == null) {
			trigger = castedTarget.gameObject.AddComponent<BoxCollider2D> ();
			castedTarget.trigger = trigger;
			Vector2 center = collider.offset;
			center.y -= collider.size.y * 1.0f * Mathf.Cos(Mathf.Deg2Rad*castedTarget.transform.rotation.eulerAngles.z);
			center.x -= collider.size.x * 1.0f * Mathf.Sin(Mathf.Deg2Rad*castedTarget.transform.rotation.eulerAngles.z);
			trigger.offset = center;
			trigger.size = Vector2.Scale(collider.size,new Vector2(1.2f+(0.8f*Mathf.Sin(Mathf.Deg2Rad*castedTarget.transform.rotation.eulerAngles.z)),
			                                                       1.2f+(0.8f*Mathf.Cos(Mathf.Deg2Rad*castedTarget.transform.rotation.eulerAngles.z))));
			trigger.isTrigger = true;
			trigger.hideFlags = HideFlags.HideInInspector;
		}
	}
	
	void OnDestroy ()
	{
		if ((PassThroughPlatform)target == null) {
			DestroyImmediate(trigger);
		}
	}
}
