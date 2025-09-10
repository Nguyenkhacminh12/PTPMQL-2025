using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Baitapbuoi3.Models;

namespace Baitapbuoi3.Controllers;

public class TinhtongController : Controller
{
    private readonly ILogger<TinhtongController> _logger;

    public TinhtongController(ILogger<TinhtongController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Index(Bai2 model)
    {
        if (model.So1.HasValue && model.So2.HasValue && !string.IsNullOrEmpty(model.PhepTinh))
        {
            switch (model.PhepTinh)
            {
                case "Tong":
                    model.Ketqua = model.So1 + model.So2;
                    break;
                case "Hieu":
                    model.Ketqua = model.So1 - model.So2;
                    break;
                case "Tich":
                    model.Ketqua = model.So1 * model.So2;
                    break;
                case "Thuong":
                    if (model.So2 != 0)
                        model.Ketqua = model.So1 / model.So2;
                    else
                        ModelState.AddModelError("", "Lỗi");
                    break;
            }
        }
        return View(model);
    }
}
