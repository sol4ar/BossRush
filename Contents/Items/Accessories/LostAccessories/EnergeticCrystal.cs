﻿using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.Accessories.LostAccessories {
	class EnergeticOrb : ModItem {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void SetDefaults() {
			Item.DefaultToAccessory(28, 30);
			Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
			Item.value = 1000000;
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.statManaMax2 += 30;
			player.manaRegen += 15;
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MagicDMG, 1.06f);
			player.GetModPlayer<EnergeticOrbPlayer>().EnergeticOrb = true;
		}
	}
	public class EnergeticOrbPlayer : ModPlayer {
		public bool EnergeticOrb = false;
		public override void ResetEffects() {
			EnergeticOrb = false;
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (EnergeticOrb) {
				Player.statMana += Main.rand.Next(3, 10);
				if (Player.statMana > Player.statManaMax2) Player.statMana = Player.statManaMax2;
				Player.ManaEffect(Main.rand.Next(3, 10));
			}
		}
	}
}
