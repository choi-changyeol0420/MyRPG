using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRPG.Effect
{
    public abstract class TriggerEffect : MonoBehaviour
    {
        #region Variables
        private HashSet<GameObject> damageobject = new HashSet<GameObject>();

        public GameObject damageEffect;
        [HideInInspector] public GameObject damageEff;
        #endregion
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy")
            {
                EffectTrigger(other);
            }
            damageobject.Add(other.gameObject);
            StartCoroutine(ResetCollision(other.gameObject));
        }
        IEnumerator ResetCollision(GameObject other)
        {
            yield return new WaitForSeconds(0.5f);
            damageobject.Remove(other);
        }
        public virtual void EffectTrigger(Collider other)
        {

        }
    }
}