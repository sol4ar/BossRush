﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using BossRush.Contents.Items.Weapon;
using BossRush.Contents.BuffAndDebuff;

namespace BossRush.Contents.Items.Accessories.SynergyAccessories.FuryEmblem {
	class FuryEmblem : SynergyModItem {
		public override void SetDefaults() {
			Item.accessory = true;
			Item.height = 40;
			Item.width = 40;
			Item.rare = ItemRarityID.Lime;
			Item.value = 10000000;
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.05f);
			modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 5);
			modplayer.AddStatsToPlayer(PlayerStats.MaxHP, 1.25f);
			player.GetModPlayer<FuryPlayer>().Furious2 = true;
		}
		public override void AddRecipes() {
			CreateRecipe()
		   .AddIngredient(ItemID.AvengerEmblem, 1)
		   .AddIngredient(ItemID.Vitamins, 1)
		   .Register();
		}
	}
	class FuryPlayer : ModPlayer {
		public bool Furious2 = false;
		public override void ResetEffects() {
			Furious2 = false;
		}
		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
			if (Furious2 && !Player.HasBuff<FuriousCoolDown>()) Player.AddBuff(ModContent.BuffType<Furious>(), 600);
		}
		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
			if (Furious2 && !Player.HasBuff<FuriousCoolDown>()) Player.AddBuff(ModContent.BuffType<Furious>(), 600);
		}
	}
}
