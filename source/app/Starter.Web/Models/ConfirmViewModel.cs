namespace DQF.Web.Models
{
    public class ConfirmViewModel
    {
        public string OkBtnText { get; set; }

        public string UrlAction { get; set; }

        public string Header { get; set; }

        public string BodyText { get; set; }

        public ConfirmViewModel()
        {
            OkBtnText = "Confirm";
            Header = "Confirm Deletion";
            BodyText = "Are you sure?";
        }
    }
}