using System.Collections.Generic;
using Aiv.Fast2D;
using OpenTK;

namespace Heads
{
    static class Game
    {
        public static Window Window { get; private set; }
        public static float DeltaTime { get { return Window.DeltaTime; } }
        public static int WindowWidth { get { return Window.Width; } }
        public static int WindowHeight { get { return Window.Height; } }
        public static int HalfWindowWidth { get { return (int)(WindowWidth * 0.5f); } }
        public static int HalfWindowHeight { get { return (int)(WindowHeight * 0.5f); } }
        public static float OrthoWidth { get { return Window.OrthoWidth; } }
        public static float OrthoHeight { get { return Window.OrthoHeight; } }
        public static float OrthoHalfWidth { get { return OrthoWidth * 0.5f; } }
        public static float OrthoHalfHeight { get { return OrthoHeight * 0.5f; } }
        public static Vector2 ScreenCenter { get => new Vector2(OrthoHalfWidth, OrthoHalfHeight); }
        public static float HalfDiagonalSquared { get => ScreenCenter.LengthSquared; }
        public static Scene CurrentScene;
        public static List<KeyboardController> KeyboardCtrls;
        public static List<JoypadController> JoypadCtrls;
        public static int NumMaxPlayers { get; private set; }
        private static float optimalUnitSize;
        private static float optimalScreenHeight;
        private static bool isMousePressed;
        private static Vector2 lastMousePositionClicked;

        public static void Init()
        {
            Window = new Window(1280, 720, "Heads");
            Window.Position = Vector2.Zero;
            Window.SetDefaultViewportOrthographicSize(10);
            optimalScreenHeight = 1080;
            optimalUnitSize = optimalScreenHeight / Window.OrthoHeight;

            //System.Console.WriteLine(Window.CurrentViewportOrthographicSize + "\n" + Window.OrthoWidth + "\n" + Window.OrthoHeight);

            string[] joypadsConnected = Window.Joysticks;
            JoypadCtrls = new List<JoypadController>();
            KeyboardCtrls = new List<KeyboardController>();
            NumMaxPlayers = 2;

            List<KeyCode>[] keysConfig = new List<KeyCode>[NumMaxPlayers];

            //the order of keys config is: up, down, left, right and fire

            //player 1 config
            keysConfig[0] = new List<KeyCode>
            {
                KeyCode.W,
                KeyCode.S,
                KeyCode.A,
                KeyCode.D,
                KeyCode.Space
            };

            //player 2 config
            keysConfig[1] = new List<KeyCode>
            {
                KeyCode.Up,
                KeyCode.Down,
                KeyCode.Left,
                KeyCode.Right,
                KeyCode.Keypad0
            };

            for (int i = 0; i < NumMaxPlayers; i++)
            {
                KeyboardCtrls.Add(new KeyboardController(i, keysConfig[i]));

                //System.Console.WriteLine(joypadsConnected[i] + "pos: " + i);

                if (joypadsConnected[i] != null && joypadsConnected[i] != "Unmapped Controller")
                {
                    JoypadCtrls.Add(new PS4Controller(i));
                }
            }

            CurrentScene = new PlayScene();
        }

        public static void Run()
        {
            CurrentScene.Start();

            while (Window.IsOpened)
            {
                //System.Console.WriteLine("Mouse X: " + (Window.MouseX) + "\nMouse Y: " + (Window.MouseY));

                //for (int i = 0; i < JoypadCtrls.Count; i++)
                //{
                //    System.Console.WriteLine(Window.JoystickDebug(i));
                //}

                if (CurrentScene.IsPlaying)
                {
                    if (Window.MouseLeft)
                    {
                        if (!isMousePressed)
                        {
                            lastMousePositionClicked = Window.MousePosition;
                            isMousePressed = true;
                        }
                    }

                    else if (isMousePressed)
                    {
                        isMousePressed = false;
                    }

                    if (Window.MousePosition == lastMousePositionClicked &&
                        Window.MouseX >= -0.1178782f &&
                        Window.MouseX <= 17.84246f &&
                        Window.MouseY >= -0.476787925f &&
                        Window.MouseY <= -0.01254705f &&
                        isMousePressed &&
                        !Window.IsFullScreen())
                    {
                        Window.Update();
                        continue;
                    }

                    CurrentScene.Update();

                    //window update
                    Window.Update();
                }

                else
                {
                    CurrentScene.OnExit();

                    if (CurrentScene.NextScene != null)
                    {
                        CurrentScene = CurrentScene.NextScene;
                        CurrentScene.Start();
                    }

                    else
                    {
                        return;
                    }
                }
            }
        }

        public static float PixelsToUnits(float val)
        {
            return val / optimalUnitSize;
        }
    }
}
