﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Artifact
{
    internal class VampirismCrystal : ModItem, IArtifactItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 7));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 58);
            Item.UseSound = SoundID.Zombie105;
            Item.rare = ItemRarityID.Cyan;
        }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID = 3;
            return true;
        }
    }
}