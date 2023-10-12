using FrooxEngine;
using HarmonyLib;
using ResoniteModLoader;
using SkyFrost.Base;
using System;

namespace Status;

public class Status : ResoniteMod
{
    public override string Name => "Status";
    public override string Author => "dfgHiatus";
    public override string Version => "2.0.0";
    public override string Link => "https://github.com/dfgHiatus/Status";

    internal static ModConfiguration Config;

    [AutoRegisterConfigKey]
    internal static readonly ModConfigurationKey<string> tagLine = 
        new("tagLine", "Tagline", () => "Tagline");

    [AutoRegisterConfigKey]
    internal static readonly ModConfigurationKey<string> description = 
        new("description", "Description", () => "Description");

    [AutoRegisterConfigKey]
    internal static readonly ModConfigurationKey<string> profileWorldUri = 
        new("profileWorldUri", "Featured World URL", () => "");

    public override void OnEngineInit()
    {
        new Harmony("net.dfgHiatus.Status").PatchAll();
        Config = GetConfiguration();
        Config.OnThisConfigurationChanged += OnConfigChange;
    }

    private async void OnConfigChange(ConfigurationChangedEvent configurationChangedEvent)
    {
        UserProfile profile = Engine.Current.Cloud.CurrentUser.Profile;

        switch (configurationChangedEvent.Key.Name)
        {
            case "tagLine":
                profile.Tagline = Config.GetValue(tagLine);
                break;
            case "description":
                profile.Description = Config.GetValue(description);
                break;
            case "profileWorldUri":
                profile.IconUrl = Config.GetValue(profileWorldUri);
                break;
            default:
                throw new ArgumentOutOfRangeException(configurationChangedEvent.Key.Name);
        }

        await Engine.Current.Cloud.Profile.UpdateProfile(profile);
    }
}