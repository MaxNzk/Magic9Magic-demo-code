using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Volume))]
    public class CVDFilter : MonoBehaviour
    {
        [SerializeField] private CVDProfilesSO _profile;
        private int _profileIndex;

        [SerializeField, Space(10)] private Image _rgbColorImage;

        private Volume postProcessVolume;

        public void SetCurrentColorType(string value)
        {
            if (value == "OFF") _profileIndex = 0;
            if (value == "Protanopia") _profileIndex = 1;
            if (value == "Protanomaly") _profileIndex = 2;
            if (value == "Deuteranopia") _profileIndex = 3;
            if (value == "Deuteranomaly") _profileIndex = 4;
            if (value == "Tritanopia") _profileIndex = 5;
            if (value == "Tritanomaly") _profileIndex = 6;
            if (value == "Achromatopsia") _profileIndex = 7;
            if (value == "Achromatomaly") _profileIndex = 8;

            ChangeProfile();
        }

        public string GetNextColorType()
        {
            _profileIndex = _profileIndex < 8 ? ++_profileIndex : 0;

            ChangeProfile();

            if (_profileIndex == 0) return "OFF";
            if (_profileIndex == 1) return "Protanopia";
            if (_profileIndex == 2) return "Protanomaly";
            if (_profileIndex == 3) return "Deuteranopia";
            if (_profileIndex == 4) return "Deuteranomaly";
            if (_profileIndex == 5) return "Tritanopia";
            if (_profileIndex == 6) return "Tritanomaly";
            if (_profileIndex == 7) return "Achromatopsia";
            if (_profileIndex == 8) return "Achromatomaly";

            Tools.LogError("CVDFilter GetNextColorType() return Error");
            return "Error";
        }

        public string GetPrevColorType()
        {
            _profileIndex = _profileIndex == 0 ? 8 : --_profileIndex;

            ChangeProfile();

            if (_profileIndex == 0) return "OFF";
            if (_profileIndex == 1) return "Protanopia";
            if (_profileIndex == 2) return "Protanomaly";
            if (_profileIndex == 3) return "Deuteranopia";
            if (_profileIndex == 4) return "Deuteranomaly";
            if (_profileIndex == 5) return "Tritanopia";
            if (_profileIndex == 6) return "Tritanomaly";
            if (_profileIndex == 7) return "Achromatopsia";
            if (_profileIndex == 8) return "Achromatomaly";

            Tools.LogError("CVDFilter GetNextColorType() return Error");
            return "Error";
        }

        private void ChangeProfile()
        {
            if (_profile == null)
            {
                Tools.LogError("CVDFilter: ChangeProfile() _profile == null");
                return;
            }

            postProcessVolume = GetComponent<Volume>();
            postProcessVolume.profile = _profile.VisionTypes[_profileIndex].profile;

            if (_rgbColorImage != null)
                _rgbColorImage.sprite = _profile.VisionTypes[_profileIndex].previewImage;
        }
    }

    public enum VisionTypeNames
    {
        Normal,
        Protanopia,
        Protanomaly,
        Deuteranopia,
        Deuteranomaly,
        Tritanopia,
        Tritanomaly,
        Achromatopsia,
        Achromatomaly
    }

    [System.Serializable]
    public struct VisionTypeInfo
    {
        public VisionTypeNames typeName;
        public string description;
        public VolumeProfile profile;
        public Sprite previewImage;
    }