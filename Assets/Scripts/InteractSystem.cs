using UnityEngine;
using UnityEngine.Events;

public class InteractSystem : MonoBehaviour
{
    [SerializeField] private BoxCollider2D triggerZone;
    
    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerExit;
    public UnityEvent onInteractionSuccess;
    public UnityEvent onInteractionFailed;

    public enum InteractionType { GivePart, TakeItem }
    public InteractionType type;

    [Header("Monster")]
    public FrankensteinMonster frankensteinMonster;

    [Header("Item")]
    public ItemSO item;

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.SetCurrentInteractable(this);
            onPlayerEnter?.Invoke();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.ClearCurrentInteractable(this);
            onPlayerExit?.Invoke();
        }
    }
    
    public bool Interact(PlayerMovement player)
    {
        bool success = false;
        
        switch (type)
        {
            case InteractionType.GivePart:
                if (frankensteinMonster.OnFire)
                {
                    UseExtintor(player);
                    break;
                }
                GivePart(player);
                break;
            case InteractionType.TakeItem:
                TakeItem(player);
                break;
        }
        
        if (success)
            onInteractionSuccess?.Invoke();
        else
            onInteractionFailed?.Invoke();
            
        return success;
    }

    public void GivePart(PlayerMovement player)
    {
        if (player.inventory.currentItem == null || player.inventory.currentItem.Type != ItemSO.ItemType.FrankeinsteinPart)
        {
            return;
        }
        if (frankensteinMonster.AttachPart(player.inventory.currentItem.FrankensteinPartSO))
        {
            player.inventory.RemovePart();
        }
    }
    
    public void UseExtintor(PlayerMovement player)
    {
        if (player.inventory.currentItem == null)
        {
            Debug.Log("Você precisa apagar o fogo!");
            return;
        }
        if (player.inventory.currentItem.Type != ItemSO.ItemType.ExtintorDeFogo)
        {
            Debug.Log("Apague o fogo antes de colocar partes novas!");
            return;
        }
        
        frankensteinMonster.SetOffFire();
        player.inventory.RemovePart(); 
    }
    
    public void TakeItem(PlayerMovement _player)
    {
        bool canPlayerTakeItem = (_player != null) && (_player.inventory.currentItem == null);

        if (canPlayerTakeItem)
        {
            _player.inventory.AddPart(item);
        }
    }
}