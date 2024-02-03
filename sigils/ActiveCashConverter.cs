using System.Collections;
using DiskCardGame;
using UnityEngine;
using LifeCost.Patchers;

namespace LifeCost.sigils
{
    public class lifecost_ActivateCashConverter : LifeActiveAbilityCost
    {
        public override Ability Ability
        {
            get
            {
                return lifecost_ActivateCashConverter.ability;
            }
        }

        public override int MoneyCost
        {
            get
            {
                return 4;
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
                float waitTime = 0.1f;
                Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
                yield return new WaitForSeconds(waitTime);
                yield return PayCost.ShowDamageSequence(1, 1, false, 0.125f, null, 0f, false);
                yield return new WaitForSeconds(waitTime);
                Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
                Singleton<ViewManager>.Instance.Controller.LockState = 0;
            }
            else
            {
                float waitTime2 = 0.5f;
                yield return new WaitForSeconds(waitTime2);
                yield return PayCost.ShowDamageSequence(1, 1, false, 0.125f, null, 0f, false);
                yield return new WaitForSeconds(waitTime2);
            }
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
            yield break;
        }


        public static Ability ability;
    }
}
