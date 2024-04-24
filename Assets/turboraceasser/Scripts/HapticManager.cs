using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;


public enum HapticType
{
    LightImpact,
    MediumImpact,
    HeavyImpact,
    Success,
    Failure
}

public class HapticManager : Singleton<HapticManager>
{

    private void Start()
    {
        HapticController.Init();
    }

    public void PlayHaptic(HapticType _hapticType)
    {
        //        if (PlayerPrefs.GetInt(Constants.Haptic) == 1) return;

        //        bool hapticSupported = true;

        //#if UNITY_IOS
        //        hapticSupported = DeviceCapabilities.isVersionSupported;
        //#endif

        bool hapticSupported = true;

        if (hapticSupported)
        {
            switch (_hapticType)
            {
                case HapticType.LightImpact:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
                    break;

                case HapticType.MediumImpact:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
                    break;

                case HapticType.HeavyImpact:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
                    break;

                case HapticType.Success:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
                    break;

                case HapticType.Failure:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
                    break;

                default:
                    break;
            }
        }
        else
        {
            Handheld.Vibrate();
        }

    }
}
