using UnityEngine;

[CreateAssetMenu(fileName = "Attribute", menuName = "Scriptable Objects/EnemyAttribute")]
public class EnemyAttribute : ScriptableObject
{
    #region Variables
    public string enemyname;
    public int level;
    public float maxHp;
    public int attackMax;
    public int attackMin;
    public int defense;
    public int exp;
    public int rewardMoneyMax;
    public int rewardMoneyMin;
    #endregion
}
