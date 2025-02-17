using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRPG.Player
{
    public class TriggerLandMine : MonoBehaviour
    {
        #region Variables
        private HashSet<GameObject> damageobject = new HashSet<GameObject>();

        public GameObject effect;
        public GameObject damageEffect;
        #endregion
        private void Start()
        {
            if(effect)
            {
                GameObject effectgo = Instantiate(effect, transform.position, Quaternion.identity);
                Destroy(effectgo, 2f);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Enemy")
            {
                if(damageEffect)
                {
                    GameObject damageEff = Instantiate(damageEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject,2);
                }
            }
            damageobject.Add(other.gameObject);
            StartCoroutine(ResetCollision(other.gameObject));
        }
        IEnumerator ResetCollision (GameObject other)
        {
            yield return new WaitForSeconds(0.5f);
            damageobject.Remove(other);
        }
    }
}