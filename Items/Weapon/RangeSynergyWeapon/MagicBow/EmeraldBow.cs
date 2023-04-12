﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class EmeraldBow : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("well at least it don't consume arrow");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 32;

            Item.damage = 17;
            Item.knockBack = 1f;

            Item.useTime = 28;
            Item.useAnimation = 28;

            Item.rare = 2;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.mana = 12;

            Item.shoot = ModContent.ProjectileType<EmeraldBolt>();
            Item.shootSpeed = 6f;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item75;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 CircularRan = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
                CircularRan.X += Main.rand.Next(-3, 3);
                CircularRan.Y += Main.rand.Next(-3, 3);
                Dust.NewDustPerfect(position, DustID.GemEmerald, CircularRan, 100, default, 0.5f);
            }
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<EmeraldBolt>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TungstenBow)
                .AddIngredient(ItemID.EmeraldStaff)
                .Register();
        }
    }
}
