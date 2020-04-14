using System.ComponentModel.DataAnnotations;

namespace BizCard.API.ViewModel
{
    public class CardViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Company is required")]
        public string Company { get; set; }
        
        [Required(ErrorMessage = "Contact is required")]
        public string Contact { get; set; }
        
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
    }
}