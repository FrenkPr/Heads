using Aiv.Fast2D;
using OpenTK;

namespace Heads
{
    class Background : GameObject
    {
        public Background() : base("background", layer: DrawLayer.Background)
        {
            IsActive = true;
            sprite.pivot = Vector2.Zero;

            DrawMngr.Add(this);
        }
    }
}