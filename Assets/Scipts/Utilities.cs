using UnityEngine;
using System.Collections;

public static class Utilities{
	public const int PlayerLayer = 1 << 8;
	public const int TargetLayer = 1 << 9;
	public const int DoodadLayer = 1 << 10;
	public const int BTZoneLayer = 1 << 11;

	/// <summary>
	/// Gets the mouse position in the game plane.
	/// </summary>
	/// <returns>The mouse position in the game plane.</returns>
	public static Vector3 GetMousePosition(int layerMask){
		Ray target = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast (target, out hit, float.PositiveInfinity, layerMask)){
			return hit.point;
		}
		else return new Vector3(Mathf.Infinity,Mathf.Infinity,Mathf.Infinity);
	}

	/// <summary>
	/// Gets the GameObject the mouse is over.
	/// </summary>
	/// <returns>The GameObject the mouse is over.</returns>
	public static GameObject GetMouseTarget(int layerMask){
		Ray target = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast (target, out hit, float.PositiveInfinity, layerMask)){
			return hit.collider.gameObject;
		}
		else return null;
	}

	/// <summary>
	/// Gets the 2D GameObject the mouse is over.
	/// </summary>
	/// <returns>The GameObject the mouse is over.</returns>
	public static GameObject GetMouseTarget2D(int layerMask){
		Vector3 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast (new Vector2(origin.x, origin.y), Vector2.zero, 0f, layerMask);
		if (hit){
			return hit.collider.gameObject;
		}
		else return null;
	}
	//http://unity3d.com/learn/tutorials/modules/intermediate/scripting/events
	//Define a deligate.
	//This works as a pointer to a function
	public delegate void GameEvent();

	//Declare the events of type that is the deligate.
	//You can add functions to the event with the +=
	public static event GameEvent GameStart, GameOver, GameEnd;
	
	public static void OnGameStart(){
		if(GameStart != null){
			GameStart();
		}
	}
	
	public static void OnGameOver(){
		if(GameOver != null){
			GameOver();
		}
	}
	
	public static void OnGameEnd(){
		if (GameEnd != null) {
			GameEnd();
		}
	}

	//Coroutines
	//http://unity3d.com/learn/tutorials/modules/intermediate/scripting/coroutines
	//http://docs.unity3d.com/ScriptReference/Coroutine.html
	//Called with StartCoroutine()
	static IEnumerator myCoroutine(){
		//Wait for the next frame.
		yield return null;
		//Wait for time
		yield return new WaitForSeconds(5);
	}
}
