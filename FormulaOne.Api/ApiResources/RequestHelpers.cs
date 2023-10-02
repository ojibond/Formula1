using System.Runtime.CompilerServices;

namespace FormulaOne.Api.ApiResources;

public static class RequestHelpers
{
    public static HttpResponseMessage WithReasonPhrase(this HttpResponseMessage responseMessage, string reasonPhrase)
    {
        responseMessage.ReasonPhrase = reasonPhrase;
        return responseMessage;
    }
}
