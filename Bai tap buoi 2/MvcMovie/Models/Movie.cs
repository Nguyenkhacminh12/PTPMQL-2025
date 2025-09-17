using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models
{
    [Table("Movie")]
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public string? Genre { get; set; }

        public decimal Price { get; set; }
    }

    public class Person
    {
        [Key]                              
        public string PersonId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
        public string? Address { get; set; }
    }

    public class Employee
    {
        [Key]                              
        public string EmployeeID { get; set; } = string.Empty;

        public int Age { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
    }

    public class HeThongPhanPhoi
    {
        [Key]                                
        public string MaHTPP { get; set; } = string.Empty;

        public string TenHTPP { get; set; } = string.Empty;
    }

    public class DaiLy
    {
        [Key]                               
        public string MaDaiLy { get; set; } = string.Empty;

        public string TenDaiLy { get; set; } = string.Empty;
        public string? DiaChi { get; set; }
        public string? NguoiDaiDien { get; set; }
        public string? DienThoai { get; set; }

        public string MaHTPP { get; set; } = string.Empty; 
    }
}
