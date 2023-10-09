using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game3.Shapes
{
    public interface IShape
    {
        public Vector2 Position { get; set; }

        public void LoadContent(ContentManager content) { }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
    }
}
