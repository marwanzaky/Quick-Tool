using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class QuickTool : EditorWindow {
    VisualElement root;
    FloatField iconSize;
    StyleSheet styleSheet;

    [MenuItem("QuickTool/Open _%#T")]
    public static void ShowWindows() {
        var window = GetWindow<QuickTool>();
        window.titleContent = new GUIContent("QuickTool");
        window.minSize = new Vector2(250, 50);
    }

    void OnEnable() {
        root = rootVisualElement;

        // Icon size
        var baseSettingsLabel = new Label("Base Settings");

        baseSettingsLabel.AddToClassList("label");

        iconSize = new FloatField("Icon Size");
        iconSize.value = 50;
        iconSize.RegisterValueChangedCallback(changeEvent => {
            const float SIZE_OFFSET = 18f;

            foreach (var el in root.Query<Button>().ToList().ToArray()) {
                el.style.width = changeEvent.newValue;
                el.style.height = changeEvent.newValue;
                el.Q(className: "quicktool-button-icon").style.width = changeEvent.newValue - SIZE_OFFSET;
                el.Q(className: "quicktool-button-icon").style.height = changeEvent.newValue - SIZE_OFFSET;
            }
        });

        root.Add(baseSettingsLabel);
        root.Add(iconSize);

        // Attach the USS Style to the root
        styleSheet = Resources.Load<StyleSheet>("UIElements/QuickTool_Style");
        root.styleSheets.Add(styleSheet);

        // Attach the main XML content
        var quickToolVisualTree = Resources.Load<VisualTreeAsset>("UIElements/QuickTool_Main");
        quickToolVisualTree.CloneTree(root);

        LoadPrimitiveTypeButtons();
        LoadCustomDataButtons();
    }

    void LoadPrimitiveTypeButtons() {
        var primitiveTypeButtons = root.Query<Button>();
        primitiveTypeButtons.ForEach(SetupPrimitiveTypeButton);
    }

    void LoadCustomDataButtons() {
        var quickToolCustonData = Resources.Load<QuickToolCustomData>("QuickTool Custom Data");

        var customPrefabs = root.Q(className: "custom-prefabs");

        foreach (var el in quickToolCustonData.Datas) {
            var btn = new Button();
            var visualElement = new VisualElement();

            btn.AddToClassList("quicktool-button");
            btn.clicked += () => { Spawn(el.Prefab); };
            customPrefabs.Insert(0, btn);

            visualElement.AddToClassList("quicktool-button-icon");
            visualElement.style.backgroundImage = AssetPreview.GetAssetPreview(el.Prefab);
            btn.Insert(0, visualElement);
        }
    }

    void SetupPrimitiveTypeButton(Button button) {
        var buttonIcon = button.Q(className: "quicktool-button-icon");

        var iconPath = "Icons/" + button.parent.name + " Icon";
        var iconAsset = Resources.Load<Sprite>(iconPath);

        buttonIcon.style.backgroundImage = iconAsset.texture;
        button.clickable.clicked += () => CreateObject(button.parent.name);
        button.tooltip = button.parent.name;
    }

    void CreateObject(string primitiveTypeName) {
        var pt = (PrimitiveType)Enum.Parse(typeof(PrimitiveType), primitiveTypeName, true);
        var go = ObjectFactory.CreatePrimitive(pt);
        go.transform.position = Vector3.zero;
    }

    void Spawn(GameObject prefab) {
        var go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        go.transform.position = Vector3.zero;
    }
}