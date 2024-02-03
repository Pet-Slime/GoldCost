using System.Collections;
using DiskCardGame;
using LifeCost.Patchers;
using UnityEngine;
using Random = System.Random;

namespace LifeCost.sigils
{
    public class lifecost_ActivateEnergyGamble : LifeActiveAbilityCost
    {
        public override Ability Ability
        {
            get
            {
                return lifecost_ActivateEnergyGamble.ability;
            }
        }

        public override int EnergyCost
        {
            get
            {
                return 6;
            }
        }

        public override IEnumerator Activate()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.15f);
            bool flag = !SaveManager.SaveFile.IsPart2;
            int amount = UnityEngine.Random.Range(0, 3);
            bool flag2 = amount == 0;
            if (flag2)
            {
                yield return base.LearnAbility(0.25f);
                yield return new WaitForSeconds(0.1f);
                yield break;
            }
            Random rnd = new Random();
            bool whoGetsit = rnd.Next(2) == 0;
            bool flag3 = flag;
            if (flag3)
            {
                float waitTime = 0.1f;
                Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
                yield return new WaitForSeconds(waitTime);
                base.Card.Anim.LightNegationEffect();
                yield return PayCost.ShowDamageSequence(amount, amount, whoGetsit, 0.125f, null, 0f, false);
                yield return new WaitForSeconds(waitTime);
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                Singleton<ViewManager>.Instance.Controller.LockState = 0;
            }
            else
            {
                float waitTime2 = 0.5f;
                yield return new WaitForSeconds(waitTime2);
                base.Card.Anim.LightNegationEffect();
                yield return PayCost.ShowDamageSequence(amount, amount, whoGetsit, 0.125f, null, 0f, false);
                yield return new WaitForSeconds(waitTime2);
            }
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
            yield break;
        }

        public static Ability ability;
    }
}
