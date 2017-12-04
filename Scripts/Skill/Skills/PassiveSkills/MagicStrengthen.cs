﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WordJourney
{
	public class MagicStrengthen : TalentSkill {

		public float manaScalerBase;

		protected override void Awake ()
		{
			base.Awake ();
			skillName = "魔法强化";
			skillDescription = string.Format("提升<color=orange>{0}*技能等级%</color>的魔法值",(int)(manaScalerBase*100));
		}

		protected override void ExcuteSkillLogic (BattleAgentController self, BattleAgentController enemy)
		{
			self.agent.SetBasePropertyGainScalers (0, 0, 0, 0, 0, 0, 0, manaScalerBase * skillLevel);
			self.agent.ResetBattleAgentProperties (false);
		}


	}
}