namespace BiiGBackend.SharedModels.Enums
{
	public enum ErrorCodes
	{
		IncorrectPassword,
		UserAuthDoesNotExist,
		UserDoesNotExist,
		CourseDoesNotExist,
		UserExist,
		ErrorWhileSaving,
		ErrorWhileAdding,
		CartItemExist


	}

	public enum ErrorIdentifiers
	{
		ValidationErrors,
		CustomError,
		UnclassifiedError,
	}
}

