using MyRPG.Effect;
using UnityEngine;

namespace MyRPG.Player
{
    public class TriggerLandMine : TriggerEffect
    {
        #region Variables
        public GameObject effect;

        private float timer;
        [SerializeField] private float timeSet = 10;
        #endregion
        private void Start()
        {
            if(effect)
            {
                GameObject effectgo = Instantiate(effect, transform.position, Quaternion.identity);
                Destroy(effectgo, 2f);
            }
        }
        
        private void Update()
        {
            if(!damageEff)
            {
                timer += Time.deltaTime;
                if(timer >= timeSet)
                {
                    damageEff = Instantiate(damageEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    Destroy(damageEff, 1);
                }
            }
        }
        public override void EffectTrigger(Collider other)
        {
            if (damageEffect)
            {
                damageEff = Instantiate(damageEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
                Destroy(damageEff, 1);
            }
        }
    }
}