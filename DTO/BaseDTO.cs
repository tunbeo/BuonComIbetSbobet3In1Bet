using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using iBet.Utilities;
namespace iBet.DTO
{
	[System.Serializable]
	public class BaseDTO
	{
		public string ID
		{
			get;
			set;
		}
		public static T DeepClone<T>(T obj)
		{
			T result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
                try
                {
                    binaryFormatter.Serialize(memoryStream, obj);
                    memoryStream.Position = 0;
                    result = (T)binaryFormatter.Deserialize(memoryStream);
                }
                catch (Exception ex)
                {
                    Utilities.WriteLog.Write("*** baseDTO: " + ex.Message);
                    result = default(T);
 
                }
				
			}
			return result;
		}
        public object Clone()
        {
            return this.MemberwiseClone();
        }
	}
}
