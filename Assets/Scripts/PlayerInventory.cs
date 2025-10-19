using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Current Item")]
    public ItemSO currentItem;

    [SerializeField] AudioSource itemAudioSource;
    [SerializeField] AudioClip takeItemClip;
    
    public bool AddPart(ItemSO part)
    {
        if (part != null && currentItem == null)
        {
            currentItem = part;
            itemAudioSource.clip = takeItemClip;
            itemAudioSource.Play();
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
            Debug.Log($"Removeu: {currentItem}");
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