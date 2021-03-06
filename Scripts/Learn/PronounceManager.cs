﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WordJourney
{
	public class PronounceManager:MonoBehaviour {

		private class Pronunciation
		{
			public LearnWord word;
			public AudioClip pronunciation;

			public Pronunciation(LearnWord word,AudioClip pronunciation){
				this.word = word;
				this.pronunciation = pronunciation;
			}
		}

		private WWW pronunciationWWW;

		// 下载发音的超时时长
		public float wwwTimeOutInterval = 2f;

		// 当前正在学习的单词的发音
		private AudioClip pronunciationOfCurrentWord;

		// 读音缓存
		private List<Pronunciation> pronunciationCache;

		private IEnumerator waitDownloadFinishCoroutine;

		private LearnWord wordToPronounce;


		private string pronunciationBaseURL;

		void Awake(){
			pronunciationCache = new List<Pronunciation> ();
			pronunciationBaseURL = "https://wordsound.b0.upaiyun.com/voice";
		}

		/// <summary>
		/// 从缓存中读取单词发音
		/// </summary>
		/// <returns>The pronunciation from cache.</returns>
		/// <param name="word">Word.</param>
		private Pronunciation GetPronunciationFromCache(LearnWord word){
			Pronunciation pro = pronunciationCache.Find (delegate(Pronunciation obj) {
				return obj.word.wordId == word.wordId;
			});
			return pro;
		}


		/// <summary>
		/// 下载单词发音
		/// </summary>
		/// <returns>The pronunciation with WW.</returns>
		/// <param name="word">Word.</param>
		private WWW GetPronunciationWithWWW(LearnWord word){

			string firstLetter = word.spell.Substring (0, 1);

			string url = string.Format ("{0}/{1}/{2}.wav", pronunciationBaseURL, firstLetter, word.spell);

			return new WWW (url);

		}

		/// <summary>
		/// 如果缓存中有单词发音，则直接发音，如果没有，则下载完成后发音
		/// </summary>
		/// <param name="word">Word.</param>
		public void PronounceWord(LearnWord word){

			wordToPronounce = word;

			Pronunciation pro = GetPronunciationFromCache (word);

			if (pro == null) {
				pronunciationWWW = GetPronunciationWithWWW (word);
				waitDownloadFinishCoroutine = PlayPronunciationWhenFinishDownloading (pronunciationWWW);
				StartCoroutine (waitDownloadFinishCoroutine);
			} else {
				SoundManager.Instance.PlayPronuncitaion (pro.pronunciation,false);
			}

		}

		public void CancelPronounce(){

			if (waitDownloadFinishCoroutine != null) {
				StopCoroutine (waitDownloadFinishCoroutine);
			}
			
			if (pronunciationWWW != null && !pronunciationWWW.isDone) {
				pronunciationWWW.Dispose ();
			}

		}

		public void ClearPronunciationCache(){
			pronunciationCache.Clear ();
		}

		/// <summary>
		/// 下载读音文件并在下载完成后播放单词读音的协程
		/// </summary>
		/// <returns>The pronunciation when finish downloading.</returns>
		/// <param name="www">Www.</param>
		private IEnumerator PlayPronunciationWhenFinishDownloading(WWW www){

			float timer = 0;

			while (!www.isDone && timer < wwwTimeOutInterval) {
				timer += Time.deltaTime;
				yield return null;
			}

			if (www.isDone) {

				AudioClip pronunciationClip = www.GetAudioClip (false);

				Pronunciation pro = new Pronunciation (wordToPronounce, pronunciationClip);

				pronunciationCache.Add (pro);

				SoundManager.Instance.PlayPronuncitaion (pronunciationClip, false);
			} else {
				// 下载超时时不播放读音,并关闭下载任务
				www.Dispose ();
			}

		}



	}






}
