using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Catastrophe
{
    
    public class EnemyInfo
    {
        public bool isEnemyStatic;
        public float CatSpeed = 0.0f;
        public float DogSpeed = 0.0f;
        public int KillScore;
    }

    public class FishInfo
    {
        public string Texture;
        public float BounceHeight = 0.0f;
        public float BounceRate = 0.0f;
        public Color Color;
        public int Score;
        public float Size;
    }
    public class ShieldInfo
    {
        public float timer;
        public Color Color;
        public float Size;
    }
    public class SpeedPowerUpInfo
    {
        public float timer;
        public int speed;
        public Color Color;
        public float Size;
    }
    public class GameInfo
    {
        private static GameInfo mInstance = null;
        public static GameInfo Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new GameInfo();
                return mInstance;
            }

            set { mInstance = value; }
        }

        public EnemyInfo EnemyInfo;
        public FishInfo fishInfo;
        public SpeedPowerUpInfo SpeedInfo;
        public ShieldInfo ShieldInfo;
    }
}
