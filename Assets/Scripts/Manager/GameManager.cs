using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {



	IEnumerator Start()
	{
        AsyncOperation async = Application.LoadLevelAdditiveAsync("Level01");

		do
		{
			UIGamePlayManager.instance.loadingText.text = async.progress*80.00 + " %";
			yield return null;
		}while (!async.isDone);

		UIGamePlayManager.instance.loadingText.text = async.progress*80.00 + " %";

		yield return StartCoroutine(PoolingManager.instance.Initialize());

		UIGamePlayManager.instance.loadingText.text = "100.00%";
		yield return new WaitForSeconds(0.5f);
		UIGamePlayManager.instance.loadingImage.SetActive(false);
		UIGamePlayManager.instance.loadingText.gameObject.SetActive(false);

        PlayerControl.instance.gameObject.SetActive(true);
		PlayerControl.instance.ActivedGun();
		PlayerControl.instance.StartMovementControl();
        LevelManager.instance.ActiveLevelUp();
	}



}