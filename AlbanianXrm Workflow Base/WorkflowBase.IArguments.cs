﻿using System.Activities;

namespace AlbanianXrm
{
	public partial class WorkflowBase
	{
		protected interface IArguments
		{
			T GetValue<T>(InArgument<T> @in);
			T GetValue<T>(InOutArgument<T> @in);
			void SetValue<T>(InOutArgument<T> @out, T value);
			void SetValue<T>(OutArgument<T> @out, T value);
		}
	}
}
