using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Baitapbuoi3.Models;

namespace Baitapbuoi3.Controllers;

public class ThongtinController : Controller
{
    private readonly ILogger<ThongtinController> _logger;

    public ThongtinController(ILogger<ThongtinController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Index(Thongtin tt)
    {
        string Thongtin = $"Họ tên: {tt.Hoten}, Giới tính: {tt.Gioitinh}, Ngày sinh: {tt.Ngaysinh}, Địa chỉ: {tt.Diachi}";
        ViewBag.Thongtin = tt;
        return View();
    }
    
}