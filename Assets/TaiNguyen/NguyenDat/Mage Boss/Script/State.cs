﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void EnterState();
    public abstract void ExitState();
    public virtual void UpdateState() { }
}
