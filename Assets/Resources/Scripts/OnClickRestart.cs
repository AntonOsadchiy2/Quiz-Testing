using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnClickRestart : MonoBehaviour, IPointerDownHandler //выполняется при нажатии на рестарт
{
    public void OnPointerDown(PointerEventData eventData)
    {
        AnimationManager.Restart(GameObject.Find("FadeScreen")); 
        AnimationManager.Restart(GameObject.Find("LoadingText"));
        GameObject.Find("FadeScreen").GetComponent<Image>().raycastTarget = false;
        GameObject.Find("Canvas").GetComponent<UISpawner>().Restart();
        Destroy(gameObject);
    }
}