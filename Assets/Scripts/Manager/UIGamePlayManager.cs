using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIGamePlayManager : MonoBehaviour {

	public Text loadingText;
	public GameObject uiRoot;
	public GameObject loadingImage;

	public static UIGamePlayManager instance;



	void Awake()
	{
		instance = this;
	}

	public void HideUIGameplay()
	{
		uiRoot.SetActive(false);
	}


	public void FlipButton()
	{
		PlayerControl.instance.Flip();
	}

}
