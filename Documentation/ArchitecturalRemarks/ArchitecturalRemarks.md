# Architectural Remarks

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)

1. The project should not have warning messages in the console.

2. Don't use Singleton Pattern unless it's absolutely necessary.

3. You have to ask if you use "GameObject.Find" or "FindObjectOfType" - something is probably wrong.

4. Use a minimum of static events.

5. gameObject.SetActive(true/false) to do through a script on a gameObject with public methods Show() and Hide(). For example, you can use the following script: Scripts > UI > UIShowHide.cs. This is needed to easily understand all the places from which the gameObject is activated/deactivated.

6. The name of scriptableObject classes must end with "SO". For example: InventoryItemSO, ActionSO, CursorSettingsSO.

7. Serializable enums must be numbered. For example:

            private enum SaveProviderType { Local = 0, UnityCloud = 5 };
    
            public enum SoundNamesUI { None = 0, Click = 5, SetActiveSlot = 10, ... }

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)
