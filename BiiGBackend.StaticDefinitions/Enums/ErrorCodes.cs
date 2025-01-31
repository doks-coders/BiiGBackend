namespace BiiGBackend.SharedModels.Enums
{
    public enum ErrorCodes
    {
        NotAuthenticatedLogin,
        IncorrectPassword,
        UserAuthDoesNotExist,
        CartItemNotRetrievable,
        UserDoesNotExist,
        CourseDoesNotExist,
        UserExist,
        ErrorWhileSaving,
        ErrorWhileAdding,
        CartItemExist,
        ProductDeleted


    }

    public enum ErrorIdentifiers
    {
        ValidationErrors,
        CustomError,
        UnclassifiedError,
    }
}

