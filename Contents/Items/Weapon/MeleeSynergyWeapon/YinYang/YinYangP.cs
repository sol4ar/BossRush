﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.YinYang
{
    internal class YinYangP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 25f;
        }

        public override void SetDefaults()
        {
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 400f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 12;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 99;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 24;
            Projectile.height = 24;
        }
        Player player;
        int count = 0;
        int charge = 0;
        public override void AI()
        {
            if (count == 0)
            {
                player = Main.player[Projectile.owner];
                Projectile.ai[1] = player.ownedProjectileCounts[Projectile.type];
                count++;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            charge++;
            if (charge == 17 && player.yoyoGlove == false)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<YinYangShockWave>(), 0, 0, player.whoAmI);
                Projectile.damage = (int)(Projectile.damage * 1.75f);
                ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 24;
                ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 600f;
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<YinLight>(), (int)(Projectile.damage * 0.75f), 2f, player.whoAmI, Projectile.ai[1]);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<YangDark>(), (int)(Projectile.damage * 0.75f), 2f, player.whoAmI, Projectile.ai[1]);
            }
            if (charge > 17 || player.yoyoGlove == true)
            {
                Vector2 RotatePos = Main.rand.NextVector2CircularEdge(75f, 75f) * 5 + Projectile.position;
                Vector2 Aimto = (target.Center - RotatePos).SafeNormalize(Vector2.UnitX) * 3;
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), RotatePos, Aimto, ModContent.ProjectileType<YinLight>(), (int)(Projectile.damage * 0.75f), 2f, player.whoAmI);
                RotatePos = Main.rand.NextVector2CircularEdge(75f, 75f) * 5 + Projectile.position;
                Aimto = (target.Center - RotatePos).SafeNormalize(Vector2.UnitX) * 3;
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), RotatePos, Aimto, ModContent.ProjectileType<YangDark>(), (int)(Projectile.damage * 0.75f), 2f, player.whoAmI);
            }
        }
    }

    abstract class BaseBoltProjectile : ModProjectile
    {
        protected Projectile GetYoyoMainProjectile()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].ModProjectile is YinYangP && Main.projectile[i].ai[1] == Projectile.ai[0])
                {
                    return Main.projectile[i];
                }
            }
            return null;
        }
        public Vector2 getPosToReturn(float offSet, int Counter, float Distance = 50)
        {
            Vector2 Rotate = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(offSet));
            Projectile proj = GetYoyoMainProjectile();
            Vector2 ProjectileMainCenter = proj != null ? proj.Center : Vector2.Zero;
            return ProjectileMainCenter + Rotate.RotatedBy(Counter * 0.05f) * Distance;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Projectile.light = 1f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor, .02f);
            return true;
        }
    }
}
