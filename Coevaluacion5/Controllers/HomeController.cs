using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Coevaluacion5.Models;
using System.Data;

namespace Coevaluacion5.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;

  public HomeController(ILogger<HomeController> logger)
  {
    _logger = logger;
  }

  public IActionResult Index()
  {
    //Restart
    ViewData["encrypted-input"] = string.Empty;
    ViewData["decrypted-input"] = string.Empty;

    DataSet evm = new DataSet();
    evm.ReadXml(Program.bdPath);

    //Si DO POST
    if (HttpMethods.IsPost(Request.Method))
    {
      bool continuar = true;
      string? chainSent = Request.Form["chain-to-encrypt"].ToString();
      continuar = continuar ? chainSent != null : continuar;//If it is still true, we evaluate, otherwise, is false (continuar value).
      if (continuar)
      {
        DataRow registro = evm.Tables[(int)Program.Tables.Historical].NewRow();
        foreach (string columna in Program.Columns[(int)Program.Tables.Historical])
        {
          registro[columna] =
            columna.Equals(Program.Columns[(int)Program.Tables.Historical][(int)Program.ColumnsEnum.Chain]) ? chainSent :
            columna.Equals(Program.Columns[(int)Program.Tables.Historical][(int)Program.ColumnsEnum.Encrypt]) ? chainSent.EncryptBPP() :
            string.Empty;
        }
        ViewData["encrypted-input"] = registro[Program.Columns[(int)Program.Tables.Historical][(int)Program.ColumnsEnum.Encrypt]];
        ViewData["decrypted-input"] = registro[Program.Columns[(int)Program.Tables.Historical][(int)Program.ColumnsEnum.Chain]];
        evm.Tables[(int)Program.Tables.Historical].Rows.Add(registro);
        evm.WriteXml(Program.bdPath);
      }
    }
    return View(evm);
  }

  public IActionResult Privacy()
  {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
