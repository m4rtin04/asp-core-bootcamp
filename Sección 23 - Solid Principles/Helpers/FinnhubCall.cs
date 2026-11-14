using Sección_23___Solid_Principles.Exceptions;

namespace Sección_23___Solid_Principles.Helpers
{
    internal static class FinnhubCall
    {
        // Ejecuta una llamada al repositorio y envuelve cualquier falla en FinnhubException
        internal static async Task<T> Execute<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                throw new FinnhubException("Unable to connect to finnhub server", ex);
            }
        }
    }
}
