using System.Collections;
using GBC;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Art = LifeCost.Resources.Sigils;
using System.Linq;
using BepInEx.Bootstrap;
using LifeCost.sigils;

namespace LifeCost.sigils
{
    public class lifecost_ActivateLifeConverter : LifeActiveAbilityCost
    {
        public override Ability Ability
        {
            get
            {
                return lifecost_ActivateLifeConverter.ability;
            }
        }

        public override int LifeCost
        {
            get
            {
                return 2;
            }
        }

        public override IEnumerator Activate()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.15f);
            bool flag = !SaveManager.SaveFile.IsPart2;
            bool flag2 = flag;
            if (flag2)
            {
                bool flag3 = Chainloader.PluginInfos.ContainsKey("extraVoid.inscryption.LifeCost");
                if (flag3)
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
                    yield return new WaitForSeconds(0.25f);
                    RunState.Run.currency += 2;
                    yield return Singleton<CurrencyBowl>.Instance.DropWeightsIn(2);
                    yield return new WaitForSeconds(0.75f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                    Singleton<ViewManager>.Instance.Controller.LockState = 0;
                }
                else
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
                    yield return new WaitForSeconds(0.25f);
                    RunState.Run.currency += 2;
                    yield return Singleton<CurrencyBowl>.Instance.ShowGain(2, true, false);
                    yield return new WaitForSeconds(0.25f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                    Singleton<ViewManager>.Instance.Controller.LockState = 0;
                }
            }
            else
            {
                SaveData.Data.currency += 2;
                base.Card.Anim.LightNegationEffect();
            }
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
            yield break;
        }

        // Token: 0x0400000F RID: 15
        public static Ability ability;
    }
}
