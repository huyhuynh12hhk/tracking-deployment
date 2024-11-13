namespace ProductTrackingAPI.DTOs
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
        

        public static BaseResponse Fail(string ErrorMessage)
        {
            return new BaseResponse
            {
                
                ErrorMessage = ErrorMessage,
            };
        }
    }

    public class BaseResponse<T> : BaseResponse
    {
        public T Data { get; set; }
        public static BaseResponse<T> Success(T data)
        {
            return new BaseResponse<T>
            {
                Data = data,
                IsSuccess = true,
            };
        }

        


    }
}
