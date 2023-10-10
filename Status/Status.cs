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

    internal ModConfiguration config;

    [AutoRegisterConfigKey]
    internal ModConfigurationKey<string> tagLine = 
        new("tagLine", "Tagline", () => "Tagline");

    [AutoRegisterConfigKey]
    internal ModConfigurationKey<string> description = 
        new("description", "Description", () => "Description");

    [AutoRegisterConfigKey]
    internal ModConfigurationKey<string> profileWorldUri = 
        new("profileWorldUri", "Featured World URL", () => "");

    public override void OnEngineInit()
    {
        new Harmony("net.dfgHiatus.Status").PatchAll();
        config = GetConfiguration();
        config.OnThisConfigurationChanged += OnConfigChange;
    }

    private async void OnConfigChange(ConfigurationChangedEvent configurationChangedEvent)
    {
        UserProfile profile = Engine.Current.Cloud.CurrentUser.Profile;

        // Update everything else
        switch (configurationChangedEvent.Key.Name)
        {
            case "tagLine":
                profile.Tagline = config.GetValue(tagLine);
                break;
            case "description":
                profile.Description = config.GetValue(description);
                break;
            case "profileWorldUri":
                profile.IconUrl = config.GetValue(profileWorldUri);
                break;
            default:
                throw new ArgumentOutOfRangeException(configurationChangedEvent.Key.Name);
        }

        await Engine.Current.Cloud.Profile.UpdateProfile(profile);
    }
}