using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToolKid.TimerSystem {

    public class GameWatch : TKBehaviour {
        private bool isStart;
        private bool isPause;
        public Text display;

        [SerializeField]
        private MainWatch mainWatch;
        public static MainWatch Main {
            get {
                return main;
            }
        }
        private static readonly MainWatch main = new MainWatch();

        public event EventHandler<bool> Pause;

        public List<Timer> eventWatches;

        protected void Awake() {
            Begin();            
        }

        protected void Begin() {
            isStart = true;
            isPause = false;
            main.Reset(this);
        }

        public void OnPause(object sender, bool isPause) {
            if (isPause) {
                main.SetPauseTimeOf(PauseState.Begin);
            }
            else {
                main.SetPauseTimeOf(PauseState.End);
            }
            this.isPause = isPause;
            Pause?.Invoke(sender, isPause);
        }

        private void End(object sender, EventArgs e) {
            isStart = false;
        }

        void Update() {
            if (!isPause && isStart) {
                mainWatch = main.Update(this);
                if (display) {
                    display.text = mainWatch.PlayTime.ToString("000.00");
                }
            }
        }
    }
}