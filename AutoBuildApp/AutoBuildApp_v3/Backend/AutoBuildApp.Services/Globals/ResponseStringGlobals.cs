using System;
namespace AutoBuildApp.Services
{
    public static class ResponseStringGlobals
    {
        public const string SUCCESSFUL_CREATION = "Successfully created.";
        public const string SUCCESSFUL_DELETION = "Successfully deleted.";
        public const string SUCCESSFUL_MODIFICATION = "Successfully modified.";
        public const string SUCCESSFUL_ADDITION = "Successfully added.";
        public const string SUCCESSFUL_REMOVAL = "Successfully removed.";
        public const string FAILED_CREATION = "Failed to create.";
        public const string FAILED_DELETION = "Failed to delete.";
        public const string FAILED_MODIFICATION = "Failed modification.";
        public const string FAILED_ADDITION = "Failed to be added.";
        public const string FAILED_REMOVAL = "Failed to remove.";
        public const string SYSTEM_FAILURE = "Internal error.";
        public const string DATABASE_FAILURE = "Database error.";
        public const string REQUEST_FAILURE = "Request failed.";
        public const string CALL_TIMEOUT = "Timeout error.";
    }
}
