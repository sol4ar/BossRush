﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.BurningPassion
{
    class BurningPassion : ModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.BossRushSetDefault(74, 74, 40, 6.7f, 28, 28, ItemUseStyleID.Shoot, true);
            Item.BossRushSetDefaultSpear(ModContent.ProjectileType<BurningPassionP>(), 3.7f);
            Item.rare = 3;
            Item.value = Item.sellPrice(silver: 1000);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                player.velocity = Vector2.Zero;

                player.velocity.X = velocity.X * 5f;
                player.velocity.Y = velocity.Y * 5f;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Spear, 2)
                .Register();
        }
    }
    class BurningPassionPlayer : ModPlayer
    {
        int check = 1;
        public override void PostUpdate()
        {
            if (Player.HeldItem.type == ModContent.ItemType<BurningPassion>())
            {
                if (!Player.ItemAnimationActive && check == 0)
                {
                    Player.velocity *= .25f;
                    check++;
                }
                else if (Player.ItemAnimationActive && Main.mouseRight)
                {
                    Player.gravity = 0;
                    Player.velocity.Y -= 0.3f;
                    Player.ignoreWater = true;
                    check = 0;
                }
            }
        }
        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            if (Player.ItemAnimationActive && Player.HeldItem.ModItem is BurningPassion && Player.ownedProjectileCounts[ModContent.ProjectileType<BurningPassionP>()] > 0)
            {
                return true;
            }
            return base.ImmuneTo(damageSource, cooldownCounter, dodgeable);
        }
    }
}
