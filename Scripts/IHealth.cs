using System;
using UnityEngine;

namespace Beto.Health
{
    [SerializeField]
    public interface IHealth
    {
        void Damage(int damage, Vector3 hitPos);

        void GiveHealth(int health);

        void Die();
    }

    public abstract class BaseHealth : MonoBehaviour, IHealth
    {
        public Action<float> OnHealthChanged = delegate { };
        public Action<int> OnDamagePerformed = delegate { };
        public Action<int, int> OnCurrentChange = delegate { };

        public abstract void GiveHealth(int health);

        public abstract void Damage(int damage, Vector3 hitPos);

        public abstract void Die();
    }
}
