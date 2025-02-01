using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace MyRPG.Enemy
{
    public class EnemyAni : MonoBehaviour
    {
        #region Varibles
        public const int IDLE = 0;
        public const int WALK = 1;
        public const int ATTACK = 2;
        public const int DIE = 3;

        private Animator animator;
        #endregion
        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void ChangeState(int Number)
        {
            animator.SetInteger("EnemyAni", Number);
        }
    }
}