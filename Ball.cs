using Game3.Collisions;
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
using Game3.Shapes;

namespace Game3
{
    public class Ball
    {
        private KeyboardState _keyboardState;

        private Texture2D _ball;

        private BoundingCircle _bounds = new BoundingCircle(new Vector2(Constants.BALL_RADIUS, Constants.BALL_RADIUS), 32.0f);

        private float _rotation;

        /// <summary>
        /// If false then left was last 
        /// </summary>
        private bool _lastRotation;

        public Vector2 Velocity = new Vector2(50,0);

        public Vector2 Acceleration { get; set; }

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

            Velocity += Acceleration * t;
            Position += Velocity * t;

            if (_keyboardState.IsKeyDown(Keys.Left) || _keyboardState.IsKeyDown(Keys.A))
            {
                Position -= Vector2.UnitX * 400 * t;
                _rotation -= .1f;
                _lastRotation = false;
            }
            if (_keyboardState.IsKeyDown(Keys.Right) || _keyboardState.IsKeyDown(Keys.D))
            {
                Position += Vector2.UnitX * 400 * t;
                _rotation += .1f;
                _lastRotation = true;
            }
            if (_keyboardState.IsKeyDown(Keys.Up) || _keyboardState.IsKeyDown(Keys.W))
            {
                Position -= Vector2.UnitY * 400 * t;
                if(_lastRotation)
                {
                    _rotation += .1f;
                }
                else
                {
                    _rotation -= .1f;
                }
                
            }
            _bounds.Center.X = Position.X;
            _bounds.Center.Y = Position.Y;
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_ball, Position, null, Color, _rotation, new Vector2(Constants.BALL_RADIUS, Constants.BALL_RADIUS), 1f, SpriteEffects.None, 0);
        }
    }
}
