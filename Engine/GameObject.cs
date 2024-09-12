using Aiv.Fast2D;
using OpenTK;
using System;

namespace Heads
{
    class GameObject : IDrawable, IUpdatable
    {
        protected Texture texture;
        protected Sprite sprite;
        public RigidBody RigidBody;
        public bool IsActive;
        public virtual Vector2 Position { get { return sprite.position; } set { sprite.position = value; } }
        public virtual float X { get { return sprite.position.X; } set { sprite.position.X = value; } }
        public virtual float Y { get { return sprite.position.Y; } set { sprite.position.Y = value; } }
        public float Width { get { return sprite.Width; } }
        public float Height { get { return sprite.Height; } }
        public float HalfWidth { get { return Width * 0.5f; } }
        public float HalfHeight { get { return Height * 0.5f; } }
        public Vector2 Forward { get => new Vector2((float)Math.Cos(sprite.Rotation), (float)Math.Sin(sprite.Rotation)); set => sprite.Rotation = (float)Math.Atan2(value.Y, value.X); }
        public DrawLayer DrawLayer { get; protected set; }
        protected int numFrames;
        public int CurrentFrame;
        private float pixelsWidth;
        public float pixelsHeight;

        public GameObject(string textureId, int numFrames = 1, float spriteWidth = 0, float spriteHeight = 0, DrawLayer layer = DrawLayer.Playground)
        {
            DrawLayer = layer;

            texture = TextureMngr.GetTexture(textureId);

            this.numFrames = numFrames;
            CurrentFrame = 0;

            spriteWidth = spriteWidth <= 0 ? texture.Width : spriteWidth;
            spriteHeight = spriteHeight <= 0 ? texture.Height : spriteHeight;

            if (numFrames > 1)
            {
                spriteWidth /= numFrames;
            }

            pixelsWidth = spriteWidth;
            pixelsHeight = spriteHeight;

            spriteWidth = Game.PixelsToUnits(spriteWidth);
            spriteHeight = Game.PixelsToUnits(spriteHeight);

            sprite = new Sprite(spriteWidth, spriteHeight);

            sprite.pivot = new Vector2(spriteWidth * 0.5f, spriteHeight * 0.5f);
        }

        public virtual void OnCollision(CollisionInfo collisionInfo)
        {

        }

        public virtual void Update()
        {
            if (IsActive)
            {
                CheckOutOfScreen();
            }
        }

        protected virtual void CheckOutOfScreen()
        {
            //horizontal collisions
            if (Position.X - HalfWidth < 0)
            {
                sprite.position.X = HalfWidth;
            }

            else if (Position.X + HalfWidth >= Game.OrthoWidth)
            {
                sprite.position.X = Game.OrthoWidth - HalfWidth;
            }

            //vertical collisions
            if (Position.Y - HalfHeight < 0)
            {
                sprite.position.Y = HalfHeight;
            }

            else if (Position.Y + HalfHeight >= Game.OrthoHeight)
            {
                sprite.position.Y = Game.OrthoHeight - HalfHeight;
            }
        }

        public virtual void Draw()
        {
            if (IsActive)
            {
                if (numFrames <= 1)
                {
                    sprite.DrawTexture(texture);
                }

                else
                {
                    sprite.DrawTexture(texture, (int)(pixelsWidth * CurrentFrame), 0, (int)pixelsWidth, (int)pixelsHeight);
                }
            }
        }

        public virtual void Destroy()
        {
            sprite = null;
            texture = null;

            UpdateMngr.Remove(this);
            DrawMngr.Remove(this);

            if (RigidBody != null)
            {
                RigidBody.Destroy();
            }
        }
    }
}
