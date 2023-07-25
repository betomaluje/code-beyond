using UnityEngine;

public class AttackForwarder : MonoBehaviour
{
    private AttackManager attackManager;

    private void Start()
    {
        attackManager = GetComponentInParent<AttackManager>();
    }

    public void AddEnemy(AttackModel enemy)
    {
        attackManager.AddEnemy(enemy);
    }

    public void RemoveEnemy(AttackModel enemy)
    {
        attackManager.RemoveEnemy(enemy);
    }

    /**
     * This methos is in invoked by the Animation tab
     */
    public void PerformDamage()
    {
        attackManager.DamageEnemy();
    }
}
