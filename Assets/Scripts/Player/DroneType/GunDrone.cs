using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace MyRPG.Drone
{
    public class GunDrone : DroneFSM
    {
        #region Variables
        
        #endregion
        protected override IEnumerator FireProjectile()
        {
            yield return base.FireProjectile();
            // if(missile && missilePoint.Length > 0)
            // {
            //     yield return new WaitForSeconds(missileFireRate);
            //     foreach (Transform missilePoint in missilePoint)
            //     {
            //         GameObject missiles = Instantiate(missile, missilePoint.position, missilePoint.rotation);
            //     }
            // }
            // if(machinegun && machinegunPoint.Length > 0)
            // {
            //     yield return new WaitForSeconds(machinegunFireRate);
            //     foreach (Transform machinegunPoint in machinegunPoint)
            //     {
            //         GameObject machineguns = Instantiate(machinegun, machinegunPoint.position, machinegunPoint.rotation);
            //     }
            // }
        }
    }
}