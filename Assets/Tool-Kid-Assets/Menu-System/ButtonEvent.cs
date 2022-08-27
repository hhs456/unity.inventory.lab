using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class ButtonTransition {
    public Color Color { get; set; }
    public Sprite Sprite { get; set; }
    public Animator animator;
    public ButtonTransition(Color color, Sprite sprite) {
        Color = color;
        Sprite = sprite;
    }
}

public class ButtonEvent : MonoBehaviour, IPointerClickHandler
{
    protected ButtonTransition origin;    
    protected Image image;
    protected Button button;
    protected ButtonGroup master;
    public bool isBackButton = false;
    public bool setToInitial = false;
    public ButtonGroup initialTarget;

    protected System.Action[] Reset;

    public ButtonGroup Master {
        get {            
            return master;
        }
        set {
            master = value;
        }
    }
    public int pageId = -1;    
    public ButtonSoundEffect soundEffect;
    

    void Awake() {
        image = GetComponent<Image>();
        origin = new ButtonTransition(image.color, image.sprite);
        button = GetComponent<Button>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (!button.interactable) return;
            Storage.AudioSource.PlayOneShot(Storage.ButtonSoundEffect[(int)soundEffect]);
        if (Master) Select();
        if (isBackButton)
            Storage.CurrentPage.SetActive(false);
        if (initialTarget) {
            if (setToInitial)
                initialTarget.initialId = pageId;
            initialTarget.StatusInitial();
        }
    }

    public void Select() {
        if (pageId != master.SelectId && master.SelectId != -1)
            ResetPage(master.SelectId);
        master.SetPageTo(pageId);
        if (pageId == master.SelectId) {
            SetEffect();            
        }
        Debug.Log("Select '" + this + "'", this);
    }

    public void ResetPage(int pageId) {
        master.buttonFields[pageId].GetComponent<ButtonEvent>().ResetEffect();
    }

    public void ResetEffect() {
        switch (button.transition) {
            case Selectable.Transition.None:
                break;
            case Selectable.Transition.ColorTint:
                image.color = origin.Color;
                break;
            case Selectable.Transition.SpriteSwap:
                image.sprite = origin.Sprite;
                break;
            case Selectable.Transition.Animation:
                break;
            default:
                break;
        }        
    }

    public void SetEffect() {
        switch (button.transition) {
            case Selectable.Transition.None:
                break;
            case Selectable.Transition.ColorTint:
                image.color = button.colors.selectedColor;
                break;
            case Selectable.Transition.SpriteSwap:
                if(button.spriteState.selectedSprite)
                    image.sprite = button.spriteState.selectedSprite;
                break;
            case Selectable.Transition.Animation:
                break;
            default:
                break;
        }
    }
}

