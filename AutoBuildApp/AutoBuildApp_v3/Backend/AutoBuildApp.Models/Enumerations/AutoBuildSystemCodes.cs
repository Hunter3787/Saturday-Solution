using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.Enumerations
{
    public enum AutoBuildSystemCodes
    {
        Success,
        DatabaseTimeout,
        DuplicateValue,
        DefaultError,
        ArguementNull,
        UndeclaredVariable,
        FailedParse,
        ProductCreationFailed,
        Unauthorized,
        ConnectionError,
        InsertFailed,
        DeleteFailed,
        UpdateFailed,
        NoEntryFound,
        NoChangeOccurred,
        NullValue

    }
}
