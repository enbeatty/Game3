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
using SharpDX.Direct2D1;

namespace Game3.Screens
{
    public class ParallaxScreen : GameScreen
    {
        private ContentManager _content;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;


        /// <summary>
        /// Characters
        /// </summary>
        private Ball _ball;

        /// <summary>
        /// Layer textures
        /// </summary>\
        private Texture2D _test;
        //private Texture2D _foreground;
        private Texture2D _midground;
        private Texture2D _background;

        private IShape[] _foreground = { new Shapes.Line(new Vector2(0, 500)), new Shapes.Line(new Vector2(128, 500)), new SemiCircle(new Vector2(256, 500)), new Shapes.Line(new Vector2(512, 500)), new SemiCircle(new Vector2(640, 500)) };

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
            _test = _content.Load<Texture2D>("ball");


            foreach ( IShape s in  _foreground )
            {
                s.LoadContent(_content);
            }

            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(ScreenManager.GraphicsDevice);
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

            _ball.Acceleration = new Vector2(0, 30f);
            foreach( IShape s in _foreground )
            {
                if( s is SemiCircle )
                {
                    SemiCircle c = (SemiCircle)s;

                    if(_ball.Bounds.CollidesWith(c.Bounds))
                    {
                        _ball.Acceleration = new Vector2(0, 0);
                        _ball.Position -= (_ball.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                        _ball.Velocity = new Vector2(0, 0);
                    }
                }
                if( s is Line )
                {
                    Line l = (Line)s;

                    if (_ball.Bounds.CollidesWith(l.Bounds))
                    {
                        _ball.Acceleration = new Vector2(0, 0);
                        _ball.Position -= (_ball.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                        _ball.Velocity = new Vector2(0, 0);
                    }
                }
            }

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
            foreach( IShape s in _foreground )
            {
                s.Draw(gameTime, _spriteBatch);
                /*if( s is SemiCircle )
                {
                    SemiCircle c = (SemiCircle)s;
                    var rectb = new Rectangle((int)(c.Bounds.Center.X - c.Bounds.Radius),
                                         (int)(c.Bounds.Center.Y - c.Bounds.Radius),
                                         (int)(2 * c.Bounds.Radius), (int)(2 * c.Bounds.Radius));
                    _spriteBatch.Draw(_test, rectb, Color.Red);
                }
                if( s is Line )
                {
                    Line l = (Line)s;
                    var rect = new Rectangle((int)l.Bounds.X, (int)l.Bounds.Y, (int)l.Bounds.Width, (int)l.Bounds.Height);
                    _spriteBatch.Draw(_background, rect, Color.Black);
                }*/
                
                
            }
            _ball.Draw(gameTime, _spriteBatch);
            /*var recta = new Rectangle((int)(_ball.Bounds.Center.X - _ball.Bounds.Radius),
                                         (int)(_ball.Bounds.Center.Y - _ball.Bounds.Radius),
                                         (int)(2 * _ball.Bounds.Radius), (int)(2 * _ball.Bounds.Radius));
            _spriteBatch.Draw(_test, recta, Color.Red);*/
            _spriteBatch.End();
        }
    }
}
