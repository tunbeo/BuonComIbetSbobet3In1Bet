using System;
namespace iBet.Engine
{
    public class EngineEventArgs : System.EventArgs
	{
		public eEngineEventType Type
		{
			get;
			set;
		}
		public object Data
		{
			get;
			set;
		}
	}
}
