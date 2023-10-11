using Game3.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game3.Shapes
{
    public class SemiCircle : IShape
    {
        private BoundingDip _bounds; //TODO

        private Texture2D _semiCircle;

        public float LeftBound => Position.X;

        public float RightBound => Position.X + 256;

        public Vector2 Position { get; set; }

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingDip Bounds => _bounds;

        public SemiCircle(Vector2 position)
        {
            Position = position;
            _bounds = new BoundingDip(position + new Vector2(128, 0), 128);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            _semiCircle = content.Load<Texture2D>("semiCircle");
        }

        /// <summary>
        /// Updates the sprite's _position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_semiCircle, Position, null, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
        }

    }
}
