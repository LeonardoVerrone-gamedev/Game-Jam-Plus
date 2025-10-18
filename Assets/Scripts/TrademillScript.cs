using UnityEngine;
using System.Collections.Generic;

public class TrademillScript : MonoBehaviour
{
    [Header("Area Effector")]
    public AreaEffector2D areaEffector;
    
    [Header("Force Curve")]
    public float startValue = 0.5f;
    public float endValue = 10f;
    public float durationInMinutes = 2.5f; // 2.30 minutos = 2.5 minutos
    float durationInSeconds;

    public AnimationCurve forceCurve;
    
    [Header("Jam Settings")]
    public float minTimeBetweenJams = 10f;
    public float maxTimeBetweenJams = 30f;
    public float jamDuration = 2f;
    public float recoverySpeed = 20f;
    
    private float jamTimer;
    private bool isJammed;
    private bool isRecovering;
    private float nextJamTime;
    private float targetForce;
    private float currentCurveTime;
    public float currentForce { get; private set; }
    
    // Controla objetos dentro da área
    private HashSet<Rigidbody2D> objectsInArea = new HashSet<Rigidbody2D>();
    private Dictionary<Rigidbody2D, float> objectForces = new Dictionary<Rigidbody2D, float>();

    void Awake()
    {
        durationInSeconds = durationInMinutes * 60f; // 150 segundos
        forceCurve = AnimationCurve.Linear(0, startValue, durationInSeconds, endValue);
    }

    void Start()
    {
        if (areaEffector == null)
            areaEffector = GetComponent<AreaEffector2D>();
            
        nextJamTime = Random.Range(minTimeBetweenJams, maxTimeBetweenJams);
        
        // Desativa o forceMagnitude nativo pois vamos controlar manualmente
        if (areaEffector != null)
            areaEffector.forceMagnitude = 0f;
    }

    void Update()
    {
        UpdateForce();
        UpdateJamSystem();
        ApplyForcesToObjectsInArea();
    }
    
    void UpdateForce()
    {
        if (isJammed)
        {
            currentForce = 0f;
            return;
        }
        
        if (isRecovering)
        {
            currentForce = Mathf.MoveTowards(currentForce, targetForce, recoverySpeed * Time.deltaTime);
            if (Mathf.Approximately(currentForce, targetForce))
            {
                isRecovering = false;
                nextJamTime = Random.Range(minTimeBetweenJams, maxTimeBetweenJams);
                jamTimer = 0f;
            }
            return;
        }
        
        // Operação normal - segue a curva
        currentCurveTime += Time.deltaTime;
        
        if (currentCurveTime <= forceCurve.keys[forceCurve.length - 1].time)
        {
            currentForce = forceCurve.Evaluate(currentCurveTime);
        }
        else
        {
            currentForce = forceCurve.Evaluate(forceCurve.keys[forceCurve.length - 1].time);
        }
        
        targetForce = currentForce;
    }
    
    void UpdateJamSystem()
    {
        if (isJammed || isRecovering) return;
        
        jamTimer += Time.deltaTime;
        if (jamTimer >= nextJamTime)
        {
            StartJam();
        }
    }
    
    void StartJam()
    {
        isJammed = true;
        jamTimer = 0f;
        Debug.Log($"AreaEffector travou! Força: {currentForce} → 0");
        Invoke(nameof(EndJam), jamDuration);
    }
    
    void EndJam()
    {
        isJammed = false;
        isRecovering = true;
        Debug.Log($"Iniciando recuperação para força: {targetForce}");
    }
    
    void ApplyForcesToObjectsInArea()
    {
        // Aplica força apenas aos objetos que estão dentro da área
        foreach (var rb in objectsInArea)
        {
            if (rb != null && !rb.isKinematic)
            {
                // Calcula direção baseada no ângulo do AreaEffector
                float angle = areaEffector.forceAngle * Mathf.Deg2Rad;
                Vector2 forceDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                Vector2 force = forceDirection * currentForce;
                
                // Aplica força
                rb.AddForce(force);
                
                // Guarda a força aplicada para referência
                objectForces[rb] = currentForce;
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null && !rb.isKinematic)
        {
            objectsInArea.Add(rb);
            objectForces[rb] = 0f;
            Debug.Log($"Objeto entrou na área: {other.name}");
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null && objectsInArea.Contains(rb))
        {
            // Para imediatamente de aplicar força neste objeto
            objectsInArea.Remove(rb);
            objectForces.Remove(rb);
            Debug.Log($"Objeto saiu da área: {other.name} - Força removida");

            rb.linearVelocity = Vector2.zero;
        }
    }
    
    // Limpa objetos que podem ter sido destruídos
    void FixedUpdate()
    {
        objectsInArea.RemoveWhere(rb => rb == null);
        
        var destroyedObjects = new List<Rigidbody2D>();
        foreach (var kvp in objectForces)
        {
            if (kvp.Key == null)
                destroyedObjects.Add(kvp.Key);
        }
        
        foreach (var rb in destroyedObjects)
        {
            objectForces.Remove(rb);
        }
    }
    
    // Métodos públicos para controle
    public void ForceJam()
    {
        if (!isJammed && !isRecovering)
        {
            StartJam();
        }
    }
    
    public void SetCurveTime(float time)
    {
        currentCurveTime = time;
    }
    
    public bool IsJammed()
    {
        return isJammed;
    }
    
    public bool IsRecovering()
    {
        return isRecovering;
    }
    
    public float GetCurrentForce()
    {
        return currentForce;
    }
    
    public int GetObjectsInAreaCount()
    {
        return objectsInArea.Count;
    }
    
    void OnDestroy()
    {
        // Limpa todas as forças ao ser destruído
        objectsInArea.Clear();
        objectForces.Clear();
    }
}