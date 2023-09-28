namespace Models
{
    public class NormalEnemy : Enemy
    {
        public NormalEnemy(float speed, int health, int shield, EnemyType enemyType) : base(speed, health, shield, enemyType)
        {
        }
    }
}