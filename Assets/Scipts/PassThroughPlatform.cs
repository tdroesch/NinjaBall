using UnityEngine;
using System.Collections.Generic;
//I need to write an editor for this class.
//The editor will add a trigger collider to the gameobject and hide it.
//Write accessors and mutators for the fields.
[RequireComponent(typeof(BoxCollider2D))]
public class PassThroughPlatform : MonoBehaviour {
	[HideInInspector] public BoxCollider2D platformCollider;
	[HideInInspector] public BoxCollider2D trigger;
	public LayerMask objectLayers;
	List<CompoundCollider2D> trackedColliders;


	void Start(){
		trackedColliders = new List<CompoundCollider2D>();
	}


	void OnTriggerEnter2D(Collider2D other2D){
		if ((1 << other2D.gameObject.layer & ~objectLayers.value) == 0){
			foreach(CompoundCollider2D cc in trackedColliders){
				if (cc.gameObject == other2D.transform.root.gameObject){
					cc.AddTrigger(other2D);
					return;
				}
			}
			CompoundCollider2D newCC = new CompoundCollider2D(other2D);
			trackedColliders.Add(newCC);
			newCC.IgnoreCollision(platformCollider, true);
		}
	}

	void OnTriggerExit2D(Collider2D other2D){
		if ((1 << other2D.gameObject.layer & ~objectLayers.value) == 0){
			foreach(CompoundCollider2D cc in trackedColliders){
				if (cc.gameObject == other2D.transform.root.gameObject){
					cc.RemoveTrigger(other2D);
					if (cc.Triggers == 0){
						cc.IgnoreCollision(platformCollider, false);
						trackedColliders.Remove(cc);
						return;
					}
				}
			}
		}
	}

	class CompoundCollider2D {
		List<Collider2D> colliders;
		List<Collider2D> triggeringColliders;
		public GameObject gameObject;

		public int Triggers {
			get { return triggeringColliders.Count; }
		}

		public CompoundCollider2D(Collider2D _trigger){
			gameObject = _trigger.transform.root.gameObject;
			colliders = new List<Collider2D>();
			gameObject.GetComponentsInChildren<Collider2D>(colliders);
			triggeringColliders = new List<Collider2D>();
			triggeringColliders.Add(_trigger);
		}

		public void IgnoreCollision(Collider2D _collider, bool ignore){
			foreach (Collider2D c in colliders){
				Physics2D.IgnoreCollision(_collider, c, ignore);
			}
		}

		public void AddTrigger(Collider2D _collider){
			if (!triggeringColliders.Contains(_collider)) {
				triggeringColliders.Add(_collider);
			}
		}

		public bool RemoveTrigger(Collider2D _collider){
			return triggeringColliders.Remove(_collider);
		}
	}
}
