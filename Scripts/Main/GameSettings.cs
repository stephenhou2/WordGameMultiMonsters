﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordJourney{

	using System.IO;

	[System.Serializable]
	public class GameSettings {

		public enum LearnMode
		{
			Test,
			Learn
		}

		public bool isAutoPronounce = false;

		public float systemVolume = 0.5f;

		public WordType wordType = WordType.CET4;

		public LearnMode learnMode;

		public string GetWordTypeString(){

			string wordTypeString = "";

			switch (wordType) {
			case WordType.CET4:
				wordTypeString = "大学英语四级";
				break;
			case WordType.CET6:
				wordTypeString = "大学英语六级";
				break;
			}

			return wordTypeString;


		}


		public override string ToString ()
		{
			return string.Format ("[GameSettings]-isAutoPronounce{0},systemVolume{3},wordType{4}",isAutoPronounce,systemVolume,wordType);
		}


	}
}
