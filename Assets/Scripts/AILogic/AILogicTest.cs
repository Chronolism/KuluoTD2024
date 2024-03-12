using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILogicTest : AILogicBase
{
    //加入了会主动防守
    public override int AIDone(List<Slot> slots)
    {
        foreach (Slot slot in slots)
        {
            if (slot == null) continue;
            if (!slot.isFilled)
            {
                if (GameLogic.Instance.LogicWinTest(slot.index,(GameLogic.Instance.nowPattern == 0 ? 1 : 0))) return slot.index;
            }            
        }
        return base.AIDone(slots);
    }
}
