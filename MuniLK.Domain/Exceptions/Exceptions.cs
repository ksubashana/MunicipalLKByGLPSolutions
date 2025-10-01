using System;

namespace Domain.Exceptions
{
    public class LicenseNotFoundException : Exception
    {
        public LicenseNotFoundException(int licenseId)
            : base($"License with ID {licenseId} not found.")
        {
        }
    }

    public class LicenseLimitExceededException : Exception
    {
        public LicenseLimitExceededException(string message)
            : base(message)
        {
        }
    }

    public class InvalidLicensePropertyException : Exception
    {
        public InvalidLicensePropertyException(string propertyName, string message)
            : base($"Invalid license property '{propertyName}': {message}")
        {
        }
    }

    public class LicenseOperationException : Exception
    {
        public LicenseOperationException(string message)
            : base(message)
        {
        }
    }

    public class PropertyOwnerAlreadyExistsException : Exception
    {
        public PropertyOwnerAlreadyExistsException()
            : base("This contact is already added as an owner for the selected property.")
        {
        }
    }
}