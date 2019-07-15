using System.Reflection;

namespace Etutor.Core.Extensions
{
    public static class MemberInfoExtensions
    {
        public static string GetCleanNameFromDto(this MemberInfo memberInfo)
        {
            return memberInfo.Name.Replace("Dto", "");
        }
    }
}
