using UnityEngine;
using System.Collections;

public class Items : MonoBehaviour {

    public enum ItemType
    {
        Missile, Holming, Upgrade
    }

    public ItemType itemType;

    PlayerControl playerScript;

    void Start()
    {
        StartCoroutine(Swing());
    }

    IEnumerator Swing()
    {
        for (; ; )
        {

            yield return null;
        }
    }

    public void ActiveItem(PlayerControl player)
    {
        playerScript = player;
        switch (itemType)
        {
            case ItemType.Holming: Holming(playerScript);
                break;
            case ItemType.Missile: Missile(playerScript);
                break;
            case ItemType.Upgrade: Upgrade(playerScript);
                break;                
        }
    }

    void Holming(PlayerControl player)
    {
        player.subGuns[0].SetActive(false);
        player.subGuns[1].SetActive(true);
    }

    void Missile(PlayerControl player)
    {
        player.subGuns[0].SetActive(true);
        player.subGuns[1].SetActive(false);
    }

    void Upgrade(PlayerControl player)
    {
        foreach (Gun gun in player.mainGuns)
        {
            gun.LevelUp();
        }
    }

}
