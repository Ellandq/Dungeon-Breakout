using System;

namespace Characters.Movement
{
    [System.Serializable]
    public struct MovementSettings
    {
        public float defaultSpeed;
        public float runningSpeed;
        public float sneakingSpeed;

        public float GetSpeed(MovementType type)
        {
            return type switch
            {
                MovementType.Walking => defaultSpeed,
                MovementType.Sprinting => runningSpeed,
                MovementType.Sneaking => sneakingSpeed,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}