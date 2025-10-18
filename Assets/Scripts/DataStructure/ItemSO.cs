using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Frankenstein/Item")]
public class ItemSO : ScriptableObject
{
    public string ItemName;
    public enum ItemType { FrankeinsteinPart, ExtintorDeFogo };
    public ItemType Type;

    [Header("Frankenstein Part")]
    public FrankensteinPartSO FrankensteinPartSO;
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(ItemSO))]
public class ItemSOEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        var item = (ItemSO)target;

        // Desenha o tipo primeiro
        item.Type = (ItemSO.ItemType)UnityEditor.EditorGUILayout.EnumPopup("Type", item.Type);

        // Mostra as propriedades baseado no tipo
        if (item.Type == ItemSO.ItemType.FrankeinsteinPart)
        {
            UnityEditor.EditorGUILayout.LabelField("Frankenstein Part", UnityEditor.EditorStyles.boldLabel);
            item.FrankensteinPartSO = (FrankensteinPartSO)UnityEditor.EditorGUILayout.ObjectField("Part Type", item.FrankensteinPartSO, typeof(FrankensteinPartSO), false);
        }

        // Salva as mudanças
        if (GUI.changed)
        {
            UnityEditor.EditorUtility.SetDirty(item);
        }
    }
}
#endif