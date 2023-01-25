﻿using NTec.Domain.Enums;
using System;

namespace NTec.Aplicacao.VizualizacoesDeModelo.Colaboradores
{
    public class ColaboradorDetalheViewModel
    {
        public DateTime Aniversario { get; set; }
        public long Cpf { get; set; }
        public GeneroEnum Genero { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public int CargoId { get; set; }
        public Guid? ChefeId { get; set; }
        public int SetorId { get; set; }
    }
}
