using System;

namespace AIToolkitDemo
{
	public class TAIToolkit
	{
		static public void Init()
		{
			TLogger.instance.Init();
		}
		static public void Uninit()
		{
			TLogger.instance.Uninit();
		}
	}
}

