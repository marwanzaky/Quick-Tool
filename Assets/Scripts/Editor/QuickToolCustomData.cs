
using UnityEngine;

[CreateAssetMenu(fileName = "New QuickTool Custom Data", menuName = "Scriptable Objects/QuickTool Custom Data", order = 20)]
public class QuickToolCustomData : ScriptableObject {
    [SerializeField] PrefabData[] datas;

    public PrefabData[] Datas {
        get {
            return datas;
        }
    }
}