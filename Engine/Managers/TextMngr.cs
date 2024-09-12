using System;
using System.Collections.Generic;
using Aiv.Fast2D;
using OpenTK;

namespace Heads
{
    static class TextMngr
    {
        private static Texture texture;
        private static Dictionary<string, string> texts;
        private static Dictionary<string, bool> textsVisibility;
        private static Dictionary<string, Sprite[]> textsSprite;
        private static float defaultCharWidth;
        private static float defaultCharHeight;
        private static float horizontalSpace;
        private static Dictionary<char, Vector2> charsSpriteOffset;
        private static int charsAsciiCode;

        public static void Init()
        {
            charsAsciiCode = 32;  //starting char = space

            LoadFont();

            texture = TextureMngr.GetTexture("comics");
            texts = new Dictionary<string, string>();
            textsSprite = new Dictionary<string, Sprite[]>();
            textsVisibility = new Dictionary<string, bool>();
            defaultCharWidth = texture.Width / 10;
            defaultCharHeight = texture.Height / 10;
            charsSpriteOffset = new Dictionary<char, Vector2>(95);
            float startX = 0;
            float startY = 0;

            for (int i = 0; i < 10; i++)
            {
                int numCol = i == 9 ? 5 : 10;

                for (int j = 0; j < numCol; j++)
                {
                    charsSpriteOffset.Add((char)charsAsciiCode, new Vector2(startX, startY));
                    startX += defaultCharWidth;
                    charsAsciiCode++;
                }

                startX = 0;
                startY += defaultCharHeight;
            }
        }

        private static void LoadFont()
        {
            TextureMngr.AddTexture("comics", "Assets/comics.png");
        }

        public static void AddTextAt(string id, string text, Vector2 startPos, float charWidth = 0, float charHeight = 0, float hSpace = 0, bool visible = true)
        {
            charWidth = charWidth <= 0 ? defaultCharWidth : charWidth;
            charHeight = charHeight <= 0 ? defaultCharHeight : charHeight;

            charWidth = Game.PixelsToUnits(charWidth);
            charHeight = Game.PixelsToUnits(charHeight);
            horizontalSpace = hSpace;

            if (text == "" || text == null || id == "" || id == null)
            {
                return;
            }

            if (textsSprite.ContainsKey(id))
            {
                return;
            }

            if (startPos == null)
            {
                startPos = new Vector2(0);
            }

            Sprite[] textSprite = new Sprite[text.Length];
            Vector2 currentPos = startPos;

            for (int i = 0; i < text.Length; i++)
            {
                textSprite[i] = new Sprite(charWidth, charHeight);

                if (text[i] == '\n')
                {
                    currentPos.X = startPos.X;
                    currentPos.Y += charHeight;
                    textSprite[i].position = currentPos;
                }

                else if (text[i] == ' ')
                {
                    textSprite[i].position = currentPos;
                    currentPos.X += horizontalSpace;
                }

                else
                {
                    textSprite[i].position = currentPos;
                    currentPos.X += charWidth;
                }
            }

            texts.Add(id, text);
            textsSprite.Add(id, textSprite);
            textsVisibility.Add(id, visible);
        }

        public static float GetTextLength(string id)
        {
            //takes a single sprite char width of that text
            //and multiplies it for number of characters
            return textsSprite[id][0].Width * textsSprite[id].Length;
        }

        public static bool IsTextVisible(string id)
        {
            if (!textsVisibility.ContainsKey(id))
            {
                return false;
            }

            return textsVisibility[id];
        }

        public static void SetTextVisibility(string id, bool value)
        {
            if (!textsVisibility.ContainsKey(id))
            {
                return;
            }

            textsVisibility[id] = value;
        }

        public static void RemoveText(string id)
        {
            if (id == null)
            {
                return;
            }

            if (texts.ContainsKey(id))
            {
                texts.Remove(id);
                textsSprite.Remove(id);
                textsVisibility.Remove(id);
            }
        }

        public static void EditText(string oldId, string newText, string newId = "")
        {
            if (oldId == null || newId == null || newText == null || newText == "")
            {
                return;
            }

            if (!texts.ContainsKey(oldId))
            {
                return;
            }

            Vector2 position = textsSprite[oldId][0].position;
            float charWidth = textsSprite[oldId][0].Width;
            float charHeight = textsSprite[oldId][0].Height;
            newId = newId == "" ? oldId : newId;

            bool visibility = textsVisibility[newId];

            RemoveText(oldId);
            AddTextAt(newId, newText, position, charWidth, charHeight, horizontalSpace, visibility);
        }

        public static void ClearAll()
        {
            texts = null;
            textsSprite = null;
            textsVisibility = null;
            texture = null;
            charsSpriteOffset = null;
            defaultCharWidth = 0;
            defaultCharHeight = 0;
            charsAsciiCode = 0;
        }

        public static void PrintTexts()
        {
            int i = 0;
            string[] textsId = new string[texts.Count];

            foreach (string id in texts.Keys)
            {
                textsId[i] = id;
                i++;
            }

            i = 0;

            foreach (Sprite[] textSprite in textsSprite.Values)
            {
                for (int j = 0; j < textSprite.Length; j++)
                {
                    if (!textsVisibility[textsId[i]])
                    {
                        break;
                    }

                    if (texts[textsId[i]][j] == '\n')
                    {
                        continue;
                    }

                    textSprite[j].DrawTexture(texture, (int)charsSpriteOffset[texts[textsId[i]][j]].X, (int)charsSpriteOffset[texts[textsId[i]][j]].Y, (int)defaultCharWidth, (int)defaultCharHeight);
                }

                i++;
            }
        }
    }
}
