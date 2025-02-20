using UnityEngine.UI;

public interface IDamageable
{
    #region Variables
    public void TakeDamage(float damage, float CritChance = 0, float critMult = 2);
    #endregion
}
