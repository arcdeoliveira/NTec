using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NTec.Domain.Dtos.Colaboradores
{
    public class ColaboradorNodeDto
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image {get; set;}

        public IEnumerable<ColaboradorNodeDto> Children { get; set; }
    }
}
