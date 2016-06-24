using System;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMProjectStep
    {
        public SxVMProjectStep()
        {
            Steps = new SxVMProjectStep[0];
        }
        public int Id { get; set; }
        public int? ParentStepId { get; set; }
        public SxVMProjectStep[] Steps { get; set; }
        public string Title { get; set; }
        public string Foreword { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
        public int Order { get; set; }
        public bool IsDone { get; set; }
    }
}
