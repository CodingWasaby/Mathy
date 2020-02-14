using Mathy.Planning;
using Mathy.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Client.Model
{
    public class JobVM
    {
        public JobVM(EvaluationContext context, Job job)
        {
            AutoID = job.AutoID;
            EditID = Guid.NewGuid().ToString();
            Settings = new SettingsVM(context.Settings);
            PlanTitle = context.Plan.Title;
            Name = job.Name;
            Description = context.Plan.Description;
            Author = context.Plan.Author;
            Steps = context.Steps.Select((i, index) => new JobStepListCellVM(i, index)).ToArray();
            IsCompleted = context.Steps.All(i => i.State != StepState.Unready);

            ExportButtonClass = IsCompleted ? "button" : "button-disabled";
            ExportButtonCode = IsCompleted ? "javascript:exportAsWordDocument()" : null;
            DrawSourceVariables = context.SourceVariables.Where(m => m.Key.Contains("Draw_")).Select(m => m.Key).ToList();
        }

        public List<string> DrawSourceVariables { get; set; }

        public bool IsDraw { get { return DrawSourceVariables.Count > 0; } }

        public int AutoID { get; set; }

        public string EditID { get; set; }

        public SettingsVM Settings { get; set; }

        public string PlanTitle { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public string Author { get; private set; }

        public JobStepListCellVM[] Steps { get; private set; }

        public string ExportButtonClass { get; private set; }

        public string ExportButtonCode { get; private set; }

        public bool IsCompleted { get; private set; }
    }
}