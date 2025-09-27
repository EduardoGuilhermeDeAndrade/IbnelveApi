namespace IbnelveApi.Application.Dto
{
    /// <summary>
    /// DTO m�nimo para cria��o de Pessoa.
    /// </summary>
    public class CreatePessoaDto
    {
        public string Nome { get; set; }
    }

    /// <summary>
    /// DTO m�nimo para atualiza��o de Pessoa.
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