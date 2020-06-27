using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Libraries.Lang;
using LojaVirtual.Models;
using LojaVirtual.Models.Contants;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace LojaVirtual.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class ClienteController : Controller
    {
        private LoginColaborador _loginColaborador;
        private IClienteRepository _clienteRepository;
        public ClienteController(IClienteRepository clienteRepository, LoginColaborador loginColaborador)
        {
            _clienteRepository = clienteRepository;
            _loginColaborador = loginColaborador;
        }
        public IActionResult Index(int? pagina, string pesquisa)
        {
            IPagedList<Models.Cliente> clientes = _clienteRepository.ObterTodosClientes(pagina, pesquisa);
            return View(clientes);
        }

        [ValidateHttpReferer]
        public IActionResult AtivarDesativar(int id)
        {
            Models.Cliente cliente = _clienteRepository.ObterCliente(id);
            cliente.Situacao = (cliente.Situacao == SituacaoConstant.Ativo) ? cliente.Situacao = SituacaoConstant.Desativado : cliente.Situacao = SituacaoConstant.Ativo;
            _clienteRepository.Atualizar(cliente);

            TempData["MSG_S"] = Mensagem.MSG_S001;

            return RedirectToAction(nameof(Index));
        }

        [ColaboradorAutorizacao]
        [ValidateHttpReferer]
        public IActionResult Logout()
        {
            _loginColaborador.Logout();
            return RedirectToAction("Login","Home");
        }

    }
}