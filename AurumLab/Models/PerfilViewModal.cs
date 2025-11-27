using System.Drawing;

namespace AurumLab.Models
{
    public class PerfilViewModal
    {
        public int IdUsuario {get; set;}
        public string NomeCompleto {get; set;}
        public string? NomeUsuario {get; set;}//* deixa peggar valores nulos

        public string Email {get;set;}
        public int RegraId{get; set;}
        public List<RegraPerfil> Regras {get; set;}
        public string? NovaSenha {get; set;}
        public string? ConfirmarSenha {get; set;}

        public String? FotoBase64 {get; set;} //* Como converter o formato da foto

    }
}