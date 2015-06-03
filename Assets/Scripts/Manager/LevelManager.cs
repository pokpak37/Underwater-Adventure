using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;

    public int gameLevel = 0;
    public float increseLevelDelay = 30f;

    void Awake()
    {
        instance = this;
    }

    public EnemyAI CalculateLevel(EnemyAI targetAI, int areaLevel)
    {
        int totalLevel = gameLevel + areaLevel;
        targetAI.protectionRange += 0.5f * totalLevel;
        targetAI.detectRange += 0.5f * totalLevel;
        targetAI.hp *= 0.2f * totalLevel;
        targetAI.hitDmg *= 0.2f * totalLevel;
        targetAI.lineOfSightRange += 0.5f * totalLevel;
        return targetAI;
    }


    public void ActiveLevelUp()
    {
        StartCoroutine(GameLevelUp());
    }

    IEnumerator GameLevelUp()
    {
        for(;;)
        {
            yield return new WaitForSeconds(increseLevelDelay);
            gameLevel++;
            yield return null;
        }
    }    

}
