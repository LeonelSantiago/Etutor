using Etutor.Core.Models.Enums;

namespace Etutor.Core.Models
{
    public class OperationResult
    {
        public string Message { get; set; }
        public CriticalLevel CriticalLevel { get; set; }
        public OperationResult(string message, CriticalLevel criticalLevel = CriticalLevel.Warning)
        {
            Message = message;
            CriticalLevel = criticalLevel;
        }
    }
}
