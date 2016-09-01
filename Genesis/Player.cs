﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis
{
    class Player
    {
        public Texture2D Texture { get; set; }
        public ParticleEngine ParticleEngine { get; set; }
        public EnemySpawner EnemySpawner { get; set; }
        public double AttackSpeed { get; set; }
        public double Counter { get; set; }
        public Weapon Weapon { get; set; }
        public float Scale { get; set; }
        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Camera Camera { get; set; }

        private Space space;
        public Space Space
        {
            get
            {
                return space;
            }
            set
            {
                space = value;
            }
        }

        private int velocity;
        public int Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }

        public Player(Camera camera, Space space, ParticleEngine particleEngine, GraphicsDevice device, Vector2 position)
        {
            Camera = camera;
            ParticleEngine = particleEngine;
            Space = space;
            Position = position;
            Scale = 0.3f;

            Velocity = 4;
            AttackSpeed = 7;
            Counter = 0;  
        }

        public void LoadContent(ContentManager Content)
        {
            Weapon = new Weapon(this);
            Texture = Content.Load<Texture2D>("Textures/player");
            Width = (int)(Texture.Width * Scale);
            Height = (int)(Texture.Height * Scale);
        }

        public bool Intersects(Rectangle rectangle)
        {
            Rectangle playerRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            if (playerRectangle.Intersects(rectangle))
                return true;
            else
                return false;

        }

        public void Move()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Vector2 newPosition = new Vector2(Position.X + Velocity, Position.Y);
                // Sprawdzenie kolizji
                Position = newPosition;
                if (Camera.Position.X > 0 && Camera.Position.X + 4 + Genesis.Width < Space.Width && Camera.Position.Y > 0 && Camera.Position.Y + Genesis.Height < Space.Height)
                {
                    Camera.MoveCameraRight(new Vector2(4, 0));
                }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Vector2 newPosition = new Vector2(Position.X - Velocity, Position.Y);
                // Sprawdzenie kolizji
                Position = newPosition;
                if (Camera.Position.X - 4 > 0 && Camera.Position.X + Genesis.Width < Space.Width && Camera.Position.Y > 0 && Camera.Position.Y + Genesis.Height < Space.Height)
                {
                    Camera.MoveCameraLeft(new Vector2(4, 0));
                }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Vector2 newPosition = new Vector2(Position.X, Position.Y - Velocity);
                // Sprawdzenie kolizji
                Position = newPosition;
                if (Camera.Position.X > 0 && Camera.Position.X + Genesis.Width < Space.Width && Camera.Position.Y - 4 > 0 && Camera.Position.Y + Genesis.Height < Space.Height)
                {
                    Camera.MoveCameraUp(new Vector2(0, 4));
                }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Vector2 newPosition = new Vector2(Position.X, Position.Y + Velocity);
                // Sprawdzenie kolizji
                Position = newPosition;
                if (Camera.Position.X > 0 && Camera.Position.X + Genesis.Width < Space.Width && Camera.Position.Y > 0 && Camera.Position.Y + 4 + Genesis.Height < Space.Height)
                {
                    Camera.MoveCameraDown(new Vector2(0, 4));
                }
            }
        }

        public void Update(GameTime gameTime, MouseState mouseState)
        {
            Move();
            Weapon.Update(gameTime, mouseState);
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            Weapon.Draw(spriteBatch, graphics);

            if (Position.X >= 0 && Position.Y >= 0 && Position.X < Space.Width && Position.Y < Space.Height)
            {
                Rectangle sourceRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Space.Camera.getTransformation(graphics));
                spriteBatch.Draw(Texture, sourceRectangle, Color.White);
                spriteBatch.End();
            }
        }


    }
}
