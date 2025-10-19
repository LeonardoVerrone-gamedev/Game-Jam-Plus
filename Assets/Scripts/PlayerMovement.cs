using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 last_movement; // for animation system
    private Vector2 rawInput;
    private Animator anim;

    [Header("Sistema de Interação")]
    [SerializeField]private InteractSystem currentInteractable;
    [SerializeField] public PlayerInventory inventory;
    bool isHoldingSomething;
    [SerializeField] SpriteRenderer holdingObjectRender;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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

    public void OnReleaseItem(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            inventory.RemovePart();
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

        if (movement != Vector2.zero)
        {
            last_movement = movement;
        }

        //Animation
        anim.SetFloat("Horizontal", last_movement.x);
        anim.SetFloat("Vertical", last_movement.y);
        anim.SetFloat("Move", movement.magnitude);
    }
    
    void Update()
    {
        isHoldingSomething = inventory.currentItem != null;
        anim.SetBool("Segurando", isHoldingSomething);

        if (isHoldingSomething)
        {
            holdingObjectRender.sprite = inventory.currentItem.holdingSprite;
        }
        else
        {
            holdingObjectRender.sprite = null;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}