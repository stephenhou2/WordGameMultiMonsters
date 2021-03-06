﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WordJourney
{
	public enum SpecialAttackResult{
		None,
		Crit,
		Miss,
		Gain,
		Status
	}

	public class AgentPropertyCalculator {

		private float armorSeed = 0.01f; //计算护甲抵消伤害的种子数

		private float magicResistSeed = 0.01f; //计算魔抗抵消伤害的种子数

		public SpecialAttackResult specialAttackResult = SpecialAttackResult.None;

		public BattleAgentController self;

		public BattleAgentController enemy;

		public int maxHealth;
		public int health;
		public int mana;

		public int attack;
		public int attackSpeed;
		public int armor;
		public int magicResist;
		public int dodge;
		public int crit;
		public int hit;

//		public int physicalHurtFromNomalAttack;

		public float physicalHurtScaler;
		public float magicalHurtScaler;

		public float critHurtScaler;//暴击倍率

		public int physicalHurtToEnemy;
		public int magicalHurtToEnemy;


		public float critFixScaler;
		public float dodgeFixScaler;

//		public int maxHealthChangeFromTriggeredSkill;
//		public int hitChangeFromTriggeredSkill;
//		public int manaChangeFromTriggeredSkill;
//		public int attackChangeFromTriggeredSkill;
//		public int attackSpeedChangeFromTriggeredSkill;
//		public int armorChangeFromTriggeredSkill;
//		public int magicResistChangeFromTriggeredSkill;
//		public int dodgeChangeFromTriggeredSkill;
//		public int critChangeFromTriggeredSkill;

//		public float maxHealthChangeScalerFromTriggeredSkill;
//		public float hitChangeScalerFromTriggeredSkill;
//		public float manaChangeScalerFromTriggeredSkill;
//		public float attackChangeScalerFromTriggeredSkill;
//		public float attackSpeedChangeScalerFromTriggeredSkill;
//		public float armorChangeScalerFromTriggeredSkill;
//		public float magicResistChangeScalerFromTriggeredSkill;
//		public float dodgeChangeScalerFromTriggeredSkill;
//		public float critChangeScalerFromTriggeredSkill;
//		public float physicalHurtScalerChangeFromTriggeredSkill;
//		public float magicalHurtScalerChangeFromTriggeredSkill;
//		public float critHurtScalerChangeFromTriggeredSkill;

		/// <summary>
		/// 如果是即时性的属性变化，需要使用这个方法
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="propertyType">Property type.</param>
		/// <param name="change">Change.</param>
		public void InstantPropertyChange(BattleAgentController target,PropertyType propertyType,float change,bool fromTriggeredSkill = true){

//			string instantChange = string.Format ("即时性属性变化: 角色名称：{0}，类型：{1}，变化：{2}",
//				target.agent.agentName, propertyType,change);
//
//			Debug.Log (instantChange);

			if (change == 0) {
				return;
			}

			int healthChange = 0;
			string tintText = "";

			if (propertyType == PropertyType.Health) {
				if (change > 0 && change < 1) {
					healthChange = (int)(target.agent.maxHealth * change);
					tintText = string.Format ("<color=green>{0}</color>", healthChange.ToString ());
					target.AddFightTextToQueue (tintText, SpecialAttackResult.Gain);
				} else if (change >= 1) {
					healthChange = (int)change;
					tintText = string.Format ("<color=green>{0}</color>", healthChange.ToString ());
					target.AddFightTextToQueue (tintText, SpecialAttackResult.Gain);
				} else if (change < 0 && change > -1) {
					healthChange = (int)(target.agent.maxHealth * change);
					tintText = string.Format ("<color=red>{0}</color>", (-healthChange).ToString ());
					target.AddFightTextToQueue (tintText, SpecialAttackResult.None);
				} else {
					healthChange = (int)change;
					tintText = string.Format ("<color=red>{0}</color>", (-healthChange).ToString ());
					target.AddFightTextToQueue (tintText, SpecialAttackResult.None);
				}

				target.agent.health += healthChange;
				target.propertyCalculator.health = target.agent.health;

				if (target.agent.health <= 0) {
					target.AgentDie ();
				}

			} else {

				AgentPropertyChange (propertyType, change, fromTriggeredSkill);
		
				target.agent.ResetPropertiesWithPropertyCalculator (this);

			}
	
			target.UpdateStatusPlane ();
		}


		public void CalculateAgentHealth(){

			int actualPhysicalHurt = (int)(enemy.propertyCalculator.physicalHurtToEnemy * enemy.propertyCalculator.physicalHurtScaler / (1 + armorSeed * armor));

			int actualMagicalHurt= (int)(enemy.propertyCalculator.magicalHurtToEnemy * enemy.propertyCalculator.magicalHurtScaler / (1 + magicResistSeed * magicResist));

			health -=  actualPhysicalHurt + actualMagicalHurt;

		}

		public void ResetAllHurt(){
//			physicalHurtFromNomalAttack = 0;
			physicalHurtToEnemy = 0;
			magicalHurtToEnemy = 0;
//			hurtReflect = 0;
//			healthAbsorb = 0;
		}

		public List<TriggeredSkill> triggeredSkills = new List<TriggeredSkill>();
		public List<ConsumablesSkill> consumablesSkills = new List<ConsumablesSkill> ();

		public void SkillTriggered<T>(T skill){

			if (typeof(T) == typeof(TriggeredSkill)) {
				
				TriggeredSkill trigSkill = skill as TriggeredSkill;

				if (trigSkill.statusName == "") {
					return;
				}

				if(!triggeredSkills.Contains(trigSkill)){
					triggeredSkills.Add (trigSkill);
				}

				if (!self.agent.allStatus.Contains (trigSkill.statusName)) {
					self.agent.allStatus.Add (trigSkill.statusName);
				}

				string statusTint = "";
				if (MyTool.propertyChangeStrings.TryGetValue (trigSkill.statusName,out statusTint)) {
					self.AddFightTextToQueue (statusTint,SpecialAttackResult.Status);
				}

				self.UpdateStatusPlane ();

			} else if (typeof(T) == typeof(ConsumablesSkill)) {
				ConsumablesSkill consSkill = skill as ConsumablesSkill;
				if (consSkill.statusName == "") {
					return;
				}
				if (!consumablesSkills.Contains (consSkill)) {
					consumablesSkills.Add (consSkill);
				}
				if (!self.agent.allStatus.Contains (consSkill.statusName)) {
					self.agent.allStatus.Add (consSkill.statusName);
				}
				self.UpdateStatusPlane ();
			}
		}

		public void RemoveAttachedSkill<T>(T skill){
			
			if (typeof(T) == typeof(TriggeredSkill) && triggeredSkills.Contains(skill as TriggeredSkill)) {
				TriggeredSkill trigSkill = skill as TriggeredSkill;

				if (trigSkill.statusName == "") {
					return;
				}

				if (triggeredSkills.Contains (trigSkill)) {
					triggeredSkills.Remove (trigSkill);
				}

				if (self.agent.allStatus.Contains (trigSkill.statusName)) {
					self.agent.allStatus.Remove (trigSkill.statusName);
				}

				self.UpdateStatusPlane ();

			} else if (typeof(T) == typeof(ConsumablesSkill) && consumablesSkills.Contains(skill as ConsumablesSkill)) {

				ConsumablesSkill consSkill = skill as ConsumablesSkill;

				if (consSkill.statusName == "") {
					return;
				}

				if (consumablesSkills.Contains (consSkill)) {
					consumablesSkills.Remove (consSkill);
				}

				if (self.agent.allStatus.Contains (consSkill.statusName)) {
					self.agent.allStatus.Remove (consSkill.statusName);
				}

				self.UpdateStatusPlane ();
			}
		}

		public List<TriggeredSkill> GetTriggeredSkillsWithSameStatus(string statusName){
			
			if (statusName == "") {
				string error = "技能的状态名不能为空";
				Debug.LogError (error);
				return null;
			}

			List<TriggeredSkill> sameStatusSkills = new List<TriggeredSkill> ();

			for (int i = 0; i < triggeredSkills.Count; i++) {
				if (triggeredSkills [i].statusName == statusName) {
					sameStatusSkills.Add (triggeredSkills [i]);
				}
			}

			return sameStatusSkills;

		}

		public List<ConsumablesSkill> GetConsumablesSkillsWithSameStatus(string statusName){
			
			List<ConsumablesSkill> sameStatusSkills = new List<ConsumablesSkill> ();

			for (int i = 0; i < triggeredSkills.Count; i++) {
				if (consumablesSkills [i].statusName == statusName) {
					sameStatusSkills.Add (consumablesSkills [i]);
				}
			}

			return sameStatusSkills;
		}

		public void ClearSkillsOfType<T>(){
			if (typeof(T) == typeof(TriggeredSkill)) {
				while (triggeredSkills.Count > 0) {
					triggeredSkills [0].CancelSkillEffect (true);
				}
			} else if (typeof(T) == typeof(ConsumablesSkill)) {
				while (consumablesSkills.Count > 0) {
					consumablesSkills [0].CancelSkillEffect (self);
				}
			}
		}
			





		/// <summary>
		/// 属性变更
		/// </summary>
		/// <param name="propertyType">Property type.</param>
		/// <param name="change">change.</param>
		private void AgentPropertyChange(PropertyType propertyType,float change,bool fromTriggeredSkill = true){

			if (propertyType == PropertyType.Health) {
				return;
			}

			self.agent.AddPropertyChangeFromOther (propertyType, change);

//			switch (propertyType) {
//			case PropertyType.MaxHealth:
//				if (change > -1 && change < 1) {
//					maxHealth = (int)(maxHealth * (1 + change));
//					health = (int)(health * (1 + change));
//					if (fromTriggeredSkill) {
//						maxHealthChangeScalerFromTriggeredSkill += change;
//					}
//				} else {
//					maxHealth += (int)change;
//					health += (int)(health * change / maxHealth);
//					if (fromTriggeredSkill) {
//						maxHealthChangeFromTriggeredSkill += (int)change;
//					}
//				}
//				break;
//			case PropertyType.Hit:
//				if (change > -1 && change < 1) {
//					hit = (int)(mana * (1 + change));
//					if (fromTriggeredSkill) {
//						hitChangeScalerFromTriggeredSkill += (int)change;
//					}
//				} else {
//					hit += (int)change;
//					if (fromTriggeredSkill) {
//						hitChangeFromTriggeredSkill += (int)change;
//					}
//				}
//				break;
//			case PropertyType.Mana:
//				if (change > -1 && change < 1) {
//					mana = (int)(mana * (1 + change));
//					if (fromTriggeredSkill) {
//						manaChangeScalerFromTriggeredSkill += change;
//					}
//				} else {
//					mana += (int)change;
//					if (fromTriggeredSkill) {
//						manaChangeFromTriggeredSkill += (int)change;
//					}
//				}
//
//				break;
//			case PropertyType.Attack:
//				if (change > -1 && change < 1) {
//					attack = (int)(attack * (1 + change));
//					if (fromTriggeredSkill) {
//						attackChangeScalerFromTriggeredSkill += change;
//					}
//				} else {
//					attack += (int)change;
//					if (fromTriggeredSkill) {
//						attackChangeFromTriggeredSkill += (int)change;
//					}
//				}
//
//				break;
//			case PropertyType.AttackSpeed:
//				if (change > -1 && change < 1) {
//					attackSpeed = (int)(attackSpeed * (1 + change));
//					if (fromTriggeredSkill) {
//						attackSpeedChangeScalerFromTriggeredSkill += change;
//					}
//				} else {
//					attackSpeed += (int)change;
//					if (fromTriggeredSkill) {
//						attackSpeedChangeFromTriggeredSkill += (int)change;
//					}
//				}
//				break;
//			case PropertyType.Armor:
//				if (change > -1 && change < 1) {
//					armor = (int)(armor * (1 + change));
//					if (fromTriggeredSkill) {
//						armorChangeScalerFromTriggeredSkill += change;
//					}
//				} else {
//					armor += (int)change;
//					if (fromTriggeredSkill) {
//						armorChangeFromTriggeredSkill += (int)change;
//					}
//				}
//
//				break;
//			case PropertyType.MagicResist:
//				if (change > -1 && change < 1) {
//					magicResist = (int)(magicResist * (1 + change));
//					if (fromTriggeredSkill) {
//						magicResistChangeScalerFromTriggeredSkill += change;
//					}
//				} else {
//					magicResist += (int)change;
//					if (fromTriggeredSkill) {
//						magicResistChangeFromTriggeredSkill += (int)change;
//					}
//				}
//
//				break;
//			case PropertyType.Dodge:
//				if (change > -1 && change < 1) {
//					dodge = (int)(dodge * (1 + change));
//					if (fromTriggeredSkill) {
//						dodgeChangeScalerFromTriggeredSkill += change;
//					}
//				} else {
//					dodge += (int)change;
//					if (fromTriggeredSkill) {
//						dodgeChangeFromTriggeredSkill += (int)change;
//					}
//				}
//
//				break;
//			case PropertyType.Crit:
//				if (change > -1 && change < 1) {
//					crit = (int)(crit * (1 + change));
//					if (fromTriggeredSkill) {
//						critChangeScalerFromTriggeredSkill += change;
//					}
//				} else {
//					crit += (int)change;
//					if (fromTriggeredSkill) {
//						critChangeFromTriggeredSkill += (int)change;
//					}
//				}
//
//				break;
//			case PropertyType.PhysicalHurtScaler:
//				physicalHurtScaler += change;
//				if (fromTriggeredSkill) {
//					physicalHurtScalerChangeFromTriggeredSkill += change;
//				}
//				break;
//			case PropertyType.MagicalHurtScaler:
//				magicalHurtScaler += change;
//				if (fromTriggeredSkill) {
//					magicalHurtScalerChangeFromTriggeredSkill += change;
//				}
//				break;
//			case PropertyType.CritHurtScaler:
//				critHurtScaler += change;
//				if (fromTriggeredSkill) {
//					critHurtScalerChangeFromTriggeredSkill += change;
//				}
//				break;
//			case PropertyType.WholeProperty:
//				if (change > -1 && change < 1) {
//					maxHealth = (int)(health * (1 + change));
//					mana = (int)(mana * (1 + change));
//					attack = (int)(attack * (1 + change));
//					attackSpeed = (int)(attackSpeed * (1 + change));
//					armor = (int)(armor * (1 + change));
//					magicResist = (int)(magicResist * (1 + change));
//					dodge = (int)(dodge * (1 + change));
//					crit = (int)(crit * (1 + change));
//					hit = (int)(hit * (1 + change));
//					if (fromTriggeredSkill) {
//						maxHealthChangeScalerFromTriggeredSkill += change;
//						manaChangeScalerFromTriggeredSkill += change;
//						attackChangeScalerFromTriggeredSkill += change;
//						attackSpeedChangeScalerFromTriggeredSkill += change;
//						armorChangeScalerFromTriggeredSkill += change;
//						magicResistChangeScalerFromTriggeredSkill += change;
//						dodgeChangeScalerFromTriggeredSkill += change;
//						critChangeScalerFromTriggeredSkill += change;
//						hitChangeScalerFromTriggeredSkill += change;
//					}
//				} else {
//					health += (int)change;
//					mana += (int)change;
//					attack += (int)change;
//					attackSpeed += (int)change;
//					armor += (int)change;
//					magicResist += (int)change;
//					dodge += (int)change;
//					crit += (int)change;
//					hit += (int)change;
//					if (fromTriggeredSkill) {
//						maxHealthChangeFromTriggeredSkill += (int)change;
//						manaChangeFromTriggeredSkill += (int)change;
//						attackChangeFromTriggeredSkill += (int)change;
//						attackSpeedChangeFromTriggeredSkill += (int)change;
//						armorChangeFromTriggeredSkill += (int)change;
//						magicResistChangeFromTriggeredSkill += (int)change;
//						dodgeChangeFromTriggeredSkill += (int)change;
//						critChangeFromTriggeredSkill += (int)change;
//						hitChangeFromTriggeredSkill += (int)change;
//					}
//				}
//
//				break;
//			}

		}





		// 判断概率性技能是否生效
		protected bool isEffective(float chance){
			float randomNum = Random.Range (0, 100)/100f;
			return randomNum <= chance;
		}

	}
}
