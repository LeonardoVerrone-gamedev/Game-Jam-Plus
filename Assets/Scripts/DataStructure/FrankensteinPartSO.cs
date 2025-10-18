using UnityEngine;

[CreateAssetMenu(fileName = "New Frankenstein Part", menuName = "Frankenstein/Part")]
public class FrankensteinPartSO : ScriptableObject
{
    public FrankensteinPartType partType;
    public string partName;
    public Sprite partSprite;
    public int qualityScore = 1; // Para pontuação ou qualidade da parte
    public AudioClip attachSound;
}