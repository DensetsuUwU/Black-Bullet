using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BlackBullet
{
    public abstract class Bonus
    {
        Texture2D sprite;

        public Vector2 position;
        private float timer;
        private float speed;
        private int damage;

        public bool isVisible;
        public Bonus(Texture2D sprite, Vector2 position, float timer, float speed, int damage)
        {
            this.sprite = sprite;
            this.position = position;
            this.timer = timer;
            this.speed = speed;
            this.damage = damage;

            isVisible = true;
        }

        public float Timer
        {
            get { return timer; }
        }

        public int Damage
        {
            get { return damage; }
        }
        public float Speed
        {
            get { return speed; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
