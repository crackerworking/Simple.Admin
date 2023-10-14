namespace Mi.Domain.Shared.Response
{
    public class ResponseStructure
    {
        public response_type Code { get; set; }
        public string? Message { get; set; }

        public ResponseStructure()
        { }

        public ResponseStructure(response_type code, string? message)
        {
            Code = code;
            Message = message;
        }

        public virtual bool IsOk()
        {
            return Code == response_type.Success;
        }
    }

    public class ResponseStructure<T> : ResponseStructure
    {
        public override bool IsOk()
        {
            return Code == response_type.Success && Result != null;
        }

        public T? Result { get; set; }

        public ResponseStructure()
        { }

        public ResponseStructure(response_type code, string msg, T? result)
        {
            Code = code;
            Message = msg;
            Result = result;
        }

        public ResponseStructure(bool successed, string msg, T? result)
        {
            Code = successed ? response_type.Success : response_type.Fail;
            Message = msg;
            Result = result;
        }

        public ResponseStructure(bool successed, T? result)
        {
            Code = successed ? response_type.Success : response_type.Fail;
            Message = "search " + (successed ? "success" : "fail");
            Result = result;
        }

        public ResponseStructure(T? result)
        {
            Code = response_type.Success;
            Message = "success";
            Result = result;
        }
    }
}