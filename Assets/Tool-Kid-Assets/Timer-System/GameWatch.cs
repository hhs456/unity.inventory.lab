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
            main.Start(this);            
            eventWatches.Add(new Timer("After Abundon 10 sec" ,10d));
            eventWatches[0].Trigger += TriggerTest;
            eventWatches[1].Trigger += TriggerBackTest;
        }

        public void StartFromUnityEvent(int index) {
            eventWatches[index].Start(this);
        }

        private void TriggerTest(object sender, WatchArgs e) {
            eventWatches[1].Start(this);
        }
        private void TriggerBackTest(object sender, WatchArgs e) {
            eventWatches[0].Start(this);
        }

        public void OnPause(object sender, bool isPause) {
            if (isPause) {
                main.OnPause(ActionState.Begin);
            }
            else {
                main.OnPause(ActionState.End);
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