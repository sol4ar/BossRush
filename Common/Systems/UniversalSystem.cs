﻿using System;
using Terraria;
using Terraria.UI;
using Terraria.ID;
using System.Linq;
using ReLogic.Content;
using BossRush.Texture;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.ModLoader.IO;
using BossRush.Contents.Perks;
using Microsoft.Xna.Framework;
using BossRush.Contents.Skill;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using Terraria.GameContent.UI.States;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BossRush.Contents.Items.RelicItem;
using BossRush.Contents.WeaponEnchantment;
using System.Drawing.Drawing2D;
using Terraria.Localization;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.Artifacts;
using BossRush.Contents.Items.aDebugItem.RelicDebug;

namespace BossRush.Common.Systems;
/// <summary>
/// This not only include main stuff that make everything work but also contain some fixes to vanilla
/// </summary>
internal class UniversalSystem : ModSystem {
	public const string SYNERGY_MODE = "SynergyModeEnable";
	public const string BOSSRUSH_MODE = "ChallengeModeEnable";
	public const string NIGHTMARE_MODE = "NightmareEnable";
	public const string HARDCORE_MODE = "Hardcore";
	public const string TRUE_MODE = "TrueMode";
	public const string SYNERGYFEVER_MODE = "SynergyFeverMode";
	/// <summary>
	/// Use this to universally lock content behind hardcore, it basically act like a wrapper for <see cref="BossRushModConfig"/>
	/// </summary>
	/// <param name="player"></param>
	/// <param name="context">Use <see cref="BOSSRUSH_MODE"/> or any kind of mode that seem fit</param>
	/// <returns></returns>
	public static bool CanAccessContent(Player player, string context) {
		BossRushModConfig config = ModContent.GetInstance<BossRushModConfig>();
		if (config.HardEnableFeature || player.IsDebugPlayer())
			return true;
		if (context == NIGHTMARE_MODE)
			return config.Nightmare;
		if (context == HARDCORE_MODE)
			return player.difficulty == PlayerDifficultyID.Hardcore || config.AutoHardCore;
		if (player.difficulty != PlayerDifficultyID.Hardcore)
			return false;
		if (context == BOSSRUSH_MODE)
			return config.BossRushMode;
		if (context == SYNERGY_MODE)
			return config.SynergyMode;
		if (context == SYNERGYFEVER_MODE)
			return config.SynergyMode && config.SynergyFeverMode;
		if (context == TRUE_MODE)
			return config.SynergyMode && config.BossRushMode;
		return false;
	}
	/// <summary>
	/// Use this to lock content behind certain config, it basically act like a wrapper for <see cref="BossRushModConfig"/>
	/// </summary>
	/// <param name="context">Use <see cref="BOSSRUSH_MODE"/> or any kind of mode that seem fit</param>
	/// <returns></returns>
	public static bool CanAccessContent(string context) {
		BossRushModConfig config = ModContent.GetInstance<BossRushModConfig>();
		if (context == BOSSRUSH_MODE)
			return config.BossRushMode;
		if (config.HardEnableFeature)
			return true;
		if (context == NIGHTMARE_MODE)
			return config.Nightmare;
		if (context == HARDCORE_MODE)
			return config.AutoHardCore;
		if (context == SYNERGY_MODE)
			return config.SynergyMode;
		if (context == SYNERGYFEVER_MODE)
			return config.SynergyMode && config.SynergyFeverMode;
		if (context == TRUE_MODE)
			return config.SynergyMode && config.BossRushMode;
		return false;
	}
	internal UserInterface userInterface;
	internal UserInterface perkInterface;
	internal UserInterface skillInterface;
	internal UserInterface enchantInterface;
	internal UserInterface systemMenuInterface;
	internal UserInterface transmutationInterface;
	internal UserInterface relicTest;

	public EnchantmentUIState Enchant_uiState;
	public PerkUIState perkUIstate;
	public SkillUI skillUIstate;
	public DefaultUI defaultUI;
	public UISystemMenu UIsystemmenu;
	public TransmutationUIState transmutationUI;
	public RelicTransmuteUI relicUI;

	public static bool EnchantingState = false;
	public override void Load() {

		//UI stuff
		if (!Main.dedServ) {
			//Mod custom UI
			Enchant_uiState = new();
			enchantInterface = new();

			perkUIstate = new();
			perkInterface = new();

			skillUIstate = new();
			skillInterface = new();

			defaultUI = new();
			userInterface = new();

			UIsystemmenu = new();
			systemMenuInterface = new();

			transmutationUI = new();
			transmutationInterface = new();

			relicTest = new();
			relicUI = new();
		}
		On_UIElement.OnActivate += On_UIElement_OnActivate;
	}

	private void On_UIElement_OnActivate(On_UIElement.orig_OnActivate orig, UIElement self) {
		try {
			if (ModContent.GetInstance<BossRushModConfig>().AutoRandomizeCharacter) {
				if (self is UICharacterCreation el && Main.MenuUI.CurrentState is UICharacterCreation) {
					MethodInfo method = typeof(UICharacterCreation).GetMethod("Click_RandomizePlayer", BindingFlags.NonPublic | BindingFlags.Instance);
					method.Invoke(el, new object[] { null, null });
				}
			}
		}
		finally {
			orig(self);
		}
	}
	public override void Unload() {
		Enchant_uiState = null;
		perkUIstate = null;

		skillUIstate = null;
		defaultUI = null;

		userInterface = null;
		perkInterface = null;
		skillInterface = null;
		enchantInterface = null;
		UIsystemmenu = null;
		systemMenuInterface = null;
		transmutationUI = null;
		transmutationInterface = null;
		relicUI = null;
		relicTest = null;
	}
	public override void UpdateUI(GameTime gameTime) {
		userInterface?.Update(gameTime);
		perkInterface?.Update(gameTime);
		skillInterface?.Update(gameTime);
		enchantInterface?.Update(gameTime);
		systemMenuInterface?.Update(gameTime);
		transmutationInterface?.Update(gameTime);
		relicTest?.Update(gameTime);
	}
	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
		int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
		if (InventoryIndex != -1)
			layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
				"BossRush: UI",
				delegate {
					userInterface.Draw(Main.spriteBatch, new GameTime());
					perkInterface.Draw(Main.spriteBatch, new GameTime());
					skillInterface.Draw(Main.spriteBatch, new GameTime());
					enchantInterface.Draw(Main.spriteBatch, new GameTime());
					systemMenuInterface.Draw(Main.spriteBatch, new GameTime());
					transmutationInterface.Draw(Main.spriteBatch, new GameTime());
					relicTest.Draw(Main.spriteBatch, new GameTime());
					return true;
				},
				InterfaceScaleType.UI)
			);
	}
	public void ActivatePerkUI(short state) {
		DeactivateUI();
		perkUIstate.StateofState = state;
		perkInterface.SetState(perkUIstate);
	}
	public void ActivateSkillUI() {
		DeactivateUI();
		skillInterface.SetState(skillUIstate);
	}
	public void ActivateEnchantmentUI() {
		DeactivateUI();
		enchantInterface.SetState(Enchant_uiState);
	}
	public void ActivateTransmutationUI() {
		DeactivateUI();
		transmutationInterface.SetState(transmutationUI);
	}
	public void ActivateRelicUI() {
		DeactivateUI();
		relicTest.SetState(relicUI);
	}
	public void DeactivateUI() {
		perkInterface.SetState(null);
		skillInterface.SetState(null);
		enchantInterface.SetState(null);
		systemMenuInterface.SetState(null);
		transmutationInterface.SetState(null);
		relicTest.SetState(null);
	}
}
public class UniversalGlobalBuff : GlobalBuff {
	public override void SetStaticDefaults() {
		//I am unsure why this is set to true
		Main.debuff[BuffID.Campfire] = false;
		Main.debuff[BuffID.Honey] = false;
		Main.debuff[BuffID.StarInBottle] = false;
		Main.debuff[BuffID.HeartLamp] = false;
		Main.debuff[BuffID.CatBast] = false;
		Main.debuff[BuffID.Sunflower] = false;
	}
}
public class UniversalGlobalItem : GlobalItem {
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (!UniversalSystem.EnchantingState)
			return;
		for (int i = 0; i < tooltips.Count; i++) {
			if (tooltips[i].Name == "ItemName") {
				string tooltipText = "No enchantment can be found";
				if (EnchantmentLoader.GetEnchantmentItemID(item.type) != null) {
					tooltipText = EnchantmentLoader.GetEnchantmentItemID(item.type).Description;
				}
				tooltips[i].Text = tooltipText;
				continue;
			}
			tooltips[i].Hide();
		}
	}
}
public class UniversalModPlayer : ModPlayer {
	public override void OnEnterWorld() {
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		uiSystemInstance.userInterface.SetState(uiSystemInstance.defaultUI);
		uiSystemInstance.perkInterface.SetState(null);
		uiSystemInstance.skillInterface.SetState(null);
		uiSystemInstance.enchantInterface.SetState(null);
		if (!UniversalSystem.CanAccessContent(Player, UniversalSystem.HARDCORE_MODE) && WarnAlready == 0) {
			WarnAlready = 1;
		}
	}
	public override bool CanUseItem(Item item) {
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		if (uiSystemInstance.perkInterface.CurrentState != null
			|| uiSystemInstance.enchantInterface.CurrentState != null
			|| uiSystemInstance.skillInterface.CurrentState != null) {
			return false;
		}
		return base.CanUseItem(item);
	}
	int WarnAlready = 0;
	public override void SaveData(TagCompound tag) {
		tag.Add("WarnAlready", WarnAlready);
	}
	public override void LoadData(TagCompound tag) {
		WarnAlready = (int)tag["WarnAlready"];
	}
}

class DefaultUI : UIState {
	// For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
	// Once this is all set up make sure to go and do the required stuff for most UI's in the ModSystem class.
	private UIText text;
	private UIElement area;
	private UIImage barFrame;
	private Color gradientA;
	private Color gradientB;
	private UIText text2;
	private UIElement area2;
	private UIImage barFrame2;
	private Color gradientA2;
	private Color gradientB2;

	private UITextPanel<string> popUpWarning;
	private UITextPanel<string> popUpWarningClose;

	private UIImageButton staticticUI;
	public override void OnInitialize() {
		CreateEnergyBar();
		CreateCoolDownBar();
		staticticUI = new UIImageButton(TextureAssets.InventoryBack);
		staticticUI.UISetWidthHeight(52, 52);
		staticticUI.HAlign = .67f;
		staticticUI.VAlign = .06f;
		staticticUI.OnLeftClick += StaticticUI_OnLeftClick;
		Append(staticticUI);
	}

	private void StaticticUI_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (ModContent.GetInstance<UniversalSystem>().systemMenuInterface.CurrentState == null) {
			ModContent.GetInstance<UniversalSystem>().DeactivateUI();
			ModContent.GetInstance<UniversalSystem>().systemMenuInterface.SetState(ModContent.GetInstance<UniversalSystem>().UIsystemmenu);
		}
		else {
			ModContent.GetInstance<UniversalSystem>().systemMenuInterface.SetState(null);
		}
	}

	public override void OnActivate() {
		if (!UniversalSystem.CanAccessContent(Main.LocalPlayer, UniversalSystem.HARDCORE_MODE)) {
			popUpWarning = new UITextPanel<string>("Terraria: Roguelike is only compatible with freshly created Hardcore characters\nAs a result, the mod will be temporarily disabled until you leave the world.");
			popUpWarning.Height.Set(66, 0);
			popUpWarning.HAlign = .5f;
			popUpWarning.VAlign = .5f;
			Append(popUpWarning);
			popUpWarningClose = new UITextPanel<string>("Close Disclaimer");
			popUpWarningClose.HAlign = .5f;
			popUpWarningClose.VAlign = .6f;
			popUpWarningClose.OnLeftClick += PopUpWarning_OnLeftClick;
			Append(popUpWarningClose);
		}
	}

	private void PopUpWarning_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		Elements.Remove(popUpWarning);
		Elements.Remove(popUpWarningClose);
	}

	private void CreateEnergyBar() {
		area = new UIElement();
		area.Left.Set(-area.Width.Pixels - 600, 1f); // Place the resource bar to the left of the hearts.
		area.Top.Set(30, 0f); // Placing it just a bit below the top of the screen.
		area.Width.Set(182, 0f); // We will be placing the following 2 UIElements within this 182x60 area.
		area.Height.Set(60, 0f);

		barFrame = new UIImage(ModContent.Request<Texture2D>(BossRushTexture.EXAMPLEUI)); // Frame of our resource bar
		barFrame.Left.Set(22, 0f);
		barFrame.Top.Set(0, 0f);
		barFrame.Width.Set(138, 0f);
		barFrame.Height.Set(34, 0f);

		text = new UIText("0/0", 0.8f); // text to show stat
		text.Width.Set(138, 0f);
		text.Top.Set(40, 0f);
		text.Left.Set(0, 0f);

		gradientA = new Color(123, 25, 138); // A dark purple
		gradientB = new Color(207, 111, 221); // A light purple

		area.Append(text);
		area.Append(barFrame);
		Append(area);
	}
	private void CreateCoolDownBar() {
		area2 = new UIElement();
		area2.Left.Set(-area2.Width.Pixels - 600, 1f);
		area2.Top.Set(80, 0f);
		area2.Width.Set(182, 0f);
		area2.Height.Set(60, 0f);

		barFrame2 = new UIImage(ModContent.Request<Texture2D>(BossRushTexture.EXAMPLEUI));
		barFrame2.Left.Set(22, 0f);
		barFrame2.Top.Set(0, 0f);
		barFrame2.Width.Set(138, 0f);
		barFrame2.Height.Set(34, 0f);

		text2 = new UIText("0/0", 0.8f);
		text2.Width.Set(138, 0f);
		text2.Height.Set(34, 0f);
		text2.Top.Set(40, 0f);
		text2.Left.Set(0, 0f);

		gradientA2 = new Color(123, 25, 138);
		gradientB2 = new Color(207, 111, 221);

		area2.Append(text2);
		area2.Append(barFrame2);
		Append(area2);
	}
	protected override void DrawSelf(SpriteBatch spriteBatch) {
		base.DrawSelf(spriteBatch);
		DrawSkillProgressBarUI(spriteBatch);
	}
	private void DrawSkillProgressBarUI(SpriteBatch spriteBatch) {
		var modPlayer = Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>();
		// Calculate quotient
		float quotient = (float)modPlayer.Energy / modPlayer.EnergyCap; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
		quotient = Math.Clamp(quotient, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

		// Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
		Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
		hitbox.X += 12;
		hitbox.Width -= 24;
		hitbox.Y += 8;
		hitbox.Height -= 16;

		// Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
		int left = hitbox.Left;
		int right = hitbox.Right;
		int steps = (int)((right - left) * quotient);
		for (int i = 0; i < steps; i += 1) {
			// float percent = (float)i / steps; // Alternate Gradient Approach
			float percent = (float)i / (right - left);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
		}

		if (modPlayer.CoolDown > 0 && modPlayer.MaximumCoolDown > 0) {
			float quotient2 = modPlayer.CoolDown / (float)modPlayer.MaximumCoolDown; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
			quotient2 = Math.Clamp(quotient2, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

			// Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
			Rectangle hitbox2 = barFrame2.GetInnerDimensions().ToRectangle();
			hitbox2.X += 12;
			hitbox2.Width -= 24;
			hitbox2.Y += 8;
			hitbox2.Height -= 16;

			// Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
			int left2 = hitbox2.Left;
			int right2 = hitbox2.Right;
			int steps2 = (int)((right2 - left2) * quotient2);
			for (int i = 0; i < steps2; i += 1) {
				// float percent = (float)i / steps; // Alternate Gradient Approach
				float percent = (float)i / (right2 - left2);
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left2 + i, hitbox2.Y, 1, hitbox2.Height), Color.Lerp(gradientA2, gradientB2, percent));
			}
		}
	}

	public override void Update(GameTime gameTime) {
		var modPlayer = Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>();
		// Setting the text per tick to update and show our resource values.
		text.SetText($"Energy : {modPlayer.Energy}/{modPlayer.EnergyCap}");
		if (modPlayer.CoolDown > 0) {
			text2.SetText($"CoolDown : {MathF.Round(modPlayer.CoolDown / 60f, 2)}");
		}
		else {
			text2.SetText("");
		}

		if (staticticUI.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		base.Update(gameTime);
	}
}
class UISystemMenu : UIState {
	UIPanel panel;
	UITextPanel<string> uitextpanel;
	UIImageButton show_playerMod_Info;
	UIImageButton open_skill_UI;
	UIImageButton open_Enchantment_UI;
	UIImageButton open_Transmutation_UI;
	bool EnchantmentHover = false;
	bool SkillHover = false;
	bool InfoHover = false;
	bool Transmutation = false;
	public override void OnInitialize() {
		panel = new UIPanel();
		panel.HAlign = .5f;
		panel.VAlign = .4f;
		panel.UISetWidthHeight(360, 80);
		Append(panel);

		uitextpanel = new UITextPanel<string>(" ");
		uitextpanel.HAlign = .5f;
		uitextpanel.VAlign = .7f;
		uitextpanel.UISetWidthHeight(450, 350);
		Append(uitextpanel);

		show_playerMod_Info = new UIImageButton(TextureAssets.InventoryBack);
		show_playerMod_Info.UISetWidthHeight(52, 52);
		show_playerMod_Info.VAlign = .4f;
		show_playerMod_Info.HAlign = .43f;
		show_playerMod_Info.SetVisibility(1f, 67f);
		show_playerMod_Info.OnLeftClick += Show_playerMod_Info_OnLeftClick;
		show_playerMod_Info.OnUpdate += Show_playerMod_Info_OnUpdate;
		Append(show_playerMod_Info);

		open_skill_UI = new UIImageButton(TextureAssets.InventoryBack);
		open_skill_UI.UISetWidthHeight(52, 52);
		open_skill_UI.VAlign = .4f;
		open_skill_UI.HAlign = .475f;
		open_skill_UI.SetVisibility(1f, 67f);
		open_skill_UI.OnLeftClick += Open_skill_UI_OnLeftClick;
		open_skill_UI.OnUpdate += Open_skill_UI_OnUpdate;
		Append(open_skill_UI);

		open_Enchantment_UI = new UIImageButton(TextureAssets.InventoryBack);
		open_Enchantment_UI.UISetWidthHeight(52, 52);
		open_Enchantment_UI.VAlign = .4f;
		open_Enchantment_UI.HAlign = .525f;
		open_Enchantment_UI.SetVisibility(1f, 67f);
		open_Enchantment_UI.OnLeftClick += Open_Enchantment_UI_OnLeftClick;
		open_Enchantment_UI.OnUpdate += Open_Enchantment_UI_OnUpdate;
		Append(open_Enchantment_UI);

		open_Transmutation_UI = new UIImageButton(TextureAssets.InventoryBack);
		open_Transmutation_UI.UISetWidthHeight(52, 52);
		open_Transmutation_UI.VAlign = .4f;
		open_Transmutation_UI.HAlign = .57f;
		open_Transmutation_UI.SetVisibility(1f, 67f);
		open_Transmutation_UI.OnLeftClick += Open_Transmutation_UI_OnLeftClick; ;
		open_Transmutation_UI.OnUpdate += Open_Transmutation_UI_OnUpdate; ;
		Append(open_Transmutation_UI);
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (EnchantmentHover) {
			if (Main.LocalPlayer.ActiveArtifact() != Artifact.ArtifactType<GamblerSoulArtifact>()) {
				uitextpanel.SetText(Language.GetTextValue($"Mods.BossRush.SystemTooltip.WeaponEnchantment.Tooltip"));
			}
			else {
				uitextpanel.SetText(Language.GetTextValue($"Mods.BossRush.SystemTooltip.BlockWeaponEnchantment.Tooltip"));
			}
		}
		else if (SkillHover) {
			uitextpanel.SetText(Language.GetTextValue($"Mods.BossRush.SystemTooltip.Skill.Tooltip"));
		}
		else if (InfoHover) {
			uitextpanel.SetText(Language.GetTextValue($"Mods.BossRush.SystemTooltip.ShowPlayerInfo.Tooltip"));
		}
		else if (Transmutation) {
			uitextpanel.SetText(Language.GetTextValue($"Mods.BossRush.SystemTooltip.Transmutation.Tooltip"));
		}
		else {
			uitextpanel.SetText("");
		}
	}

	private void Open_Transmutation_UI_OnUpdate(UIElement affectedElement) {
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		// Otherwise, we can check a child element instead
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (affectedElement.IsMouseHovering) {
			Transmutation = true;
		}
		else {
			Transmutation = false;
		}
	}

	private void Open_skill_UI_OnUpdate(UIElement affectedElement) {
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		// Otherwise, we can check a child element instead
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (affectedElement.IsMouseHovering) {
			SkillHover = true;
		}
		else {
			SkillHover = false;
		}
	}

	private void Open_Transmutation_UI_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		ModContent.GetInstance<UniversalSystem>().ActivateTransmutationUI();
	}

	private void Open_Enchantment_UI_OnUpdate(UIElement affectedElement) {
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		// Otherwise, we can check a child element instead
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (affectedElement.IsMouseHovering) {
			EnchantmentHover = true;
		}
		else {
			EnchantmentHover = false;
		}
	}

	private void Show_playerMod_Info_OnUpdate(UIElement affectedElement) {
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		// Otherwise, we can check a child element instead
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (affectedElement.IsMouseHovering) {
			InfoHover = true;
		}
		else {
			InfoHover = false;
		}
	}

	private void Open_Enchantment_UI_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (Main.LocalPlayer.ActiveArtifact() != Artifact.ArtifactType<GamblerSoulArtifact>()) {
			ModContent.GetInstance<UniversalSystem>().ActivateEnchantmentUI();
		}
	}

	private void Open_skill_UI_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		ModContent.GetInstance<UniversalSystem>().ActivateSkillUI();
	}

	private void Show_playerMod_Info_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {

	}
}
internal class SkillUI : UIState {
	public List<btn_SkillSlotHolder> skill = new List<btn_SkillSlotHolder>();
	public List<btn_SkillSlotHolder> inventory = new List<btn_SkillSlotHolder>();
	public ExitUI exitUI;
	public btn_SkillDeletion btn_delete;
	public const string UItype_SKILL = "skill";
	public const string UIType_INVENTORY = "inventory";
	public UIPanel panel;
	public UIText energyCostText;
	public UIText durationText;
	public UIText cooldownText;
	public override void OnInitialize() {
		panel = new UIPanel();
		Append(panel);
		energyCostText = new UIText("");
		Append(energyCostText);
		durationText = new UIText("");
		Append(durationText);
		cooldownText = new UIText("");
		Append(cooldownText);
	}

	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		SkillHandlePlayer modplayer = Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>();
		modplayer.SkillStatTotal(out int energy, out int duration, out int cooldown);
		Color color =  energy <= modplayer.EnergyCap ? Color.Green : Color.Red;
		energyCostText.SetText($"[c/{color.Hex3()}:Energy cost = {energy}]");
		durationText.SetText($"Duration = {MathF.Round(duration / 60f, 2)}s");
		cooldownText.SetText($"Cool down = {MathF.Round(cooldown / 60f, 2)}s");
	}

	private void ActivateSkillUI(Player player) {
		panel.UISetWidthHeight(200, 90);
		panel.Left.Pixels = 860;
		panel.Top.Pixels = 330;
		energyCostText.Top.Pixels = 349;
		energyCostText.Left.Pixels = 880;
		durationText.Top.Pixels = 370;
		durationText.Left.Pixels = 880;
		cooldownText.Top.Pixels = 390;
		cooldownText.Left.Pixels = 880;
		if (player.TryGetModPlayer(out SkillHandlePlayer modplayer)) {
			//Explain : since most likely in the future we aren't gonna expand the skill slot, we just hard set it to 10
			//We are also pre render these UI first
			int[] SkillHolder = modplayer.GetCurrentActiveSkillHolder();
			Vector2 textureSize = new Vector2(52, 52);
			Vector2 OffSetPosition_Skill = player.Center;
			OffSetPosition_Skill.X -= textureSize.X * 5;
			if (skill.Count < 1) {
				Vector2 customOffSet = OffSetPosition_Skill;
				customOffSet.Y -= 60;
				for (int i = 0; i < 3; i++) {
					btn_SkillSlotSelection btn_Selection = new btn_SkillSlotSelection(TextureAssets.InventoryBack7, i + 1);
					btn_Selection.UISetPosition(customOffSet + new Vector2(52, 0) * i, textureSize);
					Append(btn_Selection);
				}
				for (int i = 0; i < 10; i++) {
					btn_SkillSlotHolder skillslot = new btn_SkillSlotHolder(TextureAssets.InventoryBack, i, SkillHolder[i], UItype_SKILL);
					skillslot.UISetPosition(OffSetPosition_Skill + new Vector2(52, 0) * i, textureSize);
					skill.Add(skillslot);
					Append(skill[i]);
				}
			}
			if (inventory.Count < 1) {
				Vector2 InvOffSet = new Vector2(520, -55);
				for (int i = 0; i < 30; i++) {
					btn_SkillSlotHolder skillslot = new btn_SkillSlotHolder(TextureAssets.InventoryBack, i, modplayer.SkillInventory[i], UIType_INVENTORY);
					Vector2 InvPos = OffSetPosition_Skill + new Vector2(0, 72);
					if (i >= 10) {
						InvPos -= InvOffSet;
					}
					if (i >= 20) {
						InvPos -= InvOffSet;
					}
					skillslot.UISetPosition(InvPos + new Vector2(52, 0) * i, textureSize);
					inventory.Add(skillslot);
					Append(inventory[i]);
				}
			}
			if (exitUI == null) {
				exitUI = new ExitUI(TextureAssets.InventoryBack10);
				exitUI.UISetPosition(player.Center + new Vector2(300, 0), textureSize);
				Append(exitUI);
			}
			if (btn_delete == null) {
				btn_delete = new btn_SkillDeletion(TextureAssets.InventoryBack, modplayer);
				btn_delete.UISetPosition(player.Center - new Vector2(330, 0), textureSize);
				Append(btn_delete);
			}
		}
	}
	public override void OnActivate() {
		Player player = Main.LocalPlayer;
		ActivateSkillUI(player);
	}
	public override void OnDeactivate() {
		SkillModSystem.SelectInventoryIndex = -1;
		SkillModSystem.SelectSkillIndex = -1;
	}
}
class btn_SkillSlotSelection : UIImage {
	int SelectionIndex = 0;
	public btn_SkillSlotSelection(Asset<Texture2D> texture, int selection) : base(texture) {
		SelectionIndex = selection;
	}
	public override void LeftClick(UIMouseEvent evt) {
		base.LeftClick(evt);
		if (SelectionIndex == 0) {
			return;
		}
		Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>().ChangeHolder(SelectionIndex);
	}
	public override void Draw(SpriteBatch spriteBatch) {
		if (SelectionIndex != Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>().CurrentActiveIndex) {
			Color = new Color(255, 255, 255, 100);
		}
		else {
			Color = Color.White;
		}
		base.Draw(spriteBatch);
	}
}
class btn_SkillDeletion : UIImage {
	SkillHandlePlayer modplayer;
	Vector2 size;
	public btn_SkillDeletion(Asset<Texture2D> texture, SkillHandlePlayer modplayer) : base(texture) {
		this.modplayer = modplayer;
		size = texture.Size();
	}
	public override void LeftClick(UIMouseEvent evt) {
		if (SkillModSystem.SelectInventoryIndex != -1) {
			modplayer.RequestSkillRemoval_SkillInventory(SkillModSystem.SelectInventoryIndex);
			SkillModSystem.SelectInventoryIndex = -1;
		}
		if (SkillModSystem.SelectSkillIndex != -1) {
			modplayer.RequestSkillRemoval_SkillHolder(SkillModSystem.SelectSkillIndex);
			SkillModSystem.SelectSkillIndex = -1;
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + size * .5f;
		Texture2D trashbin = TextureAssets.Trash.Value;
		float scaling = ScaleCalculation(size, trashbin.Size());
		Vector2 origin = trashbin.Size() * .5f;
		spriteBatch.Draw(trashbin, drawpos, null, new Color(0, 0, 0, 150), 0, origin, scaling, SpriteEffects.None, 0);
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
class btn_SkillSlotHolder : UIImageButton {
	public int whoAmI = -1;
	public int sKillID = -1;
	public string uitype = "";
	Texture2D Texture;
	public btn_SkillSlotHolder(Asset<Texture2D> texture, int WhoAmI, int SkillID, string UItype) : base(texture) {
		//player = Tplayer;
		whoAmI = WhoAmI;
		sKillID = SkillID;
		Texture = texture.Value;
		uitype = UItype;
		SetVisibility(1, .67f);
	}
	public override void LeftClick(UIMouseEvent evt) {
		Player player = Main.LocalPlayer;
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		//Moving skill around in inventory
		if (uitype == SkillUI.UIType_INVENTORY) {
			if (SkillModSystem.SelectInventoryIndex == -1) {
				if (SkillModSystem.SelectSkillIndex == -1) {
					//This mean the player haven't select anything
					SkillModSystem.SelectInventoryIndex = whoAmI;
				}
				else {
					//Player are Attempting to move a skill from their skill slot back to inventory
					modplayer.ReplaceSkillFromSkillHolderToInv(SkillModSystem.SelectSkillIndex, whoAmI);
					SkillModSystem.SelectSkillIndex = -1;
				}
			}
			else {
				//Player are moving skill around their inventory
				int cache = modplayer.SkillInventory[whoAmI];
				modplayer.SkillInventory[whoAmI] = modplayer.SkillInventory[SkillModSystem.SelectInventoryIndex];
				modplayer.SkillInventory[SkillModSystem.SelectInventoryIndex] = cache;
				SkillModSystem.SelectInventoryIndex = -1;
				//It is impossible where SelectSkillIndex can't be equal to -1
			}
		}
		else if (uitype == SkillUI.UItype_SKILL) {
			if (SkillModSystem.SelectSkillIndex == -1) {
				if (SkillModSystem.SelectInventoryIndex == -1) {
					//This mean the player haven't select anything
					SkillModSystem.SelectSkillIndex = whoAmI;
				}
				else {
					//Player are Attempting to move a skill from their inventory into a skill holder
					modplayer.ReplaceSkillFromInvToSkillHolder(whoAmI, SkillModSystem.SelectInventoryIndex);
					SkillModSystem.SelectInventoryIndex = -1;
				}
			}
			else {
				//Player are moving skill around their skill holder
				modplayer.SwitchSkill(whoAmI, SkillModSystem.SelectSkillIndex);
				SkillModSystem.SelectSkillIndex = -1;
			}
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		Player player = Main.LocalPlayer;
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (uitype == SkillUI.UIType_INVENTORY) {
			if (modplayer.SkillInventory[whoAmI] != sKillID) {
				sKillID = modplayer.SkillInventory[whoAmI];
			}
		}
		else if (uitype == SkillUI.UItype_SKILL) {
			int[] skillholder = modplayer.GetCurrentActiveSkillHolder();
			if (skillholder[whoAmI] != sKillID) {
				sKillID = skillholder[whoAmI];
			}
		}
		if (IsMouseHovering) {
			string tooltipText = "";
			string Name = "";
			if (SkillLoader.GetSkill(sKillID) != null) {
				Name = SkillLoader.GetSkill(sKillID).DisplayName;
				tooltipText = SkillLoader.GetSkill(sKillID).Description;
				tooltipText +=
					$"\n[c/{Color.Yellow.Hex3()}:Skill duration] : {Math.Round(SkillLoader.GetSkill(sKillID).Duration / 60f, 2)}s" +
					$"\n[c/{Color.DodgerBlue.Hex3()}:Energy require] : {SkillLoader.GetSkill(sKillID).EnergyRequire}" +
					$"\n[c/{Color.OrangeRed.Hex3()}:Skill cooldown] : {Math.Round(SkillLoader.GetSkill(sKillID).CoolDown / 60f, 2)}s";
			}
			Main.instance.MouseText(Name + "\n" + tooltipText);
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		if (sKillID < 0 || sKillID >= SkillLoader.TotalCount) {
			return;
		}
		Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + Texture.Size() * .5f;
		Texture2D skilltexture = ModContent.Request<Texture2D>(SkillLoader.GetSkill(sKillID).Texture).Value;
		Vector2 origin = skilltexture.Size() * .5f;
		float scaling = ScaleCalculation(Texture.Size(), skilltexture.Size());
		spriteBatch.Draw(skilltexture, drawpos, null, new Color(255, 255, 255), 0, origin, scaling, SpriteEffects.None, 0);
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
internal class PerkUIState : UIState {
	public const short DefaultState = 0;
	public const short StarterPerkState = 1;
	public const short DebugState = 2;
	public const short GamblerState = 3;
	public short StateofState = 0;
	public UIText toolTip;
	public override void OnActivate() {
		Elements.Clear();
		Player player = Main.LocalPlayer;
		if (player.TryGetModPlayer(out PerkPlayer modplayer)) {
			if (StateofState == DefaultState) {
				ActivateNormalPerkUI(modplayer, player);
			}
			if (StateofState == StarterPerkState) {
				ActivateStarterPerkUI(modplayer, player);
			}
			if (StateofState == DebugState) {
				ActivateDebugPerkUI(modplayer, player);
			}
			if (StateofState == GamblerState) {
				ActivateGamblerUI(modplayer, player);
			}
		}
		toolTip = new UIText("");
		Append(toolTip);
	}
	private void ActivateDebugPerkUI(PerkPlayer modplayer, Player player) {
		int amount = ModPerkLoader.TotalCount;
		Vector2 originDefault = new Vector2(26, 26);
		for (int i = 0; i < amount; i++) {
			Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(amount + 1, 360, i) * Math.Clamp(amount * 20, 0, 260);
			Asset<Texture2D> texture;
			if (ModPerkLoader.GetPerk(i).textureString is not null)
				texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(i).textureString);
			else
				texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
			//After that we assign perk
			PerkUIImageButton btn = new PerkUIImageButton(texture);
			btn.UISetWidthHeight(52, 52);
			btn.UISetPosition(player.Center + offsetPos, originDefault);
			btn.perkType = i;
			Append(btn);
			ModPerkLoader.GetPerk(i);
		}
	}
	private void ActivateNormalPerkUI(PerkPlayer modplayer, Player player) {
		List<int> listOfPerk = new List<int>();
		for (int i = 0; i < ModPerkLoader.TotalCount; i++) {
			if (modplayer.perks.ContainsKey(i)) {
				if ((!ModPerkLoader.GetPerk(i).CanBeStack && modplayer.perks[i] > 0)
					|| modplayer.perks[i] >= ModPerkLoader.GetPerk(i).StackLimit) {
					continue;
				}
			}
			if (!ModPerkLoader.GetPerk(i).CanBeChoosen) {
				continue;
			}
			listOfPerk.Add(i);
		}
		int amount = listOfPerk.Count;
		Vector2 originDefault = new Vector2(26, 26);
		int perkamount = modplayer.PerkAmountModified();
		for (int i = 0; i < perkamount; i++) {
			Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(perkamount, 360, i) * Math.Clamp(perkamount * 20, 0, 200);
			int newperk = Main.rand.Next(listOfPerk);
			Asset<Texture2D> texture;
			if (ModPerkLoader.GetPerk(newperk).textureString is not null)
				texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(newperk).textureString);
			else
				texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
			if (i >= amount || i >= perkamount - 1) {
				newperk = Main.rand.Next(new int[] { Perk.GetPerkType<SuppliesDrop>(), Perk.GetPerkType<GiftOfRelic>() });
				if (ModPerkLoader.GetPerk(newperk).textureString is not null)
					texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(newperk).textureString);
				else
					texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
				PerkUIImageButton buttonWeapon = new PerkUIImageButton(texture);
				buttonWeapon.perkType = newperk;
				buttonWeapon.UISetWidthHeight(52, 52);
				buttonWeapon.UISetPosition(player.Center + offsetPos, originDefault);
				Append(buttonWeapon);
				continue;
			}
			listOfPerk.Remove(newperk);
			//After that we assign perk
			PerkUIImageButton btn = new PerkUIImageButton(texture);
			btn.UISetWidthHeight(52, 52);
			btn.UISetPosition(player.Center + offsetPos, originDefault);
			btn.perkType = newperk;
			Append(btn);
		}
	}
	private void ActivateStarterPerkUI(PerkPlayer modplayer, Player player) {
		Vector2 originDefault = new Vector2(26, 26);
		int[] starterPerk = new int[]
		{ Perk.GetPerkType<BlessingOfSolar>(),
			Perk.GetPerkType<BlessingOfVortex>(),
			Perk.GetPerkType<BlessingOfNebula>(),
			Perk.GetPerkType<BlessingOfStarDust>(),
			Perk.GetPerkType<BlessingOfSynergy>(),
			Perk.GetPerkType<BlessingOfTitan>(),
		};
		for (int i = 0; i < starterPerk.Length; i++) {
			Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(starterPerk.Length, 360, i) * starterPerk.Length * 20;
			//After that we assign perk
			if (modplayer.perks.ContainsKey(starterPerk[i])) {
				if (modplayer.perks[starterPerk[i]] >= ModPerkLoader.GetPerk(starterPerk[i]).StackLimit) {
					continue;
				}
			}
			PerkUIImageButton btn = new PerkUIImageButton(ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(starterPerk[i]).textureString));
			btn.UISetWidthHeight(52, 52);
			btn.UISetPosition(player.Center + offsetPos, originDefault);
			btn.perkType = starterPerk[i];
			Append(btn);
		}
	}
	private void ActivateGamblerUI(PerkPlayer modplayer, Player player) {
		Vector2 originDefault = new Vector2(26, 26);
		int[] starterPerk = new int[]
		{ Perk.GetPerkType<UncertainStrike>(),
			Perk.GetPerkType<StrokeOfLuck>(),
			Perk.GetPerkType<BlessingOfPerk>()
		};
		for (int i = 0; i < starterPerk.Length; i++) {
			Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(starterPerk.Length, 360, i) * starterPerk.Length * 20;
			//After that we assign perk
			if (modplayer.perks.ContainsKey(starterPerk[i])) {
				if (modplayer.perks[starterPerk[i]] >= ModPerkLoader.GetPerk(starterPerk[i]).StackLimit) {
					continue;
				}
			}
			PerkUIImageButton btn = new PerkUIImageButton(ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(starterPerk[i]).textureString));
			btn.UISetWidthHeight(52, 52);
			btn.UISetPosition(player.Center + offsetPos, originDefault);
			btn.perkType = starterPerk[i];
			Append(btn);
		}
	}
}
//Do all the check in UI state since that is where the perk actually get create and choose
class PerkUIImageButton : UIImageButton {
	public int perkType;
	public PerkUIImageButton(Asset<Texture2D> texture) : base(texture) {
	}
	public override void LeftClick(UIMouseEvent evt) {
		PerkPlayer perkplayer = Main.LocalPlayer.GetModPlayer<PerkPlayer>();
		UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		if (ModPerkLoader.GetPerk(perkType) != null) {
			if (ModPerkLoader.GetPerk(perkType).StackLimit == -1 && ModPerkLoader.GetPerk(perkType).CanBeStack) {
				ModPerkLoader.GetPerk(perkType).OnChoose(perkplayer.Player);
				uiSystemInstance.DeactivateUI();
				return;
			}
		}
		if (perkplayer.perks.Count < 0 || !perkplayer.perks.ContainsKey(perkType))
			perkplayer.perks.Add(perkType, 1);
		else
			if (perkplayer.perks.ContainsKey(perkType) && ModPerkLoader.GetPerk(perkType).CanBeStack)
			perkplayer.perks[perkType]++;
		ModPerkLoader.GetPerk(perkType).OnChoose(perkplayer.Player);
		uiSystemInstance.DeactivateUI();
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (IsMouseHovering && ModPerkLoader.GetPerk(perkType) != null) {
			Main.instance.MouseText(ModPerkLoader.GetPerk(perkType).DisplayName + "\n" + ModPerkLoader.GetPerk(perkType).Description);
		}
		else {
			if (!Parent.Children.Where(e => e.IsMouseHovering).Any()) {
				Main.instance.MouseText("");
			}
		}
	}
}
internal class EnchantmentUIState : UIState {
	WeaponEnchantmentUIslot weaponEnchantmentUIslot;
	ExitUI weaponEnchantmentUIExit;
	public override void OnInitialize() {
		weaponEnchantmentUIslot = new WeaponEnchantmentUIslot(TextureAssets.InventoryBack);
		weaponEnchantmentUIslot.UISetWidthHeight(52, 52);
		weaponEnchantmentUIslot.UISetPosition(Main.LocalPlayer.Center + Vector2.UnitX * 120, new Vector2(26, 26));
		Append(weaponEnchantmentUIslot);
		weaponEnchantmentUIExit = new ExitUI(TextureAssets.InventoryBack13);
		weaponEnchantmentUIExit.UISetWidthHeight(52, 52);
		weaponEnchantmentUIExit.UISetPosition(Main.LocalPlayer.Center + Vector2.UnitX * 178, new Vector2(26, 26));
		Append(weaponEnchantmentUIExit);
	}
	public override void OnDeactivate() {
		int count = Children.Count();
		for (int i = count - 1; i >= 0; i--) {
			UIElement child = Children.ElementAt(i);
			if (child is EnchantmentUIslot wmslot) {
				if (wmslot.itemOwner == null) {
					continue;
				}
				else {
					child.Deactivate();
					child.Remove();
				}
			}
			if (child is UIText) {
				child.Deactivate();
				child.Remove();
			}
		}
	}
}
public class WeaponEnchantmentUIslot : UIImage {
	public int WhoAmI = -1;
	public Texture2D textureDraw;
	public Item item;

	private Texture2D texture;
	public WeaponEnchantmentUIslot(Asset<Texture2D> texture) : base(texture) {
		this.texture = texture.Value;
	}
	List<int> textUqID = new List<int>();
	public override void LeftClick(UIMouseEvent evt) {
		Player player = Main.LocalPlayer;
		if (Main.mouseItem.type != ItemID.None) {
			if (Main.mouseItem.consumable)
				return;
			Item itemcached;
			if (item != null && item.type != ItemID.None) {
				itemcached = item.Clone();
				item = Main.mouseItem.Clone();
				Main.mouseItem = itemcached.Clone();
				player.inventory[58] = itemcached.Clone();
			}
			else {
				item = Main.mouseItem.Clone();
				Main.mouseItem.TurnToAir();
				player.inventory[58].TurnToAir();
				UniversalSystem.EnchantingState = true;
			}
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalItem)) {
				int length = globalItem.EnchantmenStlot.Length;
				for (int i = 0; i < length; i++) {
					Vector2 pos = player.Center + Vector2.UnitY * 60 + Vector2.UnitX * 60 * i;
					EnchantmentUIslot slot = new EnchantmentUIslot(TextureAssets.InventoryBack);
					slot.UISetWidthHeight(52, 52);
					slot.UISetPosition(pos, new Vector2(26, 26));
					slot.WhoAmI = i;
					slot.itemOwner = item;
					slot.itemType = globalItem.EnchantmenStlot[i];
					Parent.Append(slot);
					UIText text = new UIText($"{i + 1}");
					text.UISetPosition(pos + Vector2.UnitY * 56, new Vector2(26, 26));
					textUqID.Add(text.UniqueId);
					Parent.Append(text);
				}
			}
		}
		else {
			if (item == null)
				return;
			UniversalSystem.EnchantingState = false;
			Main.mouseItem = item;
			item = null;
			int count = Parent.Children.Count();
			for (int i = count - 1; i >= 0; i--) {
				UIElement child = Parent.Children.ElementAt(i);
				if (child is EnchantmentUIslot wmslot) {
					if (wmslot.itemOwner == null)
						continue;
				}
				if (child is EnchantmentUIslot { itemOwner: not null }) {
					child.Deactivate();
					child.Remove();
				}
				if (child is UIText text && textUqID.Contains(text.UniqueId)) {
					textUqID.Remove(text.UniqueId);
					child.Deactivate();
					child.Remove();
				}
			}
		}
	}
	public override void OnDeactivate() {
		textUqID.Clear();
		Player player = Main.LocalPlayer;
		UniversalSystem.EnchantingState = false;
		if (item == null)
			return;
		for (int i = 0; i < 50; i++) {
			if (player.CanItemSlotAccept(player.inventory[i], item)) {
				player.inventory[i] = item.Clone();
				item = null;
				return;
			}
		}
		player.DropItem(player.GetSource_DropAsItem(), player.Center, ref item);
	}
	public override void Draw(SpriteBatch spriteBatch) {
		Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + texture.Size() * .5f;
		base.Draw(spriteBatch);
		if (item != null) {
			Main.instance.LoadItem(item.type);
			Texture2D texture = TextureAssets.Item[item.type].Value;
			Vector2 origin = texture.Size() * .5f;
			float scaling = ScaleCalculation(texture.Size()) * .78f;
			spriteBatch.Draw(texture, drawpos, null, Color.White, 0, origin, scaling, SpriteEffects.None, 0);
		}
		else {

			Texture2D backgroundtexture = TextureAssets.Item[ItemID.SilverBroadsword].Value;
			spriteBatch.Draw(backgroundtexture, drawpos, null, new Color(0, 0, 0, 80), 0, texture.Size() * .35f, ScaleCalculation(backgroundtexture.Size()), SpriteEffects.None, 0);
		}
	}
	private float ScaleCalculation(Vector2 textureSize) => texture.Size().Length() / (textureSize.Length() * 1.5f);
}
public class EnchantmentUIslot : UIImage {
	public int itemType = 0;
	public int WhoAmI = -1;

	public Item itemOwner = null;
	private Texture2D texture;
	public EnchantmentUIslot(Asset<Texture2D> texture) : base(texture) {
		this.texture = texture.Value;
	}
	public override void LeftClick(UIMouseEvent evt) {
		if (itemOwner == null)
			return;
		if (Main.mouseItem.type != ItemID.None) {
			if (Main.mouseItem.consumable)
				return;
			if (itemType != 0)
				return;
			if (EnchantmentLoader.GetEnchantmentItemID(Main.mouseItem.type) == null)
				return;
			itemType = Main.mouseItem.type;
			Main.mouseItem.TurnToAir();
			Main.LocalPlayer.inventory[58].TurnToAir();
			if (itemOwner.TryGetGlobalItem(out EnchantmentGlobalItem globalItem)) {
				globalItem.EnchantmenStlot[WhoAmI] = itemType;
			}
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		try {
			if (itemOwner == null)
				return;
			if (itemType != 0) {
				Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + texture.Size() * .5f;
				Main.instance.LoadItem(itemType);
				Texture2D texture1 = TextureAssets.Item[itemType].Value;
				Vector2 origin = texture1.Size() * .5f;
				spriteBatch.Draw(texture1, drawpos, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
			}
		}
		catch (Exception ex) {
			Main.NewText(ex.Message);
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (itemType == ItemID.None)
			return;
		if (IsMouseHovering) {
			string tooltipText = "No enchantment can be found";
			if (EnchantmentLoader.GetEnchantmentItemID(itemType) != null) {
				tooltipText = EnchantmentLoader.GetEnchantmentItemID(itemType).Description;
			}
			Main.instance.MouseText(tooltipText);
		}
		else {
			if (!Parent.Children.Where(e => e.IsMouseHovering).Any()) {
				Main.instance.MouseText("");
			}
		}
	}
}
