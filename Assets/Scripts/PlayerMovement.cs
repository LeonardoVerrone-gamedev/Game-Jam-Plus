using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 rawInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        rawInput = context.ReadValue<Vector2>();
        ProcessMovement();
    }

    private void ProcessMovement()
    {
        movement = rawInput;
        
        // Normaliza o vetor para manter a mesma velocidade na diagonal
        if (movement.magnitude > 1f)
        {
            movement = movement.normalized;
        }
        
        // Se não há input significativo, zera o movimento
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