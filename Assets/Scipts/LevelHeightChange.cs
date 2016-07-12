using UnityEngine;
using System.Collections;

public class LevelHeightChange : MonoBehaviour {

	void OnTriggerStay2D (Collider2D col){
		CharacterController2D cc2D = col.GetComponent<CharacterController2D>();
		if (cc2D != null){
			cc2D.CameraTracking(true, true);
		}
	}

//	void OnTriggerExit2D (Collider2D col){
//		CharacterController2D cc2D = col.GetComponent<CharacterController2D>();
//		if (cc2D != null){
//			cc2D.CameraTracking(true, false);
//		}
//	}
}
