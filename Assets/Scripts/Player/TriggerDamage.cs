using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRPG.Attack
{
    public class TriggerDamage : MonoBehaviour
    {
        #region Variables
        private HashSet<GameObject> damageobject = new HashSet<GameObject>();

        public int damageamount;
        #endregion
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Enemy")
            {
                IDamageable damageable = other.GetComponent<IDamageable>();
                if(damageable != null)
                {
                    damageable.TakeDamage(damageamount);
                }
            }
            damageobject.Add(other.gameObject);
            StartCoroutine(ResetCollision(other.gameObject));
        }
        IEnumerator ResetCollision(GameObject other)
        {
            yield return new WaitForSeconds(0.5f);
            damageobject.Remove(other);
        }
    }
}