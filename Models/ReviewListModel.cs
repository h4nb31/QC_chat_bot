using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QualityControl.Models
{
    public class ReviewListModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ReviewType { get; set; }
        public string? Object {  get; set; }
        public string? ReviewText {  get; set; }

    }
}
