using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[ExecuteAlways]
[DisallowMultipleComponent]
[RequireComponent(typeof(PostProcessVolume))]
public class MyCVDFilter : MonoBehaviour
{
    enum ColorType { Normal, Protanopia, Protanomaly, Deuteranopia, Deuteranomaly, Tritanopia, Tritanomaly, Achromatopsia, Achromatomaly }

    [SerializeField] ColorType visionType = ColorType.Normal;
    ColorType currentVisionType;
    PostProcessProfile[] profiles;
    PostProcessVolume postProcessVolume;

    void Start()
    {
        currentVisionType = visionType;
        gameObject.layer = LayerMask.NameToLayer("CVDFilter");
        InitVolume();
        LoadProfiles();
        ChangeProfile();
    }

    void Update()
    {
        if (visionType != currentVisionType)
        {
            currentVisionType = visionType;
            ChangeProfile();
        }
    }

    void InitVolume()
    {
        postProcessVolume = GetComponent<PostProcessVolume>();
        postProcessVolume.isGlobal = true;
    }

    // Нужно ли?
    public void SetVisionType()
    {
        visionType = ColorType.Protanopia;
        currentVisionType = ColorType.Protanopia;
        ChangeProfile();
    }

    //-------------------------------------------------------------------------------
    // public string GetCurrentColorType()
    // {
    //     if (currentVisionType == ColorType.Normal) return "OFF";       
    //     return currentVisionType.ToString();
    // }

    public void SetCurrentColorType(string value)
    {
        if (value == "Protanomaly") visionType = ColorType.Protanomaly;
        if (value == "Protanopia") visionType = ColorType.Protanopia;
        if (value == "Tritanomaly") visionType = ColorType.Tritanomaly;
        if (value == "Tritanopia") visionType = ColorType.Tritanopia;
        if (value == "Achromatomaly") visionType = ColorType.Achromatomaly;
        if (value == "Achromatopsia") visionType = ColorType.Achromatopsia;
        if (value == "Deuteranomaly") visionType = ColorType.Deuteranomaly;
        if (value == "Deuteranopia") visionType = ColorType.Deuteranopia;
        if (value == "OFF") visionType = ColorType.Normal;
    }

    public string GetNextColorType()
    {
        if (currentVisionType == ColorType.Normal) visionType = ColorType.Protanomaly;
        if (currentVisionType == ColorType.Protanomaly) visionType = ColorType.Protanopia;
        if (currentVisionType == ColorType.Protanopia) visionType = ColorType.Tritanomaly;
        if (currentVisionType == ColorType.Tritanomaly) visionType = ColorType.Tritanopia;
        if (currentVisionType == ColorType.Tritanopia) visionType = ColorType.Achromatomaly;
        if (currentVisionType == ColorType.Achromatomaly) visionType = ColorType.Achromatopsia;
        if (currentVisionType == ColorType.Achromatopsia) visionType = ColorType.Deuteranomaly;
        if (currentVisionType == ColorType.Deuteranomaly) visionType = ColorType.Deuteranopia;
        if (currentVisionType == ColorType.Deuteranopia)
        {
            visionType = ColorType.Normal;
            return "OFF";
        }
        return visionType.ToString();
    }

    public string GetPrevColorType()
    {
        if (currentVisionType == ColorType.Normal) visionType = ColorType.Deuteranopia;
        if (currentVisionType == ColorType.Deuteranopia) visionType = ColorType.Deuteranomaly;
        if (currentVisionType == ColorType.Deuteranomaly) visionType = ColorType.Achromatopsia;
        if (currentVisionType == ColorType.Achromatopsia) visionType = ColorType.Achromatomaly;
        if (currentVisionType == ColorType.Achromatomaly) visionType = ColorType.Tritanopia;
        if (currentVisionType == ColorType.Tritanopia) visionType = ColorType.Tritanomaly;
        if (currentVisionType == ColorType.Tritanomaly) visionType = ColorType.Protanopia;
        if (currentVisionType == ColorType.Protanopia) visionType = ColorType.Protanomaly;
        if (currentVisionType == ColorType.Protanomaly)
        {
            visionType = ColorType.Normal;
            return "OFF";
        }
        return visionType.ToString();
    }
    //-------------------------------------------------------------------------------

    public void LoadProfiles()
    {
        Object[] profileObjects = Resources.LoadAll("", typeof(PostProcessProfile));
        profiles = new PostProcessProfile[profileObjects.Length];

        for (int i = 0; i < profileObjects.Length; i++)
        {
            if (profileObjects[i].name.Contains("CVD"))
            {
                profiles[i] = (PostProcessProfile)profileObjects[i];
            }
        }
    }

    void ChangeProfile()
    {
        if (profiles.Length == 0)
        {
            Debug.LogError(string.Format("[{0}]({1}) Error: Profiles could not be loaded.\nPlease ensure that they are placed in a folder names \"Resources\" and have not been renamed", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name));
            return;
        }
        else if (profiles.Length < 9)
        {
            Debug.LogWarning(string.Format("[{0}]({1}) Warning: Not all profiles could be loaded.\nPlease ensure that they are placed in a folder names \"Resources\" and have not been renamed", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name));
            return;
        }
        else if (profiles.Length > 9)
        {
            Debug.LogWarning(string.Format("[{0}]({1}) Warning: Unrecognized profiles have been loaded.\nPlease ensure that there are no other post processing profiles containing the term \"CVD\"", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name));
            return;
        }

        postProcessVolume.profile = profiles[(int)currentVisionType];
    }
}
