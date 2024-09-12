using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads
{
    abstract class Scene
    {
        public bool IsPlaying;
        public Scene NextScene;

        public Scene()
        {

        }

        public virtual void Start()
        {
            IsPlaying = true;
        }

        public abstract void OnExit();
        public abstract void Update();
    }
}
