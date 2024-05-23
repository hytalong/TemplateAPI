namespace Template.Application.Querys
{
    public class ApiMetaDataResponse
    {
        public int code { get; set; }

        public string pagination { get; set; }

        public ApiMetaDataResponse(int codeResponse, string paginationResponse)
        {
            code = codeResponse;
            pagination = paginationResponse;
        }
    }
}