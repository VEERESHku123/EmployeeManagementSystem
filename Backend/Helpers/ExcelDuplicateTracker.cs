namespace Backend.Helpers
{
    public class ExcelDuplicateTracker
    {
        public HashSet<string> EmployeeIds { get; } = new(StringComparer.OrdinalIgnoreCase);

        public HashSet<string> CompanyEmails { get; } = new(StringComparer.OrdinalIgnoreCase);

        public HashSet<string> PhoneNumbers { get; } = new(StringComparer.OrdinalIgnoreCase);

        public HashSet<string> PersonalEmails { get; } = new(StringComparer.OrdinalIgnoreCase);
    }
}
