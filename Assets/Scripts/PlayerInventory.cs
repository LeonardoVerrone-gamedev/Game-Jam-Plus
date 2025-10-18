using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Current Item")]
    public FrankensteinPartSO currentPart;
    
    public bool AddPart(FrankensteinPartSO part)
    {
        if (part != null && currentPart == null)
        {
            currentPart = part;
            Debug.Log($"Pegou: {part.partName}");
            return true;
        }
        
        Debug.Log("Inventário cheio!");
        return false;
    }
    
    public bool RemovePart()
    {
        if (currentPart != null)
        {
            Debug.Log($"Removeu: {currentPart.partName}");
            currentPart = null;
            return true;
        }
        return false;
    }
    
    public FrankensteinPartSO GetPart()
    {
        return currentPart;
    }
    
    public bool HasPart()
    {
        return currentPart != null;
    }
    
    public bool HasPartType(FrankensteinPartType partType)
    {
        return currentPart != null && currentPart.partType == partType;
    }
}