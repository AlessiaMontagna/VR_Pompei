﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSubClass : NpcSuperClass
{
    protected override IEnumerator StartInteraction()
    {
        return base.StartInteraction();
    }

    protected override void UpdateInteraction()
    {
        base.UpdateInteraction();
    }

    protected override void StopInteraction()
    {
        base.StopInteraction();
    }
}
