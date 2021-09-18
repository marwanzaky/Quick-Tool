using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class QuickTool : EditorWindow {
    [MenuItem("QuickTool/Open _%#T")]
    public static void ShowWindows() {
        var window = GetWindow<QuickTool>();
        window.titleContent = new GUIContent("QuickTool");
        window.minSize = new Vector2(250, 50);
    }

    void OnEnable() {
        var root = rootVisualElement;

        root.styleSheets.Add(Resources.Load<StyleSheet>("UIElements/QuickTool_Style"));

        var quickToolVisualTree = Resources.Load<VisualTreeAsset>("UIElements/QuickTool_Main");
        quickToolVisualTree.CloneTree(root);

        var toolButtons = root.Query<Button>();
        toolButtons.ForEach(SetupButton);
    }

    void SetupButton(Button button) {
        var buttonIcon = button.Q(className: "quicktool-button-icon");
        
        var iconPath = "Icons/" + button.parent.name + " Icon";
        var iconAsset = Resources.Load<Sprite>(iconPath);

        button.style.backgroundImage = iconAsset.texture;
        button.clickable.clicked += () => CreateObject(button.parent.name);
        button.tooltip = button.parent.name;
    }

    void CreateObject(string primitiveTypeName) {
        var pt = (PrimitiveType)Enum.Parse(typeof(PrimitiveType), primitiveTypeName, true);
        var go = ObjectFactory.CreatePrimitive(pt);
        go.transform.position = Vector3.zero;
    }
}