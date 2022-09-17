using System;
using UnityEngine;

namespace ToolKid.TimerSystem {
    /// <summary>
    /// Base of the game timer.
    /// </summary>
    [System.Serializable]
    public class WatchArgs : IWatchSystem {
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

        public virtual void Reset(object sender) {
            Start(sender);
        }

        public virtual void Start(object sender) {
            startTime = AudioSettings.dspTime;
            Begin?.Invoke(sender, new EventArgs());
        }

        public virtual void SetPauseTimeOf(PauseState state) {
            switch (state) {
                case PauseState.Begin:
                    pauseBeginTime = AudioSettings.dspTime;
                    break;
                case PauseState.End:
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

        public MainWatch Update(object sender) {
            playTime = AudioSettings.dspTime - startTime - pauesTime;
            minute = (int)(playTime / 60f);
            second = (int)playTime - minute * 60;
            millisecond = (int)(playTime * 100f % 100f);
            WatchUpdate?.Invoke(sender, this);
            return this;
        }

        public override void SetPauseTimeOf(PauseState state) {
            switch (state) {
                case PauseState.Begin:
                    pauseBeginTime = AudioSettings.dspTime;
                    break;
                case PauseState.End:
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

        public override void Reset(object sender) {
            playTime = 0;
            base.Reset(sender);
        }

        public override void Start(object sender) {
            base.Start(sender);
            ((MainWatch)sender).WatchUpdate += Update;
        }

        private void Update(object sender, WatchArgs e) {
            if (playTime >= triggerTime) {
                Trigger?.Invoke(this, e);
                ((MainWatch)sender).WatchUpdate -= Update;
            }
        }
    }

    public interface IWatchSystem {
        public double PlayTime { get; }
        public int Minute { get; }
        public int Second { get; }
        public int Millisecond { get; }
        public double PauesTime { get; }

        public void Reset(object sender);
        public void Start(object sender);        
        public void SetPauseTimeOf(PauseState state);
    }

    public enum PauseState {
        Begin, End
    }

}