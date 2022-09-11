using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsConstants
{
    public const float gravity = 9.81f;
    public const float drag = 0.01f;

    public const float bigBounceCoef = 0.85f;
    public const float crashForceAmount = 1f;

    public const int maxSkips = 1;
    public const float minSkipSpeed = 25f;
    public const float smallSkipBoost = 1.5f;
    public const float smallSkipDragMin = 0.25f;
    public const float smallSkipDragMax = 1.1f;

    public const float flyMoveAmount = 8f;
    public const float flyBreakAmount = 6f;
}
