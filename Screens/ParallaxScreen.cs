using Game3.StateManagement;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game3.Shapes;

namespace Game3.Screens
{
    public class ParallaxScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteBatch _spriteBatch;


        /// <summary>
        /// Characters
        /// </summary>
        private Ball _ball;

        /// <summary>
        /// Layer textures
        /// </summary>
        //private Texture2D _foreground;
        private Texture2D _midground;
        private Texture2D _background;

        private IShape[] _foreground = { new Shapes.Line(), new Shapes.Line(), new SemiCircle(), new Shapes.Line() };

        public ParallaxScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _ball = new Ball();
            _ball.LoadContent(_content);
            _midground = _content.Load<Texture2D>("Midground");
            _background = _content.Load<Texture2D>("Background");

            foreach( IShape s in  _foreground )
            {
                s.LoadContent(_content);
            }

            _spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            _ball.Color = Color.White; //This is where the color of the ball can change
            _ball.Update(gameTime);

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //Calculate our offset vector
            float playerX = MathHelper.Clamp(_ball.Position.X, 300, 13600);
            float offsetX = 300 - playerX;

            Matrix transform;
            BlendState blend;

            // Background
            transform = Matrix.CreateTranslation(offsetX * 0.333f, 0, 0);
            blend = BlendState.AlphaBlend;
            _spriteBatch.Begin(transformMatrix: transform, blendState: blend);
            _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            _spriteBatch.End();

            // Midground
            transform = Matrix.CreateTranslation(offsetX * 0.666f, 0, 0);
            _spriteBatch.Begin(transformMatrix: transform, blendState: blend);
            _spriteBatch.Draw(_midground, Vector2.Zero, Color.White);
            _spriteBatch.End();

            // Forground
            transform = Matrix.CreateTranslation(offsetX, 0, 0);
            _spriteBatch.Begin(transformMatrix: transform, blendState: blend);
            var x = 0;
            foreach( IShape s in _foreground )
            {
                s.Position = new Vector2(x, 128);
                s.Draw(gameTime, _spriteBatch);
                x = x + 128;
            }
            _ball.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }
    }
}
