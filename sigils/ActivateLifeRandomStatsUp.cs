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
    public class lifecost_ActivateLifeRandomStatsUp : LifeActiveAbilityCost
    {
        public override Ability Ability
        {
            get
            {
                return lifecost_ActivateLifeRandomStatsUp.ability;
            }
        }
        public override int LifeMoneyCost
        {
            get
            {
                return 5;
            }
        }

        public override IEnumerator Activate()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.15f);
            CardModificationInfo mod = base.Card.TemporaryMods.Find((CardModificationInfo x) => x.singletonId == "LifeGambleStatsUp");
            bool isPart = !SaveManager.SaveFile.IsPart2;
            bool flag = mod == null;
            bool flag2 = flag;
            if (flag2)
            {
                mod = new CardModificationInfo();
                mod.singletonId = "LifeGambleStatsUp";
                base.Card.AddTemporaryMod(mod);
            }
            yield return new WaitForSeconds(0.5f);
            int StatsUp = Random.Range(0, 6);
            bool flag3 = isPart;
            if (flag3)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                int num;
                for (int index = 0; index < StatsUp; index = num + 1)
                {
                    int HealthorAttack = Random.Range(1, 100);
                    bool flag4 = HealthorAttack > 50;
                    if (flag4)
                    {
                        yield return new WaitForSeconds(0.2f);
                        mod.healthAdjustment++;
                        base.Card.OnStatsChanged();
                        base.Card.Anim.StrongNegationEffect();
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.2f);
                        mod.attackAdjustment++;
                        base.Card.OnStatsChanged();
                        base.Card.Anim.StrongNegationEffect();
                    }
                    num = index;
                }
                yield return new WaitForSeconds(0.1f);
                Singleton<ViewManager>.Instance.Controller.LockState = 0;
            }
            else
            {
                int num;
                for (int index2 = 0; index2 < StatsUp; index2 = num + 1)
                {
                    int HealthorAttack2 = Random.Range(1, 100);
                    bool flag5 = HealthorAttack2 > 50;
                    if (flag5)
                    {
                        yield return new WaitForSeconds(0.2f);
                        mod.healthAdjustment++;
                        base.Card.OnStatsChanged();
                        base.Card.Anim.StrongNegationEffect();
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.2f);
                        mod.attackAdjustment++;
                        base.Card.OnStatsChanged();
                        base.Card.Anim.StrongNegationEffect();
                    }
                    num = index2;
                }
            }
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
            yield break;
        }

        public static Ability ability;

        private const string MOD_ID = "LifeGambleStatsUp";
    }
}
