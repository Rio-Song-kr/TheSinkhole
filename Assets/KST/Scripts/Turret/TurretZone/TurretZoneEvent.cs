using System;
using System.Collections;
using System.Collections.Generic;
using Test;
using UnityEngine;

public class TurretZoneEvent : MonoBehaviour
{
    public static event Action<TriggerTurret> OnTowerInteract;
    public static event Action OnTowerExit;

    public static void InvokeInteract(TriggerTurret towerzone) => OnTowerInteract?.Invoke(towerzone);
    public static void InvokeExit() => OnTowerExit?.Invoke();
}
