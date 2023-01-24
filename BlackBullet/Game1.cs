using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackBullet
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private const float _delay = 0.5f;
        private const float _delayBonus = 5f;
        private float _remainingDelay = _delay;
        private float _remainingDelayBonus = _delayBonus;

        Character player;
        Boss FourArmedRetard;
        private List<Bullet> bullets = new List<Bullet>();
        private List<Bullet> bossBullets = new List<Bullet>();
        private List<DoubleDamageBonus> DoubleDamageBonus = new List<DoubleDamageBonus>();

        private SpriteFont bossHp;
        private int hp = 2000;

        private SpriteFont playerHp;
        private int hpPlayer = 10;

        private SpriteFont win;
        bool playerWin = false;

        bool playerLost = false;

        int bonusCount = 1;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            player = new Character(Content.Load<Texture2D>("tile000"), new Vector2(320, 240));
            FourArmedRetard = new Boss(Content.Load<Texture2D>("boss"), new Vector2(250, 25));
            bossHp = Content.Load<SpriteFont>("bossHp");
            playerHp = Content.Load<SpriteFont>("playerHp");
            win = Content.Load<SpriteFont>("win");
        }

        protected override void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                Shoot();
            }

            if(hpPlayer <= 0)
            {
                playerLost = true;
            }

            UpdateBonusCollision();
            SpawnBonus(gameTime);
            UpdatePhases(gameTime);
            UpdateBullets();
            UpdateBossBullets();
            base.Update(gameTime);
        }

        public void SpawnBonus(GameTime gameTime)
        {
            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _remainingDelayBonus -= timer;

            if (_remainingDelayBonus <= 0 && bonusCount > 0)
            {
                Random rnd = new Random();
                
                DoubleDamageBonus newDoubleDamageBonus = new DoubleDamageBonus(Content.Load<Texture2D>("doubleDamageBonus"), new Vector2(rnd.Next(0, 600), rnd.Next(0, 480)));
                DoubleDamageBonus.Add(newDoubleDamageBonus);
                bonusCount = -1;
            }
        }

        public void UpdatePhases(GameTime gameTime)
        {
            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _remainingDelay -= timer;

            if (_remainingDelay <= 0 && hp >= 1000)
            {
                PhaseOneShoot();
                _remainingDelay = _delay;
            }

            if (_remainingDelay <= 0 && hp <= 1000)
            {
                PhaseTwoShoot();
                _remainingDelay = _delay;
            }

            if (_remainingDelay <= 0 && hp <= 500 && hp != 0)
            {
                PhaseThreeShoot();
                _remainingDelay = _delay;
            }

            if (hp <= 0)
            {
                bullets.Clear();
                playerWin = true;
            }
        }
        public void UpdateBullets()
        {
            foreach (Bullet bullet in bullets)
            {
                bullet.position += bullet.velocity;
                if (Vector2.Distance(bullet.position, player.position) > 500)
                {
                    bullet.isVisible = false;
                }

                if (Vector2.Distance(bullet.position, new Vector2(320, 90)) < 50)
                {
                    bullet.isVisible = false;
                    hp = hp - player.damage;
                }
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].isVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }

        public void UpdateBonusCollision()
        {
            foreach (DoubleDamageBonus DoubleDamageBonus in DoubleDamageBonus)
            {

                if (Vector2.Distance(DoubleDamageBonus.position, player.position) <= 10)
                {
                    DoubleDamageBonus.isVisible = false;
                    player.bonus = true;
                    player.timerBonus = DoubleDamageBonus.Timer;
                    player.damage = player.damage * DoubleDamageBonus.Damage;
                }
            }
            for (int i = 0; i < DoubleDamageBonus.Count; i++)
            {
                if (!DoubleDamageBonus[i].isVisible)
                {
                    DoubleDamageBonus.RemoveAt(i);
                    i--;
                }
            }
        }

        public void UpdateBossBullets()
        {
            foreach (Bullet bossBullet in bossBullets)
            {
                bossBullet.position += bossBullet.velocity;
                if (Vector2.Distance(bossBullet.position, player.position) > 500)
                {
                    bossBullet.isVisible = false;
                }

                if (Vector2.Distance(bossBullet.position, player.position) <= 5)
                {
                    bossBullet.isVisible = false;
                    hpPlayer = hpPlayer - 1;
                }
            }
            for (int i = 0; i < bossBullets.Count; i++)
            {
                if (!bossBullets[i].isVisible)
                {
                    bossBullets.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Shoot()
        {
            Bullet newBullet = new Bullet(Content.Load<Texture2D>("Bullet"));
            newBullet.velocity = new Vector2(0, -5f);
            newBullet.position = new Vector2(player.position.X+10f, player.position.Y);
            newBullet.isVisible = true;

            bullets.Add(newBullet);
        }

        public void PhaseOneShoot()
        {
            int bullets = 20;
            int count = 0;
            Random rnd = new Random();
            
            for (int X = rnd.Next(0, 640); bullets > count; count++)
            {
                Bullet newBullet = new Bullet(Content.Load<Texture2D>("Bullet"));
                newBullet.velocity = new Vector2(rnd.Next(-1, 1), 1f);
                newBullet.position = new Vector2(FourArmedRetard.position.X + X * count / 2, FourArmedRetard.position.Y + 50f);
                newBullet.isVisible = true;

                Bullet newSecondBullet = new Bullet(Content.Load<Texture2D>("Bullet"));
                newSecondBullet.velocity = new Vector2(rnd.Next(-1, 1), 1);
                newSecondBullet.position = new Vector2(FourArmedRetard.position.X - X * count / 2, FourArmedRetard.position.Y + 50f);
                newSecondBullet.isVisible = true;

                bossBullets.Add(newBullet);
                bossBullets.Add(newSecondBullet);
            }
        }

        public void PhaseTwoShoot()
        {
            int bullets = 40;
            int count = 0;
            Random rnd = new Random();

            for (int X = rnd.Next(0, 640); bullets > count; count++)
            {
                Bullet newBullet = new Bullet(Content.Load<Texture2D>("Bullet"));
                newBullet.velocity = new Vector2(rnd.Next(-2, 2), 2);
                newBullet.position = new Vector2(FourArmedRetard.position.X + X * count / 2, FourArmedRetard.position.Y + 50f);
                newBullet.isVisible = true;

                Bullet newSecondBullet = new Bullet(Content.Load<Texture2D>("Bullet"));
                newSecondBullet.velocity = new Vector2(rnd.Next(-2, 2), 2);
                newSecondBullet.position = new Vector2(FourArmedRetard.position.X - X * count / 2, FourArmedRetard.position.Y + 50f);
                newSecondBullet.isVisible = true;

                bossBullets.Add(newBullet);
                bossBullets.Add(newSecondBullet);
            }
        }

        public void PhaseThreeShoot()
        {
            int bullets = 40;
            int count = 0;
            Random rnd = new Random();

            for (int X = rnd.Next(0, 640); bullets > count; count++)
            {
                Bullet newBullet = new Bullet(Content.Load<Texture2D>("Bullet"));
                newBullet.velocity = new Vector2(rnd.Next(-3, 3), 3);
                newBullet.position = new Vector2(FourArmedRetard.position.X + X * count / 2, FourArmedRetard.position.Y + 50f);
                newBullet.isVisible = true;

                Bullet newSecondBullet = new Bullet(Content.Load<Texture2D>("Bullet"));
                newSecondBullet.velocity = new Vector2(rnd.Next(-3, 3), 3);
                newSecondBullet.position = new Vector2(FourArmedRetard.position.X - X * count / 2, FourArmedRetard.position.Y + 50f);
                newSecondBullet.isVisible = true;

                bossBullets.Add(newBullet);
                bossBullets.Add(newSecondBullet);
            }
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(Content.Load<Texture2D>("space"), new Vector2(0, 0), Color.White);
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(_spriteBatch);
            }
            foreach (DoubleDamageBonus DoubleDamageBonus in DoubleDamageBonus)
            {
                DoubleDamageBonus.Draw(_spriteBatch);
            }
            player.Draw(_spriteBatch);
            foreach (Bullet bossBullet in bossBullets)
            {
                bossBullet.Draw(_spriteBatch);
            }
            FourArmedRetard.Draw(_spriteBatch);
            _spriteBatch.DrawString(bossHp, "HP FourArmedRetard : " + hp, new Vector2(0, 0), Color.Red);
            _spriteBatch.DrawString(playerHp, "HP UwUGirl : " + hpPlayer, new Vector2(0, 460), Color.White);
            if (playerWin == true){
                _spriteBatch.Draw(Content.Load<Texture2D>("winBack"), new Vector2(0, 0), Color.White);
                _spriteBatch.DrawString(win, "GG BOYS YOU WIN PEACE", new Vector2(0, 240), Color.White);
            }

            if (playerLost == true)
            {
                _spriteBatch.Draw(Content.Load<Texture2D>("lostBack"), new Vector2(0, 0), Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}