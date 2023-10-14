using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnusedSlot : MonoBehaviour
{
    private List<SlotHandManager> unusedSlots = new List<SlotHandManager>();

    public void AddSlot(SlotHandManager slot)
    {
        slot.transform.SetParent(transform);
        //unusedSlots.Add(slot);
    }
    
    public SlotHandManager GetSlot()
    {
        if (unusedSlots.Count > 0)
        {
            SlotHandManager slot = unusedSlots[0];
            unusedSlots.RemoveAt(0);
            return slot;
        }
        return null;
    }
    
    public int GetCount()
    {
        return unusedSlots.Count;
    }
}
