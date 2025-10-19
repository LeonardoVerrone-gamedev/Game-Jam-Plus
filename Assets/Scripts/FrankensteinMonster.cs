using UnityEngine;
using System.Collections.Generic;
using System;

public class FrankensteinMonster : MonoBehaviour
{
    [Header("Monster Parts")]
    public FrankensteinPartSO leftArm;
    public FrankensteinPartSO rightArm;
    public FrankensteinPartSO brain;
    public FrankensteinPartSO leftLeg;
    public FrankensteinPartSO rightLeg;
    
    [Header("Missing Parts")]
    public List<FrankensteinPartType> missingParts = new List<FrankensteinPartType>();

    [Header("Visual")]
    public SpriteRenderer leftArmRenderer;
    [SerializeField] ScratchAndStretch leftArmScratch;
    public SpriteRenderer rightArmRenderer;
    [SerializeField] ScratchAndStretch rightArmScratch;
    public SpriteRenderer brainRenderer;
    [SerializeField] ScratchAndStretch brainScratch;
    public SpriteRenderer leftLegRenderer;
    [SerializeField] ScratchAndStretch leftLegScratch;
    public SpriteRenderer rightLegRenderer;
    [SerializeField] ScratchAndStretch rightLegScratch;

    [SerializeField] ScratchAndStretch squash;

    [Header("FIre")]
    public bool OnFire;
    bool HasBeenSetOnFireOnce = false;
    [SerializeField] float minStartFireTime = 10f;
    [SerializeField] float maxStartFireTime = 100f;
    [SerializeField] ParticleSystem fireParticles;
    public int fireAmount = 5;

    [Header("Sound")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource fireAudioSource;

    int initialMissingParts = 0;


    void Start()
    {
        initialMissingParts = UnityEngine.Random.Range(1, 4);
        InitializeMissingParts(initialMissingParts);
        squash.PlayStretchAnimation("FrankEnter");
        StartRandomFire();
    }

    public void InitializeMissingParts(int numberOfMissingParts)
    {
        missingParts.Clear();

        // Cria lista de todas as partes possíveis
        List<FrankensteinPartType> allParts = new List<FrankensteinPartType>
        {
            FrankensteinPartType.leftArm,
            FrankensteinPartType.rightArm,
            FrankensteinPartType.Brain,
            FrankensteinPartType.leftLeg,
            FrankensteinPartType.rightLeg
        };

        // Remove partes aleatórias
        for (int i = 0; i < numberOfMissingParts && allParts.Count > 0; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, allParts.Count);
            FrankensteinPartType missingPart = allParts[randomIndex];
            missingParts.Add(missingPart);
            allParts.RemoveAt(randomIndex);

            // Remove a parte visualmente
            RemovePartVisual(missingPart);
        }
    }

    void StartRandomFire()
    {
        // Agenda o primeiro incêndio em um tempo aleatório
        float firstFireTime = UnityEngine.Random.Range(minStartFireTime, maxStartFireTime);
        Invoke(nameof(TriggerRandomFire), firstFireTime);
    }
    
    void TriggerRandomFire()
    {
        // Tenta colocar fogo
        SetOnFire();
        
        // Agenda o próximo incêndio
        float nextFireTime = UnityEngine.Random.Range(minStartFireTime, maxStartFireTime);
        Invoke(nameof(TriggerRandomFire), nextFireTime);
    }
    
    public bool CanAttachPart(FrankensteinPartSO part)
    {
        return missingParts.Contains(part.partType);
    }
    
    public bool AttachPart(FrankensteinPartSO part)
    {
        if (!CanAttachPart(part)) return false;

        // Atribui a parte ao monstro
        switch (part.partType)
        {
            case FrankensteinPartType.leftArm:
                leftArm = part;
                leftArmRenderer.sprite = part.partSprite;
                leftArmScratch.PlayStretchAnimation("PartPlacePattern");
                break;
            case FrankensteinPartType.rightArm:
                rightArm = part;
                rightArmRenderer.sprite = part.partSprite;
                rightArmScratch.PlayStretchAnimation("PartPlacePattern");
                break;
            case FrankensteinPartType.Brain:
                brain = part;
                brainRenderer.sprite = part.partSprite;
                brainScratch.PlayStretchAnimation("PartPlacePattern");
                break;
            case FrankensteinPartType.leftLeg:
                leftLeg = part;
                leftLegRenderer.sprite = part.partSprite;
                leftLegScratch.PlayStretchAnimation("PartPlacePattern");
                break;
            case FrankensteinPartType.rightLeg:
                rightLeg = part;
                rightLegRenderer.sprite = part.partSprite;
                rightLegScratch.PlayStretchAnimation("PartPlacePattern");
                break;
        }

        audioSource.clip = part.attachSound;
        audioSource.Play();
        
        // Remove da lista de partes faltantes
        missingParts.Remove(part.partType);

        // Toca som de attach
        if (part.attachSound != null)
        {
            AudioSource.PlayClipAtPoint(part.attachSound, transform.position);
        }

        if (IsComplete())
        {
            PlayOnCompleteSquashAnimation();
        }
        
        return true;
    }

    private void RemovePartVisual(FrankensteinPartType partType)
    {
        switch (partType)
        {
            case FrankensteinPartType.leftArm:
                leftArmRenderer.sprite = null;
                break;
            case FrankensteinPartType.rightArm:
                rightArmRenderer.sprite = null;
                break;
            case FrankensteinPartType.Brain:
                brainRenderer.sprite = null; //trocar para cabeça aberta dps
                break;
            case FrankensteinPartType.leftLeg:
                leftLegRenderer.sprite = null;
                break;
            case FrankensteinPartType.rightLeg:
                rightLegRenderer.sprite = null;
                break;
        }
    }

    public void EndOfLine()
    {
        squash.PlayStretchAnimation("FrankExit");
        float timeToFinish = squash.GetAnimationTime();

        Invoke("Destroy", 1f);
    }
    
    void Destroy()
    {
        Destroy(this.gameObject);
    }

    public bool IsComplete()
    {
        return missingParts.Count == 0;
    }

    public void SetOnFire()
    {
        if ((IsComplete() || HasBeenSetOnFireOnce || initialMissingParts <= 2) && !OnFire)
        {
            return;
        }

        var emission = fireParticles.emission;
        emission.rateOverTime = fireAmount;

        fireAudioSource.Play();

        OnFire = true;
        HasBeenSetOnFireOnce = true;
    }

    public void SetOffFire()
    {
        var emission = fireParticles.emission;
        emission.rateOverTime = 0f;

        fireAudioSource.Stop();

        OnFire = false;
        Debug.Log("Fogo apagado!");
    }

    public void PlayOnCompleteSquashAnimation()
    {
        squash.PlayStretchAnimation("FrankComplete");
    }
}