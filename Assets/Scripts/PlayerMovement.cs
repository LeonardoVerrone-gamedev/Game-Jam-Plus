using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 rawInput;

    [Header("Sistema de Interação")]
    [SerializeField]private InteractSystem currentInteractable;
    [SerializeField]public PlayerInventory inventory;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inventory = GetComponent<PlayerInventory>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        rawInput = context.ReadValue<Vector2>();
        ProcessMovement();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && currentInteractable != null)
        {
            currentInteractable.Interact(this);
        }
    }
    
    public void SetCurrentInteractable(InteractSystem interactable)
    {
        currentInteractable = interactable;
    }
    
    public void ClearCurrentInteractable(InteractSystem interactable)
    {
        if (currentInteractable == interactable)
        {
            currentInteractable = null;
        }
    }

    private void ProcessMovement()
    {
        movement = rawInput;
        
        if (movement.magnitude > 1f)
        {
            movement = movement.normalized;
        }
        
        if (rawInput.magnitude < 0.1f)
        {
            movement = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}