using CloudX.Shared;
using FrooxEngine;
using HarmonyLib;
using NeosModLoader;
using System;
using System.Linq;
using System.Reflection;

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
        internal ModConfigurationKey<string> tagLine = new("tagLine", "Tagline", () => "Tagline");

        [AutoRegisterConfigKey]
        internal ModConfigurationKey<string> description = new("description", "Description", () => "Description");

        [AutoRegisterConfigKey]
        internal ModConfigurationKey<string> backgroundUri = new("backgroundUri", "Background Image URL", () => "");

        [AutoRegisterConfigKey]
        internal ModConfigurationKey<string> profileWorldUri = new("profileWorldUri", "Featured World URL", () => "");

        // According to art0007i#2305, there are 6 maximum featured items one can have
        [AutoRegisterConfigKey]
        internal ModConfigurationKey<string> featuredItemUri_1 = new("featuredItemUri1", "Featured Item #1 URL", () => "");
        [AutoRegisterConfigKey]
        internal ModConfigurationKey<string> featuredItemUri_2 = new("featuredItemUri2", "Featured Item #2 URL", () => "");
        [AutoRegisterConfigKey]
        internal ModConfigurationKey<string> featuredItemUri_3 = new("featuredItemUri3", "Featured Item #3 URL", () => "");
        [AutoRegisterConfigKey]
        internal ModConfigurationKey<string> featuredItemUri_4 = new("featuredItemUri4", "Featured Item #4 URL", () => "");
        [AutoRegisterConfigKey]
        internal ModConfigurationKey<string> featuredItemUri_5 = new("featuredItemUri5", "Featured Item #5 URL", () => "");
        [AutoRegisterConfigKey]
        internal ModConfigurationKey<string> featuredItemUri_6 = new("featuredItemUri6", "Featured Item #6 URL", () => "");

        public override void DefineConfiguration(ModConfigurationDefinitionBuilder builder)
        {
            builder
                .Version(new Version(1, 1, 0))
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
            UserProfile profile = Engine.Current.Cloud.CurrentUser.Profile;
            switch (configurationChangedEvent.Key.Name)
            {
                case "tagLine": 
                    profile.Tagline = config.GetValue(tagLine);
                    break;
                case "description":
                    profile.Description = config.GetValue(description);
                    break;
                case "backgroundUri":
                    profile.BackgroundUrl = config.GetValue(backgroundUri);
                    break;
                case "profileWorldUri":
                    profile.ProfileWorldUrl = config.GetValue(profileWorldUri);
                    break;
                case "featuredItemUri1":
                    profile.ShowcaseItems[0] = config.GetValue(featuredItemUri_1);   
                    break;
                case "featuredItemUri2":
                    profile.ShowcaseItems[1] = config.GetValue(featuredItemUri_2);
                    break;
                case "featuredItemUri3":
                    profile.ShowcaseItems[2] = config.GetValue(featuredItemUri_3);
                    break;
                case "featuredItemUri4":
                    profile.ShowcaseItems[3] = config.GetValue(featuredItemUri_4);
                    break;
                case "featuredItemUri5":
                    profile.ShowcaseItems[4] = config.GetValue(featuredItemUri_5);
                    break;
                case "featuredItemUri6":
                    profile.ShowcaseItems[5] = config.GetValue(featuredItemUri_6);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(configurationChangedEvent.Key.Name);
            }
            await Engine.Current.Cloud.UpdateProfile(profile);
        }
    }
}