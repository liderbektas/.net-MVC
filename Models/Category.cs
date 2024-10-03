namespace ProductManagament_MVC.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Products>? Products { get; set; }
}