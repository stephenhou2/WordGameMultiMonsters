﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WordJourney
{
	public abstract class PassiveSkill : Skill {

//		public enum PassiveSkillTriggerCondition{
//			NotTriggered,
//			BeforeFight,
//			Attack,
//			AttackFinish,
//			BeAttacked,
//			FightEnd
//		}

//		public PassiveSkillTriggerCondition passiveTriggerCondition;

		[System.Serializable]
		public struct TriggerInfo{
			public bool triggered;
			public SkillEffectTarget excutor;
		}


		public TriggerInfo beforeFightTriggerInfo;
		public TriggerInfo attackTriggerInfo;
		public TriggerInfo attackFinishTriggerInfo;
		public TriggerInfo BeAttackedTriggerInfo;
		public TriggerInfo fightEndTriggerInfo;

		public string stateName;

//		public SkillEffectTarget effectTarget;

//		protected BattleAgentController targetBa;

		void Start(){
			if (skillType != SkillType.Passive) {
				Debug.LogError (string.Format ("{0}技能类型必须是被动类型", skillName));
			}
		}

		public override void AffectAgents (BattleAgentController self, BattleAgentController enemy)
		{


			ExcuteSkillLogic (self, enemy);

			BattleAgentController targetBa = null;

			if (beforeFightTriggerInfo.triggered) {
				switch (beforeFightTriggerInfo.excutor) {
				case SkillEffectTarget.Self:
					targetBa = self;
					break;
				case SkillEffectTarget.Enemy:
					targetBa = enemy;
					break;
				}
				targetBa.beforeFightTriggerCallBacks.Add (BeforeFightTriggerCallBack);
			}
			if (attackTriggerInfo.triggered) {
				switch (attackTriggerInfo.excutor) {
				case SkillEffectTarget.Self:
					targetBa = self;
					break;
				case SkillEffectTarget.Enemy:
					targetBa = enemy;
					break;
				}
				targetBa.attackTriggerCallBacks.Add (AttackTriggerCallBack);
			}
			if (attackFinishTriggerInfo.triggered) {
				switch (attackFinishTriggerInfo.excutor) {
				case SkillEffectTarget.Self:
					targetBa = self;
					break;
				case SkillEffectTarget.Enemy:
					targetBa = enemy;
					break;
				}
				targetBa.attackFinishTriggerCallBacks.Add (AttackFinishTriggerCallBack);
			}
			if (BeAttackedTriggerInfo.triggered) {
				switch (BeAttackedTriggerInfo.excutor) {
				case SkillEffectTarget.Self:
					targetBa = self;
					break;
				case SkillEffectTarget.Enemy:
					targetBa = enemy;
					break;
				}
				targetBa.beAttackedTriggerCallBacks.Add (BeAttackedTriggerCallBack);
			}
			if (fightEndTriggerInfo.triggered) {
				switch (fightEndTriggerInfo.excutor) {
				case SkillEffectTarget.Self:
					targetBa = self;
					break;
				case SkillEffectTarget.Enemy:
					targetBa = enemy;
					break;
				}
				targetBa.fightEndTriggerCallBacks.Add (FightEndTriggerCallBack);
			}
		}


		protected virtual void BeforeFightTriggerCallBack(BattleAgentController self,BattleAgentController enemy){

		}

		protected virtual void AttackTriggerCallBack(BattleAgentController self,BattleAgentController enemy){

		}

		protected virtual void AttackFinishTriggerCallBack(BattleAgentController self,BattleAgentController enemy){

		}

		protected virtual void BeAttackedTriggerCallBack(BattleAgentController self,BattleAgentController enemy){

		}

		protected virtual void FightEndTriggerCallBack(BattleAgentController self,BattleAgentController enemy){

		}

	}
}
