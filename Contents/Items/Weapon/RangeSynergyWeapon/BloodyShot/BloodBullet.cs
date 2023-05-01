﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.BloodyShot
{
    internal class BloodBullet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            int ran = Main.rand.Next(7);
            if (ran == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 newPos = new Vector2(Projectile.position.X + Main.rand.Next(-500, 500) + 5, Projectile.position.Y - (600 + Main.rand.Next(1, 100)) + 5);
                    Vector2 aimto = Projectile.position - newPos;
                    Vector2 safeAimto = aimto.SafeNormalize(Vector2.UnitX);
                    Projectile.NewProjectile(new EntitySource_ItemUse_WithAmmo(player, new Item(ModContent.ItemType<BloodyShot>()), 0), newPos, safeAimto * 5, ModContent.ProjectileType<BloodBullet>(), hit.Damage, hit.Knockback, player.whoAmI);
                }
            }
            int randNum = 1 + Main.rand.Next(3, 6);
            for (int i = 0; i < randNum; i++)
            {
                Vector2 newPos = new Vector2(Projectile.position.X + Main.rand.Next(-200, 200) + 5, Projectile.position.Y - (600 + Main.rand.Next(1, 200)) + 5);
                Vector2 aimto = Projectile.position - newPos;
                Projectile.position.X += Main.rand.Next(-50, 50);
                Vector2 safeAimto = aimto.SafeNormalize(Vector2.UnitX);
                Projectile.NewProjectile(new EntitySource_ItemUse_WithAmmo(player, new Item(ModContent.ItemType<BloodyShot>()), 0), newPos, safeAimto * 25, ProjectileID.BloodArrow, (int)(hit.Damage * 0.75f), hit.Knockback, player.whoAmI);
            }

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor);
            return true;
        }
    }
}
