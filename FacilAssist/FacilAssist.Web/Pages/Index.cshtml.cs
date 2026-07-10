using System.Net.Http.Json;
using System.Text.Json;
using FacilAssist.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FacilAssist.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public ClienteFormModel Formulario { get; set; } = new();

    public List<ClienteViewModel> Clientes { get; set; } = [];

    [TempData]
    public string? MensagemSucesso { get; set; }

    [TempData]
    public string? MensagemErro { get; set; }

    public bool EstaEditando => Formulario.Id.HasValue;

    public async Task OnGetAsync()
    {
        await CarregarClientesAsync();
    }

    public async Task<IActionResult> OnPostSalvarAsync()
    {
        if (!ModelState.IsValid)
        {
            await CarregarClientesAsync();
            return Page();
        }

        var request = new ClienteRequest
        {
            Nome = Formulario.Nome,
            Cpf = ApenasDigitos(Formulario.Cpf),
            DataNascimento = Formulario.DataNascimento!.Value,
            Sexo = Formulario.Sexo![0],
            SituacaoClienteId = Formulario.SituacaoClienteId
        };

        var api = CriarClienteApi();
        var response = Formulario.Id.HasValue
            ? await api.PutAsJsonAsync($"api/clientes/{Formulario.Id.Value}", request)
            : await api.PostAsJsonAsync("api/clientes", request);

        if (!response.IsSuccessStatusCode)
        {
            MensagemErro = await LerErroAsync(response);
            await CarregarClientesAsync();
            return Page();
        }

        MensagemSucesso = Formulario.Id.HasValue
            ? "Cliente atualizado com sucesso."
            : "Cliente cadastrado com sucesso.";

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEditarAsync(int id)
    {
        await CarregarClientesAsync();

        var cliente = Clientes.FirstOrDefault(cliente => cliente.Id == id);
        if (cliente is null)
        {
            MensagemErro = "Cliente não encontrado.";
            return Page();
        }

        Formulario = new ClienteFormModel
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Cpf = cliente.Cpf,
            DataNascimento = cliente.DataNascimento,
            Sexo = cliente.Sexo.ToString(),
            SituacaoClienteId = cliente.SituacaoClienteId
        };

        return Page();
    }

    public async Task<IActionResult> OnPostExcluirAsync(int id)
    {
        var api = CriarClienteApi();
        var response = await api.DeleteAsync($"api/clientes/{id}");

        if (!response.IsSuccessStatusCode)
        {
            MensagemErro = await LerErroAsync(response);
            return RedirectToPage();
        }

        MensagemSucesso = "Cliente excluído com sucesso.";
        return RedirectToPage();
    }

    private async Task CarregarClientesAsync()
    {
        try
        {
            var api = CriarClienteApi();
            Clientes = await api.GetFromJsonAsync<List<ClienteViewModel>>("api/clientes") ?? [];
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            Clientes = [];
            MensagemErro = "Não foi possível carregar os clientes. Verifique se a API está em execução.";
        }
    }

    private HttpClient CriarClienteApi()
    {
        return _httpClientFactory.CreateClient("FacilAssistApi");
    }

    private static string ApenasDigitos(string? valor)
    {
        return new string((valor ?? string.Empty).Where(char.IsDigit).ToArray());
    }

    private static async Task<string> LerErroAsync(HttpResponseMessage response)
    {
        var conteudo = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(conteudo))
            return "A operação não pôde ser concluída.";

        try
        {
            using var json = JsonDocument.Parse(conteudo);

            if (json.RootElement.TryGetProperty("erro", out var erro))
                return erro.GetString() ?? "A operação não pôde ser concluída.";

            if (json.RootElement.TryGetProperty("title", out var titulo))
                return titulo.GetString() ?? "A operação não pôde ser concluída.";
        }
        catch (JsonException)
        {
            return conteudo;
        }

        return "A operação não pôde ser concluída.";
    }
}
