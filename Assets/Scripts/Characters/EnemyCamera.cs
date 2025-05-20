using Characters.Movement;
using UnityEngine;

namespace Characters
{
    public class EnemyCamera : Characters
    {
        public override void Initialize(Vector3 spawnPos = new Vector3(), float rotation = 0)
        {
            base.Initialize(transform.position, rotation);
        }
    }
}