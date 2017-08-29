﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



namespace WordJourney
{
	public class Monster : Agent{

		public int monsterId;

		private BattleMonsterController mBaMonsterController;

		// 角色UIView
		public BattleMonsterController baView{

			get{
				if (mBaMonsterController == null) {
					mBaMonsterController = GetComponent<BattleMonsterController> ();
				}
				return mBaMonsterController;
			}

		}

		public void SetupMonster(int gameProcess){
			//		GameManager.gameManager.OnGenerateSkill ();
			//		GameManager.gameManager.skillGenerator.GenerateSkillWithIds (2, 20,this);
		}

		//怪物的技能选择
		//	public Skill SkillOfMonster(){
		//		Skill monsterSkill = null;
		//		switch (validActionType) {
		//		case ValidActionType.All:
		//			
		//			break;
		//		case ValidActionType.PhysicalExcption:
		//			
		//			break;
		//		case ValidActionType.MagicException:
		//			
		//			break;
		//		case ValidActionType.PhysicalOnly:
		//			
		//			break;
		//		case ValidActionType.MagicOnly:
		//			
		//			break;
		//		default:
		//			break;
		//		}
		//
		//		return monsterSkill;
		//
		//	}


		public void ManageSkillAvalibility(){
			// 如果技能还在冷却中或者怪物气力值小于技能消耗的气力值，则相应技能不可用
			for (int i = 0;i < equipedSkills.Count;i++) {
				Skill s = equipedSkills [i];

				if (s.isAvalible == false) {
					if (mana >= s.manaConsume) {
						s.isAvalible = true;
					} 
				}
			}

		}

	}
}
