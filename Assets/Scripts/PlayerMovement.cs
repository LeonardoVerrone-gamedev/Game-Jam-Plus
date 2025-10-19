using System.Diagnostics;
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

    public bool canMove = false;
    public float contagemInicial = 3f;

    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioSource itemAudioSource;
    [SerializeField] AudioClip dropItemClip;

    [SerializeField] GameObject brainDropPrefab;
    [SerializeField] GameObject armDropPrefab;
    [SerializeField] GameObject legDropPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        inventory = GetComponent<PlayerInventory>();

        Invoke("SetCanMove", 3f);
    }

    void SetCanMove()
    {
        canMove = !canMove;
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        rawInput = context.ReadValue<Vector2>();
        ProcessMovement();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && currentInteractable != null && canMove)
        {
            currentInteractable.Interact(this);
        }
    }

    public void OnReleaseItem(InputAction.CallbackContext context)
    {
        if (context.started && canMove)
        {
            ReleaseItem();
        }
    }

    void ReleaseItem()
    {
        if(inventory.currentItem.Type == ItemSO.ItemType.FrankeinsteinPart)
        {
            FrankensteinPartSO _part = inventory.currentItem.FrankensteinPartSO;
            FrankensteinPartType part = _part.partType;
            if (part == FrankensteinPartType.leftArm || part == FrankensteinPartType.rightArm)
            {
                Instantiate(armDropPrefab, holdingObjectRender.transform.position, Quaternion.identity);
            }
            else if (part == FrankensteinPartType.leftLeg || part == FrankensteinPartType.rightLeg)
            {
                Instantiate(legDropPrefab, holdingObjectRender.transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(brainDropPrefab, holdingObjectRender.transform.position, Quaternion.identity);
            }
            
            itemAudioSource.clip = dropItemClip;
            itemAudioSource.Play();

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
        if (!canMove)
        {
            movement = Vector2.zero;
            last_movement = movement;
            return;
        }

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

        //Audio
        if(movement.magnitude > 0.1f)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
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
        if (!canMove)
        {
            return;
        }

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}