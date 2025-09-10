using Microsoft.AspNetCore.Mvc;
using Baitapbuoi3.Models;

namespace Baitapbuoi3.Controllers;

public class BMIController : Controller
{
    private readonly ILogger<BMIController> _logger;

    public BMIController(ILogger<BMIController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(BMI? model)
    {
        if (model != null && model.ChieuCao > 0 && model.CanNang > 0)
        {
            double bmi = model.CanNang / Math.Pow(model.ChieuCao / 100, 2);
            ViewBag.KetQua = $"Họ tên: {model.Ten}, Tuổi: {model.Tuoi}, Chiều cao: {model.ChieuCao} cm, Cân nặng: {model.CanNang} kg,  BMI = {bmi:F2}";
        }
        return View();
    }
}
