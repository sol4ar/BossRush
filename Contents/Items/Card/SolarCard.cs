﻿using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Card
{
    internal class SolarCard : Card
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FragmentSolar);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increase your chance to get melee weapon from chest by 50% !");
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
            modplayer.ChestLoot.MeleeChanceMutilplier += .5f;
        }
    }
    internal class VortexCard : Card
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FragmentVortex);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increase your chance to get range weapon from chest by 50% !");
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
            modplayer.ChestLoot.RangeChanceMutilplier += .5f;
        }
    }
    internal class NebulaCard : Card
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FragmentNebula);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increase your chance to get magic weapon from chest by 50% !");
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
            modplayer.ChestLoot.MagicChanceMutilplier += .5f;
        }
    }
    internal class StarDustCard : Card
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FragmentStardust);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increase your chance to get summoner weapon from chest by 50% !");
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
            modplayer.ChestLoot.SummonChanceMutilplier += .5f;
        }
    }
    internal class ResetCard : Card
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.LunarBar);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("For Debug purpose only !" +
                "\n Reset all of the card that manipulate the chance from chest");
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
            modplayer.ChestLoot.MeleeChanceMutilplier = 1f;
            modplayer.ChestLoot.RangeChanceMutilplier = 1f;
            modplayer.ChestLoot.MagicChanceMutilplier = 1f;
            modplayer.ChestLoot.SummonChanceMutilplier = 1f;
        }
    }
    internal class EmptyCard : Card
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Glass);
        public override void SetDefaults()
        {
            Item.width = 0;
            Item.height = 0;
            Item.material = true;
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Empty card, you can fill something in it");
        }
        public override bool CanBeCraft => false;
    }
}
