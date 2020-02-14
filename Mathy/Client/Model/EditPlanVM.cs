using Mathy.Planning;
using System;
using System.Linq;

namespace Mathy.Client.Model
{
    public class EditPlanVM
    {
        public EditPlanVM()
        {
            EditID = Guid.NewGuid().ToString();
            Expressions = new EditSourceExpressionVM[] { };
            Variables = new EditSourceVariableVM[] { };
        }

        public EditPlanVM(string id, Plan plan)
        {
            ID = id;
            EditID = Guid.NewGuid().ToString();
            Title = plan.Title;
            Description = plan.Description;
            Author = plan.Author;
            PlanType = plan.PlanType;
            PlanCategory = plan.PlanCategory;
            Expressions = plan.Expressions.Select((i, index) =>
                new EditSourceExpressionVM()
                {
                    ID = index,
                    Title = i.Title,
                    Description = i.Description,
                    Expression = string.Join("\r\n", i.Expression.Split(new string[] { "\r\n", "\t" }, StringSplitOptions.RemoveEmptyEntries).Select(j => j.Trim())),
                    Condition = string.IsNullOrEmpty(i.Condition) ? string.Empty : string.Join("\r\n", i.Condition.Split(new string[] { "\r\n", "\t" }, StringSplitOptions.RemoveEmptyEntries).Select(j => j.Trim()))
                }).ToArray();
            Variables = plan.Variables.Select((i, index) =>
                new EditSourceVariableVM()
                {
                    ID = index,
                    Name = i.Name,
                    Type = EditDataTypeVM.FromName(i.Type.ToString()),
                    Description = i.Description,
                    Style = plan.Styles.FirstOrDefault(j => j.Target == i.Name) == null ? new EditStyleVM() : new EditStyleVM(plan.Styles.FirstOrDefault(j => j.Target == i.Name))
                }).ToArray();
        }


        public string ID { get; private set; }

        public string EditID { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public string Author { get; private set; }

        public int PlanType { get; set; }
        public string PlanCategory { get; set; }

        public EditSourceExpressionVM[] Expressions { get; set; }

        public EditSourceVariableVM[] Variables { get; set; }
    }

    public class EditSourceExpressionVM
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Expression { get; set; }

        public string Condition { get; set; }
    }

    public class EditDataTypeVM
    {
        public static EditDataTypeVM FromName(string name)
        {
            return new EditDataTypeVM()
            {
                Name = name,
                //DisplayName = Funcs.GetDataTypeDisplayName(name)
            };
        }

        public string Name { get; private set; }

        public string DisplayName { get; private set; }
    }

    public class EditSourceVariableVM
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public EditDataTypeVM Type { get; set; }

        public string Description { get; set; }

        public EditStyleVM Style { get; set; }
    }

    public class EditStyleVM
    {
        public string Target { get; set; }

        public string Size { get; set; }

        public string ColumnNames { get; set; }

        public string RowHeaderWidth { get; set; }

        public string RowName { get; set; }


        public EditStyleVM()
        {
        }

        public EditStyleVM(Style style)
        {
            Target = style.Target;
            Size = style.Size == 0 ? null : style.Size.ToString();
            ColumnNames = style.ColumnNames == null ? string.Empty : string.Join(",", style.ColumnNames);
            RowHeaderWidth = style.RowHeaderWidth == 0 ? null : style.RowHeaderWidth.ToString();
            RowName = style.RowName;
        }
    }
}