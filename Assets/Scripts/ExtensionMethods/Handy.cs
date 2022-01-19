using System.Linq;
using UnityEngine.InputSystem;

namespace OpenXR_OpenFracture
{
    public enum Hand
    {
        None = 0,
        Left,
        Right,
    }

    /// <summary>
    /// A set of extension methods to determine from which hand an input originates.
    /// </summary>
    public static class Handy
    {
        public static Hand GetHand(this InputAction.CallbackContext _context)
        {
            if (_context.control.device.usages.Contains(CommonUsages.LeftHand))
                return Hand.Left;
            if (_context.control.device.usages.Contains(CommonUsages.RightHand))
                return Hand.Right;
            return Hand.None;
        }
    }
}