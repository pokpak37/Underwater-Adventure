using UnityEngine;
using System.Collections;

public class Items : MonoBehaviour {

    public enum ItemType
    {
        Missile, Holming, Upgrade
    }

    public ItemType itemType;

    PlayerControl playerScript;

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
        player.subGuns[0].enabled = false;
        player.subGuns[1].enabled = true;
    }

    void Missile(PlayerControl player)
    {
        player.subGuns[0].enabled = true;
        player.subGuns[1].enabled = false;
    }

    void Upgrade(PlayerControl player)
    {
        foreach (Gun gun in player.mainGuns)
        {
            gun.LevelUp();
        }
    }

}
