﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.Chat;

namespace BossRush.ExtraChallengeConfig
{
    internal class ExtraChallengeSystem : ModSystem
    {
        public bool BoulderRain;
        public bool Hellfirerain;
        public bool Closecombatfight;
        int count = 0;
        int NumberToCompare = 0;
        public const int BoulderRainCooldown = 60;
        public int BoulderRainCountTime = 0;
        public const int HellFireRainCoolDown = 30;
        public int HellFireRainCount = 0;
        public override void PostUpdateWorld()
        {
            if (ModContent.GetInstance<BossRushModConfig>().ExtraChallenge)
            {
                Player player = Main.LocalPlayer;
                if (NumberToCompare != player.GetModPlayer<ExtraChallengePlayer>().BossSlayedCount)
                {
                    count--;
                    player.GetModPlayer<ExtraChallengePlayer>().WASDChallenge = false;
                    player.GetModPlayer<ExtraChallengePlayer>().OnlyUseOneClass = false;
                    player.GetModPlayer<ExtraChallengePlayer>().spawnRatex3 = false;
                    BoulderRain = false;
                    player.GetModPlayer<ExtraChallengePlayer>().strongerEnemy = false;
                    Hellfirerain = false;
                    player.GetModPlayer<ExtraChallengePlayer>().Badbuff = false;
                    Closecombatfight = false;
                }
                if (count == 0)
                {
                    switch (player.GetModPlayer<ExtraChallengePlayer>().ChallengeChooser)
                    {
                        case 1:
                            player.GetModPlayer<ExtraChallengePlayer>().WASDChallenge = true;
                            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("No WASD (W.I.P)"), Colors.RarityDarkRed);
                            break;
                        case 2:
                            player.GetModPlayer<ExtraChallengePlayer>().ClassChooser = Main.rand.Next(4);
                            player.GetModPlayer<ExtraChallengePlayer>().OnlyUseOneClass = true;
                            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("restrict to 1 class"), Colors.RarityDarkRed);
                            break;
                        case 3:
                            player.GetModPlayer<ExtraChallengePlayer>().spawnRatex3 = true;
                            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("massive increase spawn rate"), Colors.RarityDarkRed);
                            break;
                        case 4:
                            BoulderRain = true;//done ?
                            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("boulder rain time"), Colors.RarityDarkRed);
                            break;
                        case 5:
                            player.GetModPlayer<ExtraChallengePlayer>().strongerEnemy = true;
                            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("enemy get extra defense"), Colors.RarityDarkRed);
                            break;
                        case 6:
                            Hellfirerain = true; //done
                            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("hell fire rain"), Colors.RarityDarkRed);
                            break;
                        case 7:
                            player.GetModPlayer<ExtraChallengePlayer>().Badbuff = true;
                            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("very nasty debuff"), Colors.RarityDarkRed);
                            break;
                    }
                    NumberToCompare = player.GetModPlayer<ExtraChallengePlayer>().BossSlayedCount;
                    count++;
                }
                //Custom stuff
                if (BoulderRain)
                {
                    if (player.active)
                    {
                        if (BoulderRainCooldown == BoulderRainCountTime)
                        {
                            Vector2 spawn = new Vector2(Main.rand.Next(-1000, 1000) + player.Center.X, -1000 + player.Center.Y);
                            int projectile = Projectile.NewProjectile(null, spawn, Vector2.Zero, ProjectileID.Boulder, 400, 10f, Main.myPlayer, 0f, 0f);
                            Main.projectile[projectile].hostile = true;
                            BoulderRainCountTime = 0;
                        }
                        else
                        {
                            BoulderRainCountTime++;
                        }
                    }
                }
                if (Hellfirerain)
                {
                    if (player.active)
                    {
                        if (HellFireRainCount == HellFireRainCoolDown)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                Vector2 spawn = new Vector2(Main.rand.Next(-1500, 1500) + player.Center.X, -1000 + player.Center.Y);
                                int projectile = Projectile.NewProjectile(null, spawn, Vector2.Zero, ProjectileID.HellfireArrow, 10, 1f, Main.myPlayer, 0f, 0f);
                                Main.projectile[projectile].hostile = true;
                                HellFireRainCount = 0;
                            }
                        }
                        else
                        {
                            HellFireRainCount++;
                        }
                    }
                }
            }
        }
    }
}
