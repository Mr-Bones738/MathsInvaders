using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MathsInvaders.Util
{
    internal static class SpawnEnemies
    {
        public static void spawnEnemies(List<IEntity> entities, Game1 g, int maxrows, int maxcolumns)
        {
            const int pixelperrow = 70;
            const int pixelpercolumn = 70;
            const int defaultXSpawn = 8;
            const int defaultySpawn = 32;
            Random rnd = new Random();
            Texture2D enemyTexture = g.Content.Load<Texture2D>("minvaders/alien");

            for (int row = 0; row < maxrows; row++)
            {
                for (int column = 0; column < maxcolumns; column++)
                {
                    entities.Add(new Entities.Enemy(g, defaultXSpawn + (pixelpercolumn * column), defaultySpawn + (pixelperrow * row), enemyTexture, rnd));
                }
            }
        }
    }
}