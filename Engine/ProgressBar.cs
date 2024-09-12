using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Heads
{
    class ProgressBar : GameObject
    {
        private Texture barTexture;
        public Sprite BarSprite { get; private set; }
        private Vector2 barOffset;
        private int barPixelsWidthScaled;
        private int fullBarPixelsWidth;
        private int barPixelsHeight;
        public override Vector2 Position { get { return base.Position; } set { base.Position = value; BarSprite.position = value + barOffset; } }

        public ProgressBar(string frameTextureId, string barId, Vector2 barOffset) : base(frameTextureId, layer: DrawLayer.GUI)
        {
            sprite.pivot = Vector2.Zero;

            barTexture = TextureMngr.GetTexture(barId);
            BarSprite = new Sprite(Game.PixelsToUnits(barTexture.Width), Game.PixelsToUnits(barTexture.Height));

            fullBarPixelsWidth = barTexture.Width;
            barPixelsWidthScaled = barTexture.Width;
            barPixelsHeight = barTexture.Height;
            this.barOffset = barOffset;

            DrawMngr.Add(this);
        }

        public void Scale(float scale)
        {
            scale = MathHelper.Clamp(scale, 0, 1);

            BarSprite.scale.X = scale;
            barPixelsWidthScaled = (int)(fullBarPixelsWidth * scale);

            BarSprite.SetMultiplyTint((1 - scale) * 50, scale * 2, scale, 1);
        }

        public override void Draw()
        {
            if (IsActive)
            {
                base.Draw();
                BarSprite.DrawTexture(barTexture, 0, 0, barPixelsWidthScaled, barPixelsHeight);
            }
        }
    }
}
