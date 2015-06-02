using UnityEngine;
using System.Collections;

public class LevelControl : MonoBehaviour {

    public static LevelControl instance;

    public int gameLevel = 0;

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

}
