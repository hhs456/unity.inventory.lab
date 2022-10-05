using System;
using UnityEngine;

namespace ToolKid.TimerSystem {
    /// <summary>
    /// Base of the game timer.
    /// </summary>
    [System.Serializable]
    public class WatchArgs : IWatchSystem {
        [SerializeField]
        protected string name;
        [SerializeField]
        protected double startTime;
        [SerializeField]
        protected double playTime;
        public double PlayTime {
            get {
                return playTime;
            }
        }
        [SerializeField]
        protected int minute;
        [SerializeField]
        protected int second;
        [SerializeField]
        protected int millisecond;
        public int Minute {
            get {
                return minute;
            }
        }
        public int Second {
            get {
                return second;
            }
        }
        public int Millisecond {
            get {
                return millisecond;
            }
        }
        [SerializeField]
        protected double pauesTime;
        [SerializeField]
        protected double pauseBeginTime;
        [SerializeField]
        protected double pauseEndTime;

        public double PauesTime {
            get {
                return pauesTime;
            }
        }

        public event EventHandler<EventArgs> Begin;

        public virtual void Start(object sender) {
            startTime = AudioSettings.dspTime;
            Begin?.Invoke(sender, new EventArgs());
        }
        /// <summary>
        /// Call method while scene's pause event is trigger. 
        /// </summary>
        /// <param name="state"></param>
        public virtual void OnPause(ActionState state) {
            switch (state) {
                case ActionState.Begin:
                    pauseBeginTime = AudioSettings.dspTime;
                    break;
                case ActionState.End:
                    pauseEndTime = AudioSettings.dspTime;
                    pauesTime += pauseEndTime - pauseBeginTime;
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Main timer of scene.
    /// </summary>
    [System.Serializable]
    public class MainWatch : WatchArgs {        
        public event EventHandler<WatchArgs> WatchUpdate;

        public void Start(UnityEngine.Object sender) {
            name = "Main";
            base.Start(sender);
        }

        public MainWatch Update(object sender) {
            playTime = AudioSettings.dspTime - startTime - pauesTime;
            minute = (int)(playTime / 60f);
            second = (int)playTime - minute * 60;
            millisecond = (int)(playTime * 100f % 100f);
            WatchUpdate?.Invoke(sender, this);
            return this;
        }

        public override void OnPause(ActionState state) {
            switch (state) {
                case ActionState.Begin:
                    pauseBeginTime = AudioSettings.dspTime;
                    break;
                case ActionState.End:
                    pauseEndTime = AudioSettings.dspTime;
                    pauesTime += pauseEndTime - pauseBeginTime;
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// Sub timer of scene.
    /// </summary>
    [System.Serializable]
    public class Timer : WatchArgs {
        [SerializeField]
        protected double triggerTime;
        
        public event EventHandler<WatchArgs> Trigger;

        public Timer(string name, double triggerTime) {
            this.name = name;
            this.triggerTime = triggerTime;
        }

        public void Start(UnityEngine.Object sender) {
            playTime = 0;
            base.Start(sender);
            GameWatch.Main.WatchUpdate += Update;
        }

        public override void Start(object sender) {
            base.Start(sender);
            GameWatch.Main.WatchUpdate += Update;
        }

        private void Update(object sender, WatchArgs e) {
            playTime = AudioSettings.dspTime - startTime - pauesTime;
            if (playTime >= triggerTime) {
                Trigger?.Invoke(this, e);
                GameWatch.Main.WatchUpdate -= Update;
            }
        }
    }

    public interface IWatchSystem {
        public double PlayTime { get; }
        public int Minute { get; }
        public int Second { get; }
        public int Millisecond { get; }
        public double PauesTime { get; }
        
        public void Start(object sender);        
        public void OnPause(ActionState state);
    }

    public enum ActionState {
        Begin, End
    }

}