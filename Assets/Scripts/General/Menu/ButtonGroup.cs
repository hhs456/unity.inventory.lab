using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PageType {
    Sprite, Object
}

[System.Serializable]
public class PageSet : GUIListArgs {
    public string description;
    public bool changeObject;
    public GameObject Base;
    public List<Sprite> styles;
    public List<GameObject> objects;

    public PageSet() {
        enable = false;
    }

    public PageSet(string name, GameObject target) {
        enable = false;
        description = name;
        Base = target;
    }
}
public class ButtonGroup : MonoBehaviour {    
    public bool isFullScreen;
    public List<GameObject> buttonFields;
    public List<Sprite> buttonStyles;
    public List<PageSet> pageSettings;

    protected int selectId = -1;
    public int SelectId {
        get { return selectId; }
        set { 
            if (value != -1)
                selectId = value;
        }
    }    
    public int initialId = 0;

    // Start is called before the first frame update
    void OnValidate() {
        FieldsInitial();        
    }

    public void FieldsInitial() {
        if (buttonFields == null)
            buttonFields = new List<GameObject>();
        if (buttonStyles == null)
            buttonStyles = new List<Sprite>();
        if (pageSettings == null)
            pageSettings = new List<PageSet>();
        int i_size = buttonStyles.Count;
        for (int i = 0; i < i_size; i++) {
            if(buttonFields[i])
                buttonFields[i].GetComponent<Image>().sprite = buttonStyles[i];
        }        
    }

    public void Awake() {
        PropertiesInitial();
    }

    public void PropertiesInitial() {
        int i_size = buttonFields.Count;
        for (int i = 0; i < i_size; i++) {
            buttonFields[i].GetComponent<ButtonEvent>().pageId = i;
            buttonFields[i].GetComponent<ButtonEvent>().Master = this;
        }
    }

    public void StatusInitial() {
        if (SelectId != -1)
            if (buttonFields.Count > 0)
                buttonFields[selectId].GetComponent<ButtonEvent>().ResetPage(selectId);        
        SetPageTo(initialId);
        if(buttonFields.Count > 0)
            buttonFields[SelectId].GetComponent<ButtonEvent>().Select();
    }
    public void SetPageTo(int index) {
        int i_size = pageSettings.Count;
        for (int i = 0; i < i_size; i++) {
            if (pageSettings[i].changeObject) {
                #region # Change Page With GameObject
                if (SelectId != -1)
                    pageSettings[i].objects[SelectId].SetActive(false);
                pageSettings[i].objects[index].SetActive(true);
                if (isFullScreen) {
                    Storage.LastPage.Add(Storage.CurrentPage);
                    Storage.CurrentPage = pageSettings[i].objects[index];
                }
                #endregion
            }
            else {
                #region # Change Page With Sprite
                if (pageSettings[i].Base.GetComponent<Image>().sprite != null) {
                    pageSettings[i].Base.GetComponent<Image>().sprite = pageSettings[i].styles[index];
                    pageSettings[index].Base.SetActive(true);
                }
                else
                    Debug.LogWarning("Null Image!");
                #endregion
            }
        }
        SelectId = index;
    }
}
