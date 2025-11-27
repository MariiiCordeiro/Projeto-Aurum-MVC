using AurumLab.Data;
using AurumLab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

            var usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.IdUsuario == UsuarioId);

            //* Tipos de dispositivos JOIN + AGRUPAMENTO
            //* Consultar a tabela dispositivos atraves da ViewModel
            //* SELECT * FROM Dispositivos

            var DispositivosPorTipo = _context.Dispositivos
            //* join tabela dispositivo e dispostivo por tipo
            .Join(
                _context.TipoDispositivos, //* JOIN TiposDipositivos
                dispositivo => dispositivo.IdTipoDispositivo, //* ON Dipositivo.IdTipoDispositivo
                tipoDispositivo => tipoDispositivo.IdTipoDispositivo, //* =tipoDispositivo.IdDipositivo
                (dispositivo, tipoDispositivo) => new { dispositivo, tipoDispositivo.Nome }
            )

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

        .GroupBy(itemAgrupado => itemAgrupado.Nome) //* Agrupa dispositivos por nome do tipo
        .Select(grupo => new ItemAgrupado
        {
            Nome = grupo.Key,            //* retorna o tipo de dispositivo
            Quantidade = grupo.Count()  //* Retorna quantidade de dispositivos daquele tipo
        })
        .ToList();


            //* Lista Locais
            var locais = _context.LocalDispositivos
                .OrderBy(local => local.Nome)//* ordena locais por nome
                .ToList();    //* buscar locais cadatsrados, ordenar por nome e converter para lista.

        //* VIEW MODEL
        //* Cria uma viewmodel com todas as informacoes que a pagina precisa
        DashboardViewModel viewModel = new DashboardViewModel
        { //*usuario?.NomeUsuario -> Se o usuario noa for null, entao pegue NomeUsuario (nome que esta no banco)
          //* ?? "Usuario" -> senao, se for for null retorne "Usuarios" como nome por padrao
            NomeUsuario = usuario?.NomeUsuario ?? "Usuário",
            FotoUsuario = "/assests/img/img-perfil.png",

            TotalDispositivos = _context.Dispositivos.Count(),
            TotalAtivos = _context.Dispositivos.Count(dispositivos => dispositivos.SituacaoOperacional == "Operando"),
            TotalEmManutencao = _context.Dispositivos.Count(dispositivos => dispositivos.SituacaoOperacional == "Manutenção"),
            TotalInoperantes = _context.Dispositivos.Count(dispositivos => dispositivos.SituacaoOperacional == "Inoperante"),

            DispositivosPorTipo = DispositivosPorTipo,
            Locais = locais
        };

        return View(viewModel);
        }
    }
}