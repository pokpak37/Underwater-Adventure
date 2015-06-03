using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;

    public int gameLevel = 0;
    public float increseLevelDelay = 30f; 
    float timer;


    public void CalculateLevel(int areaLevel, ref float protectionRange, ref float detectRange,
                                ref float hp, ref float hitDmg, ref float lineOfSightRange)
    {
        int totalLevel = gameLevel + areaLevel;
        protectionRange += 0.5f * totalLevel;
        detectRange += 0.5f * totalLevel;
        hp *= 0.2f * totalLevel;
        hitDmg *= 0.2f * totalLevel;
        lineOfSightRange += 0.5f * totalLevel;
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
