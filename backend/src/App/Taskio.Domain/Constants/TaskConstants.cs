namespace Taskio.Domain.Constants;

public static class TaskConstants
{
    public static class OrderedBy
    {
        public const string Title = "title";
        public const string Priority = "priority";
        public const string Status = "status";
        public const string DueDate = "dueDate";
        public const string CreatedDate = "createdDate";
        public const string UpdatedDate = "updatedDate";
    }

    public static class FilteredBy
    {
        public const string Priority = "priority";
        public const string Status = "status";
        public const string DueDate = "dueDate";
        public const string CreatedDate = "createdDate";
        public const string UpdatedDate = "updatedDate";
    }

    public static class RangedBy
    {
        public const string DueDate = "dueDate";
        public const string CreatedDate = "createdDate";
        public const string UpdatedDate = "updatedDate";
    }
}
