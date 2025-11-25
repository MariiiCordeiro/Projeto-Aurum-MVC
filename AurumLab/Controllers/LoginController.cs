using Microsoft.AspNetCore.Mvc;
using AurumLab.Data;
using AurumLab.Services;
using AurumLab.Models;

namespace AurumLab.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Entrar(string email, string senha)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                ViewBag.Erro = "Preencha todos os campos.";
                return View("Index");
            }

            byte[] senhaDigitadaHash = HashService.GerarHashBytes(senha);

            var usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.Email == email); //* Puxando usuario do banco, procurando as entidades no banco se nao encontrar retorna null

            if(usuario == null){
                ViewBag.Erro = "Email ou senha incorretos!";
                return View("Index");
            }

            if(!usuario.Senha.SequenceEqual(senhaDigitadaHash)){
                ViewBag.Erro = "Email ou senha incorretos!";
                return View("Index");
            }
            HttpContext.Session.SetString("UsuarioNome", usuario.NomeCompleto);
            HttpContext.Session.SetInt32("UsuarioID", usuario.IdUsuario);

            return RedirectToAction("Dashboard", "Dashboard");
        }
        public IActionResult Sair(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");  
        }
    }

}