using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyBoss_RockFish : EnemyAI
{
    public AttackType attackType1;
    public AttackType attackType2;

    public int totalHp;


    Dictionary<string, List<GameObject>> dicComponents = new Dictionary<string, List<GameObject>>();

    public List<GameObject> breakAbleComponents = new List<GameObject>();
    public List<GameObject> unbreakAbleComponents = new List<GameObject>();
    public List<GameObject> weaknessComponents = new List<GameObject>();



    public override void SeePlayer()
    {
        ThisAttackType = Random.Range(0, 2) == 0 ? attackType1 : attackType2;
        print(ThisAttackType.ToString());
        base.SeePlayer();
    }

    public override IEnumerator Retreat()
    {
        base.Retreat();



        yield return null;
    }
}