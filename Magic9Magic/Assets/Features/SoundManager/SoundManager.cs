using UnityEngine;

public abstract class SoundManager : MonoBehaviour
{
    public enum ParentType { Character, Magic }

    public enum SnapshotName
    {
        Gameplay = 5,
        Menu = 10,
        Cutscene = 15,
        Dialogue = 20,
        Popup = 25
    }
   
    public enum SoundNamesUI
    {
        None = 0,
        Click = 5, SetActiveSlot = 10, FinishMissionCompleted = 15, FinishMissionFailed = 20, AppearingPortal = 25, SendingPortal = 30,
        MenuOpening = 105, MenuSelecting = 106, MenuClosing = 107, RingOpening = 110, RingClosing = 111, MagicBookOpening = 115, MagicBookClosing = 116, JournalOpening = 120, JournalClosing = 121, 
        MannaAlarmLevel = 205, HealthAlarmLevel = 210,
        PickupItem = 305, EquipItem = 310, TakeOffItem = 315, BeginDragItem = 320, EndDragItem = 325, SellItem = 330, BuyItem = 335, DropItem = 340
    }

    public enum SoundNamesCharacter
    {
        None = 0,
        PlayerHit = 5,
        GoblinDead = 105, HobgoblinDead = 110, WolfDead = 115
    }

    public enum SoundNamesMagic
    {
        None = 0,
        FireballStart = 105, Fireball = 110, FireMeteor01 = 115, Shield01 = 120
    }

    public enum SoundNamesFootsteps
    {
        None = 0,
        FootstepsSandWalk = 1, FootstepsSandRun = 2, FootstepsSandJump = 3,
        FootstepsDirtyGroundWalk = 11, FootstepsDirtyGroundRun = 12, FootstepsDirtyGroundJump = 13,
        FootstepsGrassWalk = 21, FootstepsGrassRun = 22, FootstepsGrassJump = 23,
        FootstepsGravelWalk = 31, FootstepsGravelRun = 32, FootstepsGravelJump = 33,
        FootstepsLeavesWalk = 41, FootstepsLeavesRun = 42, FootstepsLeavesJump = 43,
        FootstepsMetalWalk = 51, FootstepsMetalRun = 52, FootstepsMetalJump = 53,
        FootstepsMudWalk = 61, FootstepsMudRun = 62, FootstepsMudJump = 63,
        FootstepsRockWalk = 71, FootstepsRockRun = 72, FootstepsRockJump = 73,
        FootstepsSnowWalk = 81, FootstepsSnowRun = 82, FootstepsSnowJump = 83,
        FootstepsTileWalk = 91, FootstepsTileRun = 92, FootstepsTileJump = 93,
        FootstepsWaterWalk = 101, FootstepsWaterRun = 102, FootstepsWaterJump = 103,
        FootstepsWoodWalk = 111, FootstepsWoodRun = 112, FootstepsWoodJump = 113
    }

    public enum SoundNamesDialogues
    {
        FirstMaster00001 = 3, FirstMax00001 = 6, FirstMaster00002 = 9, FirstMax00002 = 12,
        FirstDeathMaster00001 = 23, FirstDeathMax00001 = 26,
        FirstSeller00001 = 33, FirstSeller00002 = 36,
        PickUpFirstItemMaster00001 = 43      
    }
    
}