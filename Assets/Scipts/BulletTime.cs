using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class BulletTime : MonoBehaviour {
	public float slow;
	public float fade;
	public float hold;

	void OnTriggerEnter2D(Collider2D collider)
	{
		StartCoroutine (StartBT());
	}

	IEnumerator StartBT ()
	{

		while (Time.timeScale > this.slow) {
			float deltaTimeScale = (1-this.slow)/fade*Time.deltaTime;
			Time.timeScale -= deltaTimeScale;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
			yield return null;
		}
		Debug.Log ("Bullet Time Start!");
		Camera.main.GetComponent<BloomOptimized>().intensity = 2;
		GameManager.bulletTime = true;
		yield return new WaitForSeconds(hold*Time.timeScale);
		Debug.Log ("Bullet Time Stop!");
		Camera.main.GetComponent<BloomOptimized>().intensity = 0.5f;
		Time.timeScale = 1;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		GameManager.bulletTime = false;
		GameObject.Destroy (gameObject);
	}
}
