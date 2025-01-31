using UnityEngine;

namespace MyRPG.Player
{
    /// <summary>
    /// 애니메이터 컨트롤러의 전이 관계에서 설정한 번호에 맞춥니다
    /// </summary>
    public class PlayerAni : MonoBehaviour
    {
        public const int Ani_idle = 0;
        public const int Ani_move = 1;
        public const int Ani_attack = 2;
        public const int Ani_attackidle = 3;
        public const int Ani_die = 4;
        public const int Ani_Run = 5;

        private Animator animator;
        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        public void ChangeAni(int Number)
        {
            animator.SetInteger("aniName", Number);
        }
    }
}