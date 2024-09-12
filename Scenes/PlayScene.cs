using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OpenTK;

namespace Heads
{
    class PlayScene : Scene
    {
        public Background Background;
        public List<Player> Players;
        private int numPlayersAlive;
        private bool gameEnded;

        public PlayScene() : base()
        {

        }

        private void LoadAssets()
        {
            TextureMngr.AddTexture("player", "Assets/player_1.png");
            TextureMngr.AddTexture("enemy", "Assets/enemy_0.png");
            TextureMngr.AddTexture("background", "Assets/background.png");
            TextureMngr.AddTexture("bullet", "Assets/fireball.png");
            TextureMngr.AddTexture("textImage", "Assets/comics.png");
            TextureMngr.AddTexture("frameProgressBar", "Assets/loadingBar_frame.png");
            TextureMngr.AddTexture("progressBar", "Assets/loadingBar_bar.png");
            TextureMngr.AddTexture("energyPowerUp", "Assets/heart.png");
        }

        public override void Start()
        {
            base.Start();

            numPlayersAlive = Game.NumMaxPlayers;

            LoadAssets();

            TextMngr.Init();

            Background = new Background();

            Players = new List<Player>();
            float playerStartX = 1;
            float playerStartY = 3;
            float playerTextStartY = 0.5f;

            for (int i = 0; i < Game.NumMaxPlayers; i++)
            {
                TextMngr.AddTextAt("p" + i, "PLAYER " + (i + 1), new Vector2(0.5f, playerTextStartY), 50, 50, 0.2f);

                Players.Add(new Player(i));

                Players[i].Position = new Vector2(playerStartX, playerStartY);
                Players[i].EnergyBar.Position = new Vector2(TextMngr.GetTextLength("p" + i) + 0.4f, playerTextStartY + 0.15f);

                Actor.ActiveActors.Add(Players[i], Players[i].RigidBody);

                playerStartY += 1.5f;
                playerTextStartY += 0.5f;
            }

            BulletMngr.Init();
            EnemyMngr.Init();
            PowerUpsMngr.Init();

            EnemyMngr.SpawnEnemy();
        }

        public override void Update()
        {
            for (int i = 0; i < Game.NumMaxPlayers; i++)
            {
                Players[i].Input();
            }

            //powerUps spawn
            PowerUpsMngr.Spawn();

            //update
            PhysicsMngr.Update();
            UpdateMngr.Update();

            PhysicsMngr.CheckCollisions();

            //draw
            DrawMngr.Draw();
            DebugMngr.Draw();

            //printing texts
            TextMngr.PrintTexts();

            

            if (gameEnded)
            {
                IsPlaying = false;

                Game.Window.Update();
                Thread.Sleep(2000);
            }
        }

        public void OnPlayerDies()
        {
            numPlayersAlive--;

            if (numPlayersAlive <= 0)
            {
                gameEnded = true;
            }
        }

        public override void OnExit()
        {
            EnemyMngr.ClearAll();
            BulletMngr.ClearAll();
            UpdateMngr.ClearAll();
            PhysicsMngr.ClearAll();
            PowerUpsMngr.ClearAll();
            TextMngr.ClearAll();
            DrawMngr.ClearAll();
            TextureMngr.ClearAll();
            DebugMngr.ClearAll();

            Players = null;
            Background = null;
            NextScene = null;
        }
    }
}
