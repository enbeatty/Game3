using Game3.Collisions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game3
{
    public class Spiral
    {
        private BoundingCircle _bounds;

        private Texture2D _spiral;

        private Vector2 _position;

        private float _rotation = 0f;

        private Color _color;

        /// <summary>
        /// Is the rock collected yet?
        /// </summary>
        public bool Collected { get; set; } = false;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingCircle Bounds => _bounds;

        /// <summary>
        /// The color to blend with the ball
        /// </summary>
        public Color Color => _color;

        public Spiral(Vector2 position, Color color)
        {
            _position = position;
            _bounds = new BoundingCircle(position + new Vector2(8, 8), 16);
            _color = color;

        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {

            _spiral = content.Load<Texture2D>("spiral");
        }

        /// <summary>
        /// Updates the sprite's _position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Collected)
            {
                return;
            }

            spriteBatch.Draw(_spiral, _position, null, Color, _rotation, new Vector2(16, 16), 1f, SpriteEffects.None, 0);
            _rotation += .01f;
        }
    }
}
