using System;
using System.Collections.Generic;
using System.Text;

namespace Catastrophe
{
    public struct ScoreEventArgs
    {
        public int value;
        public powerUpType type;
        public float timer;
    }

    class ScoreManager
    {
        public delegate void PowerUpAction(float timer);

        private Dictionary<powerUpType, PowerUpAction> m_powerActions = new Dictionary<powerUpType, PowerUpAction>();

        public event EventHandler<ScoreEventArgs> FishCollected = delegate { };
         public event EventHandler<ScoreEventArgs> EnemyKilled = delegate { };
         public event EventHandler<ScoreEventArgs> PowerUpCollected = delegate { };

        public void addScore(ScoreEventArgs e)
        {
            FishCollected(this, e);
        }
        public void OnEnemyKilled(ScoreEventArgs e)
        {
            EnemyKilled(this, e);
        }
        public void OnPowerUpCollected(ScoreEventArgs e)
        {
            PowerUpCollected(this, e);
        }
        public delegate void updateScore(int value);
        private Dictionary<int, updateScore> m_KeyBindings = new Dictionary<int, updateScore>();

        public ScoreManager()
        {
            FishCollected += this.incrementScore;
            PowerUpCollected += this.incrementScore;
            PowerUpCollected += this.ActivatePowerUp;
        }

        private void ActivatePowerUp(object sender, ScoreEventArgs e)
        {
           PowerUpAction action = m_powerActions[e.type];
            if (action != null)
            {
                action(e.timer);
            }
        }

        private void incrementScore(object sender, ScoreEventArgs e)
        {
            PlatformerGame.GameScore += e.value;
        }
        
        public void AddPowerUP(powerUpType type, PowerUpAction action)
        {
            m_powerActions.Add(type, action);
        }

    }
}
