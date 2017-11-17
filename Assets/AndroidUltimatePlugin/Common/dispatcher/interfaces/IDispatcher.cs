using UnityEngine;
using System.Collections;
using System;

namespace AUP{
	public interface IDispatcher{
		void InvokeAction(Action fn);
		void InvokePendingAction();
	}
}
