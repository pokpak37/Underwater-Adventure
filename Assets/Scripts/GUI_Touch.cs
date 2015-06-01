using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUI_Touch : MonoBehaviour {
	
	PlayerControl player;

	public Text text;
	
	public RectTransform MovementController;
	public RectTransform Core_MovementControl;
	Touch touch;
	
	float x = 0;
	float y = 0;
	
	Vector2 movementControllerPos;
	Vector2 corePos;
	


	void Update()
	{
		player = PlayerControl.instance;
		foreach (Touch touch in Input.touches) 
		{
			x = touch.position.x - Screen.width/2;
			y = touch.position.y - Screen.height/2;
			//int num = touch.fingerId;
			//if(num == 0) // movement
			//{
			if(touch.position.x < Screen.width/2)
			{
				if(touch.phase == TouchPhase.Began)
				{
					MovementController.gameObject.SetActive(true);
					movementControllerPos.x = x;
					movementControllerPos.y = y;
					MovementController.anchoredPosition = movementControllerPos;
					Core_MovementControl.anchoredPosition = new Vector2(0,0);
				}
				else if(touch.phase == TouchPhase.Moved)
				{
					corePos.x = x - movementControllerPos.x;
					corePos.y = y - movementControllerPos.y;
					
					corePos = Vector2.ClampMagnitude(corePos, 100);
					Core_MovementControl.anchoredPosition = corePos;
				}
				else if(touch.phase == TouchPhase.Ended)
				{
					MovementController.gameObject.SetActive(false);
					corePos.x = 0;
					corePos.y = 0;
				}
				player.powerX = corePos.x/100.000f;
				player.powerY = corePos.y/100.000f;
				print(corePos.x + " : " + corePos.y);
			}
			//}
			//if(num == 1) // flip
			//{
				
			//}
		}
		
		text.text = player.powerX.ToString("F2") + " : " + player.powerY.ToString("F2") +" ";
		
	}
}
