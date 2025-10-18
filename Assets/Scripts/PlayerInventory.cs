using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Current Item")]
    public ItemSO currentItem;
    
    public bool AddPart(ItemSO part)
    {
        if (part != null && currentItem == null)
        {
            currentItem = part;
            Debug.Log($"Pegou: {part.ItemName}");
            return true;
        }
        
        Debug.Log("Inventário cheio!");
        return false;
    }
    
    public bool RemovePart()
    {
        if (currentItem != null)
        {
            Debug.Log($"Removeu: {currentItem.FrankensteinPartSO.partName}");
            currentItem = null;
            return true;
        }
        return false;
    }
    
    public ItemSO GetPart()
    {
        return currentItem;
    }
    
    public bool HasPart()
    {
        return currentItem != null;
    }
    
    public bool HasPartType(FrankensteinPartType partType)
    {
        return currentItem != null && currentItem.Type == ItemSO.ItemType.FrankeinsteinPart && currentItem.FrankensteinPartSO.partType == partType;
    }
}