namespace IbnelveApi.Application.Dto
{
    /// <summary>
    /// DTO mínimo para criação de Pessoa.
    /// </summary>
    public class CreatePessoaDto
    {
        public string Nome { get; set; }
    }

    /// <summary>
    /// DTO mínimo para atualização de Pessoa.
    /// </summary>
    public class UpdatePessoaDto
    {
        public string Nome { get; set; }
    }

    /// <summary>
    /// Resposta padronizada da API.
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public ApiResponse(bool success, T data, string message = null)
        {
            Success = success;
            Data = data;
            Message = message;
        }
    }
}