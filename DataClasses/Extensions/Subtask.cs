namespace ZombiesMod.Extensions
{
  public enum Subtask
  {
    AimedShootingOnFoot = 4,
    GettingUp = 16, // 0x00000010
    MovingOnFootNoCombat = 35, // 0x00000023
    MovingOnFootCombat = 38, // 0x00000026
    UsingLadder = 47, // 0x0000002F
    Climbing = 50, // 0x00000032
    GettingOffSomething = 51, // 0x00000033
    SwappingWeapon = 56, // 0x00000038
    RemovingHelmet = 92, // 0x0000005C
    Dead = 97, // 0x00000061
    HittingMelee = 130, // 0x00000082
    MeleeCombat = 130, // 0x00000082
    SittingInVehicle = 150, // 0x00000096
    DrivingWandering = 151, // 0x00000097
    ExitingVehicle = 152, // 0x00000098
    EnteringVehicleGeneral = 160, // 0x000000A0
    EnteringVehicleBreakingWindow = 161, // 0x000000A1
    EnteringVehicleOpeningDoor = 162, // 0x000000A2
    EnteringVehicleEntering = 163, // 0x000000A3
    EnteringVehicleClosingDoor = 164, // 0x000000A4
    ExiingVehicleOpeningDoorExiting = 167, // 0x000000A7
    ExitingVehicleClosingDoor = 168, // 0x000000A8
    DrivingGoingToDestinationOrEscorting = 169, // 0x000000A9
    UsingMountedWeapon = 199, // 0x000000C7
    InCoverGeneral = 287, // 0x0000011F
    InCoverFullyInCover = 288, // 0x00000120
    AimingThrowable = 289, // 0x00000121
    AimingGun = 290, // 0x00000122
    Reloading = 298, // 0x0000012A
    AimingPreventedByObstacle = 299, // 0x0000012B
    RunningToCover = 300, // 0x0000012C
    InCoverTransitionToAimingFromCover = 302, // 0x0000012E
    InCoverTransitionFromAimingFromCover = 303, // 0x0000012F
    InCoverBlindFire = 304, // 0x00000130
    Parachuting = 334, // 0x0000014E
    PuttingOffParachute = 336, // 0x00000150
    JumpingOrClimbingGeneral = 420, // 0x000001A4
    JumpingAir = 421, // 0x000001A5
    JumpingFinishingJump = 422, // 0x000001A6
  }
}
