using System.Collections.Generic;
using Ocelot.Errors;

namespace Iris.Ocelot.Middlewares.Responder
{
    /// <summary>
    /// Map a list OceoltErrors to a single appropriate HTTP status code
    /// </summary>
    public interface IErrorsToHttpStatusCodeMapper
    {
        int Map(List<Error> errors);
    }
}
