// FrankensteinRecipeSO.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Frankenstein Recipe", menuName = "Frankenstein/Recipe")]
public class FrankensteinRecipeSO : ScriptableObject
{
    public string recipeName;
    public int requiredScore = 5;
    
    [Header("Required Parts")]
    public FrankensteinPartSO leftArm;
    public FrankensteinPartSO rightArm;
    public FrankensteinPartSO brain;
    public FrankensteinPartSO leftLeg;
    public FrankensteinPartSO rightLeg;
    
    public int GetCurrentScore(FrankensteinMonster monster)
    {
        int score = 0;
        if (monster.leftArm != null && monster.leftArm == leftArm) score += monster.leftArm.qualityScore;
        if (monster.rightArm != null && monster.rightArm == rightArm) score += monster.rightArm.qualityScore;
        if (monster.brain != null && monster.brain == brain) score += monster.brain.qualityScore;
        if (monster.leftLeg != null && monster.leftLeg == leftLeg) score += monster.leftLeg.qualityScore;
        if (monster.rightLeg != null && monster.rightLeg == rightLeg) score += monster.rightLeg.qualityScore;
        return score;
    }
    
    public bool IsRecipeComplete(FrankensteinMonster monster)
    {
        return GetCurrentScore(monster) >= requiredScore;
    }
}