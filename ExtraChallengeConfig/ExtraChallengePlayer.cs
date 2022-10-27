﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.ExtraChallengeConfig
{
    public class ExtraChallengePlayer : ModPlayer
    {
        public int ChallengeChooser = 0;
        public int ClassChooser = 0;
        public int BossSlayedCount = 0;
        public bool WASDChallenge = false;//Todo : figure out how to do this
        public bool OnlyUseOneClass = false;
        public bool Badbuff = false;
        public bool spawnRatex3 = false;
        public bool strongerEnemy = false;

        public override void PostUpdate()
        {
            if(Badbuff)
            {
                Player.AddBuff(BuffID.Darkness, 10);
                Player.AddBuff(BuffID.Confused, 10);
                Player.AddBuff(BuffID.Weak, 10);
                Player.AddBuff(BuffID.Silenced, 10);
                Player.AddBuff(BuffID.ManaSickness, 10);
                Player.AddBuff(BuffID.BrokenArmor, 10);
                Player.AddBuff(BuffID.Bleeding, 10);
                Player.AddBuff(BuffID.WitheredArmor, 10);
                Player.AddBuff(BuffID.WitheredWeapon, 10);
                Player.AddBuff(BuffID.NoBuilding, 10);
            }
        }

        public override bool CanUseItem(Item item)
        {
            if(OnlyUseOneClass)
            {   
                if (item.damage > 0)
                {
                    if (item.DamageType == DamageClass.Melee && ClassChooser == 0)
                    {
                        return true;
                    }
                    if (item.DamageType == DamageClass.Ranged && ClassChooser == 1)
                    {
                        return true;
                    }
                    if (item.DamageType == DamageClass.Magic && ClassChooser == 2)
                    {
                        return true;
                    }
                    if (item.DamageType == DamageClass.Summon && ClassChooser == 3)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return true;
        }
    }
}
