﻿using Game3.Collisions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D9;

namespace Game3
{
    public class Ball
    {
        private KeyboardState _keyboardState;

        private Texture2D _ball;

        private BoundingCircle _bounds = new BoundingCircle(new Vector2(32,32), 32.0f);

        private float _rotation;

        public Vector2 Position = new Vector2(64, 64);

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingCircle Bounds => _bounds;

        /// <summary>
        /// The color to blend with the ball
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            _ball = content.Load<Texture2D>("ball");
            _rotation = 0;
        }

        /// <summary>
        /// Updates the sprite's _position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();

            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_keyboardState.IsKeyDown(Keys.Left))
            {
                Position -= Vector2.UnitX * 400 * t;
                _rotation -= .1f;
            }
            if (_keyboardState.IsKeyDown(Keys.Right))
            {
                Position += Vector2.UnitX * 400 * t;
                _rotation += .1f;
            }
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_ball, Position, null, Color, _rotation, new Vector2(32, 32), 1f, SpriteEffects.None, 0);
        }
    }
}