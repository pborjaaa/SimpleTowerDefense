namespace Models
{
    public class ShieldedEnemy : Enemy
    {
        public ShieldedEnemy(float speed, int health, int shield, EnemyType enemyType) : base(speed, health, shield, enemyType)
        {
        }
    }
}