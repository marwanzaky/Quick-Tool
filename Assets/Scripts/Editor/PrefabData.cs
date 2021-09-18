using UnityEngine;

[System.Serializable]
public struct PrefabData {
    [SerializeField] string name;
    [SerializeField] GameObject prefab;

    public GameObject Prefab {
        get => prefab;
    }

    public string Name {
        get => name;
    }
}