using System.Collections.Generic;
using JetBrains.Annotations;

namespace BFB.Engine.Inventory
{
    public interface IInventoryManager
    {

        /// <summary>
        /// Gets the active slot id
        /// </summary>
        /// <returns></returns>
        byte GetActiveSlotId();
        
        /// <summary>
        /// Increments the Active slot by one
        /// </summary>
        void IncrementHotBar();

        /// <summary>
        /// Decrements the Active slot by one
        /// </summary>
        void DecrementHotBar();

        /// <summary>
        /// Moves the active slot to the given index
        /// </summary>
        /// <param name="slotId">The slot index</param>
        void MoveActiveSlot(byte slotId);

        /// <summary>
        /// Gets the selected item
        /// </summary>
        /// <returns>The active item or null if the slot is empty</returns>
        [CanBeNull]
        IItem GetActiveSlot();

        /// <summary>
        /// Gets the maximum size of the inventory
        /// </summary>
        /// <returns></returns>
        byte MaxInventorySize();
        
        /// <summary>
        /// Indicates if the inventory is full or not
        /// </summary>
        /// <returns>A boolean declaring if each slot is filled but not necessarily if each of those slots are full</returns>
        bool IsSlotsAvailable();
        
        /// <summary>
        /// Indicates of the given slot has reached its stack limit
        /// </summary>
        /// <param name="slotId">The slot id to check</param>
        /// <returns>A boolean indicating if the stack is full</returns>
        [CanBeNull]
        bool IsSlotFull(byte slotId);
        
        /// <summary>
        /// Inserts a new item into the first available spot in the inventory
        /// </summary>
        /// <param name="item">The item to insert</param>
        /// <returns>A boolean indicating if the insert succeeded</returns>
        [CanBeNull]
        IItem Insert(IItem item);

        /// <summary>
        /// Inserts a item at a given slot in the inventory
        /// </summary>
        /// <param name="slotId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        bool InsertAtSlot(byte slotId, IItem item);
        
        /// <summary>
        /// Removes the items at a given slot Id
        /// </summary>
        /// <param name="slotId">The slot to remove from</param>
        /// <returns>The items that were at that slot or null if it was empty</returns>
        [CanBeNull]
        IItem Remove(byte slotId);
            
        /// <summary>
        /// Inserts an item at a given location and returns the previous item at the position
        /// </summary>
        /// <param name="slotId">The slot to swap on</param>
        /// <param name="item">The item to insert</param>
        /// <returns>The item that was in the slot previously</returns>
        [CanBeNull]
        IItem Swap(byte slotId, IItem item);
            
        /// <summary>
        /// Splits the stack of items at a slot in two. If there is only one item it is removed and returned
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        [CanBeNull]
        IItem Split(byte slotId);

        /// <summary>
        /// Attempts to merge two items together if the types match and gives back overflow
        /// </summary>
        /// <param name="slotId">The slot to merge</param>
        /// <param name="item">The item to merge</param>
        /// <returns>Any items that did not fit in the stack</returns>
        [CanBeNull]
        IItem Merge(byte slotId, IItem item);
            
        /// <summary>
        /// Gets the item at a given slot
        /// </summary>
        /// <param name="slotId">The slot to get</param>
        /// <returns>The items at the spot</returns>
        [CanBeNull]
        IItem GetSlot(byte slotId);
        
        /// <summary>
        /// Indicates if the slot is occupied or not. Does not indicate of the slot is full.
        /// </summary>
        /// <param name="slotId">The slot to check</param>
        /// <returns>A boolean indicating if the slot is occupied</returns>
        bool SlotOccupied(byte slotId);

        /// <summary>
        /// Indicates if the slotId Is valid
        /// </summary>
        /// <param name="slotId">The slotId to check</param>
        /// <returns>A boolean indicating if the slot is a valid range</returns>
        bool SlotInRange(byte slotId);

        /// <summary>
        /// Finds a slot with the type of item
        /// </summary>
        /// <param name="itemType">The item type</param>
        /// <returns>Null if not slots found or the slot number to match</returns>
        byte? SlotWithItemTypeAvailable(string itemType);

        /// <summary>
        /// Clears the inventory
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets all items as a list from the inventory
        /// </summary>
        /// <returns>A list of all items in the inventory</returns>
        List<KeyValuePair<byte, IItem>> GetAllItems();

    }

}