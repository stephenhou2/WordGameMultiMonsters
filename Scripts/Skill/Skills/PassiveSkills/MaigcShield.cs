﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WordJourney
{
	public class MaigcShield : TriggeredPassiveSkill {

		public int duration;

		public float decreaseHurtScalerBase;

		public float probability;

		private Coroutine magicShieldCoroutine;

		protected override void Awake ()
		{
			base.Awake ();
			skillName = "魔法盾";
			skillDescription = string.Format ("受到攻击时有<color=orange>{0}%</color>的概率产生一个魔法盾,魔法盾存期间可以减少<color=orange>{1}*技能等级%</color>的伤害，持续时间<color=orange>{2}s</color>",(int)(probability * 100),(int)(decreaseHurtScalerBase * 100),duration);
		}

		protected override void ExcuteSkillLogic (BattleAgentController self, BattleAgentController enemy){
		
//			if (levelChanged) {
//				self.agent.decreaseHurtScaler = decreaseHurtScalerBase * skillLevel;
//				self.agent.ResetBattleAgentProperties (false);
//				levelChanged = false;
//			}
		}

		protected override void BeAttackedTriggerCallBack (BattleAgentController self, BattleAgentController enemy)
		{

			if (isEffective (probability)) {

				StopCoroutine (magicShieldCoroutine);

				self.SetEffectAnim (selfEffectName, true);

				self.agent.decreaseHurtScaler = decreaseHurtScalerBase * skillLevel;

				magicShieldCoroutine = StartCoroutine ("EndMagicShield",self);
			}

		}

		private IEnumerator EndMagicShield(BattleAgentController targetBa){
			yield return new WaitForSeconds (duration);
			targetBa.SetEffectAnim (selfEffectName, false);
			targetBa.agent.decreaseHurtScaler = 0;
		}

	}
}