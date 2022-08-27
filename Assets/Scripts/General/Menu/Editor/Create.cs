using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class Create
{
    [MenuItem("GameObject/Create Other/ToolKid Button")]
    static void ToolKidButton(MenuCommand menuCommand) {
        // Create a custom game object
        GameObject btn = new GameObject("ToolKid Button");
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(btn, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(btn, "Create " + btn.name);
        Selection.activeObject = btn;
        btn.AddComponent<RectTransform>();
        btn.AddComponent<CanvasRenderer>();
        Image image = btn.AddComponent<Image>();
        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/InputFieldBackground.psd");
        Button button = btn.AddComponent<Button>();
        button.transition = Selectable.Transition.SpriteSwap;
        btn.AddComponent<ButtonEvent>();
    }
    static void ToolKidButton(ref ButtonGroup group) {
        // Create a custom game object
        GameObject btn = new GameObject("ToolKid Button");
        btn.transform.SetParent(group.transform);
        btn.AddComponent<RectTransform>();
        btn.AddComponent<CanvasRenderer>();
        Image image = btn.AddComponent<Image>();
        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/InputFieldBackground.psd");
        Button button = btn.AddComponent<Button>();
        button.transition = Selectable.Transition.SpriteSwap;
        btn.AddComponent<ButtonEvent>();

        GameObject page = new GameObject("Sub Page Base");
        page.transform.SetParent(group.transform);
        page.AddComponent<RectTransform>();
        page.AddComponent<CanvasRenderer>();

        group.buttonFields = new List<GameObject>();
        group.buttonFields.Add(btn);
        group.buttonStyles = new List<Sprite>();
        group.buttonStyles.Add(image.sprite);
        group.pageSettings = new List<PageSet>();
        group.pageSettings.Add(new PageSet("·s­¶­±", page));
    }

    [MenuItem("GameObject/Create Other/ToolKid Button Group")]
    static void ToolKidButtonGroup(MenuCommand menuCommand) {
        // Create a custom game object
        GameObject go = new GameObject("ToolKid Button Group");
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
        go.AddComponent<RectTransform>();
        go.AddComponent<CanvasRenderer>();
        ButtonGroup group = go.AddComponent<ButtonGroup>();        
        ToolKidButton(ref group);
    }
}
