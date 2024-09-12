using System;
using System.Collections.Generic;

namespace Heads
{
    static class BulletMngr
    {
        private static Queue<Bullet>[] bullets;
        private static int numBullets;

        public static void Init()
        {
            numBullets = 10;
            bullets = new Queue<Bullet>[(int)BulletType.Length];

            Type[] bulletTypes = new Type[bullets.Length];
            bulletTypes[0] = typeof(PlayerBullet);
            bulletTypes[1] = typeof(EnemyBullet);

            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new Queue<Bullet>(numBullets);

                for (int j = 0; j < numBullets; j++)
                {
                    bullets[i].Enqueue((Bullet)Activator.CreateInstance(bulletTypes[i]));
                }
            }
        }

        public static void RestoreBullet(Bullet bullet)
        {
            bullets[(int)bullet.Type].Enqueue(bullet);
            bullet.Reset();
        }

        public static Bullet GetBullet(BulletType bulletType)
        {
            if (bullets[(int)bulletType].Count > 0)
            {
                Bullet b = bullets[(int)bulletType].Dequeue();
                b.IsActive = true;

                return b;
            }

            return null;
        }

        public static void ClearAll()
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i].Clear();
            }
        }
    }
}
