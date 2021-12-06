using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnClickSquare : MonoBehaviour //выполняется при нажатии на ячейку ответа
{
	[SerializeField] private bool isRight;
	[SerializeField] private GameObject StarParticles;
	[SerializeField] private GameSettings GameSettings;
	[SerializeField] private GameObject Restart;
	private UISpawner UIManager;
    private void Awake()
    {
		UIManager = GameObject.Find("Canvas").GetComponent<UISpawner>();
	}
    public void IsRightAnswer()
	{
		isRight = true;
	}
	public void OnClick()
	{
		if (isRight == true)
		{
			Instantiate(StarParticles, transform);
			isRight = true;
			if (UIManager.GetCurrentLevel() < GameSettings.GetNumberOfLevels())
				UIManager.NextLevel();
			else
			{
				Instantiate(Restart, GameObject.Find("Canvas").transform);
				AnimationManager.FadeIn(GameObject.Find("FadeScreen"));
				GameObject.Find("FadeScreen").GetComponent<Image>().raycastTarget = true;
			}
		}
		else 
			AnimationManager.WrongAnswerBounce(transform.Find("AnswerSprite").gameObject);
	}

}
