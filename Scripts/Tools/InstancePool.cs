﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InstancePool: MonoBehaviour{



	private List<GameObject> mInstancePool = new List<GameObject>();


	public T GetInstance<T>(GameObject instanceModel,Transform instanceParent)
		where T:Component
	{
		GameObject mInstance = null;

		if (mInstancePool.Count != 0) {
			mInstance = mInstancePool [0];
			mInstancePool.RemoveAt (0);
			mInstance.transform.SetParent (instanceParent);
		} else {
			mInstance = Instantiate (instanceModel,instanceParent);	
		}

		return mInstance.GetComponent<T>();
	}

	public void AddInstanceToPool(GameObject instance,string poolName){
		instance.transform.SetParent(ContainerManager.FindContainer (CommonData.poolContainerName + "/" + poolName));
		mInstancePool.Add (instance);
	}
}
