using AurumLab.Data;
using AurumLab.Models;
using Microsoft.AspNetCore.Mvc;

namespace AurumLab.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            //* Nao deixara ir para o dashboard o login e obrigatorio
            if (HttpContext.Session.GetInt32("UsuarioId") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var UsuarioId = HttpContext.Session.GetInt32("UsuarioId");

            var usuario = _context.Usuarios.FirstOrDefault(usuario=> usuario.IdUsuario == UsuarioId);

            //* Tipos de dispositivos JOIN + AGRUPAMENTO
            //* Consultar a tabela dispositivos atraves da ViewModel
            //* SELECT * FROM Dispositivos

            var DispositivosPorTipo = _context.Dispositivos
            //* join tabela dispositivo e dispostivo por tipo
            .Join(
                _context.TipoDispositivos, //* JOIN TiposDipositivos
                dispositivo => dispositivo.IdTipoDispositivo, //* ON Dipositivo.IdTipoDispositivo
                tipoDispositivo => tipoDispositivo.IdTipoDispositivo, //* =tipoDispositivo.IdDipositivo
                (dispositivo, tipoDispositivo) => new{dispositivo, tipoDispositivo.Nome}
            );

            //* Para cada par encontrado - um dispositivo e seu tipoDispositivo correspondente - monta um objeto
            //* dispositivo -> objeto completo
            //* nome -> o nome do tipo de dispositivo
            //* {
            //* dispositivo = {objeto Dispositivo inteiro}
            //* Nome = "Sensor" (exemplo)
            //*}

            //* SELECT * FROM Dispositivo dispositivo
            //* JOIN TipoDispositivo tipoDispositivo
            //* ON Dispositivo d.IdTipoDispositivo = TempData.IdTipoDispositivo

        }
    }
}