using Microsoft.AspNetCore.Components;
using FrontCodOperacional.Services.UI;
using System.Net;

namespace FrontCodOperacional.Services.Http
{
    public class HttpErrorHandler : DelegatingHandler
    {
        private readonly NavigationManager _nav;
        private readonly ToastService _toast;


        public HttpErrorHandler(NavigationManager nav, ToastService toast)
        {
            _nav = nav;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage response;

            try
            {
                response = await base.SendAsync(request, cancellationToken);
            }
            catch
            {
                _toast.Error("Error de conexión con el servidor");
                throw;
            }

            if (!response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        _toast.Error("Sesión expirada");
                        _nav.NavigateTo("/login", true);
                        break;

                    case HttpStatusCode.Forbidden:
                        _toast.Error("No tienes permisos para esta acción");
                        break;

                    case HttpStatusCode.NotFound:
                        _toast.Error("Recurso no encontrado");
                        break;

                    case HttpStatusCode.InternalServerError:
                        _toast.Error("Error interno del servidor");
                        break;

                    default:
                        _toast.Error($"Error HTTP: {(int)response.StatusCode}");
                        break;
                }
            }

            return response;
        }
    }
}
