using UnityEngine;

namespace MyRPG.Enemy
{
    public class AniEventControl : MonoBehaviour
    {
        public void SendAttackEnemy()
        {
            this.SendMessage("AttackCalculate");
        }
    }
}