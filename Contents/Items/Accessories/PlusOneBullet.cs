﻿using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Accessories
{
    internal class PlusOneBullet : ModItem, ISynergyItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 28;
            Item.rare = 2;
            Item.value = 1000000;
        }
        public override void UpdateEquip(Player player)
        {
            RangeWeaponOverhaul.NumOfProModify += 1;
            RangeWeaponOverhaul.SpreadModify += .35f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Minishark)
                .AddIngredient(ItemID.CloudinaBottle)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
