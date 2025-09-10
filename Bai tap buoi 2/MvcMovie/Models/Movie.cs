using System.ComponentModel.DataAnnotations;
namespace MvcMovie.Models;

public class Movie
{
    public int Id { get; set; }

    [Required]
    public string? Title { get; set; }

    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    public string? Genre { get; set; }

    public decimal Price { get; set; }
}

public class Person
{
    public string PersonID { get; set; }
    public string FullName { get; set; }
    public string Address { get; set; }
}

public class Employee : Person
{
    public string EmployeeID { get; set; }
    public int Age { get; set; }

}

public class HeThongPhanPhoi
{
    public string MaHTPP { get; set; }
    public string TenHTPP { get; set; }
}

public class DaiLy : HeThongPhanPhoi
{
    public string MaDaiLy { get; set; }
    public string TenDaiLy { get; set; }
    public string DiaChi { get; set; }
    public string NguoiDaiDien { get; set; }
    public string DienThoai { get; set; }
    public string MaHTPP { get; set; }
}