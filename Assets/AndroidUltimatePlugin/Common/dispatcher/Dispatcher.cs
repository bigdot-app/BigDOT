using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using AUP;

namespace AUP{
	public class Dispatcher :MonoBehaviour,IDispatcher {

		private static Dispatcher instance;
		private static GameObject container;
		private const string TAG="[Dispatcher]: ";

		private static AUPHolder aupHolder;

		public List<Action> pending = new List<Action>();


		public static Dispatcher GetInstance(){
			if(instance==null){
				aupHolder = AUPHolder.GetInstance();

				container = new GameObject();
				container.name="Dispatcher";
				instance = container.AddComponent( typeof(Dispatcher) ) as Dispatcher;
				DontDestroyOnLoad(instance.gameObject);
				instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
			}

			return instance;
		}

		//
		// Schedule code for execution in the main-thread.
		//
		public void InvokeAction(Action fn){		
			// design for intra-thread communication it must be thread safe
			// that's why we use lock
			lock (pending)
			{
				pending.Add(fn);
			}
		}

		//
		// Execute pending actions.
		//
		public void InvokePendingAction(){
			lock (pending)
			{
				foreach (var action in pending)
				{
					action(); // Invoke the action.
				}

				pending.Clear(); // Clear the pending list.
			}
		}


		void Update(){
			// invoke pending actions
			InvokePendingAction();
		}
	}
}
