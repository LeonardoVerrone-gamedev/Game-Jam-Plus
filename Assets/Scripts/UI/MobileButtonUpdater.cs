using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

public class OnScreenButtonEventSpriteChanger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Sprites")]
    public Sprite pressedSprite;
    public Sprite releasedSprite;
    
    private Image buttonImage;
    private OnScreenButton onScreenButton;
    
    private void Start()
    {
        buttonImage = GetComponent<Image>();
        onScreenButton = GetComponent<OnScreenButton>();
        
        // Garante que o GameObject tenha EventTrigger
        if (GetComponent<EventTrigger>() == null)
        {
            gameObject.AddComponent<EventTrigger>();
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonImage != null && pressedSprite != null)
        {
            buttonImage.sprite = pressedSprite;
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonImage != null && releasedSprite != null)
        {
            buttonImage.sprite = releasedSprite;
        }
    }
}