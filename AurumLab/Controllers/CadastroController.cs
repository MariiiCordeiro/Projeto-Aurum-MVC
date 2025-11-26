using AurumLab.Data;
using AurumLab.Models;
using AurumLab.Services;
using Microsoft.AspNetCore.Mvc;

namespace AurumLab.Controllers
{
    public class CadastroController : Controller
    {
        private readonly AppDbContext _context;

        public CadastroController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Criar(string nome, string email, string senha, string confirmar)
        {
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha) || string.IsNullOrWhiteSpace(confirmar))
            {
                ViewBag.Erro = "Preencha todos os campos!";
                return View("Index");
            }

            if (senha != confirmar)
            {
                ViewBag.Erro = "As senhas nao conferem";
                return View("Index");
            }

            //* Verifica se o email ja esta cadastrado
            //* Parecido com o FirstOrDefault()
            //* Diferença: FirstOrDefault traz o objeto por completo ex: nome, foto
            //* Any(): serve somente para validar se existe esse email
            if (_context.Usuarios.Any(usuario => usuario.Email == email))
            {
                //*auxiliar usuario percorre os usuarios pelo email ate encontrar o email cadastrado igual ao digitado no input
                ViewBag.Erro = "Email cadastrado!";
                return View("Index");
            }

            byte[] hash = HashService.GerarHashBytes(senha);

            Usuario usuario = new Usuario{
                NomeCompleto = nome,
                Email = email,
                Senha = hash,
                Foto = null,
                RegraId = 1 //* aluno por padrao
            };

            //* Salvar no banco
            //* Add refere-se ao insert no banco
            _context.Usuarios.Add(usuario);

            //* Alem de adiconar essas informacoes e preciso salva-las salvar de forma assincrona
            _context.SaveChanges();

            //* Redireciona para o login
            return RedirectToAction ("Ïndex", "Login");
        }
    }
}