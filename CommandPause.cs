using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using On.RoR2;
using On.RoR2.UI;
using RoR2.UI;
using EntityStates;
using R2API.Networking;
using R2API.Networking.Interfaces;


namespace cabally
{
	//This is an example plugin that can be put in BepInEx/plugins/ExamplePlugin/ExamplePlugin.dll to test out.
	//It's a small plugin that adds a relatively simple item to the game, and gives you that item whenever you press F2.

	//This attribute specifies that we have a dependency on R2API, as we're using it to add our item to the game.
	//You don't need this if you're not using R2API in your plugin, it's just to tell BepInEx to initialize R2API before this plugin so it's safe to use R2API.
	//[BepInDependency("com.bepis.r2api")]	

	//This attribute is required, and lists metadata for your plugin.
	//[BepInPlugin(PluginGUID, PluginName, PluginVersion)]

	//We will be using 3 modules from R2API: ItemAPI to add our item, ItemDropAPI to have our item drop ingame, and LanguageAPI to add our language tokens.
	//[R2APISubmoduleDependency(nameof(ItemAPI), nameof(ItemDropAPI), nameof(LanguageAPI))]

	//This is the main declaration of our plugin class. BepInEx searches for all classes inheriting from BaseUnityPlugin to initialize on startup.
	//BaseUnityPlugin itself inherits from MonoBehaviour, so you can use this as a reference for what you can declare and use in your plugin class: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// Token: 0x02000002 RID: 2
	//[BepInDependency("com.bepis.r2api", 1)]
	[BepInPlugin("com.cabally.CommandPause", "CommandPause", "2.0.0")]
	public class MyModName : BaseUnityPlugin
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public void Awake()
		{
			On.RoR2.PickupPickerController.CreatePickup_PickupIndex += delegate (On.RoR2.PickupPickerController.orig_CreatePickup_PickupIndex orig, RoR2.PickupPickerController self, RoR2.PickupIndex PickupIndex)
			{
				orig.Invoke(self, PickupIndex);
				Time.timeScale = 1f;
			};
			On.RoR2.UI.PickupPickerPanel.Awake += delegate (On.RoR2.UI.PickupPickerPanel.orig_Awake orig, RoR2.UI.PickupPickerPanel self)
			{
				MethodInfo methodCached = Reflection.GetMethodCached(typeof(RoR2.CharacterMaster), "ToggleGod");
				bool flag = true;
				foreach (RoR2.PlayerCharacterMasterController playerCharacterMasterController in RoR2.PlayerCharacterMasterController.instances)
				{
					methodCached.Invoke(playerCharacterMasterController.master, null);
					bool flag2 = flag;
					if (flag2)
					{
						flag = false;
					}
				}

				//RoR2.CharacterBody.AddBuff(RoR2.LocalUserManager.GetFirstLocalUser ,RoR2.RoR2Content.Buffs.Cloak);
				RoR2.LocalUser user = RoR2.LocalUserManager.GetFirstLocalUser();
				RoR2.CharacterBody body = user.cachedBody;
				body.AddBuff(RoR2.RoR2Content.Buffs.Cloak);
				orig.Invoke(self);
			};
			On.RoR2.PickupPickerController.OnDisplayEnd += delegate (On.RoR2.PickupPickerController.orig_OnDisplayEnd orig, RoR2.PickupPickerController self, RoR2.NetworkUIPromptController NetworkUIPromptController, RoR2.LocalUser LocalUser, RoR2.CameraRigController CameraRigController)
			{
				orig.Invoke(self, NetworkUIPromptController, LocalUser, CameraRigController);
				new WaitForSeconds(5);
				MethodInfo methodCached = Reflection.GetMethodCached(typeof(RoR2.CharacterMaster), "ToggleGod");
				bool flag = false;
				foreach (RoR2.PlayerCharacterMasterController playerCharacterMasterController in RoR2.PlayerCharacterMasterController.instances)
				{
					methodCached.Invoke(playerCharacterMasterController.master, null);
					bool flag2 = flag;
					if (flag2)
					{
						flag = false;
					}
				}
				//base.characterBody.RemoveBuff(RoR2.RoR2Content.Buffs.CloakSpeed);
				RoR2.LocalUser user = RoR2.LocalUserManager.GetFirstLocalUser();
				RoR2.CharacterBody body = user.cachedBody;
				body.RemoveBuff(RoR2.RoR2Content.Buffs.Cloak);
			};
		}
	}
}
