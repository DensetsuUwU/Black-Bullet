using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BlackBullet
{
    public class Character
    {
        Texture2D sprite;

        public Vector2 position;
        public Vector2 velocity;

        private float _speed;

        private const int _baseDamage = 3;
        private int _damage = 3;

        public bool bonus;
        public float timerBonus;

        public Character(Texture2D sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.position = position;

            bonus = false;
        }

        public void Update(GameTime gametime)
        {
            position += velocity;

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                velocity.X = _speed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                velocity.X = -_speed;
            }
            else
            {
                velocity.X = 0;
            }


            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                velocity.Y = -_speed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                velocity.Y = _speed;
            }
            else
            {
                velocity.Y = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                _speed = 6;
            }
            else
            {
                _speed = 3;
            }

            if (bonus)
            {
                var timer = (float)gametime.ElapsedGameTime.TotalSeconds;

                timerBonus -= timer;

                damage = _baseDamage * 3;

                if (timerBonus <= 0)
                {
                    damage = _baseDamage;
                }
            }
        }

        public int damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        public float speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
