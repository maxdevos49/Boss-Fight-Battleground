using System;
using System.Collections.Generic;
using System.Linq;

namespace BFB.Engine.Inventory
{
    public class InventoryManager : IInventoryManager
    {

        #region Properties

        private int _activeSlotId;

        private readonly int _hotBarRange;

        private readonly int _inventorySize;
        
        private readonly Dictionary<int, IItem> _slots;
        
        #endregion

        #region Constructor
        
        public InventoryManager(int inventorySize = 1, int hotBarRange = 1)
        {
            if(inventorySize <= 0)
                throw new Exception("Inventory size must be greater then 0");
            
            if(hotBarRange <= 0 || hotBarRange > inventorySize)
                throw new Exception("HotBar size must be greater then 0 and Less or equal to the the size of the inventory");
            
            _activeSlotId = 0;
            
            _inventorySize = inventorySize;
            _hotBarRange = hotBarRange;
            _slots = new Dictionary<int, IItem>();
        }
        
        #endregion

        #region GetActiveSlotId

        public int GetActiveSlotId()
        {
            return _activeSlotId;
        }

        #endregion
        
        #region IncrementHotBar
        
        public void IncrementHotBar()
        {
            if (_activeSlotId < _hotBarRange)
                _activeSlotId++;
            else
                _activeSlotId = 0;
        }
        
        #endregion

        #region DecrementHotBar
        
        public void DecrementHotBar()
        {
            if (_activeSlotId > 0)
                _activeSlotId--;
            else
                _activeSlotId = _hotBarRange;
        }

        #endregion
        
        #region NextHotBar
        
        
        
        #endregion
        
        #region MoveActiveSlot
        
        public void MoveActiveSlot(int slotId)
        {
            if (slotId >= 0 && slotId <= _hotBarRange)
                _activeSlotId = slotId;
        }
        
        #endregion

        #region GetActiveSlot

        public IItem GetActiveSlot()
        {
            return GetSlot(_activeSlotId);
        }

        #endregion
        
        #region MaxInventorySize
        
        public int MaxInventorySize()
        {
            return _inventorySize;
        }
        
        #endregion
        
        #region IsSlotsAvailable
        
        public bool IsSlotsAvailable()
        {
            return _slots.Count <= _inventorySize;
        }
        
        #endregion

        #region IsSlotFull
        
        public bool IsSlotFull(int slotId)
        {
            if (!SlotInRange(slotId))
                return false;

            return _slots.ContainsKey(slotId) && _slots[slotId].IsStackFull();
        }
        
        #endregion
        
        #region Insert

        public IItem Insert(IItem items)
        {
            if (items == null)
                return null;
            
            //search for similar items
            int? slotId = SlotWithItemTypeAvailable(items.ItemConfigKey);

            if (slotId != null)
            {
                IItem remainingItems = Merge((int)slotId,items);
                if (remainingItems == null)
                    return null;
                
                //assign remaining items to main item
                items = remainingItems;
            }
            
            //If anything else is available
            if (!IsSlotsAvailable())
                return items;

            slotId = 0;

            for (int i = 0; i <= MaxInventorySize(); i++)
            {//Searches for the first available slot
                if (_slots.ContainsKey(i)) continue;
                
                slotId = i;
                break;
            }
            
            _slots.Add((int)slotId, items);
            return null;
        }
        
        #endregion

        #region InsertAtSlot
        
        public bool InsertAtSlot(int slotId, IItem item)
        {
            if (!SlotInRange(slotId))
                return false;
            
            _slots.Add(slotId, item);
            return true;
        }
        
        #endregion

        #region Remove
        
        public IItem Remove(int slotId)
        {
            if (!SlotInRange(slotId))
                return null;

            if (!_slots.ContainsKey(slotId)) 
                return null;
            
            IItem tmpItem = _slots[slotId];
            _slots.Remove(slotId);
            
            return tmpItem;
        }
        
        #endregion

        #region Swap
        
        public IItem Swap(int slotId, IItem item)
        {
            //return the item it supplied if invalid slot
            if (!SlotInRange(slotId))
                return item;

            IItem tmpItem = Remove(slotId);
            InsertAtSlot(slotId, item);
            return tmpItem;

        }
        
        #endregion
        
        #region Merge

        public IItem Merge(int slotId, IItem items)
        {
            if (!SlotInRange(slotId))
                return items;

            if (!SlotOccupied(slotId))
            {
                InsertAtSlot(slotId, items);
                return null;
            }

            IItem mergeSlotItem = GetSlot(slotId);
            int existingCount = mergeSlotItem.StackSize();
            int count = items.StackSize();

            if (count + existingCount <= mergeSlotItem.MaxStackSize())
            {
                mergeSlotItem.SetStackSize(count + existingCount);
                return null;
            }

            mergeSlotItem.SetStackSize(mergeSlotItem.MaxStackSize());
            items.SetStackSize((count + existingCount) - mergeSlotItem.MaxStackSize());
            return items;
        }

        #endregion

        #region SlotWithItemTypeAvailable

        public int? SlotWithItemTypeAvailable(string itemType)
        {
            return _slots.FirstOrDefault(x => x.Value.ItemConfigKey == itemType).Key;
        }

        #endregion

        #region Split
        
        public IItem Split(int slotId)
        {
            if (!SlotInRange(slotId))
                return null;

            if (_slots.ContainsKey(slotId))
            {
                IItem item = GetSlot(slotId);

                //if stack size is 1 then we just remove
                if (item.MaxStackSize() == 1)
                    return Remove(slotId);

                IItem newItem = item.Clone();
                float stackSize = item.StackSize();
                
                //Split stacks evenly
                item.SetStackSize((int)System.Math.Floor(stackSize/2f));
                newItem.SetStackSize((int)System.Math.Ceiling(stackSize/2f));

                return newItem;
            }

            return null;
        }
        
        #endregion

        #region GetSlot
        
        public IItem GetSlot(int slotId)
        {
            if (!SlotInRange(slotId))
                return null;

            return _slots.ContainsKey(slotId) ? _slots[slotId] : null;
        }
        
        #endregion

        #region SlotOccupied

        public bool SlotOccupied(int slotId)
        {
            return SlotInRange(slotId) && _slots.ContainsKey(slotId);
        }
        
        #endregion

        #region SlotInRange
        
        public bool SlotInRange(int slotId)
        {
            return slotId >= 0 && slotId <= _inventorySize;
        }
        
        #endregion

        #region Clear
        
        public void Clear()
        {
            _slots.Clear();
        }
        
        #endregion

        #region GetAllItems
        
        public List<IItem> GetAllItems()
        {
            return _slots.Values.ToList();
        }
        
        #endregion
    }
}