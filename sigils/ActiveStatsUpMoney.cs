using System.Collections;
using GBC;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Art = LifeCost.Resources.Sigils;
using System.Linq;
using LifeCost.sigils;

namespace LifeCost.sigils
{

    public class lifecost_ActiveStatsUpMoney : LifeActiveAbilityCost
    {

        public override Ability Ability
        {
            get
            {
                return lifecost_ActiveStatsUpMoney.ability;
            }
        }

        public override int MoneyCost
        {
            get
            {
                return 5;
            }
        }

        public override IEnumerator Activate()
        {
            CardModificationInfo mod = base.Card.TemporaryMods.Find((CardModificationInfo x) => x.singletonId == "statsUp");
            bool flag = mod == null;
            bool flag2 = flag;
            if (flag2)
            {
                mod = new CardModificationInfo();
                mod.singletonId = "statsUp";
                base.Card.AddTemporaryMod(mod);
            }
            mod.attackAdjustment++;
            mod.healthAdjustment++;
            base.Card.OnStatsChanged();
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
            yield return new WaitForSeconds(0.25f);
            Singleton<ViewManager>.Instance.Controller.LockState = 0;
            yield break;
        }

        public static Ability ability;

        private const string MOD_ID = "statsUp";
    }
}