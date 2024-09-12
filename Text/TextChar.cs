using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Heads
{
    class TextChar : GameObject
    {
        protected char character;
        protected Font font;
        protected Vector2 textureOffset;

        public char Character { get { return character; } set { character = value; ComputeOffset(); } }

        public TextChar(Vector2 spritePosition, char character, Font font) : base(font.TextureName, 1, font.CharacterWidth, font.CharacterHeight, DrawLayer.GUI)
        {
            sprite.position = spritePosition;
            this.font = font;
            Character = character;
            sprite.pivot = Vector2.Zero;
        }

        protected void ComputeOffset()
        {
            textureOffset = font.GetOffset(character);
        }

        public override void Draw()
        {
            Console.WriteLine("c");

            if (IsActive)
            {
                sprite.DrawTexture(texture, (int)textureOffset.X, (int)textureOffset.Y, font.CharacterWidth, font.CharacterHeight);
            }
        }
    }
}
