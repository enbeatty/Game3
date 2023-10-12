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
using Microsoft.Xna.Framework.Media;

namespace Game3.Screens
{
    public class ParallaxScreen : GameScreen
    {
        private ContentManager _content;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;
        private Random _random = new Random();

        /// <summary>
        /// Sound
        /// </summary>
        private Song _backgroundMusic;

        /// <summary>
        /// Characters
        /// </summary>
        private Ball _ball;

        /// <summary>
        /// Layer textures
        /// </summary>
        private Texture2D _midground;
        private Texture2D _background;


        private IShape[] _foreground = new IShape[Constants.PLATFORMS]; 

        private Spiral[] _spirals =
        {
            new Spiral(new Vector2(Constants.LINE_WIDTH, Constants.BOTTOM_HEIGHT - 32), Color.Magenta),
            new Spiral(new Vector2(Constants.LINE_WIDTH*3 + Constants.SEMICIRCLE_DIAMETER*2 + 64, Constants.BOTTOM_HEIGHT - 32), Color.AntiqueWhite),
            new Spiral(new Vector2(Constants.LINE_WIDTH*5 + Constants.SEMICIRCLE_DIAMETER*2, Constants.BOTTOM_HEIGHT - 32), Color.CadetBlue),
            new Spiral(new Vector2(Constants.LINE_WIDTH*6 + Constants.SEMICIRCLE_DIAMETER*4 + 64, Constants.BOTTOM_HEIGHT - 32), Color.White)
        };
        private int _spiralCount = 4;
        private int _spiralLeft = 4;

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

            int lineCount = 0;
            int circleCount = 0;
            for (int i = 0; i < Constants.PLATFORMS; i++)
            {
                long r = _random.NextInt64(0, 3);
                if (r == 2)
                {
                    _foreground[i] = (new SemiCircle(new Vector2(Constants.LINE_WIDTH * lineCount + Constants.SEMICIRCLE_DIAMETER * circleCount, Constants.BOTTOM_HEIGHT)));
                    circleCount++;
                }
                else
                {
                    _foreground[i] = (new Line(new Vector2(Constants.LINE_WIDTH * lineCount + Constants.SEMICIRCLE_DIAMETER * circleCount, Constants.BOTTOM_HEIGHT)));
                    lineCount++;
                }
            }


            foreach ( IShape s in  _foreground )
            {
                s.LoadContent(_content);
            }
            foreach( Spiral s in _spirals )
            {
                s.LoadContent(_content);
            }
            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(ScreenManager.GraphicsDevice);

            _backgroundMusic = _content.Load<Song>("GameSongTwo");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.Play(_backgroundMusic);
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

            _ball.Acceleration = new Vector2(0, 800f);
            foreach( IShape s in _foreground )
            {
                if( !(_ball.Position.X < s.LeftBound|| _ball.Position.X > s.RightBound))
                {
                    if (s is SemiCircle)
                    {
                        SemiCircle c = (SemiCircle)s;

                        if (_ball.Bounds.CollidesWith(c.Bounds))
                        {
                            float arch = (s.Position.Y) + (float)Math.Sqrt(65536f - (float)Math.Pow(_ball.Position.X - (s.Position.X + Constants.SEMICIRCLE_RADIUS), 2));
                            _ball.Acceleration = new Vector2(0, 0);
                            if( _ball.Position.Y + 32 > arch)
                            {
                                _ball.Position.Y = arch - 32;
                            }
                            else
                            {
                                _ball.Position -= (_ball.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                            }
                            _ball.Velocity = new Vector2(0, 0);
                        }

                        break;
                    }
                    if( s is Line )
                    {
                        //_ball.Position.Y = s.Position.Y - 32;
                        Line l = (Line)s;

                        if (_ball.Position.Y >= Constants.BOTTOM_HEIGHT - 32)
                        {
                            _ball.Acceleration = new Vector2(0, 0);
                            _ball.Position.Y = s.Position.Y - 32;
                            _ball.Velocity = new Vector2(0, 0);
                        }
                        break;
                    }
                }
            }

            foreach (Spiral s in _spirals)
            {
                s.Update(gameTime);
                if (!s.Collected && _ball.Bounds.CollidesWith(s.Bounds))
                {
                    _spiralLeft--;
                    s.Collected = true;
                    _ball.Color = s.Color;
                }
            }
            _ball.Update(gameTime);

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //Calculate our offset vector
            float playerX = MathHelper.Clamp(_ball.Position.X, 500, 11000);
            float offsetX = 500 - playerX;

            Matrix transform;
            BlendState blend;

            // Background
            transform = Matrix.CreateTranslation(offsetX * 0.222f, 0, 0);
            blend = BlendState.AlphaBlend;
            _spriteBatch.Begin(transformMatrix: transform, blendState: blend);
            _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            _spriteBatch.End();

            // Midground
            transform = Matrix.CreateTranslation(offsetX * 0.555f, 0, 0);
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

            foreach( Spiral s in _spirals )
            {
                s.Draw(gameTime, _spriteBatch);
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
