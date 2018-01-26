using System.Linq;
using System.ServiceModel;

namespace SandboxConsoleApp.ProtectionLevel
{
    //This is the minimum protection level that the binding must comply with.
    [ServiceContract(ProtectionLevel = System.Net.Security.ProtectionLevel.None)]
    public interface IConvertString
    {
        [OperationContract]
        string ConvertString(string input);
    }

    // Simple service which reverses a string and returns the result
    public class ConvertStringService : IConvertString
    {
        #region IConvertString Members

        public string ConvertString(string input)
        {
            char[] chars = input.ToCharArray();
            chars.Reverse<char>();
            return chars.ToString();
        }

        #endregion
    }
}
