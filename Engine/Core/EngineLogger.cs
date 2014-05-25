using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
namespace iBet.App.Engine.Core
{
    public enum LogMode
    {
        None,
        Normal,
        Verbose
    }
    public enum EngineAction
    {
        CHECKODDS,
        BET,
        NEXT_HDP,
        NEXT_HDP_NULL,
        UPDATE_RUNNING,
        REBET,
        BET_TOO_SMALL,
        GET_BET_LIST,
        BET_UNDER_HALF
    }
	public class EngineLogger
	{

		private const int LOG_DEPTH = 2;
		private static Dictionary<StaticLog, ILog> loggers;
		private ILog logger;
		private string name;
		static EngineLogger()
		{
			EngineLogger.loggers = new Dictionary<StaticLog, ILog>();
		}
		public EngineLogger(string name)
		{
			this.name = name;
			RollingFileAppender rollingFileAppender = new RollingFileAppender();
			rollingFileAppender.File = string.Format("logs/{0}.dat", name);
			rollingFileAppender.AppendToFile = true;
			rollingFileAppender.MaxSizeRollBackups = 20;
            rollingFileAppender.MaximumFileSize = "20000";
			ILayout layout = new PatternLayout("%date [%thread] %message%newline");
			rollingFileAppender.Layout = layout;
			rollingFileAppender.Name = name;
			rollingFileAppender.ActivateOptions();
			Hierarchy hierarchy = LogManager.GetRepository() as Hierarchy;
			Logger logger = hierarchy.GetLogger(name) as Logger;
			logger.AddAppender(rollingFileAppender);
			this.logger = LogManager.GetLogger(name);
		}
		public static void Configure()
		{
			XmlConfigurator.Configure();
			string[] names = Enum.GetNames(typeof(StaticLog));
			for (int i = 0; i < names.Length; i++)
			{
				string text = names[i];
				ILog value = LogManager.GetLogger(text.ToLower());
				EngineLogger.loggers.Add((StaticLog)Enum.Parse(typeof(StaticLog), text), value);
			}
		}
        public static LogMode LogMode
        {
            get;
            set;
        }
		public static void Log(Exception ex, LogMode mode, StaticLog type)
		{
			string message = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
			EngineLogger.Log(ex.TargetSite.Name, message, mode, type);
		}
		public static void Log(object key, string message, LogMode mode, StaticLog type)
		{
			int logMode = 5;
			if (mode <= (LogMode)logMode)
			{
				string message2 = string.Format("{0} - {1}", key, message);
				string message3 = EngineLogger.AppendTraces(message2);
				ILog log = EngineLogger.loggers[type];				
				log.Info(message3);
				
			}
		}
		private static string AppendTraces(string message)
		{
			StackTrace stackTrace = new StackTrace();
			StackFrame[] frames = stackTrace.GetFrames();
			int num = (frames.Length > 4) ? 4 : frames.Length;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 2; i < num; i++)
			{
				stringBuilder.Append(string.Format("{0}->", frames[i].GetMethod().Name));
			}
			stringBuilder.Append(string.Format(": {0}", message));
			return stringBuilder.ToString();
		}
		private static bool CheckMode(LogMode mode)
		{
			int logMode = 5;
			return mode <= (LogMode)logMode;
		}
		public void Log(string message, LogMode mode)
		{
			if (EngineLogger.CheckMode(mode))
			{
				string message2 = EngineLogger.AppendTraces(message);				
				this.logger.Info(message2);
				
			}
		}
		public void Log(Exception ex, LogMode mode)
		{
			if (EngineLogger.CheckMode(mode))
			{
				string message = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
				this.Log(message, mode);
			}
		}
		public void Log(object key, EngineAction action, Bet b, LogMode mode)
		{
			if (EngineLogger.CheckMode(mode))
			{
				string message = string.Format("{0}:{1}-->{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}_{12}_{13}", new object[]
				{
					key,
					action,
					b.Id,
					b.Home,
					b.Away,
					(int)b.Type,
					b.Choice.ToString().ToLower(),
					b.Handicap,
					b.OddsValue,
					b.MinBetAllowed,
					b.MaxBetAllowed,
					b.Stake,
					b.Step,
					b.Status
				});
				this.Log(message, mode);
			}
		}
		public void Log(object key, string message, LogMode mode)
		{
			if (EngineLogger.CheckMode(mode))
			{
				string message2 = string.Format("{0} - {1}", key, message);
				this.Log(message2, mode);
			}
		}
	}
    public enum StaticLog
    {
        Main
    }
}
