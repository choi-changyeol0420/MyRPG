using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MyRPG
{
    /// <summary>
    /// CharacterParams는 플레이어의 파라미터 클래스와 몬스터 파라미터 클래스의 부모 클래스 역활을 하게 됨
    /// </summary>
    public class CharacterParams : MonoBehaviour
    {
        //퍼블릭 변수가 아니라 약식프로퍼티, 속성으로 지정
        //퍼블릭 변수와 똑같이 사용할 수 있지만 유니티 인스펙터에 노출되는 것을 막고 보안을 위해 정식 프로퍼티로 전환이 쉬워짐
        #region Variables
        public int level {  get;  set; }
        public float maxHP {  get;  set; }
        public float curHP {  get;  set; }
        public int attackMin {  get;  set; }
        public int attackMax {  get;  set; }
        public int defense {  get;  set; }
        public  bool isDie { get; set; }

        [System.NonSerialized]
        public UnityAction<GameObject> dieEvent;
        public Image healthBar;
        #endregion
        //나중에 CharacterParams 클래스를 상속한 자식클래스에서 InitParams 함수에 자신만의 명령어를 추가하기만 하면 자동으로 필요한 명령어들이 실행
        public virtual void InitParams()
        {

        }
        public int GetRandomAttack()
        {
            int randAttack = Random.Range(attackMin, attackMax+1);
            return randAttack; 
        }
        public void TakeDamage(float Damage)
        {
            if(isDie) return;
            // 방어력 적용 공식: 받은 피해량 = 기본 피해량 - (방어력 * 피해 감소율)
            float damageReduction = defense * 0.2f; // 방어력의 20%만큼 피해 감소
            float finalDamage = Mathf.Max(Damage - damageReduction, 1); // 최소 1 이상의 피해 적용
            curHP -= finalDamage;
            UpdateAfterReceiveAttack();
        }
        //캐릭터가 적으로 부터 공격을 받은 뒤에 자동으로 실행될 함수를 가상함수로 만듬
        protected virtual void UpdateAfterReceiveAttack()
        {
            //print(name + "'s HP: " + curHP);
            if(curHP <= 0)
            {
                curHP = 0;
                isDie = true;
                dieEvent?.Invoke(gameObject);
            }
        }
    }
}