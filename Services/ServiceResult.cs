using System;
namespace api.Services
{
    public enum ServiceResultType
    {
        FoundSuccess,
        CreatedSuccess,
        InternalError,
        ValidationError,
        NotFoundError
    }

    public readonly struct ServiceResult // returned by services so that the controllers know what response it should sent back to requester
	{
		public ServiceResultType Type { get; }
		public string Description { get; }
        public object? Data { get; } // some results will require data to be sent back while others will not

        private ServiceResult(ServiceResultType type, string description, object? data)
		{
            Type = type;
			Description = description;
            Data = data;
		}

        public static ServiceResult WithData (ServiceResultType resultType, string description, object data)
        {
            return new ServiceResult(resultType, description, data);
        }

        public static ServiceResult WithoutData (ServiceResultType resultType, string description)
        {
            return new ServiceResult(resultType, description, null);
        }
	}
}

