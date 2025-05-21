using UnityEngine;

namespace Characters
{
    public class PlayerManager : ManagerBase<PlayerManager>
    {
        [Header("Player Reference")] 
        [SerializeField] private Player player;
        
        public static Player GetPlayer()
        {
            return Instance.player;
        }
    }
}