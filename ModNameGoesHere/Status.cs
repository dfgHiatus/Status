using CloudX.Shared;
using FrooxEngine;
using HarmonyLib;
using NeosModLoader;
using System;

namespace Status
{
    public class Status : NeosMod
    {
        public override string Name => "Status";
        public override string Author => "dfgHiatus";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/dfgHiatus/Status";

        internal ModConfiguration config;

        [AutoRegisterConfigKey]
        internal ModConfigurationKey<string> status = new("customUserStatus", "Status", () => "Hello World!");

        public override void DefineConfiguration(ModConfigurationDefinitionBuilder builder)
        {
            builder
                .Version(new Version(1, 0, 0))
                .AutoSave(true);
        }

        public override void OnEngineInit()
        {
            new Harmony("net.dfgHiatus.Status").PatchAll();
            config = GetConfiguration();
            config.OnThisConfigurationChanged += OnConfigChange;
        }

        private async void OnConfigChange(ConfigurationChangedEvent configurationChangedEvent)
        {
            switch (configurationChangedEvent.Key.Name)
            {
                case "customUserStatus":
                    UserProfile profile = Engine.Current.Cloud.CurrentUser.Profile;
                    profile.Description = config.GetValue(status);
                    await Engine.Current.Cloud.UpdateProfile(profile);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(configurationChangedEvent.Key.Name);
            }
        }
    }
}