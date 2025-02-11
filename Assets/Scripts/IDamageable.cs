using UnityEngine.UI;

public interface IDamageable
{
    #region Variables
    public float curHP { get; set; }
    public bool isDie { get; set; }
    public void TakeDamage(float damage);
    public int GetRandomAttack();
    #endregion
}
