using System.Collections.Generic;
using Characters.Movement;
using UnityEngine;

namespace Utils
{
    public static class Vector2Utils
    {
        public static Vector2 ConvertFromDirections(Dictionary<MoveDirection, bool> directions)
        {
            var result = Vector2.zero;

            if (directions[MoveDirection.Up]) result += Vector2.up;
            if (directions[MoveDirection.Down]) result += Vector2.down;
            if (directions[MoveDirection.Left]) result += Vector2.left;
            if (directions[MoveDirection.Right]) result += Vector2.right;

            return result.normalized;
        }
    }
}