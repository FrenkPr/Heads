

namespace Heads
{
    interface IDrawable
    {
        DrawLayer DrawLayer { get; }

        void Draw();
    }
}
